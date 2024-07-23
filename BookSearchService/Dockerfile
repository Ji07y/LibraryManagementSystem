FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["BookSearchService.csproj", "BookSearchService/"]
RUN dotnet restore "BookSearchService/BookSearchService.csproj"
COPY . .
WORKDIR "/src/BookSearchService"
RUN dotnet build "BookSearchService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BookSearchService.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BookSearchService.dll"]
