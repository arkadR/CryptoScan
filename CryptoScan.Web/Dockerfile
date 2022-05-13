FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS restore
ARG GH_NUGET_PAT
ARG GH_NUGET_USERNAME

# Install Node.js
RUN curl -fsSL https://deb.nodesource.com/setup_14.x | bash - \
    && apt-get install -y \
        nodejs \
    && rm -rf /var/lib/apt/lists/*

WORKDIR /src
COPY ["CryptoScan.Web.csproj", "."]

RUN dotnet nuget add source --username $GH_NUGET_USERNAME --password $GH_NUGET_PAT --store-password-in-clear-text --name github "https://nuget.pkg.github.com/arkadR/index.json"
RUN dotnet restore "CryptoScan.Web.csproj"
COPY ["client-app/package.json", "client-app/package.json"]
COPY ["client-app/package-lock.json", "client-app/package-lock.json"]
WORKDIR /src/client-app
RUN npm install

FROM restore AS build
WORKDIR /src
COPY . .
RUN dotnet build "CryptoScan.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CryptoScan.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CryptoScan.Web.dll"]
