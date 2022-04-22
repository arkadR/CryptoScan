FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["CryptoScan.Subscriptions.API.csproj", "."]
RUN dotnet restore "CryptoScan.Subscriptions.API.csproj"
COPY . .
RUN dotnet build "CryptoScan.Subscriptions.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CryptoScan.Subscriptions.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CryptoScan.Subscriptions.API.dll"]
