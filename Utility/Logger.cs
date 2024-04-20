using Godot;

namespace Utility
{
    internal static class Logger
    {
        private static void PrintLog(string type, string message)
        {
            if (type == "ERROR" || type == "EXCEPTION" || type == "FATAL")
            {
                GD.PrintErr($"[{type}] {SystemUtility.GetProcessId}: {message}");
                return;
            }

            GD.Print($"[{type}] {SystemUtility.GetProcessId}: {message}");
        }

        public static void Warning(string message)
        {
            PrintLog("WARNING", message);
        }

        public static void Error(string message)
        {
            PrintLog("ERROR", message);
        }

        public static void Exception(string message)
        {
            PrintLog("EXCEPTION", message);
        }

        public static void Fatal(string message)
        {
            PrintLog("FATAL", message);
        }

        public static void Debug(string message)
        {
            PrintLog("DEBUG", message);
        }
    }
}
