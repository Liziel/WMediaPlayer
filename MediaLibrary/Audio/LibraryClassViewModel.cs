using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using DispatcherLibrary;
using MediaLibrary.Annotations;
using MediaLibrary.Audio.SubViews;
using MediaPropertiesLibrary.Audio;
using WPFUiLibrary.Utils;
using AlbumView = MediaLibrary.Audio.SubViews.AlbumView;
using AlbumViewModel = MediaLibrary.Audio.SubViews.AlbumViewModel;

namespace MediaLibrary.Audio
{
    public class TabItem : INotifyPropertyChanged
    {
        #region TabItems Properties

        private bool _selected = false;
        public bool Selected { get { return _selected; } internal set { _selected = value; OnPropertyChanged(nameof(Selected)); } }
        public string Name { get; internal set; }

        private UiCommand _onSelected;

        public UiCommand OnSelected
        {
            get { return _onSelected; }
            set
            {
                _onSelected = value;
                OnPropertyChanged(nameof(OnSelected));
            }
        }

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

    public class LibraryClassViewModel : Listener, INotifyPropertyChanged
    {
        #region Pages

        private ObservableCollection<UIElement> _pages = new ObservableCollection<UIElement>();
        public ObservableCollection<UIElement> Pages
        {
            get { return _pages; }
            set
            {
                _pages = value;
                OnPropertyChanged(nameof(Pages));
            }
        }

        [EventHook("AudioLibrary: View Album")]
        public void AccessAlbum(Album album)
        {
            if (album != null)
                Pages.Add(new Pages.AlbumView(new Pages.AlbumViewModel(album)));
        }

        [EventHook("AudioLibrary: View Artist")]
        public void AccessArtist(Artist artist)
        {
            if (artist != null)
                Pages.Add(new Pages.ArtistView(new Pages.ArtistViewModel(artist)));
        }
        #endregion

        #region TabItems List

        private List<TabItem> _tabItems = null;

        public List<TabItem> TabItems
        {
            get { return _tabItems; }
            set
            {
                _tabItems = value;
                OnPropertyChanged(nameof(TabItems));
            }
        }

        #endregion

        #region Sub Views

        private Listener _subViewModel = null;

        public Listener SubViewModel
        {
            get { return _subViewModel; }
            set
            {
                _subViewModel = value;
                OnPropertyChanged(nameof(SubViewModel));
            }
        }

        public UserControl SearchBox { get; } = new SearchBox();

        private UserControl _subView = null;

        public UserControl SubView
        {
            get { return _subView; }
            set
            {
                _subView = value;
                OnPropertyChanged(nameof(SubView));
            }
        }

        private readonly Listener[] _subViewModels =
        {
            new TrackViewModel(), new AlbumViewModel(), new ArtistViewModel()
        };

        private readonly UserControl[] _subViews = null;

        #endregion

        #region Contructor

        public LibraryClassViewModel()
        {
            _subViews = new UserControl[] { new TrackView(_subViewModels[0] as TrackViewModel), new AlbumView(_subViewModels[1] as AlbumViewModel), new ArtistView( _subViewModels[2] as ArtistViewModel),  };
            TabItemsInitialization();
            SelectTab(0);
            Dispatcher.AddEventListener(this);
        }

        private void TabItemsInitialization()
        {
            TabItems = new List<TabItem>
            {
                new TabItem
                {
                    Selected = false,
                    Name = "Tracks",
                    OnSelected = new UiCommand(delegate { SelectTab(0); })
                },
                new TabItem
                {
                    Selected = false,
                    Name = "Albums",
                    OnSelected = new UiCommand(delegate { SelectTab(1); })
                },
                new TabItem
                {
                    Selected = false,
                    Name = "Artists",
                    OnSelected = new UiCommand(delegate { SelectTab(2); })
                }
            };
        }

        private void SelectTab(int item)
        {
            foreach (var tabItem in TabItems)
            {
                tabItem.Selected = false;
            }
            TabItems[item].Selected = true;
            SubViewModel = _subViewModels[item];
            SubView = _subViews[item];
            SearchBox.DataContext = _subViewModels[item];
            OnPropertyChanged(nameof(SearchBox));
        }

        #endregion

        #region Notifier Properties

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual
        void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}