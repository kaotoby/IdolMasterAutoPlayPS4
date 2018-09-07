using IdolMasterAutoPlayPS4.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
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
    /// Interaction logic for SongSelector.xaml
    /// </summary>
    public partial class SongSelector : UserControl
    {
        public SongSelector() {
            InitializeComponent();
        }
    }

    public class SongBorderColorValueConveter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            string type = value.ToString();
            if (type == "Quintet") {
                return "LimeGreen";
            } else if (type == "Medley") {
                return "Orange";
            } else {
                return "DodgerBlue";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
