FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["TraininApp.API/TraininApp.API.csproj", "TraininApp.API/"]
COPY ["TraininApp.Application/TraininApp.Application.csproj", "TraininApp.Application/"]
COPY ["TraininApp.Domain/TraininApp.Domain.csproj", "TraininApp.Domain/"]
COPY ["TraininApp.Infrastructure/TraininApp.Infrastructure.csproj", "TraininApp.Infrastructure/"]
RUN dotnet restore "TraininApp.API/TraininApp.API.csproj"

COPY . .
RUN dotnet publish "TraininApp.API/TraininApp.API.csproj" -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_HTTP_PORTS=8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "TraininApp.API.dll"]
