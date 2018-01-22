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
        private static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(NetworkViewModel), typeof(MainMenu));
        private NetworkViewModel ViewModel
        {
            get { return GetValue(ViewModelProperty) as NetworkViewModel; }
            set { SetValue(ViewModelProperty, value); }
        }

        public MainMenu()
        {
            ViewModel = MainWindow.ViewModel;
            InitializeComponent();
        }

        private void miNetwork_Learn_OnClick(object sender, RoutedEventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                fbd.Description = "Wybierz katalog z danymi uczącymi";
                if (fbd.ShowDialog() != DialogResult.OK || string.IsNullOrWhiteSpace(fbd.SelectedPath)) return;
                var leariningData = ImageHelper.
                    LoadAllFromCatalog(fbd.SelectedPath);
                ViewModel.HopfieldNetwork.Train(leariningData);
                ViewModel.NetworkTrained = true;
            }
        }

        private void miNetworkNew_OnClick(object sender, RoutedEventArgs e)
        {
            ViewModel.HopfieldNetwork = new HopfieldNetwork();
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
            ViewModel.ImageToRecognizePath = fileChoseDialog.FileName;
        }
    }
}
