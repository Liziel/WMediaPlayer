using System.Windows;
using System.Windows.Controls;

namespace WPFUiLibrary.UserControls.VolumeControl
{
    /// <summary>
    /// Interaction logic for Slider.xaml
    /// </summary>
    public sealed partial class Slider : UserControl
    {
        #region Constructor

        public Slider()
        {
            InitializeComponent();
            VolumeSlider.Value = 0;
        }

        #endregion

        #region Binded Values

        public double Volume
        {
            get { return (double) GetValue(VolumeProperty); }
            set { SetValue(VolumeProperty, value); }
        }

        public static readonly DependencyProperty VolumeProperty = DependencyProperty.Register("Volume", typeof(double), typeof(Slider), new PropertyMetadata
        {
            PropertyChangedCallback = (o, args) =>
            {
                ((Slider)o).VolumeSlider.Value = (double)args.NewValue;
            }
        });

        #endregion



        #region Events

        public event VolumeChanged VolumeChanged;

        private void OnVolumeChanged(double volume)
        {
            VolumeChanged?.Invoke(volume);
        }

        private void RangeBase_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Volume = VolumeSlider.Value;
            OnVolumeChanged(Volume);
        }

        public void RaiseVolumeChanged(double volume)
        {
            Volume = volume;
            VolumeSlider.Value = volume;
        }

        #endregion
    }

    public delegate void VolumeChanged(double volume);
}
