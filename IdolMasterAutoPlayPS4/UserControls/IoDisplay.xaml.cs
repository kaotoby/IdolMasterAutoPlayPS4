using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
    /// Interaction logic for IoDisplay.xaml
    /// </summary>
    public partial class IoDisplay : UserControl
    {
        public static readonly DependencyProperty ValueNameProperty = DependencyProperty.Register("ValueName", typeof(string), typeof(IoDisplay), new PropertyMetadata(""));
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(int), typeof(IoDisplay), new PropertyMetadata(0));

        public IoDisplay() {
            InitializeComponent();
            DataContext = this;
        }

        public string ValueName
        {
            get { return (string)GetValue(ValueNameProperty); }
            set { SetValue(ValueNameProperty, value); }
        }

        public int Value
        {
            get { return (int)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }
    }
}
