#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["ETender.APIGateway/ETender.APIGateway.csproj", "ETender.APIGateway/"]
RUN dotnet restore "ETender.APIGateway/ETender.APIGateway.csproj"
COPY . .
WORKDIR "/src/ETender.APIGateway"
RUN dotnet build "ETender.APIGateway.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ETender.APIGateway.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ETender.APIGateway.dll"]