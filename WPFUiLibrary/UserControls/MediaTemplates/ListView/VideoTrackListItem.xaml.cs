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
using MediaPropertiesLibrary.Annotations;
using WPFUiLibrary.UserControls.ContextMenu;
using WPFUiLibrary.UserControls.MediaTemplates.Models;
using Button = WPFUiLibrary.UserControls.ContextMenu.Button;

namespace WPFUiLibrary.UserControls.MediaTemplates.ListView
{
    /// <summary>
    /// Interaction logic for VideoTrackListItem.xaml
    /// </summary>
    public partial class VideoTrackListItem : UserControl, INotifyPropertyChanged
    {
        public VideoTrackListItem()
        {
            MediaPresentationColumns = new List<GridLength>();
            InitializeComponent();
            ContextMenuButton.SetBinding(Button.MenuProperty, new Binding("Menu") { Source = this });
            TrackControlPresenter.SetBinding(Grid.DataContextProperty, new Binding("Model") { Source = this });
        }

        #region Column Bindings

        public List<GridLength> MediaPresentationColumns { get { return (List<GridLength>)GetValue(MediaPresentationColumnsProperty); } set { SetValue(MediaPresentationColumnsProperty, value); } }
        public static readonly DependencyProperty MediaPresentationColumnsProperty =
            DependencyProperty.Register("MediaPresentationColumns", typeof(List<GridLength>), typeof(VideoTrackListItem), new PropertyMetadata()
            {
                PropertyChangedCallback = (o, args) =>
                {
                    var item = o as VideoTrackListItem;
                    if (item == null) return;
                    var gridLengths = (List<GridLength>)args.NewValue;
                    if (gridLengths == null || gridLengths.Count < 6) return;
                    item.PlayColumn.Width = gridLengths[0];
                    item.TitleColumn.Width = gridLengths[1];
                    item.ArtistColumn.Width = gridLengths[2];
                    item.AlbumColumn.Width = gridLengths[3];
                    item.SelectionColumn.Width = gridLengths[4];
                    item.TimeColumn.Width = gridLengths[5];
                }
            });

        #endregion

        public MenuModel Menu { get { return (MenuModel)GetValue(MenuProperty); } set { SetValue(MenuProperty, value); } }
        public static readonly DependencyProperty MenuProperty =
            DependencyProperty.Register("Menu", typeof(MenuModel), typeof(VideoTrackListItem));

        public VideoTrackViewModel Model { get { return (VideoTrackViewModel)GetValue(ModelProperty); } set { SetValue(ModelProperty, value); } }
        public static readonly DependencyProperty ModelProperty =
            DependencyProperty.Register("Model", typeof(VideoTrackViewModel), typeof(VideoTrackListItem));

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
