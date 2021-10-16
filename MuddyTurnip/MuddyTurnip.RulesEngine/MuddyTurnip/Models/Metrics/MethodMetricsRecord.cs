using System.Collections.Generic;

namespace MuddyTurnip.Metrics.Engine
{
    public class MethodMetricsRecord
    {
        public string MethodName { get; set; }
        public List<TagCounter> TagCounts { get; } = new List<TagCounter>();

        public MethodMetricsRecord(string methodName)
        {
            MethodName = methodName;
        }
    }
}
