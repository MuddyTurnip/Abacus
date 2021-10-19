using Newtonsoft.Json;

namespace MuddyTurnip.RulesEngine
{
    internal class Pattern
    {
        [JsonProperty(PropertyName = "pattern")]
        public string? RegexPattern { get; set; }

        [JsonProperty(PropertyName = "reject")]
        public string? RejectMatchRegexPattern { get; set; }
    }
}