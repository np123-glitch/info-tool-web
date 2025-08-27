﻿FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release

# Install Node.js
RUN curl -fsSL https://deb.nodesource.com/setup_21.x | bash - \
    && apt-get install -y nodejs \
    && rm -rf /var/lib/apt/lists/*

WORKDIR /src
COPY package*.json .
RUN npm ci
COPY ["ZmaReference.csproj", "./"]
RUN dotnet restore "ZmaReference.csproj"
COPY . .
RUN dotnet build "ZmaReference.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "ZmaReference.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
RUN apt-get update \
    && apt-get install -y wget curl
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ZmaReference.dll"]
