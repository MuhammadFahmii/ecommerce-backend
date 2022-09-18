FROM mcr.microsoft.com/dotnet/sdk:6.0-focal AS base
WORKDIR /src
COPY . .
RUN dotnet publish "netca.sln" -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:6.0-focal AS final
ENV ASPNETCORE_URLS=http://+:5000
WORKDIR /app
EXPOSE 5000
RUN apt-get  update && apt-get install curl -y
ENV TZ 'Asia/Jakarta'
RUN echo $TZ > /etc/timezone && \
apt-get update && apt-get install -y tzdata && \
rm /etc/localtime && \
ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && \
dpkg-reconfigure -f noninteractive tzdata && \
apt-get clean
    
ENV DOTNET_TieredPGO=1
ENV DOTNET_ReadyToRun=0
ENV DOTNET_TC_QuickJitForLoops=1
ENV DOTNET_EnableDiagnostics=0
ENV DOTNET_CLI_TELEMETRY_OPTOUT=1

WORKDIR /app
COPY --from=base /app .

RUN useradd -u 1234 default
USER default

ENTRYPOINT ["dotnet", "netca.Api.dll"]