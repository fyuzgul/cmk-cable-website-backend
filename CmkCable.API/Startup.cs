using CloudinaryDotNet;
using CmkCable.Business.Concrete;
using CmkCable.DataAccess;
using CmkCable.DataAccess.Abstract;
using CmkCable.DataAccess.Concrete;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace CmkCable.API
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidIssuer = Configuration["Jwt:Issuer"],
                        ValidAudience = Configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:SecretKey"])),
                        ClockSkew = TimeSpan.Zero,
                        RequireSignedTokens = true,
                        RequireExpirationTime = true
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            try
                            {
                                var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Startup>>();
                                var authHeader = context.Request.Headers["Authorization"].ToString();
                                logger.LogInformation($"Authorization Header: {authHeader}");

                                if (string.IsNullOrEmpty(authHeader))
                                {
                                    logger.LogWarning("Authorization header is missing");
                                    return Task.CompletedTask;
                                }

                                string token;
                                if (authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                                {
                                    token = authHeader.Substring("Bearer ".Length).Trim();
                                }
                                else
                                {
                                    token = authHeader.Trim();
                                }

                                if (string.IsNullOrEmpty(token))
                                {
                                    logger.LogWarning("Token is empty after extraction");
                                    return Task.CompletedTask;
                                }

                                logger.LogInformation($"Extracted Token: {token}");
                                context.Token = token;

                                return Task.CompletedTask;
                            }
                            catch (Exception ex)
                            {
                                var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Startup>>();
                                logger.LogError($"Error in OnMessageReceived: {ex.Message}");
                                return Task.CompletedTask;
                            }
                        },

                        OnAuthenticationFailed = context =>
                        {
                            var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Startup>>();
                            logger.LogError($"Authentication Failed - Exception Type: {context.Exception.GetType().Name}");
                            logger.LogError($"Error Message: {context.Exception.Message}");
                            
                            if (context.Exception is SecurityTokenExpiredException)
                            {
                                logger.LogError("Token has expired");
                                context.Response.Headers.Append("Token-Expired", "true");
                            }
                            
                            return Task.CompletedTask;
                        },

                        OnTokenValidated = context =>
                        {
                            var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Startup>>();
                            logger.LogInformation("Token was successfully validated");
                            return Task.CompletedTask;
                        },

                        OnChallenge = context =>
                        {
                            var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Startup>>();
                            logger.LogWarning($"OnChallenge: {context.Error}, {context.ErrorDescription}");

                            context.HandleResponse();
                            context.Response.StatusCode = 401;
                            context.Response.ContentType = "application/json";
                            var result = System.Text.Json.JsonSerializer.Serialize(new { message = "Token has expired or is invalid." });
                            return context.Response.WriteAsync(result);
                        }
                    };
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Bearer", policy => policy.RequireAuthenticatedUser());
            });

            services.AddControllers();

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", builder =>
                {
                    builder
                        .SetIsOriginAllowed(_ => true) // Tüm originlere izin ver
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
                });
            });

            var cloudinaryAccount = new Account(
                Configuration["Cloudinary:CloudName"],
                Configuration["Cloudinary:ApiKey"],
                Configuration["Cloudinary:ApiSecret"]
            );
            var cloudinary = new Cloudinary(cloudinaryAccount);

            services.AddSingleton(cloudinary);

            services.Configure<KestrelServerOptions>(options =>
            {
                options.Limits.MaxRequestBodySize = 1073741824; // 1 GB
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseExceptionHandler(c => c.Run(async context =>
            {
                var exception = context.Features
                    .Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerPathFeature>()
                    .Error;

                var logger = context.RequestServices.GetRequiredService<ILogger<Startup>>();
                logger.LogError($"Error: {exception.Message}");

                context.Response.ContentType = "application/json";
                object response;

                if (exception is SecurityTokenException)
                {
                    context.Response.StatusCode = 401;
                    response = new { message = "Token has expired or is invalid." };
                }
                else if (exception is UnauthorizedAccessException)
                {
                    context.Response.StatusCode = 401;
                    response = new { message = exception.Message };
                }
                else
                {
                    context.Response.StatusCode = 500;
                    if (env.IsDevelopment())
                    {
                        response = new { message = exception.Message, stackTrace = exception.StackTrace };
                    }
                    else
                    {
                        response = new { message = "An error occurred while processing your request." };
                    }
                }

                await context.Response.WriteAsJsonAsync(response);
            }));

            app.UseRouting();

            app.UseCors("AllowAll");

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
