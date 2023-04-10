#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 50051

ENV ASPNETCORE_URLS http://*:50051
ENV ASPNETCORE_ENVIRONMENT Production

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["services/Ligric.Service.AuthService/Ligric.Service.AuthService.Api/Ligric.Service.AuthService.Api.csproj", "services/Ligric.Service.AuthService/Ligric.Service.AuthService.Api/"]
COPY ["shared/Utils/Utils.csproj", "shared/Utils/"]
COPY ["services/Ligric.Service.AuthService/Ligric.Service.AuthService.IoC/Ligric.Service.AuthService.IoC.csproj", "services/Ligric.Service.AuthService/Ligric.Service.AuthService.IoC/"]
COPY ["services/Ligric.Service.AuthService/Ligric.Service.AuthService.Infrastructure/Ligric.Service.AuthService.Infrastructure.csproj", "services/Ligric.Service.AuthService/Ligric.Service.AuthService.Infrastructure/"]
COPY ["services/Ligric.Service.AuthService/Ligric.Service.AuthService.Application/Ligric.Service.AuthService.Application.csproj", "services/Ligric.Service.AuthService/Ligric.Service.AuthService.Application/"]
COPY ["services/Ligric.Service.AuthService/Ligric.Service.AuthService.Domain/Ligric.Service.AuthService.Domain.csproj", "services/Ligric.Service.AuthService/Ligric.Service.AuthService.Domain/"]
COPY ["services/Ligric.Service.AuthService/Ligric.Service.AuthService.Infrastructure.Jwt/Ligric.Service.AuthService.Infrastructure.Jwt.csproj", "services/Ligric.Service.AuthService/Ligric.Service.AuthService.Infrastructure.Jwt/"]
COPY ["services/Ligric.Service.AuthService/Ligric.Service.AuthService.Infrastructure.Nhibernate/Ligric.Service.AuthService.Infrastructure.Nhibernate.csproj", "services/Ligric.Service.AuthService/Ligric.Service.AuthService.Infrastructure.Nhibernate/"]
COPY ["services/Ligric.Service.AuthService/Ligric.Service.AuthService.UseCase/Ligric.Service.AuthService.UseCase.csproj", "services/Ligric.Service.AuthService/Ligric.Service.AuthService.UseCase/"]
RUN dotnet restore "services/Ligric.Service.AuthService/Ligric.Service.AuthService.Api/Ligric.Service.AuthService.Api.csproj"
COPY . .
WORKDIR "/src/services/Ligric.Service.AuthService/Ligric.Service.AuthService.Api"
RUN dotnet build "Ligric.Service.AuthService.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Ligric.Service.AuthService.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ligric.Service.AuthService.Api.dll"]
