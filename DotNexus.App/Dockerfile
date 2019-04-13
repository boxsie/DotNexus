FROM microsoft/dotnet:sdk AS build-env
WORKDIR /app

COPY . ./
WORKDIR /app/DotNexus.App
RUN apt-get update -yq && apt-get upgrade -yq && apt-get install -yq curl git nano
RUN curl -sL https://deb.nodesource.com/setup_8.x | bash - && apt-get install -yq nodejs build-essential
RUN npm install -g npm
RUN npm install
RUN dotnet publish -c Release -o out

FROM microsoft/dotnet:aspnetcore-runtime
WORKDIR /app/DotNexus.App
COPY --from=build-env /app/DotNexus.App/out .
ENTRYPOINT ["dotnet", "DotNexus.App.dll"]