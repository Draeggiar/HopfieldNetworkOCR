using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media.Imaging;
using HopfieldNetworkOCR.Annotations;
using HopfieldNetworkOCR.Core.Model;

namespace HopfieldNetworkOCR.ViewModels
{
    public class NetworkViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private HopfieldNetwork _hopfieldNetwork;
        public HopfieldNetwork HopfieldNetwork
        {
            get { return _hopfieldNetwork; }
            set
            {
                _hopfieldNetwork = value;
                OnPropertyChanged(nameof(NetworkExists));
            }
        }

        public bool NetworkExists => HopfieldNetwork != null;

        private bool _networkTrained;
        public bool NetworkTrained
        {
            get { return _networkTrained; }
            set {
                if (_networkTrained == value) return;
                _networkTrained = value;
                OnPropertyChanged(nameof(NetworkTrained));
            }
        }

        private string _imageToRecognizePath;
        public string ImageToRecognizePath
        {
            get { return _imageToRecognizePath; }
            set {
                if (_imageToRecognizePath == value) return;
                _imageToRecognizePath = value;
                OnPropertyChanged(nameof(ImageToRecognizePath));
                OnPropertyChanged(nameof(ImageToRecognize));
                OnPropertyChanged(nameof(RecognizeAvalible));
            }
        }

        public BitmapImage ImageToRecognize =>
            string.IsNullOrEmpty(ImageToRecognizePath) ? null : new BitmapImage(new Uri(ImageToRecognizePath));

        public Visibility RecognizeAvalible =>
            string.IsNullOrEmpty(ImageToRecognizePath) ? Visibility.Hidden : Visibility.Visible;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
