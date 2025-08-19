# Base image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Uygulama dosyalarını kopyala ve derle
COPY CmkCable.sln ./
COPY CmkCable.API/CmkCable.API.csproj CmkCable.API/
COPY CmkCable.Business/CmkCable.Business.csproj CmkCable.Business/
COPY CmkCable.DataAccess/CmkCable.DataAccess.csproj CmkCable.DataAccess/
COPY CmkCable.Entities/CmkCable.Entities.csproj CmkCable.Entities/
COPY DTOs/DTOs.csproj DTOs/

RUN dotnet restore CmkCable.sln

COPY . ./

WORKDIR /src/CmkCable.API
RUN dotnet publish -c Release -o /app/out

# Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

# OpenSSL yükle
RUN apt-get update && apt-get install -y openssl

WORKDIR /app

# Data Protection Keys için kalıcı dizin oluştur
RUN mkdir -p /root/.aspnet/DataProtection-Keys \
    && chmod 700 /root/.aspnet/DataProtection-Keys

# Derlenmiş dosyaları kopyala
COPY --from=build /app/out .

# ASPNETCORE_Kestrel__Certificates__Default__Path çevre değişkenini ayarla
ENV ASPNETCORE_Kestrel__Certificates__Default__Path=/etc/letsencrypt/live/cmkkablo.com/fullchain.pem
ENV ASPNETCORE_Kestrel__Certificates__Default__KeyPath=/etc/letsencrypt/live/cmkkablo.com/privkey.pem

# SendGrid Configuration
ENV SENDGRID_FROM_EMAIL=webcmkkablo@gmail.com
ENV SENDGRID_FROM_NAME="CMK KABLO"

# Expose port
EXPOSE 1000

ENTRYPOINT ["dotnet", "CmkCable.API.dll"]
