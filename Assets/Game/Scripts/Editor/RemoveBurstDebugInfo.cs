using System.IO;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace Editor
{
    // Burst кладёт рядом с билдом папку *_BurstDebugInformation_DoNotShip, в релиз она не нужна
    public class RemoveBurstDebugInfo : IPostprocessBuildWithReport
    {
        public int callbackOrder => int.MaxValue;

        public void OnPostprocessBuild(BuildReport report)
        {
            var outputPath = report.summary.outputPath;
            // WebGL собирается в папку, Windows — в .exe
            var buildDir = Directory.Exists(outputPath) ? outputPath : Path.GetDirectoryName(outputPath);
            if (string.IsNullOrEmpty(buildDir) || !Directory.Exists(buildDir)) return;

            foreach (var dir in Directory.GetDirectories(buildDir, "*_BurstDebugInformation_DoNotShip"))
            {
                Directory.Delete(dir, true);
                Debug.Log($"[Build] removed {dir}");
            }
        }
    }
}
