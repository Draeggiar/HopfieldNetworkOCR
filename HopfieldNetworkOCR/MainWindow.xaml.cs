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
        public static NetworkViewModel ViewModel { get; private set; }

        public MainWindow()
        {
            ViewModel = new NetworkViewModel();
            ViewModel.PropertyChanged += viewModel_OnPropertyChanged;
            InitializeComponent();

            Application.Current.DispatcherUnhandledException += HandleApplicationExceptions;
        }

        private void HandleApplicationExceptions(object sender, DispatcherUnhandledExceptionEventArgs dispatcherUnhandledExceptionEventArgs)
        {
            txtOutput.Text = dispatcherUnhandledExceptionEventArgs.Exception.Message;
        }

        private void viewModel_OnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            switch (propertyChangedEventArgs.PropertyName)
            {
                case "NetworkExists":
                    if (ViewModel.NetworkExists)
                    {
                        txtNetworkName.Visibility = Visibility.Visible;
                        pbStatus.Visibility = Visibility.Visible;
                        txtStatus.Visibility = Visibility.Visible;
                        ViewModel.HopfieldNetwork.OnItemProcessed += HopfieldNetwork_OnOnItemProcessed;
                    }
                    break;
                case "IsKnownCharacter":
                    var visibility = ViewModel.IsKnownCharacter ? Visibility.Visible : Visibility.Hidden;

                    txtRecognizedCharLabel.Visibility = visibility;
                    txtRecognizedChar.Visibility = visibility;
                    break;
            }
        }

        private void HopfieldNetwork_OnOnItemProcessed(object sender, ProcessEventArgs processEventArgs)
        {
            if (processEventArgs.ItemsCount == int.MaxValue)
            {
                txtStatus.Text = "Iteracja: " + processEventArgs.CurrentItem;
                pbStatus.Dispatcher.Invoke(() => pbStatus.Value = 100, DispatcherPriority.Background);
            }
            else
            {
                var progresPercentage = (double) processEventArgs.CurrentItem /
                                        (double) processEventArgs.ItemsCount * 100.0;
                txtStatus.Text = "Postęp nauki: " + $"{progresPercentage:0}%";
                pbStatus.Dispatcher.Invoke(() => pbStatus.Value = progresPercentage, DispatcherPriority.Background);
            }
        }

        private void btnRecognizeImage_OnClick(object sender, RoutedEventArgs e)
        {
            imgOutput.Source = null;
            txtRecognizedChar.Text = string.Empty;
            ViewModel.IsKnownCharacter = false;

            try
            {
                var imageContent = ImageHelper.LoadImage(ViewModel.ImageToRecognizePath);

                var resultImage = ViewModel.HopfieldNetwork.GetResult(imageContent);
                imgOutput.Source = ImageHelper.BitmapToImageSource(ImageHelper.ConvertVectorToImage(resultImage));

                char recognizedChar;
                if (ViewModel.HopfieldNetwork.TryGetChar(resultImage, out recognizedChar))
                {
                    ViewModel.IsKnownCharacter = true;
                    txtRecognizedChar.Text = recognizedChar.ToString();
                }
                else
                    ViewModel.IsKnownCharacter = false;
            }
            catch (Exception ex)
            {
                txtOutput.Text = ex.Message;
            }
            finally
            {
                ViewModel.HopfieldNetwork.ResetNetworkState();
            }
        }
    }
}
