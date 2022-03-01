FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
ENV ASPNETCORE_URLS=http://+:8080
WORKDIR /app
EXPOSE 8080

COPY publish .
ENTRYPOINT ["dotnet", "netca.Api.dll"]

ENV TZ 'Asia/Jakarta'
RUN echo $TZ > /etc/timezone && \
apt-get update && apt-get install -y tzdata && \
rm /etc/localtime && \
ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && \
dpkg-reconfigure -f noninteractive tzdata && \
apt-get clean