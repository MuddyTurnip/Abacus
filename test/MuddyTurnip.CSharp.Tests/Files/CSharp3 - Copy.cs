using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace MuddyTurnip.Metrics.Engine
{
    public static class CSharp3
    {
        public static void StringAndCommentTest(
            this List<TagCounter> tagCounts,
            string tag,
            int count = 1)
        {
            // Comment 1
            string fred = "//";

            // Comment 2 //
            string joe = "////";
            // Comment 3 "//
            string joe = "////";
            /*
             2 line "
            comment // */
            string bert = @"
2 line
string";
            /*
             " */
            // Comment 4 //
            string joe = @" "" ";
            // Comment 5 //
            string joe = @" ""joe"" ";
            /* Comment 6
             2 line */

            /*

            @" 

*/        /* comment 7 */
            in ben = 0;
            /*

            " 

*/
            string ken = "ken";
            string ben = " \"ben\" "
        }
    }
}
