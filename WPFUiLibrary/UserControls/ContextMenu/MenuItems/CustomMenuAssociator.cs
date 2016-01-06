using System;
using System.Globalization;
using System.Windows.Data;
using WPFUiLibrary.UserControls.PopupManager;
using static DispatcherLibrary.Dispatcher;

namespace WPFUiLibrary.UserControls.ContextMenu.MenuItems
{
    public class CustomMenuAssociator : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            IMenuClosable menuClosable = values[0] as IMenuClosable;
            MediaMenu       mediaMenu = values[1] as MediaMenu;;
            if (mediaMenu == null || menuClosable == null) return null;
            menuClosable.Close = delegate
            {
                Dispatch("Remove PopUps",
                    new Func<PopUp, bool>(popUp => ReferenceEquals(popUp.PopUpElement, mediaMenu)));
                mediaMenu.Menu?.Reset();
            };
            return menuClosable;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}