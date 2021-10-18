using Microsoft.ApplicationInspector.Commands;
using MuddyTurnip.RulesEngine;
using MuddyTurnip.RulesEngine.Commands;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace MuddyTurnip.Metrics.Engine
{
    public class AbacusRecord
    {
        [JsonProperty(PropertyName = "file")]
        public FileRecord File { get; set; }

        [JsonProperty(PropertyName = "metrics")]
        public MetricsRecord Metrics { get; set; } = new();

        public AbacusRecord(FileRecord file)
        {
            File = file;
        }
    }
}
