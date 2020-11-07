#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["src/API2.WebApi/API2.WebApi.csproj", "src/API2.WebApi/"]
RUN dotnet restore "src/API2.WebApi/API2.WebApi.csproj"
COPY . .
WORKDIR "/src/src/API2.WebApi"
RUN dotnet build "API2.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "API2.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "API2.WebApi.dll"]