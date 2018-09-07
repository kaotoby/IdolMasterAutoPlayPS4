using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace IdolMasterAutoPlayPS4.Models
{
    public class ScriptCommand
    {
        public string Command { get; private set; }
        public int Value { get; private set; }
        public int LineNumber { get; private set; }

        public ScriptCommand(string cmd, int value) {
            Command = cmd;
            Value = value;
        }

        public ScriptCommand(string cmd, int value, int ln) {
            Command = cmd;
            Value = value;
            LineNumber = ln;
        }

        public PS4Button Button {
            get {
                return PS4Button.Parse(Command);
            }
        }

        private static char[] splitCh = { ' ' };
        private static List<ScriptCommand> ParseScript(string script, int ln) {
            List<ScriptCommand> cmds = new List<ScriptCommand>();
            string[] strs = script.Split(splitCh, StringSplitOptions.RemoveEmptyEntries);
            string value = strs[strs.Length - 1];
            if (strs[0] == "touch" || PS4Button.Parse(strs[0]) != null) {

                if (value == "+") {
                    for (int i = 0; i < strs.Length - 1; i++) {
                        cmds.Add(new ScriptCommand(strs[i], 100, ln));
                    }
                } else if (value == "-") {
                    for (int i = 0; i < strs.Length - 1; i++) {
                        cmds.Add(new ScriptCommand(strs[i], 0, ln));
                    }
                } else {
                    int delay = int.Parse(value);
                    for (int i = 0; i < strs.Length - 1; i++) {
                        cmds.Add(new ScriptCommand(strs[i], 100, ln));
                    }
                    cmds.Add(new ScriptCommand("delay", delay, ln));
                    for (int i = 0; i < strs.Length - 1; i++) {
                        cmds.Add(new ScriptCommand(strs[i], 0, ln));
                    }
                }
            } else {
                int delay = int.Parse(value);
                cmds.Add(new ScriptCommand(strs[0], delay, ln));
            }
            return cmds;
        }

        public override int GetHashCode() {
            return Command.GetHashCode() ^ Value.GetHashCode() ^ LineNumber;
        }

        public override bool Equals(object obj) {
            if (obj is ScriptCommand && this == (ScriptCommand)obj) {
                return true;
            } else {
                return false;
            }
        }

        public override string ToString() {
            if (Command == "touch" ||
                (this.Button != null && !this.Button.IsRangeValue)) {
                if (Value == 0) {
                    return Command + " -;";
                } else {
                    return Command + " +;";
                }
            } else {
                return Command + " " + Value + ";";
            }
        }

        private static Regex commentSReg = new Regex(@"//[^\r\n]*");
        private static Regex commentBReg = new Regex(@"\\\*(.|\n)*\*\/");
        private static Regex commandReg = new Regex(@"\s*((?:(?:\w+|[+\-]) *)+);");
        public static List<ScriptCommand> ParseScript(string script) {
            List<ScriptCommand> cmds = new List<ScriptCommand>();
            try {
                script = commentBReg.Replace(script, "");
                script = commentSReg.Replace(script, "");
                var matches = commandReg.Matches(script);
                for (int i = 0; i < matches.Count; i++) {
                    cmds.AddRange(ParseScript(matches[i].Groups[1].Value, i + 1));
                }

            } catch (Exception) { }
            return cmds;
        }

        public static string NormalizeScript(string script) {
            var cmds = ParseScript(script);
            if (cmds.Count == 0) {
                return "";
            }

            List<CommandDelayPair> result = new List<CommandDelayPair>();

            for (int i = 0; i < cmds.Count; i++) {
                ScriptCommand cmd = cmds[i];
                if (cmd.Command == "delay") {
                    result.Add(new CommandDelayPair("delay", cmd.Value));
                } else if (cmd.Button != null) {
                    // Search down and Simplify
                    int[] delay = { 0, 0 };
                    bool noMore = false;
                    int count = 0;
                    string midResult = "";

                    for ( ; i < cmds.Count; i++) {
                        cmd = cmds[i];
                        if (cmd.Button != null) {
                            if (!noMore && cmd.Value == 100) {
                                midResult += cmd.Command + " ";
                                count++;
                            } else if (cmd.Value == 0) {
                                count--;
                                noMore = true;
                                if (count == 0 && delay[1] == 0) {
                                    break;
                                }
                            } else {
                                i--;
                                break;
                            }
                        } else if (cmd.Command == "delay") {
                            delay[noMore ? 1 : 0] += cmd.Value;
                        } else {
                            i--;
                            break;
                        }
                    }
                    if (midResult[midResult.Length - 1] == ' ') {
                        midResult = midResult.Substring(0, midResult.Length - 1);
                    }
                    result.Add(new CommandDelayPair(midResult, delay[0], true));
                    if (delay[1] > 0) {
                        result.Add(new CommandDelayPair("delay", delay[1]));
                    }
                } else {
                    // Search down for special command like: touch
                    if (cmd.Value == 100) {
                        int totalDelay = 0;
                        while (cmds[++i].Command == "delay") {
                            totalDelay += cmds[i].Value;
                        }
                        if (cmds[i].Command == cmd.Command && cmds[i].Value == 0) {
                            result.Add(new CommandDelayPair(cmd.Command , totalDelay));
                        }
                    }
                }
            }

            // Handle star
            int idx;
            int startIdx = 0;
            int iidx = result.FindIndex(startIdx, r => r.Command == "star");
            while (startIdx < result.Count &&
                (idx = result.FindIndex(startIdx, r => r.Command == "star")) != -1) {
                if (idx > 1) {
                    CommandDelayPair before = result[idx - 1];
                    if (before.Command == "touch") {
                        if (idx != 1 && result[idx - 2].Command == "delay") {
                            result[idx - 2].Delay += before.Delay;
                            result.RemoveAt(--idx);
                        } else {
                            before.Command = "delay";
                        }
                    }
                }
                if (idx < result.Count - 2) {
                    CommandDelayPair after = result[idx + 1];
                    if (after.Command == "delay" && result[idx + 2].Command == "touch") {
                        if (idx < result.Count - 3 && result[idx + 3].Command == "delay") {
                            after.Delay += result[idx + 2].Delay + result[idx + 3].Delay;
                            result.RemoveAt(idx + 2);
                            result.RemoveAt(idx + 2);
                        } else {
                            result.RemoveAt(idx + 1);
                            result.RemoveAt(idx + 1);
                        }
                    }
                }
                startIdx = idx + 1;
            }

            // Normalize Button to 100ms
            for (int i = 0; i < result.Count; i++) {
                CommandDelayPair cmd = result[i];
                if (cmd.Delay < 250 && cmd.IsButton) {
                    if (i == result.Count - 1) {
                        cmd.Delay = 100;
                    } else if (result[i + 1].Command == "delay") {
                        result[i + 1].Delay += cmd.Delay - 100;
                        cmd.Delay = 100;
                    }
                }
            }
            // Remove Last Delay
            if (result[result.Count - 1].Command == "delay") {
                result.RemoveAt(result.Count - 1);
            }
            return string.Join("\r\n", result);
        }

        private class CommandDelayPair
        {
            public string Command { get; set; }
            public int Delay { get; set; }
            public bool IsButton { get; private set; }

            public CommandDelayPair(string command, int delay, bool isButton = false) {
                string[] split = command.Split(' ');
                if (split.Length == 2 && split[0].Length == 2 && split[1].Length == 3) {
                    Command = split[1] + " " + split[0];
                } else {
                    Command = command;
                }
                Delay = delay;
                IsButton = isButton;
            }

            public override string ToString() {
                return Command + " " + Delay + ";";
            }
        }
    }
}