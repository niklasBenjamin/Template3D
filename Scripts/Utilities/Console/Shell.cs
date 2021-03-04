using Godot;
using System.Collections.Generic;

namespace Utilities.Console
{
    public static class Shell
    {
        private struct Command {
            public Object Target;
            public string Method;
            public string Description;

            public Command(Object target, string method, string description) {
                this.Target = target;
                this.Method = method;
                this.Description = description;
            }
        }

        private static Dictionary<string, Command> commands = new Dictionary<string, Command>();
        private static List<string> commandLog = new List<string>();
        private static int currentLogIndex;

        public static void RegisterCommand(string command, Object target, string method, string description) {
            string cmd = command.ToLower();

            if(commands.ContainsKey(cmd)) 
                commands[cmd] = new Command(target, method, description);
            else
                commands.Add(cmd, new Command(target, method, description));  
        }

        public static void UnregisterCommand(string command) {
            string cmd = command.ToLower();

            if(commands.ContainsKey(cmd)) 
                commands.Remove(cmd);
        }

        public static string ParseCommand(string text) {
            commandLog.Add(text);
            currentLogIndex = commandLog.Count;

            string [] input = text.Split(new char[] {' '});
            string cmd = (input.Length > 0 ? input[0] : string.Empty).ToLower();
            object arg = (input.Length > 1 ? JSON.Parse(input[1]).Result : null);

            if(commands.ContainsKey(cmd)) {
                Command command = commands[cmd];

                if(arg != null)
                    command.Target.Call(command.Method, arg);
                else
                    command.Target.Call(command.Method);

                return $"[color=#4eee91]{text}[/color]";
            }

            return $"[color=#ff7085]Command [{cmd}] not recognized[/color]";
        }

        public static string GetAutoCompleteText(string text) {
            if (!string.IsNullOrEmpty(text)) {
                string input = text.ToLower();
                string autoCompleteString = "";

                foreach (string key in commands.Keys) {
                    if (key.StartsWith(input)) {
                        autoCompleteString = key;
                        break;
                    }
                }
                return autoCompleteString;
            }
            else {
                return string.Empty;
            }
        }

        public static string GetCommandLog(bool up) {
            if(commandLog.Count == 0)
                return null;
            
            currentLogIndex = Mathf.Clamp(up ? currentLogIndex-1 : currentLogIndex+1, 0, commandLog.Count-1);
            return commandLog[currentLogIndex];
        }
    }
}
