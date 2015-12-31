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
    /// Interaction logic for BackgroundRoundedImage.xaml
    /// </summary>
    public partial class BackgroundRoundedImage : UserControl
    {
        public BackgroundRoundedImage()
        {
            InitializeComponent();
        }

        public ImageSource Source { get { return (ImageSource) GetValue(ImageSourceProperty); } set { SetValue(ImageSourceProperty, value);} }
        public static readonly DependencyProperty ImageSourceProperty = 
            DependencyProperty.Register("Source", typeof(ImageSource), typeof(BackgroundRoundedImage), new PropertyMetadata
            {
                PropertyChangedCallback = (o, args) =>
                {
                    BackgroundRoundedImage backgroundRoundedImage = o  as BackgroundRoundedImage;
                    if (backgroundRoundedImage == null) return;
                    backgroundRoundedImage.Image.Source = args.NewValue as ImageSource;
                    if (backgroundRoundedImage.Image.Source != null) backgroundRoundedImage.Ellipse.Visibility = Visibility.Visible;
                }
            });
    }
}
