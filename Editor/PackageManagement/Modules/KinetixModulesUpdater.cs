using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.Compilation;
using UnityEngine;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using PackageInfo = UnityEditor.PackageManager.PackageInfo;

namespace Kinetix.PackageManager.Editor
{
    public enum EModuleState
    {
        UNREGISTERED,
        UPDATABLE,
        UPTODATE
    }
    
    public static class KinetixModulesUpdater
    {
        private static Dictionary<string, EModuleState> modulesStates;
        
        public static async Task AsyncRefreshAllBundles()
        {
            modulesStates ??= new Dictionary<string, EModuleState>();
            await CheckModulesUpToDate(KinetixModules.CoreBundleWeb2.Modules);
        }
        
        private static async void RefreshAllBundles()
        {
            await AsyncRefreshAllBundles();
        }
        
        public static bool IsBundleRegistered(KinetixBundleInfo _KinetixBundleInfo)
        {
            if (modulesStates == null)
                RefreshAllBundles();
            
            return modulesStates != null && _KinetixBundleInfo.Modules.All(IsModuleRegistered);
        }
        
        public static bool IsBundleUpToDate(KinetixBundleInfo _KinetixBundleInfo)
        {
            if (modulesStates == null)
                RefreshAllBundles();
            
            return modulesStates != null && _KinetixBundleInfo.Modules.All(IsModuleUpToDate);
        }
        
        public static bool IsModuleRegistered(KinetixModuleInfo _KinetixModuleInfo)
        {
            if (modulesStates == null)
                RefreshAllBundles();
            
            return modulesStates != null && (modulesStates.ContainsKey(_KinetixModuleInfo.Name) && modulesStates[_KinetixModuleInfo.Name] != EModuleState.UNREGISTERED);
        }
        
        public static bool IsModuleUpToDate(KinetixModuleInfo _KinetixModuleInfo)
        {
            if (modulesStates == null)
                RefreshAllBundles();
            return modulesStates != null && (modulesStates.ContainsKey(_KinetixModuleInfo.Name) && modulesStates[_KinetixModuleInfo.Name] == EModuleState.UPTODATE);
        }

        /// <summary>
        /// Update bundle containing multiple modules
        /// </summary>
        /// <param name="_KinetixBundleInfo">Information of the bundle and modules</param>
        /// <param name="_Callback">Callback on end async method</param>
        public static async void UpdateBundle(KinetixBundleInfo _KinetixBundleInfo, Action _Callback = null)
        {
            await InternalUpdateModules(_KinetixBundleInfo.Modules, _Callback);
        }
        
        /// <summary>
        /// Updating multiple modules
        /// </summary>
        /// <param name="_KinetixModuleInfos">Information of the modules</param>
        /// <param name="_Callback">Callback on end async method</param>
        public static async void UpdateModules(KinetixModuleInfo[] _KinetixModuleInfos, Action _Callback = null)
        {
            await InternalUpdateModules(_KinetixModuleInfos, _Callback);
        }

        public static async Task InternalUpdateModules(KinetixModuleInfo[] _KinetixModuleInfos, Action _Callback = null)
        {
            UnityEditor.EditorUtility.DisplayProgressBar("Kinetix Package Manager", "Installing modules...", 0);
            for (int i = 0; i < _KinetixModuleInfos.Length; i++)
            {
                try
                {
                    await InternalUpdateModule(_KinetixModuleInfos[i]);
                }
                catch (Exception e)
                {
                    Debug.LogWarning("[KINETIX] " + e.Message);
                }
            }
            
            EditorUtility.ClearProgressBar();
            await AsyncRefreshAllBundles();
    
            AssetDatabase.Refresh();

            CompilationPipeline.RequestScriptCompilation();
            Thread.Sleep(10);

            Debug.Log("Kinetix package installed successfully");
            _Callback?.Invoke();
        }

