using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using _build.ConfigScripts;
using _build.Scripts;
using _build.Scripts.Models;
using Nuke.Common;
using Nuke.Common.CI;
using Nuke.Common.Execution;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.Docker;
using Nuke.Common.Tools.DotNet;
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

        public static int Main () => Execute<Build>(x => x.DotnetCompile);

        [Solution] readonly Solution Solution;
        
        [PathExecutable] readonly Tool Docker;
        
        [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
        readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

        [Parameter("Service Name chosen for the project, will default to all if not chosen or if it does not exist")] 
        readonly string ServiceName;

        [Parameter("Git command. " +
                   "[Example: nuke --git-command 'status'] " +
                   "If quotes are used, they must be in the form: \\\\\". " +
                   "[For example: nuke --git-command 'commit -m \\\\\"Hello world\\\\\"']")] 
        readonly string GitCommand = "status";
        
        [Parameter("Git Clone branch (i.e.) master")] 
        readonly string GitCloneBranch = "master";

        [Parameter("Docker build sets tag as latest")]
        readonly bool DockerBuildLatest = true;
        
        [Parameter("Docker registry server")] 
        readonly string DockerRegistryServer;

        [Parameter("Docker username")] 
        readonly string DockerUsername;
        
        [Parameter("Docker password")] 
        readonly string DockerPassword;
        
        [Parameter("Docker push latest tag")]
        readonly bool DockerPushLatest = false;

        readonly AbsolutePath ProjectsDirectory = RootDirectory / "..";

        // Service chosen flags
        
        /// <summary>
        /// If a specific service is specified 
        /// </summary>
        bool ServiceChosen => ServiceName != null;
        
        /// <summary>
        /// If a specific service is specified and that service exists in the configuration
        /// </summary>
        bool ChosenServiceExists => ServiceChosen && ServiceAccessUtils.ServiceExists(ServiceName);
        
        // File/Folder path accessors
        
        /// <summary>
        /// Solution file chosen to run based on if a service is chosen.
        /// If no service is chosen, the solution file from manager is used.
        /// Otherwise, the solution file for the specified service is used.
        /// </summary>
        string ChosenSolutionFile => ChosenServiceExists
            ? ServiceAccessUtils.ServiceDictionary[ServiceName].SolutionFilePath(ProjectsDirectory)
            : Solution;
        
        // Service Lists
        
        /// <summary>
        /// Services chosen for Nuke to use
        /// </summary>
        IEnumerable<ServiceDefinition> ChosenServiceDefinitions => ServiceChosen? 
            new[]{ ServiceAccessUtils.ServiceDictionary[ServiceName] }: 
            ServiceAccessUtils.ServicesList;
        
        /// <summary>
        /// Services chosen for Nuke to use
        /// </summary>
        IEnumerable<ServiceDefinition> ChosenExistingServiceDefinitions => 
            ChosenServiceDefinitions.Where(s => Directory.Exists(s.FolderPath(ProjectsDirectory)));
        
        /// <summary>
        /// All chosen services that use docker
        /// </summary>
        IEnumerable<ServiceDefinition> ChosenDockerServices => 
            ChosenExistingServiceDefinitions.Where(d => d.IsDockerService);


        Target ListServices => _ => _
            .Description("List services")
            .Executes(() =>
            {
                ServiceAccessUtils.Execute(
                    ServiceAccessUtils.ServicesList, 
                    s => Console.WriteLine(s.ServiceName));
            });

        Target DotnetClean => _ => _
            .Description("Clean .net service(s)")
            .Before(DotnetRestore)
            .Executes(() =>
            {
            });
    
        Target DotnetRestore => _ => _
            .Description("Restore .net service(s)")
            .Executes(() =>
            {
                DotNetRestore(s => s
                    .SetProjectFile(ChosenSolutionFile));
            });
        Target DotnetCompile => _ => _
            .Description("Compile .net service(s)")
            .DependsOn(DotnetRestore)
            .Executes(() =>
            {
                DotNetBuild(s => s
                    .SetProjectFile(ChosenSolutionFile)
                    .SetConfiguration(Configuration)
                    .EnableNoRestore());
            });
    
        Target DotnetTest => _ => _
            .Description("Test .net service(s)")
            .Executes(() =>
            {
                DotNetTest(s => s
                    .SetProjectFile(ChosenSolutionFile));
            });

        Target DotnetPublish => _ => _
            .Description("Publish .net service(s)")
            .DependsOn(DotnetCompile)
            .Executes(() =>
            {
                ServiceAccessUtils.Execute(ChosenExistingServiceDefinitions, service =>
                {
                    DotNetPublish(s => s
                        .SetConfiguration(Configuration)
                        .SetOutput(service.MainProjectFolderPath(ProjectsDirectory) / BuildConfig.OutputFolder)
                        .SetProject(service.MainProjectFilePath(ProjectsDirectory)));
                });
            });

        Target DotnetTestAndPublish => _ => _
            .Description("Test and publish .net service(s)")
            .DependsOn(DotnetTest, DotnetPublish);
        
        Target Version => _ => _
            .Description("Print version")
            .Executes(() =>
            {
                ServiceAccessUtils.Execute(ChosenExistingServiceDefinitions, s =>
                {
                    var version = VersionUtils.GetVersion(s.FolderPath(ProjectsDirectory));
                    Console.WriteLine($"{s.ServiceName} version: {version}");
                }); 
            });
        
        Target GitRun => _ => _
            .Description("Run git commands.\n" +
                         "    Requires the git command parameter to specify git commands.\n" +
                         "    Status is the default command.\n" +
                         "    If the service is not found, GitRun will automatically clone the repository.")
            .Executes(() =>
            {
                ServiceAccessUtils.Execute(ChosenServiceDefinitions, s => 
                {
                    GitUtils.RunGitCommandExistsOrClone(GitCommand, s, ProjectsDirectory, GitCloneBranch);
                });  
                
            });

        Target DockerBuild => _ => _
            .Description("Build docker image(s)")
            .Executes(() =>
            {
                ServiceAccessUtils.Execute(ChosenDockerServices, service =>
                {
                    Console.WriteLine("Building " + service.ServiceName);
                    ISet<string> tags = new HashSet<string>();
                    tags.Add(VersionUtils.GetVersion(service.FolderPath(ProjectsDirectory)));
                    if (DockerBuildLatest)
                    {
                        tags.Add("latest");
                    }

                    DockerTasks.DockerBuild(s => s
                        .SetFile(service.DockerFileAbsolutePath(ProjectsDirectory))
                        .SetPath(service.FolderPath(ProjectsDirectory))
                        .EnableNoCache()
                        .SetTag(tags.Select(tag => $"{service.DockerImageName}:{tag}"))
                    );
                });
            });
        
        Target DockerSignOut => _ => _
            .Description("Log out of a docker registry")
            .Executes(() =>
            {
                DockerTasks.DockerLogout(s => s.SetServer(DockerRegistryServer));
            });
        
        Target DockerSignIn => _ => _
            .Description("Log in to a docker registry")
            .DependsOn(DockerSignOut)
            .Requires(() => DockerUsername != null && DockerPassword != null)
            .Executes(() =>
            {
                DockerTasks.DockerLogin(s => s
                    .SetUsername(DockerUsername)
                    .SetPassword(DockerPassword)
                    .SetServer(DockerRegistryServer));
            });

        Target DockerPushRaw => _ => _
            .Description("Push docker image without logging in")
            .Executes(() =>
            {
                ServiceAccessUtils.Execute(ChosenDockerServices, service =>
                {
                    DockerTasks.DockerPush(s => s
                        .SetName($"{service.DockerImageName}:" +
                                 $"{VersionUtils.GetVersion(service.FolderPath(ProjectsDirectory))}"));
                    if (DockerPushLatest)
                    {
                        DockerTasks.DockerPush(s => s
                            .SetName($"{service.DockerImageName}:latest"));
                    }
                });
            });

        Target DockerPushSignedIn => _ => _
            .Description("Login to docker, then push the image to a registry")
            .DependsOn(DockerSignIn)
            .Inherit(DockerPushRaw);

    }
}
