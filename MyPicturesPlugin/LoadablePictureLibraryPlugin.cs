using System.ComponentModel.Composition;
using System.Windows;
using PluginLibrary;
using PluginLibrary.Customization;

namespace MyPicturesPlugin
{
    [Export(typeof(ILoadablePlugin))]
    public class LoadablePictureLibraryPlugin : ILoadablePlugin
    {
        public IViewPlugin View { get; } = new Views.PicturesView();
        public UIElement Logo { get; } = new Logo();
        public string PresentationName { get; } = "My Pictures";
    }
}