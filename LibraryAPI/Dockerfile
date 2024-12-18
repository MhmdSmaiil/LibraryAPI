FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["LibraryAPI.csproj", "./"]
RUN dotnet restore "LibraryAPI.csproj"
COPY . .
RUN dotnet build "LibraryAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "LibraryAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LibraryAPI.dll"]