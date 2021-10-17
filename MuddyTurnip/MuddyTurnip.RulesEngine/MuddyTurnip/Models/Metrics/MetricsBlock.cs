﻿using Microsoft.ApplicationInspector.RulesEngine;
using MuddyTurnip.RulesEngine;
using MuddyTurnip.RulesEngine.Commands;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace MuddyTurnip.Metrics.Engine
{
    public class MetricsBlock
    {
        [JsonProperty(PropertyName = "fullName")]
        public string FullName { get; set; } = string.Empty;

        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; } = string.Empty;

        [JsonProperty(PropertyName = "signature")]
        public string Signature { get; set; } = string.Empty;

        [JsonProperty(PropertyName = "content")]
        public string Content { get; set; } = string.Empty;

        [JsonProperty(PropertyName = "partial")]
        public bool Partial { get; set; } = false;

        [JsonProperty(PropertyName = "depth")]
        public int Depth { get; set; }

        [JsonProperty(PropertyName = "syblings")]
        public int SyblingCount { get; set; }

        [JsonProperty(PropertyName = "startIndex")]
        public int AdjustedOpenIndex { get; set; }

        [JsonProperty(PropertyName = "endIndex")]
        public int AdjustedCloseIndex { get; set; }

        [JsonProperty(PropertyName = "startLocation")]
        public Location BlockStartLocation { get; set; } = new();

        [JsonProperty(PropertyName = "endLocation")]
        public Location BlockEndLocation { get; set; } = new();

        [JsonIgnore]
        public List<MetricsBlock> ChildBlocks { get; set; } = new();

        [JsonIgnore]
        public MetricsBlock? Parent { get; set; }

        public MetricsBlock()
        {
        }
    }
}