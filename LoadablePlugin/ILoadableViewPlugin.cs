using PluginLibrary.Customization;

namespace LoadablePlugin
{
    public interface ILoadableViewPlugin : IViewPlugin
    {
        #region Graphical Representation

        object Logo { get; }
        string PresentationName { get; }

        #endregion
    }
}