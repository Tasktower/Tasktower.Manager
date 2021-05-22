using System.Collections.Generic;
using _build.Scripts;

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
                DockerImageName = "tasktower-migrator"
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
                DockerImageName = "tasktower-ocelot-gateway"
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
                DockerImageName = "tasktower-project-service"
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
                DockerImageName = "tasktower-ui-service"
            },
            new()
            {
                ServiceName = "SQLServerDatabase",
                ServiceFolderName = "Tasktower-SQL-Server-Database",
                RepositoryUrl = "https://github.com/Tasktower/Tasktower-SQL-Server-Database.git",
                MainProject = "Tasktower.SQLServerDatabase",
                MainProjectDirectory = ".",
                DockerFilePath = "Dockerfile",
                DockerImageName = "tasktower-sql-server-db"
            }
        };
    }
}