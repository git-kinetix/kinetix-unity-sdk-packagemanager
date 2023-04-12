using System.Linq;
using UnityEditor;
using PackageInfo = UnityEditor.PackageManager.PackageInfo;

namespace Kinetix.PackageManager.Editor
{
    public static class UnityPackageManagerUtility
    {
        public static bool PackageExists(string _PackageDomain)
        {
            bool exists = AssetDatabase.FindAssets(KinetixPackageManagerConstants.ASSET_FILTER)
                .Select(AssetDatabase.GUIDToAssetPath)
                .ToList().Exists(x => x.Contains(KinetixPackageManagerConstants.PACKAGE_JSON) && x.Contains(_PackageDomain));
            return exists;
        }

        public static PackageInfo GetPackageInfo(string _PackageDomain)
        {
            PackageInfo packageManagerInfo = AssetDatabase.FindAssets(KinetixPackageManagerConstants.ASSET_FILTER)
                .Select(AssetDatabase.GUIDToAssetPath)
                .Where(x => x.Contains(KinetixPackageManagerConstants.PACKAGE_JSON) && x.Contains(_PackageDomain))
                .Select(PackageInfo.FindForAssetPath)
                .First();

            return packageManagerInfo;
        }
    }
}
