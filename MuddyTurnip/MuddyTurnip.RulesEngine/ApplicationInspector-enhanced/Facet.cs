﻿using Newtonsoft.Json;
using System.Collections.Generic;

namespace MuddyTurnip.RulesEngine
{
    internal class Facet
    {
        [JsonProperty(PropertyName = "name")]
        public string? Name { get; set; }

        [JsonProperty(PropertyName = "enabled")]
        public bool? Enabled { get; set; }

        [JsonProperty(PropertyName = "blockType")]
        public string? BlockType { get; set; }

        [JsonProperty(PropertyName = "patterns")]
        public List<Pattern> Patterns { get; set; } = new();
    }
}