using System.Windows;
using PluginLibrary.Customization;

namespace PluginLibrary
{
    public interface ILoadablePlugin
    {
        IViewPlugin View { get; }

        UIElement Logo { get; }
        string PresentationName { get; }
    }
}