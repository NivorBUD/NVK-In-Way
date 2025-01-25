# См. статью по ссылке https://aka.ms/customizecontainer, чтобы узнать как настроить контейнер отладки и как Visual Studio использует этот Dockerfile для создания образов для ускорения отладки.

# Этот этап используется при запуске из VS в быстром режиме (по умолчанию для конфигурации отладки)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base

# Установка временной зоны на Екатеринбург
RUN apt-get update && apt-get install -y tzdata \
    && ln -snf /usr/share/zoneinfo/Asia/Yekaterinburg /etc/localtime \
    && echo "Asia/Yekaterinburg" > /etc/timezone \
    && apt-get clean && rm -rf /var/lib/apt/lists/*

USER app
WORKDIR /app
EXPOSE 5878


# Этот этап используется для сборки проекта службы
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["NvkInWayWebApi/NvkInWayWebApi.csproj", "NvkInWayWebApi/"]
COPY ["NvkInWayWebApi.Application/NvkInWayWebApi.Application.csproj", "NvkInWayWebApi.Application/"]
COPY ["NvkInWayWebApi.Domain/NvkInWayWebApi.Domain.csproj", "NvkInWayWebApi.Domain/"]
COPY ["NvkInWayWebApi.Persistence/NvkInWayWebApi.Persistence.csproj", "NvkInWayWebApi.Persistence/"]
RUN dotnet restore "./NvkInWayWebApi/NvkInWayWebApi.csproj"
COPY . .
WORKDIR "/src/NvkInWayWebApi"
RUN dotnet build "./NvkInWayWebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Этот этап используется для публикации проекта службы, который будет скопирован на последний этап
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./NvkInWayWebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Этот этап используется в рабочей среде или при запуске из VS в обычном режиме (по умолчанию, когда конфигурация отладки не используется)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NvkInWayWebApi.dll"]