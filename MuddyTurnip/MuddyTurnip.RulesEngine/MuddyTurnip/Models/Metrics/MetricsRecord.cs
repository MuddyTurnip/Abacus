using MuddyTurnip.RulesEngine;
using MuddyTurnip.RulesEngine.Commands;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace MuddyTurnip.Metrics.Engine
{
    public class MetricsRecord
    {
        //[JsonProperty(PropertyName = "blocks")]
        [JsonIgnore]
        public List<BlockStats> Blocks { get; set; } = new();

        [JsonIgnore]
        public List<MtMatchRecord> Matches { get; set; } = new();

        [JsonProperty(PropertyName = "structure")]
        public MetricsBlock Structure { get; set; }

        public MetricsRecord()
        {
            Structure = new()
            {
                FullName = "File"
            };
        }
    }
}
