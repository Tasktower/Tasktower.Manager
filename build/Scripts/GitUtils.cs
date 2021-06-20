using System;
using System.IO;
using _build.Scripts.Models;
using Nuke.Common.IO;
using Nuke.Common.Tools.Git;

namespace _build.Scripts
{
    public class GitUtils
    {
        public static void RunGitCommandExistsOrClone(string gitCommand, ServiceDefinition s, 
            AbsolutePath projectsDirectory, string gitCloneBranch)
        {
            if (!Directory.Exists(s.FolderPath(projectsDirectory)))
            {
                Console.WriteLine("Repository not found, cloning from remote");
                GitTasks.Git($"clone --branch {gitCloneBranch} {s.RepositoryUrl} {s.ServiceFolderName}",
                    projectsDirectory);
            }
            GitTasks.Git(gitCommand, s.FolderPath(projectsDirectory));
        }
    }
}