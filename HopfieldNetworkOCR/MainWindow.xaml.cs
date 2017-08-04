using System;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using HopfieldNetworkOCR.UserControls;
using Microsoft.Win32;

namespace HopfieldNetworkOCR
{
    // http://www.altcontroldelete.pl/artykuly/wpf-tutorial-tworzenie-kontrolek-uzytkownika/
    public partial class MainWindow : Window
    {
        private string _imageToRecognizePath;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnRecognizeImage_OnClick(object sender, RoutedEventArgs e)
        {
            FileDialog fileChoseDialog = new OpenFileDialog
            {
                Multiselect = false,
                Filter = "Tiff files|*.tiff",
                Title = "Wybierz obraz do rozpoznania"
            };
            if (fileChoseDialog.ShowDialog() == true)
            {
                var imgPath = fileChoseDialog.FileName;
                _imageToRecognizePath = imgPath;

                imgInput.Source = new BitmapImage(new Uri(_imageToRecognizePath));

                RecognizeImage(_imageToRecognizePath);
            }
        }

        private void RecognizeImage(string imageToRecognizePath)
        {
            var imageContent = Helpers.ImageHelper.LoadImage(imageToRecognizePath);

            var leariningData = Helpers.ImageHelper.LoadAllFromCatalog(MenuTop.LearningDataCatalogPath);
            var network = new HopfieldNetworkOCR.Core.Model.HopfieldNetwork(leariningData);
            var resultImage = network.GetResult(imageContent);

            var resultPath = @"C:\Users\t.baum\Desktop\New folder\" +
                             imageToRecognizePath[imageToRecognizePath.Length - 6] + ".tiff";
            Helpers.ImageHelper.SaveImage(resultImage, resultPath, 10, 12);

            imgOutput.Source = new BitmapImage(new Uri(resultPath));
        }
    }
}
