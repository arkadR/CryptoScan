ARG GITHUB_TOKEN

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["CryptoScan.Subscriptions.Worker/CryptoScan.Subscriptions.Worker.csproj", "CryptoScan.Subscriptions.Worker/"]
RUN dotnet nuget add source --username arkadiusz.rasz@gmail.com --password $GITHUB_TOKEN --store-password-in-clear-text --name github "https://nuget.pkg.github.com/arkadR/index.json"
RUN dotnet restore "CryptoScan.Subscriptions.Worker/CryptoScan.Subscriptions.Worker.csproj"
COPY . .
WORKDIR "/src/CryptoScan.Subscriptions.Worker"
RUN dotnet build "CryptoScan.Subscriptions.Worker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CryptoScan.Subscriptions.Worker.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CryptoScan.Subscriptions.Worker.dll"]
