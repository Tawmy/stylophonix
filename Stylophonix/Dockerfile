FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Stylophonix/Stylophonix.csproj", "Stylophonix/"]
RUN dotnet restore "Stylophonix/Stylophonix.csproj"
COPY . .
WORKDIR "/src/Stylophonix"
RUN dotnet build "Stylophonix.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Stylophonix.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Stylophonix.dll"]
