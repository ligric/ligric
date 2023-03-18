#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Ligric.Service.Gateway/Ligric.Service.Gateway.csproj", "Ligric.Service.Gateway/"]
RUN dotnet restore "Ligric.Service.Gateway/Ligric.Service.Gateway.csproj"
COPY . .
WORKDIR "/src/Ligric.Service.Gateway"
RUN dotnet build "Ligric.Service.Gateway.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Ligric.Service.Gateway.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ligric.Service.Gateway.dll"]