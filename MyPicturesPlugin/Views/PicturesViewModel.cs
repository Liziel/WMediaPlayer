using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Data;
using System.Windows.Input;
using DispatcherLibrary;
using MediaPropertiesLibrary.Annotations;
using MediaPropertiesLibrary.Pictures;
using WPFUiLibrary.Utils;
using static DispatcherLibrary.Dispatcher;
using static MediaPropertiesLibrary.Pictures.Library.Library;

namespace MyPicturesPlugin.Views
{
    public class PicturesViewModel : Listener, INotifyPropertyChanged
    {
        #region Notifier Properties

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        private readonly CollectionViewSource _folders = new CollectionViewSource();
        public ListCollectionView   FoldersView => _folders.View as ListCollectionView;

        public ObservableCollection<Picture> PicturesView => _foldersStack.Count > 0 ? _foldersStack[_foldersStack.Count - 1].Pictures : null;


        private string _previousFolderTitle;
        public string PreviousFolderTitle
        {
            get { return _previousFolderTitle; }
            set
            {
                _previousFolderTitle = value; 
                OnPropertyChanged(nameof(PreviousFolderTitle));
            }
        }

        private string _folderTitle;
        public string FolderTitle
        {
            get { return _folderTitle; }
            set
            {
                _folderTitle = value; 
                OnPropertyChanged(nameof(FolderTitle));
            }
        }

        public PicturesViewModel()
        {
            PreviousFolder = new UiCommand(o => PopFolder());
            AddEventListener(this);
            PushFolder(Root);
            FoldersView.Refresh();
            OnPropertyChanged(nameof(FoldersView));
        }


        private readonly List<Folder> _foldersStack = new List<Folder>();

        [EventHook("PictureLibrary: View Folder")]
        public void PushFolder(Folder folder)
        {
            if (folder == null)
                return;
            if (_foldersStack.Count > 0)
                _previousFolderTitle = _folderTitle;
            _foldersStack.Add(folder);
            FolderTitle = folder.Name;
            _folders.Source = folder.Folders;
            FoldersView.Refresh();
            OnPropertyChanged(nameof(FoldersView));
            OnPropertyChanged(nameof(PicturesView));
        }


        private void PopFolder()
        {
            if (_foldersStack.Count == 0)
                return;
            _foldersStack.RemoveAt(_foldersStack.Count - 1);
            _folderTitle = _foldersStack[_foldersStack.Count - 1].Name;
            _folders.Source = _foldersStack[_foldersStack.Count - 1].Folders;
            FoldersView.Refresh();
            OnPropertyChanged(nameof(FoldersView));
            OnPropertyChanged(nameof(PicturesView));
        }

        public ICommand PreviousFolder { get; }
    }
}