using System;
using System.IO;
using Nuke.Common.IO;
using Nuke.Common.Tools.Git;

namespace _build.Scripts
{
    public class GitUtils
    {
        public static void RunGitCommandExistsOrClone(string gitCommand, ServiceDefinition s, 
            AbsolutePath projectsDirectory, string gitCloneBranch)
        {
            if (Directory.Exists(s.ServiceFolder(projectsDirectory)))
            { 
                GitTasks.Git(gitCommand, s.ServiceFolder(projectsDirectory));
            }
            else
            {
                GitTasks.Git($"clone --branch {gitCloneBranch} {s.RepositoryUrl} {s.ServiceFolderName}", 
                    projectsDirectory);
            }
        }
    }
}