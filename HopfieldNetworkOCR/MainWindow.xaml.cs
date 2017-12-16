using System;
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

            Application.Current.DispatcherUnhandledException += HandleApplicationExceptions;
        }

        private void HandleApplicationExceptions(object sender, DispatcherUnhandledExceptionEventArgs dispatcherUnhandledExceptionEventArgs)
        {
            txtOutput.Text = dispatcherUnhandledExceptionEventArgs.Exception.Message;
        }

        private void model_OnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            switch (propertyChangedEventArgs.PropertyName)
            {
                case "NetworkExists":
                    if (Model.NetworkExists)
                    {
                        txtNetworkName.Visibility = Visibility.Visible;
                        pbStatus.Visibility = Visibility.Visible;
                        txtStatus.Visibility = Visibility.Visible;
                        Model.HopfieldNetwork.OnItemProcessed += HopfieldNetwork_OnOnItemProcessed;
                    }
                    break;
                case "IsKnownCharacter":
                    Visibility visibility;
                    visibility = Model.IsKnownCharacter ? Visibility.Visible : Visibility.Hidden;

                    txtRecognizedCharLabel.Visibility = visibility;
                    txtRecognizedChar.Visibility = visibility;
                    break;
            }
        }

        //TODO progress przy rozpoznaniu
        private void btnRecognizeImage_OnClick(object sender, RoutedEventArgs e)
        {
            imgOutput.Source = null;
            txtRecognizedChar.Text = string.Empty;

            try
            {
                var imageContent = ImageHelper.LoadImage(Model.ImageToRecognizePath);
                char recognizedChar;

                var resultImage = Model.HopfieldNetwork.GetResult(imageContent);

                imgOutput.Source = ImageHelper.BitmapToImageSource(ImageHelper.ConvertVectorToImage(resultImage));

                if (Model.HopfieldNetwork.TryGetChar(resultImage, out recognizedChar))
                {
                    Model.IsKnownCharacter = true;
                    txtRecognizedChar.Text = recognizedChar.ToString();
                }
                else
                    Model.IsKnownCharacter = false;

                Model.HopfieldNetwork.ResetNetworkState();
            }
            catch (Exception ex)
            {
                txtOutput.Text = ex.Message;
            }
        }

        private void HopfieldNetwork_OnOnItemProcessed(object sender, TrainEventArgs trainEventArgs)
        {
            var progresPercentage = (double) trainEventArgs.CurrentItem /
                                    (double) trainEventArgs.ItemsCount * 100.0;
            pbStatus.Dispatcher.Invoke(() => pbStatus.Value = progresPercentage, DispatcherPriority.Background);
        }
    }
}
