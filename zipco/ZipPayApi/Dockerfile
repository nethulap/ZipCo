FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY ["ZipPayApi.csproj", "./"]
RUN dotnet restore "./ZipPayApi.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "ZipPayApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ZipPayApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ZipPayApi.dll"]
