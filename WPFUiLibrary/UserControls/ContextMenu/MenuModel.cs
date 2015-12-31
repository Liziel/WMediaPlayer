using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Data;
using System.Windows.Input;
using MediaPropertiesLibrary;
using MediaPropertiesLibrary.Annotations;
using WPFUiLibrary.UserControls.ContextMenu.MenuItems;
using WPFUiLibrary.Utils;

namespace WPFUiLibrary.UserControls.ContextMenu
{
    public class MenuModel : INotifyPropertyChanged
    {
        public MenuModel()
        {
            MenuItems = new List<CommandItem>
            {
                new CommandItem
                {
                    Name = "Play",
                    Command = new UiCommand(o => DispatcherLibrary.Dispatcher.Dispatch("Multiple Track Selected For Play", Tracks, 0))
                },
                new CommandItem
                {
                    Name = "Add to Play Queue",
                    Command = new UiCommand(o => DispatcherLibrary.Dispatcher.Dispatch("Multiple Track Selected", Tracks))
                },
                new CommandItem
                {
                    Name = "Add to ...",
                    Command = null
                }
            };
        }

        #region Notifier Fields

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        public IEnumerable<TrackDefinition>     Tracks { get; set; }
        public List<CommandItem>                MenuItems { get; }
    }
}