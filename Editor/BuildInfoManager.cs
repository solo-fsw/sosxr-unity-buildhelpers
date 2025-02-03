#if UNITY_EDITOR
using System;
using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;


namespace SOSXR.BuildHelpers
{
    /// <summary>
    ///     Handles automatic semantic versioning and build information updates
    /// </summary>
    public class BuildInfoManager : IPreprocessBuildWithReport
    {
        private const string FilePath = "Assets/_SOSXR/Resources/build_info.csv";
        private const string InitialSemVer = "0_0_1";

        private static string _oldSemVer;
        private static string _newSemVer;

        private static int _oldBundleVersionCode; // Android only
        private static int _newBundleVersionCode; // Android only

        private static readonly string _developmentBuildIndicator = "d";
        private static readonly string _productionBuildIndicator = "p";

        public int callbackOrder => 0;


        public void OnPreprocessBuild(BuildReport report)
        {
            ChangeVersion(true);

            if (report.summary.platform == BuildTarget.Android)
            {
                ChangeAndroidVersion(true);
                Debug.Log("SemanticVersion: Incremented Android Bundle Version Code");
            }

            WriteBuildInfoToFile();
        }


        public static void ChangeVersion(bool increment)
        {
            _oldSemVer = PlayerSettings.bundleVersion;
            _newSemVer = GenerateNextVersion(_oldSemVer, increment);

            PlayerSettings.bundleVersion = _newSemVer;
            Debug.LogFormat("SemanticVersion: Updated SemVer from {0} to {1}", _oldSemVer, _newSemVer);
        }


        private static string GenerateNextVersion(string version, bool increment)
        {
            var buildIndicator = EditorUserBuildSettings.development ? _developmentBuildIndicator : _productionBuildIndicator;
            version = version.TrimEnd(Convert.ToChar(_developmentBuildIndicator), Convert.ToChar(_productionBuildIndicator)); // Remove build indicator if present

            var parts = version.Split('_');

            if (parts.Length < 2 || !int.TryParse(parts[^1], out var currentNumber))
            {
                var initialVersion = $"{InitialSemVer}{buildIndicator}";

                Debug.LogWarning($"[SemanticVersion] Invalid version format \"{version}\". " +
                                 $"Expected format like '0_0_0{buildIndicator}' or 'alpha-1_32_2{buildIndicator}'. Resetting to initial version {initialVersion}");

                return initialVersion;
            }

            parts[^1] = (increment ? currentNumber + 1 : Math.Max(0, currentNumber - 1)).ToString();

            return $"{string.Join("_", parts)}{buildIndicator}";
        }


        public static void ChangeAndroidVersion(bool increment)
        {
            _oldBundleVersionCode = PlayerSettings.Android.bundleVersionCode;
            _newBundleVersionCode = increment ? _oldBundleVersionCode + 1 : Math.Max(0, _oldBundleVersionCode - 1);

            PlayerSettings.Android.bundleVersionCode = _newBundleVersionCode;
            Debug.LogFormat("SemanticVersion: Updated Android Bundle Version Code from {0} to {1}", _oldBundleVersionCode, _newBundleVersionCode);
        }


        public static void WriteBuildInfoToFile()
        {
            var directoryPath = Path.GetDirectoryName(FilePath) ?? string.Empty;
            EnsureDirectoryExists(directoryPath);

            if (!File.Exists(FilePath))
            {
                WriteHeadersToFile();
            }

            AppendBuildInfoToFile();
            Debug.Log($"SemanticVersion: Appended build information to {FilePath}");
        }


        private static void EnsureDirectoryExists(string path)
        {
            if (Directory.Exists(path))
            {
                return;
            }

            Directory.CreateDirectory(path);
        }


        private static void WriteHeadersToFile()
        {
            using var sw = File.CreateText(FilePath);
            sw.WriteLine("UnityVersion, ProductionBuild, SemVer, BundleCode, BuildDate, BuildTime");
        }


        private static void AppendBuildInfoToFile()
        {
            var buildInfo = new[]
            {
                Application.unityVersion,
                (!EditorUserBuildSettings.development).ToString(),
                PlayerSettings.bundleVersion,
                PlayerSettings.Android.bundleVersionCode.ToString(),
                DateTime.Now.ToString("yyyy-MM-dd"),
                DateTime.Now.ToString("HH:mm")
            };

            using var sw = File.AppendText(FilePath);
            sw.WriteLine(string.Join(", ", buildInfo));
        }
    }
}
#endif