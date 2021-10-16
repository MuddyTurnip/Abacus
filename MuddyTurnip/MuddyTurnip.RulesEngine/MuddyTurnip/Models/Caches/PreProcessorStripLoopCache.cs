﻿using Microsoft.ApplicationInspector.RulesEngine;
using System.Collections.Generic;
using System.Text;

namespace MuddyTurnip.Metrics.Engine
{
    public class PreProcessorStripLoopCache
    {
        public List<Boundary> InputBoundaries { get; }
        public List<Boundary> OutputBoundaries { get; }
        public StringBuilder PreProcessorContent { get; }
        public int InputCounter { get; set; }
        public int InputAdjustment { get; set; }
        public int OutputAdjustment { get; set; }
        public string[] PreProcessors { get; }
        public char[] PreProcessorFirstChars { get; }

        public PreProcessorStripLoopCache(
            List<Boundary> inputBoundaries,
            List<Boundary> outputBoundaries,
            StringBuilder preProcessorContent,
            string[] preProcessors)
        {
            InputBoundaries = inputBoundaries;
            OutputBoundaries = outputBoundaries;
            PreProcessorContent = preProcessorContent;
            PreProcessors = preProcessors;

            List<char> preProcessorFirstChars = new();

            for (int i = 0; i < preProcessors.Length; i++)
            {
                if (!preProcessorFirstChars.Contains(preProcessors[i][0]))
                {
                    preProcessorFirstChars.Add(preProcessors[i][0]);
                }
            }

            PreProcessorFirstChars = preProcessorFirstChars.ToArray();
        }
    }
}
