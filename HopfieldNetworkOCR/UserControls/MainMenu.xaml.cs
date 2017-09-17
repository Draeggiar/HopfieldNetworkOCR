using System.Windows;
using System.Windows.Forms;
using HopfieldNetworkOCR.Core.Model;
using HopfieldNetworkOCR.Helpers;
using HopfieldNetworkOCR.ViewModels;
using FileDialog = Microsoft.Win32.FileDialog;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;

namespace HopfieldNetworkOCR.UserControls
{
    public partial class MainMenu
    {
        private static readonly DependencyProperty ModelProperty =
            DependencyProperty.Register("Model", typeof(NetworkViewModel), typeof(MainMenu));
        private NetworkViewModel Model
        {
            get { return GetValue(ModelProperty) as NetworkViewModel; }
            set { SetValue(ModelProperty, value); }
        }

        public MainMenu()
        {
            Model = MainWindow.Model;
            InitializeComponent();
        }

        private void miNetwork_Learn_OnClick(object sender, RoutedEventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                fbd.Description = "Wybierz katalog z danymi uczącymi";
                if (fbd.ShowDialog() != DialogResult.OK || string.IsNullOrWhiteSpace(fbd.SelectedPath)) return;
                var leariningData = ImageHelper.LoadAllFromCatalog(fbd.SelectedPath);
                Model.HopfieldNetwork.Train(leariningData);
                Model.NetworkTrained = true;
            }
        }

        private void miNetworkNew_OnClick(object sender, RoutedEventArgs e)
        {
            Model.HopfieldNetwork = new HopfieldNetwork();
        }

        private void miChooseImage_OnClick(object sender, RoutedEventArgs e)
        {
            FileDialog fileChoseDialog = new OpenFileDialog
            {
                Multiselect = false,
                Filter = "Tiff files|*.tiff",
                Title = "Wybierz obraz do rozpoznania"
            };
            if (fileChoseDialog.ShowDialog() != true) return;
            Model.ImageToRecognizePath = fileChoseDialog.FileName;
        }
    }
}
