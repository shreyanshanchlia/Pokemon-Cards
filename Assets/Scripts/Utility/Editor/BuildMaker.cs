using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace Utility.Editor
{
    public class BuildMaker : IPostprocessBuildWithReport
    {
        private const string SettingsPath = "Assets\\..\\ProjectSettings\\BuildVersions.txt";
        private static List<string> _buildVersions;
        
        public int callbackOrder => 0;

        public void OnPostprocessBuild(BuildReport report)
        {
            LoadBuildVersionsFromAssetsFile();
            IncrementBuildVersion();
            if (!EditorUserBuildSettings.development) IncrementVersionCode();
        }

        static void IncrementVersionCode()
        {
            #if UNITY_ANDROID
            PlayerSettings.Android.bundleVersionCode += 1;
            #endif
        }

        private static void IncrementBuildVersion()
        {
            var currentVersion = GetCurrentBuildVersion();
            var numbers = ExtractNumbersFromVersion(currentVersion);
            var newVersion = ReplaceLastNumber(currentVersion, ++numbers[^1]);

            SetCurrentBuildVersion(newVersion);
            _buildVersions.Add(currentVersion);
            WriteBuildVersionsToAssetsFile();
        }

        static string ReplaceLastNumber(string versionString, int newNumber)
        {
            // Regular expression pattern to match the last number in the string
            string pattern = @"\d+(?!.*\d)"; // Matches the last number in the string

            // Find the last number in the input string
            Match match = Regex.Match(versionString, pattern);

            if (match.Success)
            {
                // Replace only the last matched number with the new number
                string updatedVersion = Regex.Replace(versionString, pattern, newNumber.ToString());
                return updatedVersion;
            }

            // No number found in the string
            return versionString;
        }

        static string GetCurrentBuildVersion() => Application.version;
        static void SetCurrentBuildVersion(string newVersion) => PlayerSettings.bundleVersion = newVersion;

        static void LoadBuildVersionsFromAssetsFile()
        {
            _buildVersions = new List<string>();
            if (!System.IO.File.Exists(@SettingsPath))
            {
                System.IO.File.Create(@SettingsPath).Close();
            }

            var lines = System.IO.File.ReadAllLines(SettingsPath);
            if (lines.Length == 0)
            {
                System.IO.File.WriteAllText(SettingsPath, GetCurrentBuildVersion());
                lines = System.IO.File.ReadAllLines(SettingsPath);
            }

            foreach (var line in lines)
            {
                _buildVersions.Add(line);
            }
        }

        static string LastBuildVersion() => _buildVersions[^1];

        static void WriteBuildVersionsToAssetsFile()
        {
            var lines = new List<string>();
            foreach (var version in _buildVersions)
            {
                lines.Add(version);
            }
            System.IO.File.WriteAllLines(SettingsPath, lines);
        }

        static List<int> ExtractNumbersFromVersion(string versionString)
        {
            List<int> numbers = new List<int>();
            string pattern = @"\d+"; // Regular expression pattern to match digits

            // Match the pattern against the input version string
            MatchCollection matches = Regex.Matches(versionString, pattern);

            foreach (Match match in matches)
            {
                numbers.Add(int.Parse(match.Value));
            }
            return numbers;
        }
    }
}