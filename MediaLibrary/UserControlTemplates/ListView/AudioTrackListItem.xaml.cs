using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
using MediaLibrary.Annotations;

namespace MediaLibrary.UserControlTemplates.ListView
{
    /// <summary>
    /// Interaction logic for AudioTrackListItem.xaml
    /// </summary>
    public partial class AudioTrackListItem : UserControl, INotifyPropertyChanged
    {
        public AudioTrackListItem()
        {
            InitializeComponent();
        }

        #region ColumnSetters

        private void PlayColumnLoaded(object sender, RoutedEventArgs e)
        {
            var column = sender as ColumnDefinition;
            column?.SetBinding(ColumnDefinition.WidthProperty, new Binding("PlayColumn") {Source = this});
        }

        private void TitleColumnLoaded(object sender, RoutedEventArgs e)
        {
            var column = sender as ColumnDefinition;
            column?.SetBinding(ColumnDefinition.WidthProperty, new Binding("TitleColumn") { Source = this });
        }

        private void ArtistColumnLoaded(object sender, RoutedEventArgs e)
        {
            var column = sender as ColumnDefinition;
            column?.SetBinding(ColumnDefinition.WidthProperty, new Binding("ArtistColumn") { Source = this });
        }

        private void AlbumColumnLoaded(object sender, RoutedEventArgs e)
        {
            var column = sender as ColumnDefinition;
            column?.SetBinding(ColumnDefinition.WidthProperty, new Binding("AlbumColumn") { Source = this });
        }

        private void TimeColumnLoaded(object sender, RoutedEventArgs e)
        {
            var column = sender as ColumnDefinition;
            column?.SetBinding(ColumnDefinition.WidthProperty, new Binding("TimeColumn") { Source = this });
        }

        #endregion

        #region Column Bindings

        public List<GridLength> MediaPresentationColumns { get { return (List<GridLength>)GetValue(MediaPresentationColumnsProperty); } set { SetValue(MediaPresentationColumnsProperty, value); } }
        public static readonly DependencyProperty   MediaPresentationColumnsProperty = 
            DependencyProperty.Register("MediaPresentationColumns", typeof(List<GridLength>), typeof(AudioTrackListItem), new PropertyMetadata(new List<GridLength>())
            {
                PropertyChangedCallback = (o, args) =>
                {
                    var item = o as AudioTrackListItem;
                    if (item == null) return;
                    var gridLengths = (List<GridLength>) args.NewValue;
                    item.PlayColumn.Width = gridLengths[0];
                    item.TitleColumn.Width = gridLengths[1];
                    item.ArtistColumn.Width = gridLengths[2];
                    item.AlbumColumn.Width = gridLengths[3];
                    item.TimeColumn.Width = gridLengths[4];
                }
            });

        #endregion

        #region Notifier Properties

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
