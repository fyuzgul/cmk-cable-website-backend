#!/bin/bash

# SQL Server'ı arka planda çalıştır
/opt/mssql/bin/sqlservr &

# ASP.NET uygulamanızı başlatın
dotnet CmkCable.API.dll
