# Production multi-stage Dockerfile for DockerisedTodoApp (targeting .NET 10)
# Builds the app using the SDK image and produces a small runtime image.

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy only the project file first (project is in DockerisedTodoApp/)
COPY DockerisedTodoApp/DockerisedTodoApp.csproj DockerisedTodoApp/

# Restore as distinct layer
RUN dotnet restore DockerisedTodoApp/DockerisedTodoApp.csproj

# Copy the entire repository
COPY . .

# Set working dir to project and publish
WORKDIR /src/DockerisedTodoApp
RUN dotnet publish DockerisedTodoApp.csproj -c Release -o /app/publish /p:UseAppHost=false

# runtime image
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:80

# copy published output
COPY --from=build /app/publish ./

EXPOSE 80
ENTRYPOINT ["dotnet", "DockerisedTodoApp.dll"]
