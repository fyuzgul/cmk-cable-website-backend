# 1. .NET SDK'sını temel al
FROM mcr.microsoft.com/dotnet/sdk:8 AS build
WORKDIR /src

# 2. NuGet kaynaklarını temizle
RUN dotnet nuget locals all --clear

# 3. Çözüm ve bağımlılık dosyalarını kopyala
COPY CmkCable.sln ./
COPY CmkCable.API/CmkCable.API.csproj CmkCable.API/
COPY CmkCable.Business/CmkCable.Business.csproj CmkCable.Business/
COPY CmkCable.DataAccess/CmkCable.DataAccess.csproj CmkCable.DataAccess/
COPY CmkCable.Entities/CmkCable.Entities.csproj CmkCable.Entities/
COPY DTOs/DTOs.csproj DTOs/

# 4. Bağımlılıkları indir
RUN dotnet restore CmkCable.sln

# 5. Tüm kaynak dosyalarını kopyala
COPY . .

# 6. Projeyi derle ve yayınla
WORKDIR /src/CmkCable.API
RUN dotnet publish -c Release -o /app/out

# 7. Küçük ve hafif bir runtime container kullan
FROM mcr.microsoft.com/dotnet/aspnet:8 AS runtime
WORKDIR /app

# 8. Yayınlanan dosyaları kopyala
COPY --from=build /app/out .

# 9. Çalıştırılabilir dosyayı belirt
ENTRYPOINT ["dotnet", "CmkCable.API.dll"]
