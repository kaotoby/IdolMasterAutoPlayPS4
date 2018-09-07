using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Microsoft.Win32;
using IdolMasterAutoPlayPS4.Models;
using System.Windows.Threading;
using System.Windows.Controls;
using System.Windows.Media;

namespace IdolMasterAutoPlayPS4.UserControls
{
    /// <summary>
    /// Interaction logic for RecorderPage.xaml
    /// </summary>
    public partial class Recorder : UserControl
    {
        private readonly ScriptRunner runner = ScriptRunner.Current;
        private readonly ScriptRecorder recorder = ScriptRecorder.Current;

        public Recorder() {
            InitializeComponent();
            runner.RunnerStatusChanged += RunnerStatusChanged;
            runner.RunningLineChanged += RunningLineChanged;
            recorder.RecorderStatusChanged += RecorderStatusChanged;
            recorder.ScriptRecorded += ScriptRecorded;
        }

        private void ScriptRecorded(object sender, ScriptRecordedEventEventArgs args) {
            Dispatcher.Invoke(new Action<string>((script) =>
            {
                scriptBox.AppendText(script);
            }), args.RecordedScript);
        }

        private void RecorderStopped(object sender, ScriptRecordedEventEventArgs args) {
            Dispatcher.Invoke(new Action<string>((script) =>
            {
                scriptBox.Text = script;
            }), args.RecordedScript);
        }

        private void RunningLineChanged(object sender, RunningLineChangedEventArgs args) {
            Dispatcher.Invoke(new Action<int>((num) =>
            {
                tbLineNum.Text = num.ToString();
                scriptBox.ScrollToLine(num);
            }), args.CommandNumber);
        }

        private void RecorderStatusChanged(object sender, RecorderStatusChangedEventArgs args) {
            Dispatcher.Invoke(new Action<bool>((isRunning) =>
            {
                if (isRunning) {
                    btnRecordStop.Content = "Stop";
                    btnRecordStop.Background = Brushes.Red;
                    btnRunStop.IsEnabled = false;
                } else {
                    btnRecordStop.Content = "●Record";
                    btnRecordStop.Background = Brushes.SkyBlue;
                    btnRunStop.IsEnabled = true;
                    scriptBox.Text = ScriptCommand.NormalizeScript(scriptBox.Text);
                }
            }), args.IsRunning);
        }

        private void RunnerStatusChanged(object sender, RunnerStatusChangedEventArgs args) {
            Dispatcher.Invoke(new Action<RunnerStatus>((status) =>
            {
                switch (status) {
                    case RunnerStatus.Stopped:
                        scriptBox.IsReadOnly = false;
                        btnRecordStop.IsEnabled = true;
                        btnRunStop.Content = "Run";
                        btnRunStop.Background = Brushes.SkyBlue;
                        break;
                    case RunnerStatus.WaitingScript:
                        runner.LoadScript(scriptBox.Text);
                        break;
                    case RunnerStatus.WaitingUser:
                        scriptBox.IsReadOnly = true;
                        btnRecordStop.IsEnabled = false;
                        btnRunStop.Background = Brushes.Yellow;
                        btnRunStop.Content = "Waiting";
                        break;
                    case RunnerStatus.Running:
                        btnRunStop.Background = Brushes.Red;
                        btnRunStop.Content = "Stop";
                        break;
                }
            }), args.Status);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
            runner.RunnerStatusChanged -= RunnerStatusChanged;
            runner.RunningLineChanged -= RunningLineChanged;
            recorder.RecorderStatusChanged -= RecorderStatusChanged;
            recorder.ScriptRecorded -= ScriptRecorded;
            if (!runner.IsStopped) runner.Stop();
            if (recorder.IsRunning) recorder.Stop();
        }

        private void btnRecordStop_Click(object sender, RoutedEventArgs e) {
            if (recorder.IsRunning) {
                recorder.Stop();
            } else {
                scriptBox.Clear();
                recorder.Start();
            }
        }

        private void btnRunStop_Click(object sender, RoutedEventArgs e) {
            if (runner.IsStopped) {
                runner.Wait();
            } else {
                runner.Stop();
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e) {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Idol Master Song Script (*.imss)|*.imss";
            sfd.DefaultExt = "imss";
            sfd.FileName = "idol-master";
            if (sfd.ShowDialog() == true) {
                try {
                    scriptBox.Save(sfd.FileName);
                } catch (Exception ex) {
                    MessageBox.Show(ex.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void btnLoad_Click(object sender, RoutedEventArgs e) {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Idol Master Song Script (*.imss)|*.imss";
            ofd.DefaultExt = "imss";
            ofd.FileName = "idol-master";
            if (ofd.ShowDialog() == true) {
                try {
                    scriptBox.Load(ofd.FileName);
                } catch (Exception ex) {
                    MessageBox.Show(ex.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
