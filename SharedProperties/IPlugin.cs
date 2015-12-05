using System;

namespace SharedProperties
{
    namespace Customization
    {
        public enum Position
        {
            Invisible,
            Top,
            Bottom,
            Right,
            Left,
            Center
        }

        public interface IPlugin
        {
            /// <summary>
            /// Advise the interface on its position
            /// Top:1, Bottom:2, Right:3, Left:4, Center:5
            /// 444111111111333
            /// 444111111111333
            /// 444555555555333
            /// 444555555555333
            /// 444555555555333
            /// 444555555555333
            /// 444222222222333
            /// 444222222222333
            /// </summary>
            Position    Position { get; }
            bool        Optional { get; }

            string      PluginName { get; }
            Uri         PluginIcon { get; }
        }
    }
}