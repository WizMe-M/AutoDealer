FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["AutoDealer.Web/AutoDealer.Web.csproj", "AutoDealer.Web/"]
RUN dotnet restore "AutoDealer.Web/AutoDealer.Web.csproj"
COPY . .
WORKDIR "/src/AutoDealer.Web"
RUN dotnet build "AutoDealer.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AutoDealer.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AutoDealer.Web.dll"]
