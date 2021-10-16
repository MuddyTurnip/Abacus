using CommandLine;
using Microsoft.ApplicationInspector.Commands;
using MuddyTurnip.RulesEngine.Commands;
using NLog;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace MuddyTurnip.Abacus
{
    class Program
    {
        static int Main(string[] args)
        {
            Console.WriteLine("Started...");

            Stopwatch watch = new Stopwatch();
            watch.Start();

            int finalResult = (int)Utils.ExitCode.CriticalError;

            Utils.CLIExecutionContext = true;//set manually at start from CLI

            WriteOnce.Verbosity = WriteOnce.ConsoleVerbosity.Medium;

            try
            {
                var argsResult = Parser.Default.ParseArguments<MtCLIAnalyzeCmdOptions>(args)
                  .MapResult(
                    (MtCLIAnalyzeCmdOptions cliOptions) => VerifyOutputArgsRun(cliOptions),
                    errs => 1
                  );

                finalResult = argsResult;
            }
            catch (OpException)
            {
                //log, output file and console have already been written to ensure all are updated for NuGet and CLI callers
                //that may exit at different call points
            }
            catch (Exception e)
            {
                //unlogged exception so report out for CLI callers
                WriteOnce.SafeLog(e.Message + '\n' + e.StackTrace, NLog.LogLevel.Error);
            }

            //final exit msg to review log
            if (finalResult == (int)Utils.ExitCode.CriticalError)
            {
                if (!string.IsNullOrEmpty(Utils.LogFilePath))
                {
                    WriteOnce.Info(MsgHelp.FormatString(MsgHelp.ID.RUNTIME_ERROR_UNNAMED, Utils.LogFilePath), true, WriteOnce.ConsoleVerbosity.Low, false);
                }
                else
                {
                    WriteOnce.Info(MsgHelp.GetString(MsgHelp.ID.RUNTIME_ERROR_PRELOG), true, WriteOnce.ConsoleVerbosity.Medium, false);
                }
            }
            else
            {
                if (Utils.LogFilePath is not null && File.Exists(Utils.LogFilePath))
                {
                    var fileInfo = new FileInfo(Utils.LogFilePath);
                    if (fileInfo.Length > 0)
                    {
                        WriteOnce.Info(MsgHelp.FormatString(MsgHelp.ID.CMD_REMINDER_CHECK_LOG, Utils.LogFilePath ?? Utils.GetPath(Utils.AppPath.defaultLog)), true, WriteOnce.ConsoleVerbosity.Low, false);
                    }
                }
            }

            watch.Stop();
            TimeSpan buildJsonTime = watch.Elapsed;
            Console.WriteLine($"Total time = {buildJsonTime}");

            return finalResult;
        }


        private static int VerifyOutputArgsRun(MtCLIAnalyzeCmdOptions options)
        {
            Logger logger = Utils.SetupLogging(options, true);
            WriteOnce.Log = logger;
            options.Log = logger;

            //analyze with html format limit checks
            if (options.OutputFileFormat == "html")
            {
                options.OutputFilePath = options.OutputFilePath ?? "output.html";
                string extensionCheck = Path.GetExtension(options.OutputFilePath);
                if (extensionCheck != ".html" && extensionCheck != ".htm")
                {
                    WriteOnce.Info(MsgHelp.GetString(MsgHelp.ID.ANALYZE_HTML_EXTENSION));
                }

                if (options.AllowDupTags) //fix #183; duplicates results for html format is not supported which causes filedialog issues
                {
                    WriteOnce.Error(MsgHelp.GetString(MsgHelp.ID.ANALYZE_NODUPLICATES_HTML_FORMAT));
                    throw new OpException(MsgHelp.GetString(MsgHelp.ID.ANALYZE_NODUPLICATES_HTML_FORMAT));
                }

                if (options.SimpleTagsOnly) //won't work for html that expects full data for UI
                {
                    WriteOnce.Error(MsgHelp.GetString(MsgHelp.ID.ANALYZE_SIMPLETAGS_HTML_FORMAT));
                    throw new Exception(MsgHelp.GetString(MsgHelp.ID.ANALYZE_SIMPLETAGS_HTML_FORMAT));
                }
            }

            CommonOutputChecks((MtCLICommandOptions)options);

            return RunAnalyzeCommand(options);
        }

        /// <summary>
        /// Checks that either output filepath is valid or console verbosity is not visible to ensure
        /// some output can be achieved...other command specific inputs that are relevant to both CLI
        /// and NuGet callers are checked by the commands themselves
        /// </summary>
        /// <param name="options"></param>
        private static void CommonOutputChecks(MtCLICommandOptions options)
        {
            //validate requested format
            string fileFormatArg = options.OutputFileFormat;
            string[] validFormats =
            {
                "html",
                "text",
                "json"
            };

            string[] checkFormats;
            if (options is MtCLIAnalyzeCmdOptions cliAnalyzeOptions)
            {
                checkFormats = validFormats;
                fileFormatArg = cliAnalyzeOptions.OutputFileFormat;
            }
            else if (options is MtCLIPackRulesCmdOptions cliPackRulesOptions)
            {
                checkFormats = validFormats.Skip(2).Take(1).ToArray();
                fileFormatArg = cliPackRulesOptions.OutputFileFormat;
            }
            else
            {
                checkFormats = validFormats.Skip(1).Take(2).ToArray();
            }

            bool isValidFormat = checkFormats.Any(v => v.Equals(fileFormatArg.ToLower()));
            if (!isValidFormat)
            {
                WriteOnce.Error(MsgHelp.FormatString(MsgHelp.ID.CMD_INVALID_ARG_VALUE, "-f"));
                throw new OpException(MsgHelp.FormatString(MsgHelp.ID.CMD_INVALID_ARG_VALUE, "-f"));
            }

            //validate output is not empty if no file output specified
            if (string.IsNullOrEmpty(options.OutputFilePath))
            {
                if (options.ConsoleVerbosityLevel.ToLower() == "none")
                {
                    WriteOnce.Error(MsgHelp.GetString(MsgHelp.ID.CMD_NO_OUTPUT));
                    throw new Exception(MsgHelp.GetString(MsgHelp.ID.CMD_NO_OUTPUT));
                }
                else if (options.ConsoleVerbosityLevel.ToLower() == "low")
                {
                    WriteOnce.SafeLog("Verbosity set low.  Detailed output limited.", NLog.LogLevel.Info);
                }
            }
            else
            {
                ValidFileWritePath(options.OutputFilePath);
            }
        }

        private static int RunAnalyzeCommand(MtCLIAnalyzeCmdOptions cliOptions)
        {
            MtAnalyzeResult.ExitCode exitCode = MtAnalyzeResult.ExitCode.CriticalError;

            MtAnalyzeCommand command = new MtAnalyzeCommand(new MtAnalyzeOptions()
            {
                SourcePath = cliOptions.SourcePath ?? "",
                CustomRulesPath = cliOptions.CustomRulesPath ?? "",
                IgnoreDefaultRules = cliOptions.IgnoreDefaultRules,
                AllowDupTags = cliOptions.AllowDupTags,
                ConfidenceFilters = cliOptions.ConfidenceFilters,
                MatchDepth = cliOptions.MatchDepth,
                FilePathExclusions = cliOptions.FilePathExclusions,
                ConsoleVerbosityLevel = cliOptions.ConsoleVerbosityLevel,
                Log = cliOptions.Log,
                SingleThread = cliOptions.SingleThread,
                ScanUnknownTypes = cliOptions.ScanUnknownTypes
            }); ;

            MtAnalyzeResult analyzeResult = command.GetResult();
            exitCode = analyzeResult.ResultCode;
            MtResultsWriter.Write(analyzeResult, cliOptions);

            return (int)exitCode;
        }

        /// <summary>
        /// Ensure output file path can be written to
        /// </summary>
        /// <param name="filePath"></param>
        private static void ValidFileWritePath(string filePath)
        {
            try
            {
                File.WriteAllText(filePath, "");//verify ability to write to location
            }
            catch (Exception)
            {
                WriteOnce.Error(MsgHelp.FormatString(MsgHelp.ID.CMD_INVALID_FILE_OR_DIR, filePath));
                throw new OpException(MsgHelp.FormatString(MsgHelp.ID.CMD_INVALID_FILE_OR_DIR, filePath));
            }
        }
    }
}


