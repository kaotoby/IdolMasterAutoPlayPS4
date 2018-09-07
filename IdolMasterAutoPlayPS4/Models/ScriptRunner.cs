using LibCronusMAX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace IdolMasterAutoPlayPS4.Models
{
    public class RunnerStatusChangedEventArgs : EventArgs
    {
        public RunnerStatus Status { get; private set; }
        public RunnerStatusChangedEventArgs(RunnerStatus status) {
            Status = status;
        }
    }

    public class RunningLineChangedEventArgs : EventArgs
    {
        public int CommandNumber { get; private set; }
        public RunningLineChangedEventArgs(int num) {
            CommandNumber = num;
        }
    }

    public enum RunnerStatus
    {
        Stopped,
        WaitingScript,
        WaitingUser,
        Running
    }

    public class ScriptRunner
    {
        public event RunnerStatusChangedEventHandler RunnerStatusChanged;
        public event RunningLineChangedEventHandler RunningLineChanged;

        public delegate void RunnerStatusChangedEventHandler(object sender, RunnerStatusChangedEventArgs args);
        public delegate void RunningLineChangedEventHandler(object sender, RunningLineChangedEventArgs args);

        private static readonly ScriptRunner _current = new ScriptRunner();
        public static ScriptRunner Current { get { return _current; } }

        private RunnerStatus _status = RunnerStatus.Stopped;
        public RunnerStatus Status {
            get { return _status; }
            private set {
                if (_status != value) {
                    _status = value;
                    RunnerStatusChanged.Invoke(this, new RunnerStatusChangedEventArgs(value));
                }
            }
        }

        public bool IsStopped {
            get { return _status == RunnerStatus.Stopped; }
        }

        private int _runingLineNumber;
        public int RunningLineNumber {
            get { return _runingLineNumber; }
            private set {
                if (_runingLineNumber != value) {
                    _runingLineNumber = value;
                    RunningLineChanged.Invoke(this, new RunningLineChangedEventArgs(value));
                }
            }
        }

        private readonly DispatcherTimer apiTimer = new DispatcherTimer(DispatcherPriority.Normal, Dispatcher.CurrentDispatcher);
        private DateTime nextCmdTime = DateTime.Now;
        private List<ScriptCommand> scriptCommands;
        private int[] apiCmd;
        private int commandIndex = 0;
        private bool touchRunning = false;
        private readonly Random rnd = new Random();

        private ScriptRunner() {
            Device.IOStatusChanged += IoStatusChanged;
            apiTimer.Interval = TimeSpan.FromMilliseconds(10);
            apiTimer.Tick += ApiTimerCallback;
        }

        public void LoadScript(string script) {
            if (Status == RunnerStatus.Running) {
                Stop();
            }
            scriptCommands = ScriptCommand.ParseScript(script);
            if (scriptCommands.Count > 0) {
                Status = RunnerStatus.WaitingUser;
                System.Media.SystemSounds.Exclamation.Play();
            }
        }

        public void Start() {
            nextCmdTime = DateTime.Now;
            commandIndex = 0;
            _runingLineNumber = 0;
            touchRunning = false;
            apiCmd = new int[36];
            Device.EnterApiMode();
            apiTimer.Start();
            Status = RunnerStatus.Running;
        }

        public void Wait() {
            Status = RunnerStatus.WaitingScript;
        }

        public void Stop() {
            apiTimer.Stop();
            Device.ExitApiMode();
            Status = RunnerStatus.Stopped;
        }

        private void IoStatusChanged(object sender, IOStatus e) {
            int[] btn = e.OutputStatus.Outputs;
            switch (Status) {
                case RunnerStatus.Stopped:
                    if (btn[PS4Button.L3] == 100) {
                        Wait();
                    }
                    break;
                case RunnerStatus.WaitingUser:
                    if (btn[scriptCommands[0].Button] == 100) {
                        Start();
                    }
                    break;
            }
        }

        private void ApiTimerCallback(object sender, EventArgs e) {
            if (DateTime.Now >= nextCmdTime) { // Handle Button Event
                while (commandIndex < scriptCommands.Count &&
                    scriptCommands[commandIndex].Command != "delay") {

                    ScriptCommand cmd = scriptCommands[commandIndex];
                    if (cmd.Button != null) {
                        apiCmd[cmd.Button] = cmd.Value;
                    } else if (cmd.Command == "touch") {
                        touchRunning = (cmd.Value != 0);
                        if (touchRunning) {
                            apiCmd[PS4Button.TraceOne] = 1;
                            apiCmd[PS4Button.TraceOneX] = 10;
                            apiCmd[PS4Button.TraceOneY] = 10;
                        } else {
                            apiCmd[PS4Button.TraceOne] = 0;
                        }
                    }
                    commandIndex++;
                }
                CmCommand cmcmd = new CmCommand(apiCmd);
                Device.SendApiModeData(cmcmd);
                // Checking end of script
                if (commandIndex == scriptCommands.Count) {
                    Stop();
                    return;
                }
                ScriptCommand delay = scriptCommands[commandIndex];
                nextCmdTime = nextCmdTime.AddMilliseconds(delay.Value);
                RunningLineNumber = delay.LineNumber;

                commandIndex++;
            } else if (touchRunning) { // Handle Touch Event
                //if (touchRunningToRight) {
                //    apiCmd[PS4Button.TouchX] += 10;
                //    apiCmd[PS4Button.TouchY] += 10;
                //    if (apiCmd[PS4Button.TouchX] >= 90) touchRunningToRight = false;
                //} else {
                //    apiCmd[PS4Button.TouchX] -= 10;
                //    apiCmd[PS4Button.TouchY] -= 10;
                //    if (apiCmd[PS4Button.TouchX] <= -90) touchRunningToRight = true;
                //}
                apiCmd[PS4Button.TraceOneX] = rnd.Next(-100, 100);
                apiCmd[PS4Button.TraceOneY] = rnd.Next(-100, 100);
                CmCommand cmcmd = new CmCommand(apiCmd);
                Device.SendApiModeData(cmcmd);
            }
        }
    }
}
