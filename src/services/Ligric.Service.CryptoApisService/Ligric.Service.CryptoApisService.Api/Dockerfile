#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 50052

ENV ASPNETCORE_URLS http://*:50052
ENV ASPNETCORE_ENVIRONMENT Production

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["services/Ligric.Service.CryptoApisService/Ligric.Service.CryptoApisService.Api/Ligric.Service.CryptoApisService.Api.csproj", "services/Ligric.Service.CryptoApisService/Ligric.Service.CryptoApisService.Api/"]
COPY ["shared/Utils/Utils.csproj", "shared/Utils/"]
COPY ["services/Ligric.Service.CryptoApisService/Ligric.Service.CryptoApisService.Infrastructure/Ligric.Service.CryptoApisService.Infrastructure.csproj", "services/Ligric.Service.CryptoApisService/Ligric.Service.CryptoApisService.Infrastructure/"]
COPY ["services/Ligric.Service.CryptoApisService/Ligric.Service.CryptoApisService.Application/Ligric.Service.CryptoApisService.Application.csproj", "services/Ligric.Service.CryptoApisService/Ligric.Service.CryptoApisService.Application/"]
COPY ["services/Ligric.Service.CryptoApisService/Ligric.Service.CryptoApisService.Domain/Ligric.Service.CryptoApisService.Domain.csproj", "services/Ligric.Service.CryptoApisService/Ligric.Service.CryptoApisService.Domain/"]
COPY ["services/Ligric.Service.CryptoApisService/Ligric.Service.CryptoApisService.Infrastructure.NHibernate/Ligric.Service.CryptoApisService.Infrastructure.NHibernate.csproj", "services/Ligric.Service.CryptoApisService/Ligric.Service.CryptoApisService.Infrastructure.NHibernate/"]
COPY ["services/Ligric.Service.CryptoApisService/Ligric.Service.CryptoApisService.IoC/Ligric.Service.CryptoApisService.IoC.csproj", "services/Ligric.Service.CryptoApisService/Ligric.Service.CryptoApisService.IoC/"]
COPY ["services/Ligric.Service.CryptoApisService/Ligric.Service.CryptoApisService.UseCase/Ligric.Service.CryptoApisService.UseCase.csproj", "services/Ligric.Service.CryptoApisService/Ligric.Service.CryptoApisService.UseCase/"]
RUN dotnet restore "services/Ligric.Service.CryptoApisService/Ligric.Service.CryptoApisService.Api/Ligric.Service.CryptoApisService.Api.csproj"
COPY . .
WORKDIR "/src/services/Ligric.Service.CryptoApisService/Ligric.Service.CryptoApisService.Api"
RUN dotnet build "Ligric.Service.CryptoApisService.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Ligric.Service.CryptoApisService.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ligric.Service.CryptoApisService.Api.dll"]
