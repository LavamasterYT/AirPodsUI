namespace AirPodsUI.Configurator
{
    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class OldTemplate
    {
        [JsonProperty("TemplateName")]
        public string TemplateName { get; set; }

        [JsonProperty("UsingImage")]
        public bool UsingImage { get; set; }

        [JsonProperty("AssetLocation")]
        public string AssetLocation { get; set; }

        [JsonProperty("UseDeviceName")]
        public bool UseDeviceName { get; set; }

        [JsonProperty("DefaultDeviceName")]
        public string DefaultDeviceName { get; set; }

        [JsonProperty("ButtonText")]
        public string ButtonText { get; set; }

        [JsonProperty("LoopAnimation")]
        public bool LoopAnimation { get; set; }

        [JsonProperty("WindowBackground")]
        public string WindowBackground { get; set; }

        [JsonProperty("WindowForeground")]
        public string WindowForeground { get; set; }

        [JsonProperty("ButtonBackground")]
        public string ButtonBackground { get; set; }

        [JsonProperty("ButtonForeground")]
        public string ButtonForeground { get; set; }

        [JsonProperty("Tint")]
        public string Tint { get; set; }
    }

    public partial class OldTemplate
    {
        public static OldTemplate FromJson(string json) => JsonConvert.DeserializeObject<OldTemplate>(json, AirPodsUI.Configurator.OTConverter.Settings);
    }

    public static class OTSerialize
    {
        public static string ToJson(this OldTemplate self) => JsonConvert.SerializeObject(self, AirPodsUI.Configurator.OTConverter.Settings);
    }

    internal static class OTConverter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }
}