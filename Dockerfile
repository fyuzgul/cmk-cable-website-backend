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

# ASP.NET Core Runtime'ını temel alın (runtime aşaması)
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .

# Uygulamayı başlat
ENTRYPOINT ["dotnet", "CmkCable.API.dll"]
