using Microsoft.ApplicationInspector.Commands;
using MuddyTurnip.RulesEngine;
using MuddyTurnip.RulesEngine.Commands;
using System.Collections.Generic;

namespace MuddyTurnip.Metrics.Engine
{
    public class AbacusRecord
    {
        public FileRecord File { get; set; }
        public MetricsRecord Metrics { get; set; } = new();

        public AbacusRecord(FileRecord file)
        {
            File = file;
        }
    }
}
