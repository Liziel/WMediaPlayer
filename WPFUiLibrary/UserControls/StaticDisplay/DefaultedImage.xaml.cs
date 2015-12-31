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

namespace WPFUiLibrary.UserControls.StaticDisplay
{
    /// <summary>
    /// Interaction logic for DefaultedImage.xaml
    /// </summary>
    public partial class DefaultedImage : UserControl
    {
        public DefaultedImage()
        {
            InitializeComponent();
        }

        public ImageSource Source
        {
            get { return (ImageSource) GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value);}
        }

        public static readonly DependencyProperty SourceProperty
            = DependencyProperty.Register("Source", typeof(ImageSource), typeof(DefaultedImage), new PropertyMetadata
            {
                PropertyChangedCallback = (o, args) =>
                {
                    var dfI = o as DefaultedImage;
                    if (dfI == null) return;

                    var value = args.NewValue as ImageSource;
                    if (value == null) return;
                    dfI.Default.Visibility = Visibility.Collapsed;
                    dfI.Image.Source = value;
                }
            });

        public FrameworkElement DefaultElement
        {
            get { return (FrameworkElement) GetValue(DefaultElementProperty); }
            set { SetValue(DefaultElementProperty, value); }
        }

        public static readonly DependencyProperty DefaultElementProperty =
            DependencyProperty.Register("DefaultElement", typeof(FrameworkElement), typeof(DefaultedImage), new PropertyMetadata
            {
                PropertyChangedCallback = (o, args) =>
                {
                    var dfI = o as DefaultedImage;
                    if (dfI == null) return;

                    var value = args.NewValue as FrameworkElement;
                    if (value == null) return;
                    dfI.Default.Content = value;
                }
            });
    }
}
