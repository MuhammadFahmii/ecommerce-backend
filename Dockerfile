FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
ENV ASPNETCORE_URLS=http://+:8080
WORKDIR /app
EXPOSE 8080

COPY build .
ENTRYPOINT ["dotnet", "netca.Api.dll"]