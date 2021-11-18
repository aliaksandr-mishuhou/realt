# syntax=docker/dockerfile:1
FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY . .

# Copy everything else and build
WORKDIR /app/Realt.Parser.OnlinerBy.Worker
RUN dotnet publish -c Release -o out Realt.Parser.OnlinerBy.Worker.csproj

# Build runtime image
FROM mcr.microsoft.com/dotnet/runtime:3.1
WORKDIR /app
COPY --from=build-env /app/Realt.Parser.OnlinerBy.Worker/out .
ENTRYPOINT ["dotnet", "Realt.Parser.OnlinerBy.Worker.dll"]