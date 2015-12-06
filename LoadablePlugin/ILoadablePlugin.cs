using PluginLibrary.Customization;

namespace LoadablePlugin
{
    public interface ILoadablePlugin : IPlugin
    {
        #region Graphical Representation

        object Logo { get; }
        string PresentationName { get; }

        #endregion
    }
}