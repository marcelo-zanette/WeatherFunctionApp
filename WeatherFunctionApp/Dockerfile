#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/azure-functions/dotnet:3.0 AS base
WORKDIR /home/site/wwwroot
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["WeatherFunctionApp/WeatherFunctionApp.csproj", "WeatherFunctionApp/"]
RUN dotnet restore "WeatherFunctionApp/WeatherFunctionApp.csproj"
COPY . .
WORKDIR "/src/WeatherFunctionApp"
RUN dotnet build "WeatherFunctionApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WeatherFunctionApp.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /home/site/wwwroot
COPY --from=publish /app/publish .
ENV AzureWebJobsScriptRoot=/home/site/wwwroot \
    AzureFunctionsJobHost__Logging__Console__IsEnabled=true