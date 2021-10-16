using System;

namespace MuddyTurnip.Metrics.Engine
{
    public class TagCounter
    {
        public string Tag { get; set; } = String.Empty;
        public int Count { get; set; }

        public TagCounter()
        { }

        public TagCounter(
            string tag,
            int count)
        {
            Tag = tag;
            Count = count;
        }
    }
}
