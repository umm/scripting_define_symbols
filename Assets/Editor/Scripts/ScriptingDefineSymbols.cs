using System;
using System.Linq;
using UnityEditor;

namespace UnityModule
{
    [InitializeOnLoad]
    public class ScriptingDefineSymbols
    {
        private const string EnvironmentVariableKey = "SCRIPTING_DEFINE_SYMBOLS";

        static ScriptingDefineSymbols()
        {
            ApplyScriptingDefineSymbolsFromEnvironmentVariables();
        }

        private static void ApplyScriptingDefineSymbolsFromEnvironmentVariables()
        {
            var environmentVariable = Environment.GetEnvironmentVariable(EnvironmentVariableKey);
            if (string.IsNullOrEmpty(environmentVariable))
            {
                return;
            }
            var currentList = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup).Split(';').ToList();
            environmentVariable
                .Split(';')
                .Where(x => !string.IsNullOrEmpty(x) && !currentList.Contains(x))
                .Distinct()
                .ToList()
                .ForEach(x => currentList.Add(x));
            PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup, string.Join(";", currentList));
        }

        public static void Add(string symbol)
        {
            if (string.IsNullOrEmpty(symbol))
            {
                return;
            }
            var currentList = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup).Split(';').ToList();
            if (!currentList.Contains(symbol))
            {
                currentList.Add(symbol);
            }
            PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup, string.Join(";", currentList));
        }

        public static void Remove(string symbol)
        {
            if (string.IsNullOrEmpty(symbol))
            {
                return;
            }
            var currentList = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup).Split(';').ToList();
            if (currentList.Contains(symbol))
            {
                currentList.Remove(symbol);
            }
            PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup, string.Join(";", currentList));
        }

        public static bool Exists(string symbol)
        {
            return !string.IsNullOrEmpty(symbol)
                && PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup).Split(';').Contains(symbol);
        }
    }
}