using StardewModdingAPI;

namespace RuneMagic
{
    public class MyManifestContentPackFor : IManifestContentPackFor
    {
        public string UniqueID { get; set; }

        public ISemanticVersion MinimumVersion { get; set; }
    }
}