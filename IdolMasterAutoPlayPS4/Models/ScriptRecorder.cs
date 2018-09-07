using LibCronusMAX;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdolMasterAutoPlayPS4.Models
{
    public class ScriptRecordedEventEventArgs : EventArgs
    {
        public string RecordedScript { get; private set; }
        public ScriptRecordedEventEventArgs(string script) {
            RecordedScript = script;
        }
    }
    public class RecorderStatusChangedEventArgs : EventArgs
    {
        public bool IsRunning { get; private set; }
        public RecorderStatusChangedEventArgs(bool isRunning) {
            IsRunning = isRunning;
        }
    }

    public class ScriptRecorder
    {
        #region Define

        public event ScriptRecordedEventHandler ScriptRecorded;
        public event RecorderStatusChangedEventHandler RecorderStatusChanged;

        public delegate void ScriptRecordedEventHandler(object sender, ScriptRecordedEventEventArgs args);
        public delegate void RecorderStatusChangedEventHandler(object sender, RecorderStatusChangedEventArgs args);
        
        private static readonly ScriptRecorder _current = new ScriptRecorder();
        public static ScriptRecorder Current { get { return _current; } }

        private static PS4Button[] trackButtons = {
            PS4Button.Circle,
            PS4Button.Triangle,
            PS4Button.Square,
            PS4Button.Cross,
            PS4Button.Up,
            PS4Button.Down,
            PS4Button.Left,
            PS4Button.Right,
            PS4Button.Options,
            PS4Button.Touch
        };

        private IOStatus prevIOStatus = null;
        private bool touchTracking = false;
        private int touchDelayNext, touchPX, touchPY;
        private long elapsedMs;
        private readonly Stopwatch timer = new Stopwatch();

        private bool _isRunning = false;
        public bool IsRunning {
            get { return _isRunning; }
            private set {
                if (_isRunning != value) {
                    _isRunning = value;
                    RecorderStatusChanged.Invoke(this, new RecorderStatusChangedEventArgs(value));
                }
            }
        }

        #endregion

        public void Start() {
            prevIOStatus = null;
            touchTracking = false;
            IsRunning = true;
            Device.IOStatusChanged += IoStatusChanged;
        }

        public void Stop() {
            Device.IOStatusChanged -= IoStatusChanged;
            timer.Stop();
            IsRunning = false;
        }

        private void IoStatusChanged(object sender, IOStatus e) {
            if (prevIOStatus == null) {
                prevIOStatus = e;
                return;
            }
            string script = GenerateScript(e);
            if (script != null) {
                ScriptRecorded.Invoke(this, new ScriptRecordedEventEventArgs(script));
            }
            prevIOStatus = e;
        }

        private string GenerateScript(IOStatus status) {
            List<ScriptCommand> cmds = new List<ScriptCommand>();
            int[] prv = prevIOStatus.OutputStatus.Outputs;
            int[] cur = status.OutputStatus.Outputs;
            long delay = timer.ElapsedMilliseconds - elapsedMs;

            // Track Buttons
            foreach (var button in trackButtons) {
                if (prv[button] != cur[button]) {
                    cmds.Add(new ScriptCommand(button, cur[button]));
                }
            }

            // Track Touch
            if (touchTracking) {
                if (cmds.Count > 0) {
                    touchTracking = false;
                    cmds.Insert(0, new ScriptCommand("touch", 0));
                } else if (delay >= touchDelayNext) {
                    if (touchPX == cur[PS4Button.TouchX] &&
                        touchPY == cur[PS4Button.TouchY]) {
                        touchTracking = false;
                        cmds.Add(new ScriptCommand("touch", 0));
                    } else {
                        touchPX = cur[PS4Button.TouchX];
                        touchPY = cur[PS4Button.TouchY];
                        touchDelayNext += 200;
                    }
                }
            } else if (cmds.Count == 0 && cur[PS4Button.Touch] == 0) {
                if (prv[PS4Button.TouchX] != cur[PS4Button.TouchX] ||
                    prv[PS4Button.TouchY] != cur[PS4Button.TouchY]) {
                    touchTracking = true;
                    touchPX = cur[PS4Button.TouchX];
                    touchPY = cur[PS4Button.TouchY];
                    touchDelayNext = 200;
                    cmds.Add(new ScriptCommand("touch", 100));
                }
            }

            // Write Script
            if (cmds.Count > 0) {
                if (timer.IsRunning) {
                    cmds.Insert(0, new ScriptCommand("delay", (int)delay));
                    elapsedMs += delay;
                } else {
                    timer.Restart();
                    elapsedMs = 0;
                }
            }
            if (cmds.Count > 0) {
                return string.Join("\r\n", cmds) + "\r\n";
            } else {
                return null;
            }
        }
    }
}
