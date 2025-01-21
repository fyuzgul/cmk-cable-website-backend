# .NET SDK'sını temel alın (build aşaması)
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Çözüm ve bağımlılık dosyalarını kopyala
COPY CmkCable.sln ./
COPY CmkCable.API/CmkCable.API.csproj CmkCable.API/
COPY CmkCable.Business/CmkCable.Business.csproj CmkCable.Business/
COPY CmkCable.DataAccess/CmkCable.DataAccess.csproj CmkCable.DataAccess/
COPY CmkCable.Entities/CmkCable.Entities.csproj CmkCable.Entities/
COPY DTOs/DTOs.csproj DTOs/

# Bağımlılıkları yükle
RUN dotnet restore CmkCable.sln

# Tüm kaynak dosyalarını kopyala ve projeyi derle
COPY . .
WORKDIR /src/CmkCable.API
RUN dotnet publish -c Release -o /app/out

# SQL Server ve ASP.NET Core Runtime'ı çalıştırmak için temel alın
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# SQL Server'ı ekle
RUN apt-get update && apt-get install -y curl gnupg \
    && curl https://packages.microsoft.com/keys/microsoft.asc | apt-key add - \
    && curl https://packages.microsoft.com/config/ubuntu/20.04/prod.list > /etc/apt/sources.list.d/mssql-release.list \
    && apt-get update && ACCEPT_EULA=Y apt-get install -y mssql-server

# ASP.NET uygulamasını ekle
COPY --from=build /app/out .

# Giriş noktası scripti ekle
COPY start.sh .
RUN chmod +x start.sh

# Portları aç
EXPOSE 80
EXPOSE 1433

# Script ile hem SQL Server hem de uygulamayı başlat
ENTRYPOINT ["./start.sh"]
