using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using MediaPropertiesLibrary.Pictures;

namespace WPFUiLibrary.UserControls.MediaTemplates.GridView
{
    /// <summary>
    /// Interaction logic for PictureGridItem.xaml
    /// </summary>
    public partial class PictureGridItem : UserControl
    {
        public PictureGridItem()
        {
            InitializeComponent();
        }

        public Picture Picture { get { return (Picture) GetValue(PictureProperty); } set {SetValue(PictureProperty, value);} }
        public static readonly DependencyProperty PictureProperty =
            DependencyProperty.Register("Picture", typeof(Picture), typeof(PictureGridItem), new PropertyMetadata()
            {
                PropertyChangedCallback = delegate(DependencyObject o, DependencyPropertyChangedEventArgs args)
                {
                    var gridItem = o as PictureGridItem;
                    var picture = args.NewValue as Picture;

                    if (picture == null) return;
                    if (gridItem == null) return;
                    var image = new BitmapImage();
                    image.BeginInit();
                    image.DecodePixelWidth = 64;
                    image.DecodePixelHeight = 64;
                    image.CacheOption = BitmapCacheOption.None;
                    image.CreateOptions = BitmapCreateOptions.DelayCreation;
                    image.UriSource = new Uri(picture.Path);
                    image.EndInit();
                    gridItem.Image.Source = image;
                    gridItem.Name.Text = picture.Name;
                }
            });

    }
}
