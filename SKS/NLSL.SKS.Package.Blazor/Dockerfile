FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["NLSL.SKS.Package.Blazor/NLSL.SKS.Package.Blazor.csproj", "NLSL.SKS.Package.Blazor/"]
RUN dotnet restore "NLSL.SKS.Package.Blazor/NLSL.SKS.Package.Blazor.csproj"
COPY . .
WORKDIR "/src/NLSL.SKS.Package.Blazor"
RUN dotnet build "NLSL.SKS.Package.Blazor.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "NLSL.SKS.Package.Blazor.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NLSL.SKS.Package.Blazor.dll"]
