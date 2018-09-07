using ICSharpCode.AvalonEdit.Highlighting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;

namespace IdolMasterAutoPlayPS4
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e) {
            InitializeTextBox();
        }

        public void InitializeTextBox() {
            // Load our custom highlighting definition
            IHighlightingDefinition customHighlighting;
            Uri uri = new Uri("/Resources/ImssHighlighting.xshd", UriKind.Relative);
            using (Stream s = Application.GetResourceStream(uri).Stream) {
                if (s == null)
                    throw new InvalidOperationException("Could not find embedded resource");
                using (XmlReader reader = new XmlTextReader(s)) {
                    customHighlighting = ICSharpCode.AvalonEdit.Highlighting.Xshd.
                        HighlightingLoader.Load(reader, HighlightingManager.Instance);
                }
            }
            // and register it in the HighlightingManager
            HighlightingManager.Instance.RegisterHighlighting("IMSS", new string[] { ".imss" }, customHighlighting);
        }
    }
}
