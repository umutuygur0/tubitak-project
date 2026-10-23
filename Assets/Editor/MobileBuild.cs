using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

// Menü: Tools > TUBITAK > Build ...
// Komut satırı: Unity.exe -batchmode -quit -projectPath . -buildTarget Android -executeMethod MobileBuild.BuildAndroid
public static class MobileBuild
{
    private const string AndroidPath = "Builds/Android/tubitak-project.apk";
    private const string IOSPath = "Builds/iOS";

    [MenuItem("Tools/TUBITAK/Build Android APK")]
    public static void BuildAndroid()
    {
        EditorUserBuildSettings.buildAppBundle = false;
        Build(BuildTarget.Android, AndroidPath);
    }

    [MenuItem("Tools/TUBITAK/Export iOS Xcode Project")]
    public static void BuildIOS()
    {
        Build(BuildTarget.iOS, IOSPath);
    }

    private static void Build(BuildTarget target, string path)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(Path.GetFullPath(path)));
        var options = new BuildPlayerOptions
        {
            scenes = EditorBuildSettings.scenes.Where(s => s.enabled).Select(s => s.path).ToArray(),
            locationPathName = path,
            target = target,
            options = BuildOptions.None,
        };

        BuildReport report = BuildPipeline.BuildPlayer(options);
        BuildSummary summary = report.summary;
        Debug.Log($"[MobileBuild] {target}: {summary.result}, {summary.totalSize / (1024 * 1024)} MB, {summary.totalErrors} error(s) -> {path}");

        if (summary.result != BuildResult.Succeeded && Application.isBatchMode)
            EditorApplication.Exit(1);
    }
}
