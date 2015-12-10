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

        public interface IViewPlugin
        {
            Position    Position { get; }
            int         Layer { get; }

            bool        Optional { get; }
        }
    }
}