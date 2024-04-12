FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/net/OzonCard.Customer.Api/OzonCard.Customer.Api.csproj", "src/net/OzonCard.Customer.Api/"]
RUN dotnet restore "src/net/OzonCard.Customer.Api/OzonCard.Customer.Api.csproj"
COPY . .
WORKDIR "/src/src/net/OzonCard.Customer.Api"
RUN dotnet build "OzonCard.Customer.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "OzonCard.Customer.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OzonCard.Customer.Api.dll"]
