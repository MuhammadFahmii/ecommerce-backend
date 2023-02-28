FROM mcr.microsoft.com/dotnet/sdk:6.0-focal as build-env
WORKDIR /src
COPY . .
RUN dotnet publish "ecommerce.sln" -c Release -o /publish

FROM mcr.microsoft.com/dotnet/aspnet:6.0-focal as runtime
WORKDIR /publish
COPY --from=build-env /publish .
# COPY ./publish .

EXPOSE 5000
ENTRYPOINT ["dotnet", "ecommerce.Api.dll"]
