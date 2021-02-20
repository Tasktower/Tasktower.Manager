FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
COPY ../../../../Tasktower.Webtools Tasktower.Webtools
COPY ../../../../Tasktower.OcelotAPIGateway Tasktower.OcelotAPIGateway
WORKDIR  /Tasktower.OcelotAPIGateway/Tasktower.OcelotAPIGateway
RUN dotnet restore "Tasktower.OcelotAPIGateway.csproj"
RUN dotnet publish "Tasktower.OcelotAPIGateway.csproj" -c release -o /DockerOutput/Website --no-restore
FROM mcr.microsoft.com/dotnet/aspnet:5.0
EXPOSE 5001
EXPOSE 5000
EXPOSE 443
EXPOSE 80
WORKDIR /DockerOutput/Website
COPY --from=build /DockerOutput/Website ./
ENTRYPOINT ["dotnet", "Tasktower.OcelotAPIGateway.dll"]