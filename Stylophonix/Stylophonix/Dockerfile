FROM mcr.microsoft.com/dotnet/aspnet:11.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:11.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Stylophonix/Stylophonix/Stylophonix.csproj", "Stylophonix/Stylophonix/"]
COPY ["Stylophonix/Stylophonix.Client/Stylophonix.Client.csproj", "Stylophonix/Stylophonix.Client/"]
RUN dotnet restore "Stylophonix/Stylophonix/Stylophonix.csproj"
COPY . .
WORKDIR "/src/Stylophonix/Stylophonix"
RUN dotnet build "./Stylophonix.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Stylophonix.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Stylophonix.dll"]
