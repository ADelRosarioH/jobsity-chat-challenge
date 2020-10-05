#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/runtime:3.1-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["JobsityStocksChat.StocksBot/JobsityStocksChat.StocksBot.csproj", "JobsityStocksChat.StocksBot/"]
COPY ["JobsityStocksChat.Infrastructure/JobsityStocksChat.Infrastructure.csproj", "JobsityStocksChat.Infrastructure/"]
COPY ["JobsityStocksChat.Core/JobsityStocksChat.Core.csproj", "JobsityStocksChat.Core/"]
RUN dotnet restore "JobsityStocksChat.StocksBot/JobsityStocksChat.StocksBot.csproj"
COPY . .
WORKDIR "/src/JobsityStocksChat.StocksBot"
RUN dotnet build "JobsityStocksChat.StocksBot.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "JobsityStocksChat.StocksBot.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "JobsityStocksChat.StocksBot.dll"]