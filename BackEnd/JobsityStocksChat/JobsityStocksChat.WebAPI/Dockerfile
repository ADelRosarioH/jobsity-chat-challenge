#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["JobsityStocksChat.WebAPI/JobsityStocksChat.WebAPI.csproj", "JobsityStocksChat.WebAPI/"]
RUN dotnet restore "JobsityStocksChat.WebAPI/JobsityStocksChat.WebAPI.csproj"
COPY . .
WORKDIR "/src/JobsityStocksChat.WebAPI"
RUN dotnet build "JobsityStocksChat.WebAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "JobsityStocksChat.WebAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "JobsityStocksChat.WebAPI.dll"]