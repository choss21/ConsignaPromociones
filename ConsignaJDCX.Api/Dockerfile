#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["ConsignaJDCX.Api/ConsignaJDCX.Api.csproj", "ConsignaJDCX.Api/"]
COPY ["ConsignaJDCX.Core/ConsignaJDCX.Core.csproj", "ConsignaJDCX.Core/"]
COPY ["ConsignaJDCX.Infrastructure/ConsignaJDCX.Infrastructure.csproj", "ConsignaJDCX.Infrastructure/"]
RUN dotnet restore "ConsignaJDCX.Api/ConsignaJDCX.Api.csproj"
COPY . .
WORKDIR "/src/ConsignaJDCX.Api"
RUN dotnet build "ConsignaJDCX.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ConsignaJDCX.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ConsignaJDCX.Api.dll"]
