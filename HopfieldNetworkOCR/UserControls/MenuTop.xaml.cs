using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Forms;
using HopfieldNetworkOCR.Annotations;
using UserControl = System.Windows.Controls.UserControl;

namespace HopfieldNetworkOCR.UserControls
{
    public partial class MenuTop : UserControl
    {
        public static string LearningDataCatalogPath { get; private set; }

        public MenuTop()
        {
            InitializeComponent();
        }

        private void miNetwork_Learn_OnClick(object sender, RoutedEventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                if (fbd.ShowDialog() == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    LearningDataCatalogPath = fbd.SelectedPath;
                }
            }
        }
    }
}
