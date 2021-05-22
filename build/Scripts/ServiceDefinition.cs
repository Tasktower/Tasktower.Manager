using Nuke.Common.IO;

namespace _build.Scripts
{
    public class ServiceDefinition
    {
        public string ServiceName { get; set; }
        public string ServiceFolderName { get; set; }
        public string RepositoryUrl { get; set; }
        public string SolutionFile { get; set; }
        public string MainProject { get; set; }
        public string MainProjectDirectory { get; set; }
        
        public string DockerFilePath { get; set; }
        public AbsolutePath ServiceFolder(AbsolutePath projectsDir)
        {
            return projectsDir / ServiceFolderName;
        }
    
        public AbsolutePath ServiceSolutionFile(AbsolutePath projectsDir)
        {
            return ServiceFolder(projectsDir) / SolutionFile;
        }
    
        public AbsolutePath ServiceMainProjectFolder(AbsolutePath projectsDir)
        {
            return ServiceFolder(projectsDir) / MainProjectDirectory ;
        }
    
        public AbsolutePath ServiceMainProjectFile(AbsolutePath projectsDir)
        {
            return ServiceMainProjectFolder(projectsDir) / $"{MainProject}.csproj" ;
        }
        
        public AbsolutePath ServiceDockerFile(AbsolutePath projectsDir)
        {
            return ServiceFolder(projectsDir) / DockerFilePath;
        }

    }
}
