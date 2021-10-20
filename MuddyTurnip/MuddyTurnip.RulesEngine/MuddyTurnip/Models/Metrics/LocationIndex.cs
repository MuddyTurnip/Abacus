using Microsoft.ApplicationInspector.Commands;
using Microsoft.ApplicationInspector.RulesEngine;
using MuddyTurnip.RulesEngine;
using MuddyTurnip.RulesEngine.Commands;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics;

namespace MuddyTurnip.Metrics.Engine
{
    [DebuggerDisplay("{Location.Line}-{Location.Column}")]
    public record LocationIndex(int Index, Location Location);
}
