using System.Security.Cryptography.X509Certificates;

namespace PluginLibrary
{
    public delegate void MessageableStatusChanged(object source);
    public delegate void MessageablePluginStatusChanged(object source, IMessageablePlugin plugin);

    public interface IMessageablePlugin
    {
        event MessageableStatusChanged StatusChanged;

        bool Optional { get; }
    }
}