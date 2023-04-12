using Newtonsoft.Json;

namespace Kinetix.PackageManager.Editor
{
    public class Release
    {
        [JsonProperty("tag_name")]   public string Tag;
        [JsonProperty("prerelease")] public bool   PreRelease;
    }
}
