using System.Collections.Generic;
using Nuke.Common.IO;

namespace _build.Scripts.Models
{
    public class ServiceDefinition
    {
        public static HashSet<ServiceType> LibraryTypes = new HashSet<ServiceType>()
        {
            ServiceType.NetLibrary
        };
        public string ServiceName { get; set; }
        public string ServiceFolderName { get; set; }
        public string RepositoryUrl { get; set; }
        public string SolutionFile { get; set; }
        public string MainProject { get; set; }
        public string MainProjectDirectory { get; set; }
        public string DockerImageName { get; set; }
        public string DockerFilePath { get; set; }
        public ServiceType ServiceType { get; set; }
        public bool IsDockerService => new List<string>() {DockerImageName, DockerFilePath}
            .TrueForAll(s => !string.IsNullOrWhiteSpace(s));
        public AbsolutePath FolderPath(AbsolutePath projectsDir)
        {
            return projectsDir / ServiceFolderName;
        }
    
        public AbsolutePath SolutionFilePath(AbsolutePath projectsDir)
        {
            return FolderPath(projectsDir) / SolutionFile;
        }
    
        public AbsolutePath MainProjectFolderPath(AbsolutePath projectsDir)
        {
            return FolderPath(projectsDir) / MainProjectDirectory ;
        }
    
        public AbsolutePath MainProjectFilePath(AbsolutePath projectsDir)
        {
            return MainProjectFolderPath(projectsDir) / $"{MainProject}.csproj" ;
        }
        
        public AbsolutePath DockerFileAbsolutePath(AbsolutePath projectsDir)
        {
            return FolderPath(projectsDir) / DockerFilePath;
        }

    }
}
