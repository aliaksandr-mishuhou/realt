# syntax=docker/dockerfile:1
FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY . .

# Copy everything else and build
WORKDIR /app/Realt.Parser.RealtBy.Worker
RUN dotnet publish -c Release -o out Realt.Parser.RealtBy.Worker.csproj

# Build runtime image
FROM mcr.microsoft.com/dotnet/runtime:3.1
WORKDIR /app
COPY --from=build-env /app/Realt.Parser.RealtBy.Worker/out .
ENTRYPOINT ["dotnet", "Realt.Parser.RealtBy.Worker.dll"]