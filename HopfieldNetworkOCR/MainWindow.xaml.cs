using System.ComponentModel;
using System.Windows;
using System.Windows.Threading;
using HopfieldNetworkOCR.Core.Model;
using HopfieldNetworkOCR.Helpers;
using HopfieldNetworkOCR.ViewModels;

namespace HopfieldNetworkOCR
{
    // http://www.altcontroldelete.pl/artykuly/wpf-tutorial-tworzenie-kontrolek-uzytkownika/
    // http://www.wpf-tutorial.com/
    public partial class MainWindow
    {
        public static NetworkViewModel Model { get; private set; }

        public MainWindow()
        {
            Model = new NetworkViewModel();
            Model.PropertyChanged += model_OnPropertyChanged;
            InitializeComponent();
        }

        private void model_OnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            switch (propertyChangedEventArgs.PropertyName)
            {
                case "NetworkExists":
                    if (Model.NetworkExists)
                    {
                        pbStatus.Visibility = Visibility.Visible;
                        txtStatus.Visibility = Visibility.Visible;
                        Model.HopfieldNetwork.OnItemProcessed += HopfieldNetwork_OnOnItemProcessed;
                    }
                    break;
            }
        }

        //TODO progress przy rozpoznaniu
        private void btnRecognizeImage_OnClick(object sender, RoutedEventArgs e)
        {
            imgOutput.Source = null;

            var imageContent = ImageHelper.LoadImage(Model.ImageToRecognizePath);

            var resultImage = Model.HopfieldNetwork.GetResult(imageContent);

            imgOutput.Source = ImageHelper.BitmapToImageSource(ImageHelper.ConvertVectorToImage(resultImage));
        }

        private void HopfieldNetwork_OnOnItemProcessed(object sender, TrainEventArgs trainEventArgs)
        {
            var progresPercentage = (double) trainEventArgs.CurrentItemProcessed /
                                    (double) trainEventArgs.ItemsCount * 100.0;
            pbStatus.Dispatcher.Invoke(() => pbStatus.Value = progresPercentage, DispatcherPriority.Background);
        }
    }
}
