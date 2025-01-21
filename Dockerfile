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

# ASP.NET Core Runtime'ı temel al (runtime aşaması)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Yayınlanan dosyaları kopyala
COPY --from=build /app/out .

# PostgreSQL bağlantısı için ortam değişkenleri ayarla
ENV ASPNETCORE_URLS=http://+:80
ENV DOTNET_RUNNING_IN_CONTAINER=true

# Portu aç
EXPOSE 80

# Uygulamayı çalıştır
ENTRYPOINT ["dotnet", "CmkCable.API.dll"]
