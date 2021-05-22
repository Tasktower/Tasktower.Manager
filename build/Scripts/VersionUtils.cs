using System;
using Nuke.Common.IO;
using Nuke.Common.Tools.GitVersion;

namespace _build.Scripts
{
    public static class VersionUtils
    {
        public static string GetVersion(AbsolutePath targetPath)
        {
            var gitVersion = GitVersionTasks.GitVersion(gs =>
                gs.SetTargetPath(targetPath)).Result;
            return $"v{gitVersion.MajorMinorPatch}" +
                   $"-{gitVersion.CommitsSinceVersionSource}" +
                   $"-{gitVersion.BranchName}" +
                   $"-{gitVersion.Sha}" +
                   (gitVersion.CommitsSinceVersionSource == "0"? "": "-dirty");
        }
    }
}