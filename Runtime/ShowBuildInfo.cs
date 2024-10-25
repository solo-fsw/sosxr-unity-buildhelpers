using System;
using System.IO;
using TMPro;
using UnityEngine;


namespace SOSXR.BuildIncrementer
{
    public class ShowBuildInfo : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI m_buildInfoText;

        private string _currentTime;
        private float _elapsedTime;
        private const float _updateInterval = 10f;


        private void Start()
        {
            InvokeRepeating(nameof(SetBuildText), 0, _updateInterval);
        }


        private void SetBuildText()
        {
            m_buildInfoText.enabled = true;
            enabled = true;

            var textAsset = Resources.Load<TextAsset>("build_info");

            if (textAsset == null)
            {
                m_buildInfoText.text = "No build information available.";

                return;
            }

            var fileContent = textAsset.text;
            var lastLine = ReadLastLineOfFile(fileContent);

            if (lastLine != null)
            {
                m_buildInfoText.text = FormatBuildInfo(lastLine);
            }
            else
            {
                m_buildInfoText.text = "No build information available.";
            }
        }


        public static string ReadLastLineOfFile(string fileContent)
        {
            string lastLine = null;

            using var sr = new StringReader(fileContent);

            while (sr.ReadLine() is { } line)
            {
                lastLine = line;
            }

            return lastLine;
        }


        private string FormatBuildInfo(string line)
        {
            var parts = line.Split(',');

            // Assuming the format is "BuildDate,BuildTime,DevBuild,SemVer,BundleCode"
            if (parts.Length >= 5)
            {
                var buildDate = parts[0];
                var buildTime = parts[1];
                var devBuild = parts[2];
                var semVer = parts[3];
                var bundleCode = parts[4];

                var buildDateTime = DateTime.ParseExact($"{buildDate} {buildTime}", "yyyy-MM-dd HH:mm", null);
                var now = DateTime.Now;
                _currentTime = now.ToString("HH:mm");

                var relativeDate = GetRelativeDateDescription(buildDateTime.Date, now.Date);

                return $"DevBuild: {devBuild}\nSemVer: {semVer}\nBundleCode: {bundleCode}\nBuild Date: {relativeDate} at {buildTime}\nCurrent Time: {_currentTime}";
            }

            return "Invalid build information format.";
        }


        private string GetRelativeDateDescription(DateTime buildDate, DateTime currentDate)
        {
            var daysDifference = (currentDate - buildDate).Days;

            return daysDifference switch
                   {
                       0 => "today",
                       1 => "yesterday",
                       _ => $"{daysDifference} days ago"
                   };
        }
    }
}