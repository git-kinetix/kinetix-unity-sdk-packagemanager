using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Kinetix.Editor;
using Newtonsoft.Json;
using UnityEditor;
using UnityEditor.Compilation;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.Networking;
using EditorUtility = UnityEditor.EditorUtility;
using PackageInfo = UnityEditor.PackageManager.PackageInfo;

namespace Kinetix.PackageManager.Editor
{
    [InitializeOnLoad]
    public static class KinetixPackageManagerUpdater
    {
        /// <summary>
        /// Bool reference if there is a new package manager available
        /// </summary>
        public static bool? HasNewVersionPackageManager { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        static KinetixPackageManagerUpdater()
        {
            Kinetix.Editor.EditorUtility.OnEditorOpened += OnEditorOpened;
        }

        /// <summary>
        /// Called when editor has opened
        /// </summary>
        private static void OnEditorOpened()
        {
            Kinetix.Editor.EditorUtility.OnEditorOpened -= OnEditorOpened;
            
            CheckNewPackageManager(() =>
            {
                if (HasNewVersionPackageManager is true)
                {
                    KinetixPackageManagerEditorWindow.ShowWindowMenu();
                }
            });
        }
        
        /// <summary>
        /// Check if a new version of package manager is available
        /// </summary>
        public static async void CheckNewPackageManager(Action _Callback = null)
        {
            EditorUtility.DisplayProgressBar("Kinetix", "Checking new Updates...", 0);
            HasNewVersionPackageManager = false;

            try
            {
                if (!UnityPackageManagerUtility.PackageExists(KinetixPackageManagerConstants.PACKAGE_MANAGER_DOMAIN))
                    throw new Exception("Kinetix Package Manager doesn't exists in Unity Package Manager");
                
                PackageInfo packageInfo    = UnityPackageManagerUtility.GetPackageInfo(KinetixPackageManagerConstants.PACKAGE_MANAGER_DOMAIN);
                string      releasesURL    = GithubUtility.GetReleasesURL(packageInfo);
                Version     currentVersion = Version.Parse(packageInfo.version);

                Version latestVersion = await GetLatestVersion(packageInfo.name, releasesURL);

                if (latestVersion != null && latestVersion > currentVersion)
                {
                    HasNewVersionPackageManager = true;
                }
            }
            catch (Exception e)
            {
                Debug.LogWarning(e.Message);
            }

            EditorUtility.ClearProgressBar();
            await KinetixModulesUpdater.AsyncRefreshAllBundles();
            _Callback?.Invoke();
        }

        /// <summary>
        /// Update the package manager with the latest version available
        /// </summary>
        public static async void UpdatePackageManager()
        {
            EditorUtility.DisplayProgressBar("Kinetix", "Updating Package Manager...", 0);

            try
            {
                KinetixModuleInfo kinetixModuleInfo = KinetixModules.PackageManager;

                PackageInfo packageInfo           = UnityPackageManagerUtility.GetPackageInfo(KinetixPackageManagerConstants.PACKAGE_MANAGER_DOMAIN);
                string      releaseURL            = GithubUtility.GetReleasesURL(packageInfo);
                Version     latestManifestVersion = await GetLatestVersion(packageInfo.name, releaseURL);
                string      identifier            = GithubUtility.GetPackageURL(kinetixModuleInfo.GitURL, latestManifestVersion, KinetixPackageManagerConstants.IsProduction);

                EditorUtility.ClearProgressBar();
                AddRequest addRequest = Client.Add(identifier);
                while (!addRequest.IsCompleted)
                    Thread.Sleep(10);

                if (addRequest.Error != null)
                {
                    throw new Exception($"Failed to update package {packageInfo.name} releases. Error: {addRequest.Error.message}");
                }

                AssetDatabase.Refresh();
                CompilationPipeline.RequestScriptCompilation();
                await KinetixModulesUpdater.AsyncRefreshAllBundles();
            }
            catch (Exception e)
            {
                Debug.LogWarning("[KINETIX] " + e.Message);
            }
            
            HasNewVersionPackageManager = false;
        }

        /// <summary>
        /// Get Latest version of the package
        /// </summary>
        /// <param name="_PackageName">Name of the package</param>
        /// <param name="_GitReleaseURL">Get URL of the Github releases</param>
        /// <returns>Latest version available</returns>
        private static async Task<Version> GetLatestVersion(string _PackageName, string _GitReleaseURL)
        {
            UnityWebRequest               request = UnityWebRequest.Get(_GitReleaseURL);
            UnityWebRequestAsyncOperation op      = request.SendWebRequest();

            while (!op.isDone)
                await Task.Yield();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string    response = request.downloadHandler.text;
                Release[] releases = JsonConvert.DeserializeObject<Release[]>(response);
                Version[] versions;

                if (KinetixPackageManagerConstants.IsProduction)
                    versions = releases!.ToList().FindAll(r => !r.PreRelease).Select(r => new Version(r.Tag.Substring(1).Split('-')[0])).ToArray();
                else
                    versions = releases!.ToList().FindAll(r => r.PreRelease).Select(r => new Version(r.Tag.Substring(1).Split('-')[0])).ToArray();

                Version   latestVersion = versions.Max();
                return latestVersion;
            }

            throw new Exception($"Failed to check new Kinetix {_PackageName} releases. Error: {request.error} ");
        }
    }
}
