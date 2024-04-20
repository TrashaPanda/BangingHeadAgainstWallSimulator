using Godot;

namespace Utility
{
    internal static class SystemUtility
    {
        public static int GetProcessId => OS.GetProcessId();
    }
}
