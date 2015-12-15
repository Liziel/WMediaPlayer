using System.ComponentModel.Composition.Primitives;
using PluginLibrary;
using System.ComponentModel.Composition;
using System.Windows;
using PluginLibrary.Customization;

namespace MediaLibrary.Audio
{
    [Export(typeof(ILoadablePlugin))]
    public class LoadableAudioLibraryPlugin : ILoadablePlugin
    {
        public IViewPlugin View { get; } = new LibraryClassView();
        public UIElement Logo { get; } = new Logo();
        public string PresentationName { get; } = "My Musics";
    }
}