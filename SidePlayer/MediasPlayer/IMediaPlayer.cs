using System;
using System.Windows.Controls;

namespace SidePlayer.MediasPlayer
{
    public interface IMediaPlayer
    {
        UserControl MediaView { get; }

        void OnMaximize();
        void OnMinimize();

        void AssignUri(Uri uri, TagLib.File tag);
        void AssignMedia(object media);
    }
}