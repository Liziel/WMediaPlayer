using System.Windows;
using PluginLibrary;
using PluginLibrary.Customization;
using System.ComponentModel.Composition;

namespace PlaylistPlugin
{
    [Export(typeof(ILoadablePlugin))]
    public class LoadablePlaylistPlugin : ILoadablePlugin
    {
        public IViewPlugin View { get; } = new PlaylistMainView();
        public UIElement Logo { get; } = new Logo();
        public string PresentationName { get; } = "Playlist";
    }
}