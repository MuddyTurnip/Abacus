﻿
using System.Collections.Generic;

namespace MuddyTurnip.RulesEngine
{
    public class GroupSearchSettings
    {
        public List<BlockBoundarySettings> BlockSettings { get; }
        public List<StatementBoundarySettings> StatementSettings { get; }

        public GroupSearchSettings(
            List<BlockBoundarySettings> blockSettings,
            List<StatementBoundarySettings> statementSettings)
        {
            BlockSettings = blockSettings;
            StatementSettings = statementSettings;
        }
    }
}