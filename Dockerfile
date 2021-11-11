FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
ENV ASPNETCORE_URLS=http://+:8080
WORKDIR /app
EXPOSE 8080

COPY publish .
ENTRYPOINT ["dotnet", "netca.Api.dll"]