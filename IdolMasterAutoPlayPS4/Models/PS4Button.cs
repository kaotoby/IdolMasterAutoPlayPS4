using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdolMasterAutoPlayPS4.Models
{
    public class PS4Button
    {
        private int _optCode;
        private string _scriptCode;
        public bool IsRangeValue { get; private set; }

        private PS4Button(int optCode, string scriptCode, bool rangeValue = false) {
            _optCode = optCode;
            _scriptCode = scriptCode;
        }
        public static readonly PS4Button PS = new PS4Button(0, "");
        public static readonly PS4Button Share = new PS4Button(1, "");
        public static readonly PS4Button Options = new PS4Button(2, "option");
        public static readonly PS4Button R1 = new PS4Button(3, "");
        public static readonly PS4Button R2 = new PS4Button(4, "", true);
        public static readonly PS4Button R3 = new PS4Button(5, "");
        public static readonly PS4Button L1 = new PS4Button(6, "");
        public static readonly PS4Button L2 = new PS4Button(7, "", true);
        public static readonly PS4Button L3 = new PS4Button(8, "");
        public static readonly PS4Button RX = new PS4Button(9, "", true);
        public static readonly PS4Button RY = new PS4Button(10, "", true);
        public static readonly PS4Button LX = new PS4Button(11, "", true);
        public static readonly PS4Button LY = new PS4Button(12, "", true);
        public static readonly PS4Button Up = new PS4Button(13, "up");
        public static readonly PS4Button Down = new PS4Button(14, "dn");
        public static readonly PS4Button Left = new PS4Button(15, "lf");
        public static readonly PS4Button Right = new PS4Button(16, "rt");
        public static readonly PS4Button Triangle = new PS4Button(17, "tri");
        public static readonly PS4Button Circle = new PS4Button(18, "cir");
        public static readonly PS4Button Cross = new PS4Button(19, "crx");
        public static readonly PS4Button Square = new PS4Button(20, "sqr");
        public static readonly PS4Button GyroX = new PS4Button(21, "", true);
        public static readonly PS4Button GyroY = new PS4Button(22, "", true);
        public static readonly PS4Button GyroZ = new PS4Button(23, "", true);
        public static readonly PS4Button AccX = new PS4Button(24, "", true);
        public static readonly PS4Button AccY = new PS4Button(25, "", true);
        public static readonly PS4Button AccZ = new PS4Button(26, "", true);
        public static readonly PS4Button Touch = new PS4Button(27, "star");
        public static readonly PS4Button TouchX = new PS4Button(28, "", true);
        public static readonly PS4Button TouchY = new PS4Button(29, "", true);
        public static readonly PS4Button TraceOne = new PS4Button(30, "");
        public static readonly PS4Button TraceOneX = new PS4Button(31, "", true);
        public static readonly PS4Button TraceOneY = new PS4Button(32, "", true);
        public static readonly PS4Button TraceTwo = new PS4Button(33, "");
        public static readonly PS4Button TraceTwoX = new PS4Button(34, "", true);
        public static readonly PS4Button TraceTwoY = new PS4Button(35, "", true);

        public static implicit operator int(PS4Button button) {
            return button._optCode;
        }

        public static implicit operator string(PS4Button button) {
            return button._scriptCode;
        }

        public static PS4Button Parse(string str) {
            if (str == PS4Button.Touch) {
                return PS4Button.Touch;
            } else if (str == PS4Button.Circle) {
                return PS4Button.Circle;
            } else if (str == PS4Button.Triangle) {
                return PS4Button.Triangle;
            } else if (str == PS4Button.Square) {
                return PS4Button.Square;
            } else if (str == PS4Button.Cross) {
                return PS4Button.Cross;
            } else if (str == PS4Button.Up) {
                return PS4Button.Up;
            } else if (str == PS4Button.Down) {
                return PS4Button.Down;
            } else if (str == PS4Button.Left) {
                return PS4Button.Left;
            } else if (str == PS4Button.Right) {
                return PS4Button.Right;
            } else if (str == PS4Button.Options) {
                return PS4Button.Options;
            } else {
                return null;
            }
        }
    }
}
