using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kinetix.PackageManager.Editor
{
    public static class KinetixPackageManagerConstants
    {
        /// <summary>
        /// Define if the version is release or dev environment
        /// </summary>
        public static readonly bool IsProduction = false;

        public const string ASSET_FILTER           = "package";
        public const string PACKAGE_JSON           = "package.json";
        public const string PACKAGE_MANAGER_DOMAIN = "com.kinetix.packagemanager";
        public const string GITHUB_WEBSITE         = "https://github.com";
        public const string GITHUB_API_URL         = "https://api.github.com/repos";
    }
}