        public static async void RemoveModules(KinetixModuleInfo[] _KinetixModuleInfos, Action _Callback = null)
        {
            UnityEditor.EditorUtility.DisplayProgressBar("Kinetix Package Manager", "Removing modules...", 0);
            for (int i = 0; i < _KinetixModuleInfos.Length; i++)
            {
                try
                {
                    await InternalRemoveModule(_KinetixModuleInfos[i]);
                }
                catch (Exception e)
                {
                    Debug.LogWarning("[KINETIX] " + e.Message);
                }
            }
            
            EditorUtility.ClearProgressBar();
            await AsyncRefreshAllBundles();
            AssetDatabase.Refresh();
            CompilationPipeline.RequestScriptCompilation();
            Thread.Sleep(10);
            _Callback?.Invoke();
        }
        
        /// <summary>
        /// Internal method to update module and add it to Unity Package Manager
        /// </summary>
        /// <param name="_KinetixModuleInfo">Information of the module</param>
        private static Task InternalUpdateModule(KinetixModuleInfo _KinetixModuleInfo)
        {
            Version version    = Version.Parse(_KinetixModuleInfo.Version);
            string  identifier = GithubUtility.GetPackageURL(_KinetixModuleInfo.GitURL, version, KinetixPackageManagerConstants.IsProduction);

            AddRequest addRequest = Client.Add(identifier);
            while (!addRequest.IsCompleted)
                Thread.Sleep(10);
            
            if (addRequest.Error != null)
            {
                throw new Exception($"Failed to update package {_KinetixModuleInfo.Name} releases. Error: {addRequest.Error.message}");
            }

            if (modulesStates.ContainsKey(_KinetixModuleInfo.Name))
                modulesStates[_KinetixModuleInfo.Name] = EModuleState.UPTODATE;
            else
                modulesStates.Add(_KinetixModuleInfo.Name, EModuleState.UPTODATE);
            return Task.CompletedTask;
        }

        private static Task InternalRemoveModule(KinetixModuleInfo _KinetixModuleInfo)
        {
            RemoveRequest removeRequest = Client.Remove(_KinetixModuleInfo.Name);
            while (!removeRequest.IsCompleted)
                Thread.Sleep(10);

            if (removeRequest.Error != null)
            {
                throw new Exception($"Failed to remove package {_KinetixModuleInfo.Name}. Error: {removeRequest.Error.message}");
            }

            if (modulesStates.ContainsKey(_KinetixModuleInfo.Name))
                modulesStates[_KinetixModuleInfo.Name] = EModuleState.UNREGISTERED;
            else
                modulesStates.Add(_KinetixModuleInfo.Name, EModuleState.UNREGISTERED);
            return Task.CompletedTask;
        }
        
        private static async Task CheckModulesUpToDate(KinetixModuleInfo[] _KinetixModuleInfos)
        {
            for (int i = 0; i < _KinetixModuleInfos.Length; i++)
            {
                if (!modulesStates.ContainsKey(_KinetixModuleInfos[i].Name))
                {
                    EModuleState moduleState = await InternalIsModuleUpToDate(_KinetixModuleInfos[i]);
                    if (modulesStates.ContainsKey(_KinetixModuleInfos[i].Name))
                        modulesStates[_KinetixModuleInfos[i].Name] = moduleState;
                    else
                        modulesStates.Add(_KinetixModuleInfos[i].Name, moduleState);
                }
            }
        }
        
        private static async Task<EModuleState> InternalIsModuleUpToDate(KinetixModuleInfo _KinetixModuleInfo)
        {
            ListRequest listRequest = Client.List(true);
            while (!listRequest.IsCompleted)
                Thread.Sleep(10);

            if (listRequest.Error != null)
                throw new Exception("[KINETIX] Couldn't list registered packages");

            if (listRequest.Result.ToList().Exists(package => package.name == _KinetixModuleInfo.Name))
            {
                PackageInfo packageInfo = listRequest.Result.ToList().First(package => package.name == _KinetixModuleInfo.Name);
                bool        upToDate    = await Task.FromResult(Version.Parse(packageInfo.version) >= Version.Parse(_KinetixModuleInfo.Version));
                return upToDate ? EModuleState.UPTODATE : EModuleState.UPDATABLE;
            }

            return await Task.FromResult(EModuleState.UNREGISTERED);
        }
    }
}
