using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPFUiLibrary.UserControls.VolumeControl
{

    public delegate void VolumeChanged(double volume);
    /// <summary>
    /// Interaction logic for Slider.xaml
    /// </summary>
    public partial class Slider : UserControl
    {
        public Slider()
        {
            InitializeComponent();
            VolumeSlider.Value = 1.0;
        }


        #region Binded Values

        public double Volume
        {
            get { return (double)GetValue(VolumeProperty); }
            set { SetValue(VolumeProperty, value); }
        }

        public static readonly DependencyProperty VolumeProperty = DependencyProperty.Register("Volume", typeof(double), typeof(Slider), new PropertyMetadata(1.0)
        {
            PropertyChangedCallback = (o, args) =>
            {
                ((Slider)o).VolumeSlider.Value = (double)args.NewValue;
            }
        });

        #endregion

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

    }
}
