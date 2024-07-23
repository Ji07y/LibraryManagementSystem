# Usa la imagen base del SDK de .NET
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia el archivo .csproj al directorio de trabajo
COPY ["Libreca.csproj", "Libreca/"]

# Restaura las dependencias
RUN dotnet restore "Libreca/Libreca.csproj"

# Copia el resto del código y construye el proyecto
COPY . .
WORKDIR "/src/Libreca"
RUN dotnet build "Libreca.csproj" -c Release -o /app/build

# Publica el proyecto
FROM build AS publish
RUN dotnet publish "Libreca.csproj" -c Release -o /app/publish

# Imagen final
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Libreca.dll"]
