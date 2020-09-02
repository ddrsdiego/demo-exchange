FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["src/Demo.Exchange.Api/Demo.Exchange.Api.csproj", "src/Demo.Exchange.Api/"]
COPY ["src/Demo.Exchange.Domain/Demo.Exchange.Domain.csproj", "src/Demo.Exchange.Domain/"]
RUN dotnet restore "src/Demo.Exchange.Api/Demo.Exchange.Api.csproj"
COPY . .
WORKDIR "/src/src/Demo.Exchange.Api"
RUN dotnet build "Demo.Exchange.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Demo.Exchange.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
EXPOSE 80
ENTRYPOINT ["dotnet", "Demo.Exchange.Api.dll"]