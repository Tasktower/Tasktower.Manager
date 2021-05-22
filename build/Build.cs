using System;
using System.IO;
using System.Linq;
using _build.Scripts;
using Nuke.Common;
using Nuke.Common.CI;
using Nuke.Common.Execution;
using Nuke.Common.Git;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.Git;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

namespace _build
{
    [CheckBuildProjectConfigurations]
    [ShutdownDotNetAfterServerBuild]
    class Build : NukeBuild
    {
        /// Support plugins are available for:
        ///   - JetBrains ReSharper        https://nuke.build/resharper
        ///   - JetBrains Rider            https://nuke.build/rider
        ///   - Microsoft VisualStudio     https://nuke.build/visualstudio
        ///   - Microsoft VSCode           https://nuke.build/vscode

        public static int Main () => Execute<Build>(x => x.Compile);

        [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
        readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

        [Solution] readonly Solution Solution;
        
        [PathExecutable] readonly Tool Docker;
    
        [Parameter("Service Name chosen for the project, will default to all if not chosen or if it does not exist")] 
        readonly string ServiceName;

        [Parameter("Git command. " +
                   "[Example: nuke --git-command 'status'] " +
                   "If quotes are used, they must be in the form: \\\\\". " +
                   "[For example: nuke --git-command 'commit -m \\\\\"Hello world\\\\\"']")] 
        readonly string GitCommand = "status";
        
        [Parameter("Git Clone branch (i.e.) master")] 
        readonly string GitCloneBranch = "master";

        AbsolutePath ProjectsDirectory = RootDirectory / "..";
        readonly string OutputFolder = "Tasktower";

        private bool ServiceChosen() => ServiceName != null; 
        private bool ChosenServiceExists() => ServiceChosen() && ServiceAccessUtils.ServiceExists(ServiceName);
        private bool UsingDotNetServices() => !ServiceChosen() || !ServiceAccessUtils.ServiceExists(ServiceName) ||
                                              ServiceAccessUtils.ServiceExists(ServiceName) && 
                                              ServiceAccessUtils.ServiceDictionary[ServiceName].IsDotNetProject;
    
        private string ChosenServiceDirectoryOrFromManager() => ChosenServiceExists()
            ? ServiceAccessUtils.ServiceDictionary[ServiceName].ServiceFolder(RootDirectory)
            : Solution;
        private string ChosenServiceSolutionFileOrFromManager() => ChosenServiceExists()
            ? ServiceAccessUtils.ServiceDictionary[ServiceName].ServiceSolutionFile(ProjectsDirectory)
            : Solution;


        Target ListServices => _ => _
            .Executes(() =>
            {
                ServiceAccessUtils.Execute(
                    ServiceAccessUtils.ProjectsList, 
                    s => Console.WriteLine(s.ServiceName));
            });

        Target Clean => _ => _
            .Before(Restore)
            .Executes(() =>
            {
            });
    
        Target Restore => _ => _
            .Executes(() =>
            {
                DotNetRestore(s => s
                    .SetProjectFile(ChosenServiceSolutionFileOrFromManager()));
            });
        Target Compile => _ => _
            .DependsOn(Restore)
            .Executes(() =>
            {
                DotNetBuild(s => s
                    .SetProjectFile(ChosenServiceSolutionFileOrFromManager())
                    .SetConfiguration(Configuration)
                    .EnableNoRestore());
            });
    
        Target Test => _ => _
            .Requires(() => UsingDotNetServices())
            .Executes(() =>
            {
                DotNetTest(s => s
                    .SetProjectFile(ChosenServiceSolutionFileOrFromManager()));
            });

        Target Publish => _ => _
            .DependsOn(Compile)
            .Requires(() => UsingDotNetServices() && ChosenServiceExists())
            .Executes(() =>
            {
                DotNetPublish(s => s
                    .SetConfiguration(Configuration)
                    .SetOutput(ServiceAccessUtils.ServiceDictionary[ServiceName]
                        .ServiceMainProjectFolder(ProjectsDirectory) / "bin" / OutputFolder)
                    .SetProject(ServiceAccessUtils.ServiceDictionary[ServiceName]
                        .ServiceMainProjectFile(ProjectsDirectory)));
            });

        Target TestAndPublish => _ => _
            .DependsOn(Test)
            .Inherit(Publish);
        
        Target Version => _ => _
            .Executes(() =>
            {
                if (ServiceChosen())
                {
                    var version = VersionUtils.GetVersion(ServiceAccessUtils
                        .ServiceDictionary[ServiceName]
                        .ServiceFolder(ProjectsDirectory));
                    Console.WriteLine($"{ServiceName} version: {version}");
                    return;
                }

                ServiceAccessUtils.Execute(ServiceAccessUtils.ProjectsList, s =>
                {
                    var version = VersionUtils.GetVersion(s.ServiceFolder(ProjectsDirectory));
                    Console.WriteLine($"{s.ServiceName} version: {version}");
                }); 
            });
        
        Target GitRun => _ => _
            .Executes(() =>
            {
                if (ServiceChosen())
                {
                    GitUtils.RunGitCommandExistsOrClone(GitCommand,  
                        ServiceAccessUtils.ServiceDictionary[ServiceName], 
                        ProjectsDirectory, GitCloneBranch);
                }
                else
                {
                    ServiceAccessUtils.Execute(ServiceAccessUtils.ProjectsList, s =>
                    {
                        GitUtils.RunGitCommandExistsOrClone(GitCommand, s, ProjectsDirectory, GitCloneBranch);
                    });  
                }
            });

    }
}
