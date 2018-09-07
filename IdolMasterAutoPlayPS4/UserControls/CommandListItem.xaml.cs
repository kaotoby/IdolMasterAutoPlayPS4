using System;
using System.Collections.Generic;
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
    /// Interaction logic for CommandListItem.xaml
    /// </summary>
    public partial class CommandListItem : UserControl
    {
        public string ImssSource {
            get { return (string)GetValue(ImssSourceProperty); }
            set { SetValue(ImssSourceProperty, value); }
        }
        
        public static readonly DependencyProperty ImssSourceProperty =
             DependencyProperty.Register("ImssSource", typeof(string), typeof(CommandListItem), new PropertyMetadata(false));

        public string ImageSource {
            get { return (string)GetValue(ImssSourceProperty); }
            set { SetValue(ImssSourceProperty, value); }
        }

        public static readonly DependencyProperty ImageSourceProperty =
             DependencyProperty.Register("ImageSource", typeof(string), typeof(CommandListItem), new PropertyMetadata(false));


        public CommandListItem() {
            InitializeComponent();
        }
    }
}
