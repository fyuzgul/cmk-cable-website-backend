# Build aşaması
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# NuGet önbelleğini temizle
RUN dotnet nuget locals all --clear

# Proje dosyalarını kopyala
COPY CmkCable.sln ./
COPY CmkCable.API/CmkCable.API.csproj CmkCable.API/
COPY CmkCable.Business/CmkCable.Business.csproj CmkCable.Business/
COPY CmkCable.DataAccess/CmkCable.DataAccess.csproj CmkCable.DataAccess/
COPY CmkCable.Entities/CmkCable.Entities.csproj CmkCable.Entities/
COPY DTOs/DTOs.csproj DTOs/

# NuGet paketlerini indir
RUN dotnet restore CmkCable.sln

# Tüm dosyaları kopyala
COPY . .

# API projesine geçiş yap
WORKDIR /src/CmkCable.API

# Yayınlama işlemini yap
RUN dotnet publish -c Release -o /app/out


# Runtime aşaması
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# SSL sertifikalarını kopyala
COPY ./ssl/server.crt /etc/ssl/certs/
COPY ./ssl/server.key /etc/ssl/private/

# Build aşamasından çıkartılan dosyaları kopyala
COPY --from=build /app/out .

# HTTPS desteğini etkinleştir
EXPOSE 443

# Uygulamayı HTTPS üzerinde başlat
ENTRYPOINT ["dotnet", "CmkCable.API.dll"]
