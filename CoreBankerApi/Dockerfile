## See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.
#
## Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
## For more information, please see https://aka.ms/containercompat
#
## This stage is used when running from VS in fast mode (Default for Debug configuration)
#FROM mcr.microsoft.com/dotnet/aspnet:9.0-nanoserver-1809 AS base
#WORKDIR /app
#EXPOSE 8080
#EXPOSE 8081
#
#
## This stage is used to build the service project
#FROM mcr.microsoft.com/dotnet/sdk:9.0-nanoserver-1809 AS build
#ARG BUILD_CONFIGURATION=Release
#WORKDIR /src
#COPY ["/CoreBankerApi.csproj", "CoreBankerApi/"]
#RUN dotnet restore "/CoreBankerApi/CoreBankerApi.csproj"
#COPY . .
#WORKDIR "/src/CoreBankerApi"
#RUN dotnet build "./CoreBankerApi.csproj" -c %BUILD_CONFIGURATION% -o /app/build
#
## This stage is used to publish the service project to be copied to the final stage
#FROM build AS publish
#ARG BUILD_CONFIGURATION=Release
#RUN dotnet publish "./CoreBankerApi.csproj" -c %BUILD_CONFIGURATION% -o /app/publish /p:UseAppHost=false
#
## This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
#FROM base AS final
#WORKDIR /app
#COPY --from=publish /app/publish .
#ENTRYPOINT ["dotnet", "CoreBankerApi.dll"]


FROM mcr.microsoft.com/dotnet/sdk:9.0@sha256:3fcf6f1e809c0553f9feb222369f58749af314af6f063f389cbd2f913b4ad556 AS build
WORKDIR /App
EXPOSE 8081

# Copy everything
COPY . ./
# Restore as distinct layers
RUN dotnet restore
# Build and publish a release
RUN dotnet publish -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0@sha256:b4bea3a52a0a77317fa93c5bbdb076623f81e3e2f201078d89914da71318b5d8
WORKDIR /App
COPY --from=build /App/out .
ENTRYPOINT ["dotnet", "CoreBankerApi.dll"]