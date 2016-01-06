using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using MediaPropertiesLibrary;
using PlaylistPlugin.Models;

namespace PlaylistPlugin.ChildsViews.PlaylistItems
{
    public class PlaylistAccessTracks : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            CollectionViewSource collectionView = new CollectionViewSource();
            var list = value as ListCollectionView;
            if (list == null) return collectionView.Source;
            collectionView.Source = new ObservableCollection<TrackDefinition>(list.Cast<Playlist.Member>().Select(member => member.Track));
            return collectionView.View;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}