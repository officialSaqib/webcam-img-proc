using Mono.Options;

namespace WebcamImgProc.CLI
{
    /// <summary>
    /// General helpers used by command-line program.
    /// </summary>
    internal static class Helpers
    {
        /// <summary>
        /// Prints a help message to the console output.
        /// </summary>
        /// <param name="options">The options (acceptable arguments) for the application.</param>
        public static void PrintHelp(OptionSet options)
        {
            string execStr = Path.GetFileNameWithoutExtension(Environment.GetCommandLineArgs()[0]);

            Console.WriteLine($"Usage: {execStr} [Arguments]\r\n\r\nArguments:");
            options.WriteOptionDescriptions(Console.Out);
        }
    }
}
