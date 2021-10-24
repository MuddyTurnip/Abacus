﻿using System.Diagnostics;

namespace MuddyTurnip.RulesEngine
{
    [DebuggerDisplay("{Type}")]
    public class CommentBoundary
    {
        public int Index { get; set; }
        public string Comment { get; set; }

        public CommentBoundary(
            int index,
            string comment)
        {
            Index = index;
            Comment = comment;
        }
    }
}