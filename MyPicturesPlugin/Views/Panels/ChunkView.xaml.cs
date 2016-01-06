using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using MediaPropertiesLibrary.Pictures;

namespace MyPicturesPlugin.Views.Panels
{
    /// <summary>
    /// Interaction logic for ChunkView.xaml
    /// </summary>
    public partial class ChunkView : UserControl
    {
        public ChunkView()
        {
            Chunk = new List<Picture>();
            InitializeComponent();
        }

        public IEnumerable<Picture> Chunk {get { return (IEnumerable<Picture>) GetValue(ChunkProperty); } set {SetValue(ChunkProperty, value);} } 
        public static readonly DependencyProperty ChunkProperty =
            DependencyProperty.Register("Chunk", typeof(IEnumerable<Picture>), typeof(ChunkView));

        internal void Generate()
        {
            if (Chunk.Count() <= 6)
                Container.Rows = 1;
            else if (Chunk.Count() <= 12)
                Container.Rows = 2;
            else if (Chunk.Count() <= 18)
                Container.Rows = 3;
            foreach (var picture in Chunk)
            {
                Grid grid = new Grid() {Margin = new Thickness(5)};
                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.DecodePixelHeight = 184;
                bitmap.DecodePixelWidth = 184;
                bitmap.CreateOptions = BitmapCreateOptions.DelayCreation;
                bitmap.UriSource = new Uri(picture.Path);
                bitmap.EndInit();
                grid.Children.Add(new Image() { Source = bitmap, Stretch = Stretch.UniformToFill, HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center});
                grid.SetBinding(Grid.HeightProperty, new Binding("ActualWidth") { Source = grid });
                Container.Children.Add(grid);
            }
        }

        internal void Liberate()
        {
            Container.Children.Clear();
        }
    }
}
