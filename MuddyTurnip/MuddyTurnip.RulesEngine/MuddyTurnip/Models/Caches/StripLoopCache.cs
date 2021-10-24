using Microsoft.ApplicationInspector.RulesEngine;
using MuddyTurnip.RulesEngine;
using MuddyTurnip.RulesEngine.Commands;
using System.Collections.Generic;

namespace MuddyTurnip.Metrics.Engine
{
    public class StripLoopCache
    {
        public BlockCommentStripLoopCache BlockCommentCache { get; }
        public InlineCommentStripLoopCache InlineCommentCache { get; }
        public StringStripLoopCache StringCache { get; }
        public PreProcessorStripLoopCache PreProcessorCache { get; }


        public StripLoopCache(
            BlockCommentStripLoopCache blockCommentCache,
            InlineCommentStripLoopCache inlineCommentCache,
            StringStripLoopCache stringCache,
            PreProcessorStripLoopCache preProcessorCache)
        {
            BlockCommentCache = blockCommentCache;
            InlineCommentCache = inlineCommentCache;
            StringCache = stringCache;
            PreProcessorCache = preProcessorCache;
        }
    }
}
