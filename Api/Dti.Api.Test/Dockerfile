FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-alpine3.11 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-alpine3.11 AS build
WORKDIR /src
COPY ["Dti.Api.Test.Facades/Dti.Api.Test.Facades.csproj", "Dti.Api.Test.Facades/"]
COPY ["Dti.Api.Test.Models/Dti.Api.Test.Models.csproj", "Dti.Api.Test.Models/"]
COPY ["Dti.Api.Test.Services/Dti.Api.Test.Services.csproj", "Dti.Api.Test.Services/"]
COPY ["Dti.Api.Test/Dti.Api.Test.csproj", "Dti.Api.Test/"]

RUN dotnet restore "Dti.Api.Test/Dti.Api.Test.csproj"
COPY . .
WORKDIR "/src/Dti.Api.Test"
RUN dotnet build "Dti.Api.Test.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Dti.Api.Test.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Dti.Api.Test.dll"]