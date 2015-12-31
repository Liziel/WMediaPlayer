using System;
using DispatcherLibrary;
using PluginLibrary;
using System.ComponentModel.Composition;
using MediaPropertiesLibrary;
using SidePlayer.MediasPlayer.Audio;
using SidePlayer.MediasPlayer.Video;
using TagLib;
using static DispatcherLibrary.Dispatcher;

namespace SidePlayer
{
    [Export(typeof(IMessageablePlugin))]
    public class SidePlayerInstanciator : Listener, IMessageablePlugin
    {
        public event MessageableStatusChanged StatusChanged;
        public bool Optional { get; } = false;

        [ForwardDispatch]
        public SidePlayerPluginMainViewModel Model { get; set; }

        private SidePlayerPluginMainView _view = new SidePlayerPluginMainView();

        private SidePlayerPluginMainViewModel _musicModel = new SidePlayerPluginMainViewModel( new MusicPlayerPluginViewModel() );
        private SidePlayerPluginMainViewModel _videoModel = new SidePlayerPluginMainViewModel( new VideoPlayerPluginViewModel() );
        private SidePlayerPluginMainViewModel _pictureModel = null;

        [EventHook("Media Opening")]
        public void OnMediaOpened(Uri media)
        {
        }

        [EventHook("Video Track Selected")]
        public void OnVideoTrackSelected(MediaPropertiesLibrary.Video.Track track)
        {
            Dispatch("Stop");
            _videoModel.AssignMedia(track);
            Model = _videoModel;
            _view.DataContext = _videoModel;
            Model.MediaControlBar.SetDuration(track.Duration.TotalSeconds);
            Dispatch("Attach Plugin", _view);
        }

        [EventHook("Play")]
        public void OnPlayAudio(MediaPropertiesLibrary.Audio.Track track)
        {
            Dispatch("Stop");
            _musicModel.AssignMedia(track);
            Model = _musicModel;
            _view.DataContext = _musicModel;
            Model.MediaControlBar.SetDuration(track.Duration.TotalSeconds);
            Dispatch("Attach Plugin", _view);
            Dispatch(this, "Play");
        }

        [EventHook("Play")]
        public void OnPlayVideo(MediaPropertiesLibrary.Video.Track track)
        {
            Dispatch("Stop");
            _videoModel.AssignMedia(track);
            Model = _videoModel;
            _view.DataContext = _videoModel;
            Model.MediaControlBar.SetDuration(track.Duration.TotalSeconds);
            Dispatch("Attach Plugin", _view);
        }
    }
}