using System.ComponentModel.Composition;
using System.Windows;
using PluginLibrary;
using PluginLibrary.Customization;

namespace MediaLibrary.Video
{
    [Export(typeof(ILoadablePlugin))]
    public class LoadableVideoLibraryPlugin : ILoadablePlugin
    {
        public LoadableVideoLibraryPlugin()
        {
            var model = new Video.LibraryClassViewModel();
            View = new Video.LibraryClassView(model);
        }

        public IViewPlugin View { get; }
        public UIElement Logo { get; } = new Logo();
        public string PresentationName { get; } = "My Videos";
    }
}