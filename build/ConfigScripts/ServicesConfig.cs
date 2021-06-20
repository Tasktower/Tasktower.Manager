using System.Collections.Generic;
using _build.Scripts;
using _build.Scripts.Models;

namespace _build.ConfigScripts
{
    public static class ServicesConfig
    {
        public static readonly IEnumerable<ServiceDefinition> Services = new List<ServiceDefinition>()
        {
            new()
            {
                ServiceName = "Migrator",
                ServiceFolderName = "Tasktower-Migrator",
                RepositoryUrl = "https://github.com/Tasktower/Tasktower-Migrator.git",
                SolutionFile = "Tasktower-Migrator.sln",
                MainProject = "Tasktower.Migrator",
                MainProjectDirectory = "Tasktower.Migrator",
                DockerFilePath = "Dockerfile",
                DockerImageName = "taskmaster39/tasktower-migrator",
                ServiceType = ServiceType.ConsoleApp
            },
            new()
            {
                ServiceName = "OcelotGateway",
                ServiceFolderName = "Tasktower-Ocelot-Gateway",
                SolutionFile = "Tasktower-Ocelot-Gateway.sln",
                RepositoryUrl = "https://github.com/Tasktower/Tasktower-Ocelot-Gateway.git",
                MainProject = "Tasktower.OcelotGateway",
                MainProjectDirectory = "Tasktower.OcelotGateway",
                DockerFilePath = "Dockerfile",
                DockerImageName = "taskmaster39/tasktower-ocelot-gateway",
                ServiceType = ServiceType.WebApi
            },
            new()
            {
                ServiceName = "ProjectService",
                ServiceFolderName = "Tasktower-Project-Service",
                RepositoryUrl = "https://github.com/Tasktower/Tasktower-Project-Service.git",
                SolutionFile = "Tasktower-Project-Service.sln",
                MainProject = "Tasktower.ProjectService",
                MainProjectDirectory = "Tasktower.ProjectService",
                DockerFilePath = "Dockerfile",
                DockerImageName = "taskmaster39/tasktower-project-service",
                ServiceType = ServiceType.WebApi
            },
            new()
            {
                ServiceName = "UserService",
                ServiceFolderName = "Tasktower-User-Service",
                RepositoryUrl = "https://github.com/Tasktower/Tasktower-User-Service.git",
                SolutionFile = "Tasktower-User-Service.sln",
                MainProject = "Tasktower.UserService",
                MainProjectDirectory = "Tasktower.UserService",
                DockerFilePath = "Dockerfile",
                DockerImageName = "taskmaster39/tasktower-user-service",
                ServiceType = ServiceType.WebApi
            },
            new()
            {
                ServiceName = "UIService",
                ServiceFolderName = "Tasktower-UI-Service",
                RepositoryUrl = "https://github.com/Tasktower/Tasktower-UI-Service.git",
                SolutionFile = "Tasktower-UI-Service.sln",
                MainProject = "Tasktower.UIService",
                MainProjectDirectory = "Tasktower.UIService",
                DockerFilePath = "Dockerfile",
                DockerImageName = "taskmaster39/tasktower-ui-service",
                ServiceType = ServiceType.WebUi
            },
            new()
            {
                ServiceName = "LibAspnetcore",
                ServiceFolderName = "Tasktower-Lib-Aspnetcore",
                RepositoryUrl = "https://github.com/Tasktower/Tasktower-Lib-Aspnetcore.git",
                SolutionFile = "Tasktower-Lib-Aspnetcore.sln",
                MainProject = "Tasktower.Lib.Aspnetcore",
                MainProjectDirectory = "Tasktower.Lib.Aspnetcore",
                ServiceType = ServiceType.NetLibrary
            },
            new()
            {
                ServiceName = "SQLServerDatabase",
                ServiceFolderName = "Tasktower-SQL-Server-Database",
                RepositoryUrl = "https://github.com/Tasktower/Tasktower-SQL-Server-Database.git",
                MainProject = "Tasktower.SQLServerDatabase",
                MainProjectDirectory = ".",
                DockerFilePath = "Dockerfile",
                DockerImageName = "taskmaster39/tasktower-sql-server",
                ServiceType = ServiceType.DockerBuilder
            }
        };
    }
}