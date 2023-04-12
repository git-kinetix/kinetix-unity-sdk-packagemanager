using System;
using UnityEditor.PackageManager;
using UnityEngine;

namespace Kinetix.PackageManager.Editor
{
    public static class GithubUtility
    {
        public static string GetPackageURL(string _GitURL, Version _Version, bool _IsProduction)
        {
            string version = $"{_Version.Major}.{_Version.Minor}.{_Version.Build}";
            if (!_IsProduction)
                version += "-pr";
            return GetPackageURL(_GitURL, version);
        }
        
        private static string GetPackageURL(string _GitURL, string _Version)
        {
            return _GitURL + $"#v{_Version}";
        }
        
        public static string GetReleasesURL(PackageInfo _PackageInfo)
        {
            string repoUrl = _PackageInfo.packageId.Split('@')[1];

            string releasesUrl = repoUrl
                .Split(new[] { ".git" }, StringSplitOptions.None)[0]
                .Replace(KinetixPackageManagerConstants.GITHUB_WEBSITE, KinetixPackageManagerConstants.GITHUB_API_URL) + "/releases";

            return releasesUrl;
        }
    }
}
