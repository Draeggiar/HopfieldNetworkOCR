using System;
using System.Windows;
using System.Windows.Media.Imaging;
using HopfieldNetworkOCR.Core.Model;
using HopfieldNetworkOCR.Helpers;
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
            //if (MenuTop.LearningDataCatalogPath == null)
            //{
            //    btnChoseImageToRecognize.Visibility = Visibility.Hidden;
            //    btnRecognizeImage.Visibility = Visibility.Hidden;
            //}
            //else
            //{
            //    btnChoseImageToRecognize.Visibility = Visibility.Visible;
            //    btnRecognizeImage.Visibility = Visibility.Visible;
            //}
        }

        private void btnChoseImageToRecognize_OnClick(object sender, RoutedEventArgs e)
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
            }
        }

        private void btnRecognizeImage_OnClick(object sender, RoutedEventArgs e)
        {
            if (_imageToRecognizePath != null)
                RecognizeImage(_imageToRecognizePath);
        }

        private void RecognizeImage(string imageToRecognizePath)
        {
            imgOutput.Source=null;

            var imageContent = ImageHelper.LoadImage(imageToRecognizePath);

            var leariningData = ImageHelper.LoadAllFromCatalog(MenuTop.LearningDataCatalogPath);
            var network = new HopfieldNetwork(leariningData);
            var resultImage = network.GetResult(imageContent);

            imgOutput.Source = ImageHelper.BitmapToImageSource(ImageHelper.ConvertVectorToImage(resultImage));
        }
    }
}
