using Newtonsoft.Json;

namespace MuddyTurnip.RulesEngine
{
    internal class UnMask
    {

        [JsonProperty(PropertyName = "groupName")]
        public string? GroupName { get; set; }

        [JsonProperty(PropertyName = "actionName")]
        public string? ActionName { get; set; }

        [JsonProperty(PropertyName = "enabled")]
        public bool? Enabled { get; set; }
    }
}