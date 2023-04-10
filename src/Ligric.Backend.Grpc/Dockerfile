#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Ligric.Backend.Grpc/Ligric.Backend.Grpc.csproj", "Ligric.Backend.Grpc/"]
RUN dotnet restore "Ligric.Backend.Grpc/Ligric.Backend.Grpc.csproj"
COPY . .
WORKDIR "/src/Ligric.Backend.Grpc"
RUN dotnet build "Ligric.Backend.Grpc.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Ligric.Backend.Grpc.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ligric.Backend.Grpc.dll"]