# Etapa 1: compilar y publicar (imagen con el SDK completo)
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar todo el codigo del backend
COPY . .

# Restaurar y publicar solo la API (arrastra sus proyectos dependientes)
RUN dotnet restore "src/Quinela.Api/Quinela.Api.csproj"
RUN dotnet publish "src/Quinela.Api/Quinela.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Etapa 2: imagen ligera solo para ejecutar (sin SDK)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# La API escuchara en el puerto 8080 dentro del contenedor
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "Quinela.Api.dll"]
