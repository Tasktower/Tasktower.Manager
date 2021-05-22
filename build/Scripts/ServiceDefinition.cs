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
        public string MainProjectFilePath { get; set; }
        public bool IsDotNetProject { get; set; } = true;
    
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
            return ServiceFolder(projectsDir) / MainProjectFilePath ;
        }
    
        public AbsolutePath ServiceMainProjectFile(AbsolutePath projectsDir)
        {
            return ServiceMainProjectFolder(projectsDir) / $"{MainProject}.csproj" ;
        }

    }
}
