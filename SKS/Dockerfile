FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore "NLSL.SKS.Package.Services/NLSL.SKS.Package.Services.csproj"

WORKDIR "/src/NLSL.SKS.Package.Services"
RUN dotnet build "NLSL.SKS.Package.Services.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "NLSL.SKS.Package.Services.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NLSL.SKS.Package.Services.dll"]
