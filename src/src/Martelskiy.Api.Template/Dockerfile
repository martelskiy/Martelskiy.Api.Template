FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["Martelskiy.Api.Template/Martelskiy.Api.Template.csproj", "Martelskiy.Api.Template/"]
RUN dotnet restore "Martelskiy.Api.Template/Martelskiy.Api.Template.csproj"
COPY . .
WORKDIR "/src/Martelskiy.Api.Template"
RUN dotnet build "Martelskiy.Api.Template.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Martelskiy.Api.Template.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Martelskiy.Api.Template.dll"]