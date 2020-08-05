# NuGet restore
FROM mcr.microsoft.com/dotnet/core/sdk:3.0 AS build
WORKDIR /src
COPY *.sln .
COPY CoronaAppZim.Api/*.csproj CoronaAppZim.Api/
RUN dotnet restore
COPY . .

# publish
FROM build AS publish
WORKDIR /src/CoronaAppZim.Api
RUN dotnet publish -c Release -o /src/publish

FROM mcr.microsoft.com/dotnet/core/aspnet:3.0 AS runtime
WORKDIR /app
COPY --from=publish /src/publish .
# ENTRYPOINT ["dotnet", "Colors.API.dll"]
# heroku uses the following