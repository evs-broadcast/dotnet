#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
RUN ls > echo
COPY Structurizr.Cli/Structurizr.Cli.csproj Structurizr.Cli/
COPY Structurizr.Dsl/Structurizr.Dsl.csproj Structurizr.Dsl/
COPY Structurizr.Core/Structurizr.Core.csproj Structurizr.Core/
RUN dotnet restore "Structurizr.Cli/Structurizr.Cli.csproj"

COPY . .
WORKDIR "/src/Structurizr.Cli"
RUN dotnet build "Structurizr.Cli.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Structurizr.Cli.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Structurizr.Cli.dll"]