FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/BankApp.HttpApi.Host/BankApp.HttpApi.Host.csproj", "BankApp.HttpApi.Host/"]
COPY ["src/BankApp.Application/BankApp.Application.csproj", "BankApp.Application/"]
COPY ["src/BankApp.Domain/BankApp.Domain.csproj", "BankApp.Domain/"]
COPY ["src/BankApp.Domain.Shared/BankApp.Domain.Shared.csproj", "BankApp.Domain.Shared/"]
COPY ["src/BankApp.Application.Contracts/BankApp.Application.Contracts.csproj", "BankApp.Application.Contracts/"]
COPY ["src/BankApp.EntityFrameworkCore/BankApp.EntityFrameworkCore.csproj", "BankApp.EntityFrameworkCore/"]
COPY ["src/BankApp.HttpApi/BankApp.HttpApi.csproj", "BankApp.HttpApi/"]
COPY NuGet.Config /


RUN dotnet restore "BankApp.HttpApi.Host/BankApp.HttpApi.Host.csproj" ## src/BankApp.HttpApi.Host/BankApp.HttpApi.Host.csproj --> BankApp.HttpApi.Host/BankApp.HttpApi.Host.csproj
COPY . ./
WORKDIR "./src/BankApp.HttpApi.Host"
RUN dotnet build "BankApp.HttpApi.Host.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BankApp.HttpApi.Host.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "BankApp.HttpApi.Host.dll"]
