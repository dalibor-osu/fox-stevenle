FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
ARG PORT=5300
EXPOSE ${PORT}
ENV ASPNETCORE_URLS=http://+:${PORT}
RUN apt-get update && apt-get install -y ffmpeg

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["FoxStevenle.API/FoxStevenle.API.csproj", "FoxStevenle.API/"]
RUN dotnet restore "FoxStevenle.API/FoxStevenle.API.csproj"
COPY . .
WORKDIR "/src/FoxStevenle.API"
RUN dotnet build "FoxStevenle.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "FoxStevenle.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FoxStevenle.API.dll"]
