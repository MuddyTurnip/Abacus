﻿using MuddyTurnip.Metrics.Engine;

namespace MuddyTurnip.RulesEngine
{
    public interface IFileStructureInterpreter
    {
        void StructureFile(
            BlockStatsCache blockStatsCache,
            string strippedContent,
            IBoundaryCounter boundaryCounter,
            bool setBlockContent = false,
            string fullContent = "");
    }
}