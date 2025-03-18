# Base image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Sertifikaları kopyala
COPY ./ssl/cert.crt /etc/ssl/certs/
COPY ./ssl/cert.key /etc/ssl/private/

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
WORKDIR /app

# Derlenmiş dosyaları kopyala
COPY --from=build /app/out .

ENTRYPOINT ["dotnet", "CmkCable.API.dll"]
