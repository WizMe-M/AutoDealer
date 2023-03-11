FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
ADD ["/AutoDealer.DAL/AutoDealer.DAL.csproj", "AutoDealer.DAL/"]
RUN dotnet restore "AutoDealer.DAL/AutoDealer.DAL.csproj"
COPY ./AutoDealer.DAL/ ./AutoDealer.DAL/
RUN dotnet build "AutoDealer.DAL/AutoDealer.DAL.csproj" -c Release -o /app/build

ADD ["/AutoDealer.API/AutoDealer.API.csproj", "AutoDealer.API/"]
RUN dotnet restore "AutoDealer.API/AutoDealer.API.csproj"
COPY ./AutoDealer.API/ ./AutoDealer.API/
WORKDIR "/src/AutoDealer.API"
RUN dotnet build "AutoDealer.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AutoDealer.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AutoDealer.API.dll"]
