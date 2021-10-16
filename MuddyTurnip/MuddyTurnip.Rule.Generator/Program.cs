using System.IO;
using System.Threading.Tasks;

namespace MuddyTurnip.Rule.Generator
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string line;
            string rule;
            int counter = 1000;

            string inputPath = "C:\\Code4\\ApplicationInspector-main\\MuddyTurnip.Rule.Generator\\keywords.txt";
            string outputPath = "C:\\Code4\\ApplicationInspector-main\\MuddyTurnip.Rule.Generator\\keywordRules.json";

            if (File.Exists(@"C:\test.txt"))
            {
                File.Delete(@"C:\test.txt");
            }

            using (StreamReader input = new StreamReader(inputPath))
            using (StreamWriter output = new StreamWriter(outputPath))
            {
                await output.WriteLineAsync("[");

                while ((line = input.ReadLine()) != null)
                {
                    rule = BuildRule(
                        line,
                        counter += 100);

                    await output.WriteLineAsync(rule);
                }

                await output.WriteLineAsync("]");
            }
        }

        private static string BuildRule(
            string keyword,
            int counter)
        {
            string pattern = "00000000";

            string rule = 
$@"{{
    ""name"": ""Codemology-m: CSharp {keyword}"",
    ""id"": ""CY{counter.ToString(pattern)}"",
    ""description"": ""Codemology-m: CSharp {keyword}"",
    ""applies_to"": [ ""csharp"" ],
    ""tags"": [
      ""Codemology-m.Code.CSharp.{keyword}""
    ],
    ""severity"": ""moderate"",
    ""_comment"": """",
    ""patterns"": [
      {{
        ""pattern"": ""{keyword}"",
        ""type"": ""RegexWord"",
        ""scopes"": [ ""code"" ],
        ""confidence"": ""medium"",
        ""_comment"": """"
      }}
    ]
  }},";


            return rule;
        }
    }
}
