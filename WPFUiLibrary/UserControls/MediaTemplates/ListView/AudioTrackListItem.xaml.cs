using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using MediaPropertiesLibrary.Annotations;
using WPFUiLibrary.UserControls.ContextMenu;
using WPFUiLibrary.UserControls.MediaTemplates.Models;
using Button = WPFUiLibrary.UserControls.ContextMenu.Button;

namespace WPFUiLibrary.UserControls.MediaTemplates.ListView
{
    /// <summary>
    /// Interaction logic for AudioTrackListItem.xaml
    /// </summary>
    public partial class AudioTrackListItem : UserControl, INotifyPropertyChanged
    {
        public AudioTrackListItem()
        {
            MediaPresentationColumns = new List<GridLength>();
            InitializeComponent();
            ContextMenuButton.SetBinding(Button.MenuProperty, new Binding("Menu") {Source = this});
            TrackControlPresenter.SetBinding(Grid.DataContextProperty, new Binding("Model") {Source = this});
        }

        #region Column Bindings

        public List<GridLength> MediaPresentationColumns { get { return (List<GridLength>)GetValue(MediaPresentationColumnsProperty); } set { SetValue(MediaPresentationColumnsProperty, value); } }
        public static readonly DependencyProperty   MediaPresentationColumnsProperty = 
            DependencyProperty.Register("MediaPresentationColumns", typeof(List<GridLength>), typeof(AudioTrackListItem), new PropertyMetadata()
            {
                PropertyChangedCallback = (o, args) =>
                {
                    var item = o as AudioTrackListItem;
                    if (item == null) return;
                    var gridLengths = (List<GridLength>) args.NewValue;
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
            DependencyProperty.Register("Menu", typeof(MenuModel), typeof(AudioTrackListItem));

        public AudioTrackViewModel Model { get { return (AudioTrackViewModel) GetValue(ModelProperty); } set {SetValue(ModelProperty, value);} }
        public static readonly DependencyProperty ModelProperty =
            DependencyProperty.Register("Model", typeof(AudioTrackViewModel), typeof(AudioTrackListItem));

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
