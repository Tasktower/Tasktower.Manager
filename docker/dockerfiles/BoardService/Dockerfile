FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
COPY Tasktower.Webtools Tasktower.Webtools
COPY Tasktower.BoardService Tasktower.BoardService
WORKDIR  /Tasktower.BoardService/Tasktower.BoardService
RUN rm -rf Tasktower.BoardService.Tests
RUN dotnet restore "Tasktower.BoardService.csproj"
RUN dotnet publish "Tasktower.BoardService.csproj" -c release -o /DockerOutput/Website --no-restore
FROM mcr.microsoft.com/dotnet/aspnet:5.0
EXPOSE 5001
EXPOSE 5000
EXPOSE 443
EXPOSE 80
WORKDIR /DockerOutput/Website
COPY --from=build /DockerOutput/Website ./
ENTRYPOINT ["dotnet", "Tasktower.BoardService.dll"]