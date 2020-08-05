# nuget restore
FROM mcr.microsoft.com/dotnet/core/sdk:3.0 AS build
WORKDIR /src
COPY *.sln .
COPY CoronaAppZim.API/*.csproj CoronaAppZim.API/
RUN dotnet restore
COPY . .

# publish
FROM build AS publish
WORKDIR /src/CoronaAppZim.API
RUN dotnet publish -c Release -o /src/publish

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS runtime
WORKDIR /app
COPY --from=publish /src/publish .
# ENTRYPOINT ["dotnet", "CoronaAppZim.Api.dll"]
# heroku uses the following
CMD ASPNETCORE_URLS=http://*:$PORT dotnet  CoronaAppZim.Api.dll
