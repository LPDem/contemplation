FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
LABEL maintainer="Dem, master@lpdem.space"
LABEL project="https://github.com/LPDem/contemplation"
WORKDIR /src
COPY . .
RUN curl -fsSL https://deb.nodesource.com/setup_16.x | bash -
RUN apt-get install -y nodejs
RUN dotnet publish "Contemplation.csproj" -v:minimal -c:Release -o:/Release

FROM mcr.microsoft.com/dotnet/aspnet:5.0
RUN apt-get update && apt-get install -y libgdiplus
RUN groupadd -r webuser && useradd --no-log-init -r -g webuser webuser
WORKDIR /app
RUN chown webuser:webuser $(pwd)
COPY --chown=webuser --from=build /Release/ .
EXPOSE 80
ENV ASPNETCORE_URLS="http://*:80"
ENV ImagesFolder="/images"
USER webuser
ENTRYPOINT ["dotnet", "Contemplation.dll"]