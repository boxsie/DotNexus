FROM microsoft/dotnet:sdk AS build-env
WORKDIR /app

COPY . ./
WORKDIR /app/DotNexus.App
RUN dotnet build -o out

EXPOSE 5001

FROM microsoft/dotnet:aspnetcore-runtime
WORKDIR /app/DotNexus.App
COPY --from=build-env /app/DotNexus.App/out .
ENTRYPOINT ["dotnet", "DotNexus.App.dll"]