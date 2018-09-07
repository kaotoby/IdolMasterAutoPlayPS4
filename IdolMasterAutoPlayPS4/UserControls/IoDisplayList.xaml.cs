using LibCronusMAX;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace IdolMasterAutoPlayPS4.UserControls
{
    /// <summary>
    /// Interaction logic for IoDisplayList.xaml
    /// </summary>
    public partial class IoDisplayList : UserControl
    {
        private IOStatus.ConsoleType _currentConsole;
        private IOStatus.ControllerType _currentController;
        public IoDisplayList() {
            InitializeComponent();
        }

        public void UpdateStatus(IOStatus status) {
            Console.Text = status.Console;
            Controller.Text = status.Controller;
            Led1.Text = status.Led1;
            Led2.Text = status.Led2;
            Led3.Text = status.Led3;
            Led4.Text = status.Led4;
            RumbleA.Text = status.RumbleA;
            RumbleB.Text = status.RumbleB;
            RumbleLt.Text = status.RumbleLt;
            RumbleRt.Text = status.RumbleRt;
            Slot.Text = status.Slot;
            Battery.Text = status.Battery;
            CpuLoad.Text = status.Cpuload;
            if (_currentController != status.ConnectedController) {
                _currentController = status.ConnectedController;
                var names = IOLabels.GetLabels(_currentController);
                In00.ValueName = names[00];
                In01.ValueName = names[01];
                In02.ValueName = names[02];
                In03.ValueName = names[03];
                In04.ValueName = names[04];
                In05.ValueName = names[05];
                In06.ValueName = names[06];
                In07.ValueName = names[07];
                In08.ValueName = names[08];
                In09.ValueName = names[09];
                In10.ValueName = names[10];
                In11.ValueName = names[11];
                In12.ValueName = names[12];
                In13.ValueName = names[13];
                In14.ValueName = names[14];
                In15.ValueName = names[15];
                In16.ValueName = names[16];
                In17.ValueName = names[17];
                In18.ValueName = names[18];
                In19.ValueName = names[19];
                In20.ValueName = names[20];
                In21.ValueName = names[24];
                In22.ValueName = names[25];
                In23.ValueName = names[26];
                In24.ValueName = names[21];
                In25.ValueName = names[22];
                In26.ValueName = names[23];
                In27.ValueName = names[27];
                In28.ValueName = names[28];
                In29.ValueName = names[29];
            }
            if (_currentConsole != status.ConnectedConsole) {
                _currentConsole = status.ConnectedConsole;
                var names = IOLabels.GetLabels(_currentConsole);
                Out00.ValueName = names[00];
                Out01.ValueName = names[01];
                Out02.ValueName = names[02];
                Out03.ValueName = names[03];
                Out04.ValueName = names[04];
                Out05.ValueName = names[05];
                Out06.ValueName = names[06];
                Out07.ValueName = names[07];
                Out08.ValueName = names[08];
                Out09.ValueName = names[09];
                Out10.ValueName = names[10];
                Out11.ValueName = names[11];
                Out12.ValueName = names[12];
                Out13.ValueName = names[13];
                Out14.ValueName = names[14];
                Out15.ValueName = names[15];
                Out16.ValueName = names[16];
                Out17.ValueName = names[17];
                Out18.ValueName = names[18];
                Out19.ValueName = names[19];
                Out20.ValueName = names[20];
                Out21.ValueName = names[24];
                Out22.ValueName = names[25];
                Out23.ValueName = names[26];
                Out24.ValueName = names[21];
                Out25.ValueName = names[22];
                Out26.ValueName = names[23];
                Out27.ValueName = names[27];
                Out28.ValueName = names[28];
                Out29.ValueName = names[29];
            }
            In00.Value = status.InputStatus.Input00;
            In01.Value = status.InputStatus.Input01;
            In02.Value = status.InputStatus.Input02;
            In03.Value = status.InputStatus.Input03;
            In04.Value = status.InputStatus.Input04;
            In05.Value = status.InputStatus.Input05;
            In06.Value = status.InputStatus.Input06;
            In07.Value = status.InputStatus.Input07;
            In08.Value = status.InputStatus.Input08;
            In09.Value = status.InputStatus.Input09;
            In10.Value = status.InputStatus.Input10;
            In11.Value = status.InputStatus.Input11;
            In12.Value = status.InputStatus.Input12;
            In13.Value = status.InputStatus.Input13;
            In14.Value = status.InputStatus.Input14;
            In15.Value = status.InputStatus.Input15;
            In16.Value = status.InputStatus.Input16;
            In17.Value = status.InputStatus.Input17;
            In18.Value = status.InputStatus.Input18;
            In19.Value = status.InputStatus.Input19;
            In20.Value = status.InputStatus.Input20;
            In21.Value = status.InputStatus.Input21;
            In22.Value = status.InputStatus.Input22;
            In23.Value = status.InputStatus.Input23;
            In24.Value = status.InputStatus.Input24;
            In25.Value = status.InputStatus.Input25;
            In26.Value = status.InputStatus.Input26;
            In27.Value = status.InputStatus.Input27;
            In28.Value = status.InputStatus.Input28;
            In29.Value = status.InputStatus.Input29;
            Out00.Value = status.OutputStatus.Output00;
            Out01.Value = status.OutputStatus.Output01;
            Out02.Value = status.OutputStatus.Output02;
            Out03.Value = status.OutputStatus.Output03;
            Out04.Value = status.OutputStatus.Output04;
            Out05.Value = status.OutputStatus.Output05;
            Out06.Value = status.OutputStatus.Output06;
            Out07.Value = status.OutputStatus.Output07;
            Out08.Value = status.OutputStatus.Output08;
            Out09.Value = status.OutputStatus.Output09;
            Out10.Value = status.OutputStatus.Output10;
            Out11.Value = status.OutputStatus.Output11;
            Out12.Value = status.OutputStatus.Output12;
            Out13.Value = status.OutputStatus.Output13;
            Out14.Value = status.OutputStatus.Output14;
            Out15.Value = status.OutputStatus.Output15;
            Out16.Value = status.OutputStatus.Output16;
            Out17.Value = status.OutputStatus.Output17;
            Out18.Value = status.OutputStatus.Output18;
            Out19.Value = status.OutputStatus.Output19;
            Out20.Value = status.OutputStatus.Output20;
            Out21.Value = status.OutputStatus.Output21;
            Out22.Value = status.OutputStatus.Output22;
            Out23.Value = status.OutputStatus.Output23;
            Out24.Value = status.OutputStatus.Output24;
            Out25.Value = status.OutputStatus.Output25;
            Out26.Value = status.OutputStatus.Output26;
            Out27.Value = status.OutputStatus.Output27;
            Out28.Value = status.OutputStatus.Output28;
            Out29.Value = status.OutputStatus.Output29;
            Trace1.Value = status.OutputStatus.Trace1;
            Trace2.Value = status.OutputStatus.Trace2;
            Trace3.Value = status.OutputStatus.Trace3;
            Trace4.Value = status.OutputStatus.Trace4;
            Trace5.Value = status.OutputStatus.Trace5;
            Trace6.Value = status.OutputStatus.Trace6;
#if DEBUG
            int cx = status.OutputStatus.Output28, cy = status.OutputStatus.Output29;
            if (cx != px || cy != py) {
                Debug.WriteLine("Touch: ({0},{1})", cx, cy);
                px = cx;
                py = cy;
            }
#endif
        }
#if DEBUG
        int px = 0, py = 0;
#endif
    }
}
