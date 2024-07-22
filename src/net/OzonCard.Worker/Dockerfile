FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/net/OzonCard.Worker/OzonCard.Worker.csproj", "src/net/OzonCard.Worker/"]
RUN dotnet restore "src/net/OzonCard.Worker/OzonCard.Worker.csproj"
COPY . .
WORKDIR "/src/src/net/OzonCard.Worker"
RUN dotnet build "OzonCard.Worker.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "OzonCard.Worker.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OzonCard.Worker.dll"]
