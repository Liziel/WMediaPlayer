using System.Windows.Controls;

namespace SidePlayer.MediasPlayer
{
    public interface IMediaPlayer
    {
        UserControl MediaView { get; }

        void OnMaximize();
        void OnMinimize();
    }
}