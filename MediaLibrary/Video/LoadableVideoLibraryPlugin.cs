using System.ComponentModel.Composition;
using System.Windows;
using PluginLibrary;
using PluginLibrary.Customization;

namespace MediaLibrary.Video
{
    [Export(typeof(ILoadablePlugin))]
    public class LoadableVideoLibraryPlugin : ILoadablePlugin
    {
        public IViewPlugin View { get; } = new Video.LibraryClassView();
        public UIElement Logo { get; } = new Logo();
        public string PresentationName { get; } = "My Videos";
    }
}