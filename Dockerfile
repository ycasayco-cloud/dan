FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY RuralMed.Api.csproj ./
RUN dotnet restore ./RuralMed.Api.csproj
COPY . ./
# Rebuild NuGet assets in the container; never reuse Windows obj/project.assets.json.
RUN dotnet publish ./RuralMed.Api.csproj -c Release -o /app/publish --no-self-contained

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish ./
ENV ASPNETCORE_URLS=http://0.0.0.0:10000
EXPOSE 10000
ENTRYPOINT ["dotnet", "RuralMed.Api.dll"]
