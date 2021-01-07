# Build Environment
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /app

# Build environment build args
ARG PACKAGES_TOKEN

# Add Github NuGet Store
RUN dotnet nuget add source https://nuget.pkg.github.com/wahinekai/index.json -n github -u wahinekai -p ${PACKAGES_TOKEN} --store-password-in-clear-text

# Copy project/Solution files and restore
COPY ./src/AzureAdConnector.sln ./src/AzureAdConnector.sln
COPY ./src/AzureAdConnectorHost/AzureAdConnectorHost.csproj ./src/AzureAdConnectorHost/AzureAdConnectorHost.csproj
COPY ./src/AzureAdConnectorService/AzureAdConnectorService.csproj ./src/AzureAdConnectorService/AzureAdConnectorService.csproj
RUN dotnet restore ./src/AzureAdConnector.sln

# Copy everything and build
COPY ./src ./src
RUN dotnet restore ./src/AzureAdConnector.sln
RUN dotnet build ./src/AzureAdConnector.sln --no-restore --configuration Release --output ./out

# Build production runtime image
FROM mcr.microsoft.com/dotnet/sdk:5.0

# Set Runtime environment variables
ARG ASPNETCORE_ENVIRONMENT

ENV ASPNETCORE_ENVIRONMENT=${ASPNETCORE_ENVIRONMENT}

# Copy over production dll
WORKDIR /app
COPY --from=build /app/out .

EXPOSE 80

# Set Entrypoint
ENTRYPOINT dotnet AzureAdConnectorHost.dll --urls http://*
