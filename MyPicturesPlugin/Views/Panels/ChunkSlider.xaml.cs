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
using MediaPropertiesLibrary.Pictures;
using WPFUiLibrary.Resources.Fonts;
using WPFUiLibrary.Utils;

namespace MyPicturesPlugin.Views.Panels
{
    /// <summary>
    /// Interaction logic for ChunkSlider.xaml
    /// </summary>
    public partial class ChunkSlider : UserControl
    {
        public ChunkSlider()
        {
            InitializeComponent();
            NextButton.Command = new UiCommand(o => RefreshDisplay(SlideDirection.Right));
            PrevButton.Command = new UiCommand(o => RefreshDisplay(SlideDirection.Left));
        }

        public int Index { get; private set; }

        public ChunkView[] Batches { get; set; }
        public IEnumerable<Picture> List { get { return (IEnumerable<Picture>) GetValue(ListProperty); } set { SetValue(ListProperty, value); } } 
        public static readonly DependencyProperty ListProperty = 
            DependencyProperty.Register("List", typeof(IEnumerable<Picture>), typeof(ChunkSlider), new PropertyMetadata()
            {
                PropertyChangedCallback = delegate(DependencyObject o, DependencyPropertyChangedEventArgs args)
                {
                    var chunkScroller = o as ChunkSlider;

                    if (chunkScroller == null || args.NewValue == null) return;
                    chunkScroller.ChunkContainer.Children.Clear();
                    if (!chunkScroller.List.Any())
                    {
                        chunkScroller.Batches = null;
                        return;
                    }
                    foreach (var chunk in chunkScroller.Batches = chunkScroller.List.Batch(18).Select(pictures => new ChunkView {Chunk = pictures}).ToArray())
                        chunkScroller.ChunkContainer.Children.Add(chunk);
                    chunkScroller.RefreshDisplay();
                }
            });

        private enum SlideDirection
        {
            Creation,
            Right, Left
        }

        private void RefreshDisplay(SlideDirection slideDirection = SlideDirection.Creation)
        {
            switch (slideDirection)
            {
                case SlideDirection.Creation:
                    Index = 0;
                    Batches[Index].Generate();
                    Batches[Index].Visibility = Visibility.Visible;
                    if (Batches.Length == 1)
                        break;
                    Batches[Index + 1].Generate();
                    Batches[Index + 1].Visibility = Visibility.Collapsed;
                    break;

                case SlideDirection.Right:
                    if (Index == Batches.Length - 1)
                        break;
                    if (Index != 0)
                        Batches[Index - 1].Liberate();
                    Batches[Index].Visibility = Visibility.Collapsed;
                    Batches[Index + 1].Visibility = Visibility.Visible;
                    Index += 1;
                    if (Index == Batches.Length - 1)
                        break;
                    Batches[Index + 1].Generate();
                    break;

                case SlideDirection.Left:
                    if (Index == 0)
                        break;
                    if (Index != Batches.Length - 1)
                        Batches[Index + 1].Liberate();
                    Batches[Index].Visibility = Visibility.Collapsed;
                    Batches[Index - 1].Visibility = Visibility.Visible;
                    Index -= 1;
                    if (Index == 0)
                        break;
                    Batches[Index - 1].Generate();
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(slideDirection), slideDirection, null);
            }
        }
    }
}
