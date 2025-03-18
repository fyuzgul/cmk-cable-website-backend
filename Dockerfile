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

# SSL sertifikalarını kopyala ve izinleri ayarla
COPY ./ssl/cert.crt /etc/ssl/certs/
COPY ./ssl/cert.key /etc/ssl/private/

# Sertifika izinlerini ayarla
RUN chmod 644 /etc/ssl/certs/cert.crt \
    && chmod 600 /etc/ssl/private/cert.key \
    && chown root:root /etc/ssl/certs/cert.crt \
    && chown root:root /etc/ssl/private/cert.key

# Data Protection Keys için kalıcı dizin oluştur
RUN mkdir -p /root/.aspnet/DataProtection-Keys \
    && chmod 700 /root/.aspnet/DataProtection-Keys

# Derlenmiş dosyaları kopyala
COPY --from=build /app/out .

# ASPNETCORE_Kestrel__Certificates__Default__Path çevre değişkenini ayarla
ENV ASPNETCORE_Kestrel__Certificates__Default__Path=/etc/ssl/certs/cert.crt
ENV ASPNETCORE_Kestrel__Certificates__Default__KeyPath=/etc/ssl/private/cert.key

ENTRYPOINT ["dotnet", "CmkCable.API.dll"]
