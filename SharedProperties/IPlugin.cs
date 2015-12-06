using System;

namespace PluginLibrary
{
    namespace Customization
    {
        public enum Position
        {
            Invisible,
            All,
            Right,
            Left,
            Center
        }

        public interface IPlugin
        {
            Position    Position { get; }
            int         Layer { get; }
            bool        Optional { get; }

            string      PluginName { get; }
        }
    }
}