namespace Kinetix.PackageManager.Editor
{
    /// <summary>
    /// List of all packages related to Kinetix with last versions
    /// </summary>
    public static class KinetixModules
    {
        private static readonly string c_CORE_URL = "https://github.com/git-kinetix/";

        public static readonly KinetixModuleInfo PackageManager = new KinetixModuleInfo()
        {
            DisplayName = "Kinetix Package Manager",
            GitURL      = c_CORE_URL + "kinetix-unity-sdk-packagemanager.git"
        };

        public static readonly KinetixModuleInfo CoreWeb2Module = new KinetixModuleInfo()
        {
            DisplayName = "Kinetix Core Web2",
            Name        = "com.kinetix.coreweb2",
            GitURL      = c_CORE_URL + "kinetix-unity-sdk-coreweb2.git",
            Version     = "1.7.3"
        };
        
        public static readonly KinetixModuleInfo UICommonModule = new KinetixModuleInfo()
        {
            DisplayName = "Kinetix UI Common",
            Name        = "com.kinetix.uicommon",
            GitURL      = c_CORE_URL + "kinetix-unity-sdk-uicommon.git",
            Version     = "1.3.0"
        };
        
        public static readonly KinetixModuleInfo UIEmoteWheelModule = new KinetixModuleInfo()
        {
            DisplayName = "Kinetix UI Emote Wheel",
            Name        = "com.kinetix.uiemotewheel",
            GitURL      = c_CORE_URL + "kinetix-unity-sdk-uiemotewheel.git",
            Version     = "1.3.0"
        };

        public static readonly KinetixBundleInfo CoreBundleWeb2 = new KinetixBundleInfo()
        {
            Name = "Core Bundle",
            Modules = new[]
            {
                CoreWeb2Module
            }
        };
    }
}
