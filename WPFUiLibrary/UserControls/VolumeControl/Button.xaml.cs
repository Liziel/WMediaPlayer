using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace WPFUiLibrary.UserControls.VolumeControl
{
    /// <summary>
    /// Interaction logic for Button.xaml
    /// </summary>
    public partial class Button : UserControl
    {
        #region Constructor

        private CancellationTokenSource _temporaryToken;

        public Button()
        {
            InitializeComponent();
            VolumeButton.MouseEnter += (sender, args) =>
            {
                _temporaryToken?.Cancel();
                _temporaryToken = new CancellationTokenSource();
                SliderContainer.Visibility = Visibility.Visible;
            };
            VolumeButton.MouseLeave += (sender, args) =>
            {
                Task.Run(async delegate
                {
                    var keep = _temporaryToken;
                    await Task.Delay(TimeSpan.FromMilliseconds(500), keep.Token);
                    Dispatcher.Invoke(delegate { SliderContainer.Visibility = Visibility.Collapsed; });
                });
            };
            if (Mute)
                SetButtonVisibility(Muted);
            else if (Volume > 0.66)
                SetButtonVisibility(VolumeHigh);
            else if (Volume > 0.33)
                SetButtonVisibility(VolumeMid);
            else
                SetButtonVisibility(VolumeLow);
            VolumeSlider.SetBinding(Slider.VolumeProperty,
                new Binding("Volume") {Mode = BindingMode.TwoWay, Source = this});
        }

        #endregion

        #region Binded Values


        public double Volume
        {
            get { return (double) GetValue(VolumeProperty); }
            set { SetValue(VolumeProperty, value); }
        }

        public static readonly DependencyProperty VolumeProperty = DependencyProperty.Register("Volume", typeof(double), typeof(Button), new PropertyMetadata
        {
            PropertyChangedCallback = (o, args) =>
            {
                var button = o as Button;
                if (button == null) return;
                var volume = (double)args.NewValue;

                if (volume > 0.66)
                    button.SetButtonVisibility(button.VolumeHigh);
                else if (volume > 0.33)
                    button.SetButtonVisibility(button.VolumeMid);
                else
                    button.SetButtonVisibility(button.VolumeLow);
            },
            DefaultValue = 1.0
        });

        public bool Mute
        {
            get { return (bool) GetValue(MuteProperty); }
            set { SetValue(MuteProperty, value); }
        }

        public static readonly DependencyProperty MuteProperty = DependencyProperty.Register("Mute", typeof(bool), typeof(Button), new PropertyMetadata
        {
            PropertyChangedCallback = (o, args) =>
            {
                var button = o as Button;
                if (button == null) return;

                if ((bool)args.NewValue)
                {
                    button.SetButtonVisibility(button.Muted);
                }
                else
                {
                    if (button.Volume > 0.66)
                        button.SetButtonVisibility(button.VolumeHigh);
                    else if (button.Volume > 0.33)
                        button.SetButtonVisibility(button.VolumeMid);
                    else
                        button.SetButtonVisibility(button.VolumeLow);
                }
            },
            DefaultValue = false
        });

        #endregion

        #region Events

        private void OnMute(object sender, MouseButtonEventArgs e)
        {
            Mute = !Mute;
            if (Mute)
                SetButtonVisibility(Muted);
            else
            {
                if (Volume > 0.66)
                    SetButtonVisibility(VolumeHigh);
                else if (Volume > 0.33)
                    SetButtonVisibility(VolumeMid);
                else
                    SetButtonVisibility(VolumeLow);
            }
            OnMuteChanged(Mute);
        }

        private void SetButtonVisibility(UIElement volumeRepresentation)
        {
            VolumeHigh.Visibility = Visibility.Collapsed;
            VolumeMid.Visibility = Visibility.Collapsed;
            VolumeLow.Visibility = Visibility.Collapsed;
            Muted.Visibility = Visibility.Collapsed;
            volumeRepresentation.Visibility = Visibility.Visible;
        }

        public event MuteChanged MuteChanged;

        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        protected virtual void OnMuteChanged(bool mute)
        {
            MuteChanged?.Invoke(mute);
        }

        public event VolumeChanged VolumeChanged;

        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        protected virtual void OnVolumeChanged(double volume)
        {
            VolumeChanged?.Invoke(volume);
        }

        #endregion
    }

    public delegate void MuteChanged(bool mute);
}
