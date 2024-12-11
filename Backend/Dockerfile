# Use the .NET 8 SDK image for building the solution
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the entire solution and all its projects
COPY ./Backend.sln ./
COPY ./DataAccess ./DataAccess
COPY "./REST_API" "./REST_API"
COPY ./UDP-Server ./UDP-Server

# Build and publish the UDP-Server project
RUN dotnet publish ./UDP-Server/UDP-Server.csproj -c Release -o /app

# Use the ASP.NET runtime image for running the UDP server
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app .
EXPOSE 65000/udp
ENTRYPOINT ["dotnet", "UDP-Server.dll"]