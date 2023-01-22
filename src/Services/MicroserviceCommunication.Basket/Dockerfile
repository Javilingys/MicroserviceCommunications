#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Services/MicroserviceCommunication.Basket/MicroserviceCommunication.Basket.csproj", "src/Services/MicroserviceCommunication.Basket/"]
RUN dotnet restore "src/Services/MicroserviceCommunication.Basket/MicroserviceCommunication.Basket.csproj"
COPY . .
WORKDIR "/src/src/Services/MicroserviceCommunication.Basket"
RUN dotnet build "MicroserviceCommunication.Basket.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MicroserviceCommunication.Basket.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MicroserviceCommunication.Basket.dll"]