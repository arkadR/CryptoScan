FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG GITHUB_TOKEN
ARG GITHUB_USERNAME

WORKDIR /src
COPY CryptoScan.Subscriptions.Worker.csproj .
RUN dotnet nuget add source --username $GITHUB_USERNAME --password $GITHUB_TOKEN --store-password-in-clear-text --name github "https://nuget.pkg.github.com/arkadR/index.json"
RUN dotnet restore "CryptoScan.Subscriptions.Worker.csproj"
COPY . .
RUN dotnet build CryptoScan.Subscriptions.Worker.csproj -c Release -o /app/build

FROM build AS publish
RUN dotnet publish CryptoScan.Subscriptions.Worker.csproj -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CryptoScan.Subscriptions.Worker.dll"]
