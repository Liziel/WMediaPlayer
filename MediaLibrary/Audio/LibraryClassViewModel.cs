using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using DispatcherLibrary;
using MediaLibrary.Annotations;
using MediaLibrary.Audio.Pages;
using MediaLibrary.Audio.SubViews;
using MediaPropertiesLibrary.Audio;
using PluginLibrary;
using UiLibrary;
using UiLibrary.Utils;

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
                Pages.Add(new AlbumView(new AlbumViewModel(album)));
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
            new AudioTrackViewModel(), new AudioAlbumViewModel(), null
        };

        private readonly UserControl[] _subViews = null;

        #endregion

        #region Contructor

        public LibraryClassViewModel()
        {
            _subViews = new UserControl[] { new AudioTrackView(_subViewModels[0] as AudioTrackViewModel), new AudioAlbumView(_subViewModels[1] as AudioAlbumViewModel), null };
            TabItemsInitialization();
            SelectTab(0);
            Dispatcher.GetInstance.AddEventListener(this);
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