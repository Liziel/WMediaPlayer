using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Windows;
using System.Windows.Controls;
using PluginLibrary.Customization;

namespace MyWindowsMediaPlayerv2
{
    public class ViewAnchorer
    {
        private class Anchor
        {
            #region ContentControls Properties

            private readonly ContentControl _allControl = new ContentControl();
            private readonly ContentControl _leftControl = new ContentControl();
            private readonly ContentControl _centerControl = new ContentControl();
            private readonly ContentControl _rightControl = new ContentControl();

            #endregion

            #region Grid Property

            private readonly Grid _grid = new Grid
            {
                ColumnDefinitions =
                {
                    new ColumnDefinition {Width = new GridLength(1, GridUnitType.Auto)},
                    new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star)},
                    new ColumnDefinition {Width = new GridLength(1, GridUnitType.Auto)}
                },
            };

            public UIElement RootElement => _grid;

            #endregion

            #region Constructor

            public Anchor(int zindex)
            {
                Grid.SetColumn(_leftControl, 0);
                Grid.SetColumn(_centerControl, 1);
                Grid.SetColumn(_rightControl, 2);

                _grid.Children.Add(_allControl);
                _grid.Children.Add(_leftControl);
                _grid.Children.Add(_centerControl);
                _grid.Children.Add(_rightControl);

                Panel.SetZIndex(RootElement, zindex);
            }

            #endregion

            #region Attach Plugin

            public void AttachPlugin(IPlugin plugin, Position position)
            {
                switch (position)
                {
                    case Position.All:
                        _allControl.Content = plugin;
                        break;
                    case Position.Center:
                        _centerControl.Content = plugin;
                        break;
                    case Position.Left:
                        _leftControl.Content = plugin;
                        break;
                    case Position.Right:
                        _rightControl.Content = plugin;
                        break;
                }
            }

            #endregion
        }

        #region RootElement

        private Grid _rootElement = new Grid();
        public Grid RootElement => _rootElement;

        #endregion

        #region Properties

        private readonly Dictionary<int, Anchor> _anchorsByZindex = new Dictionary<int, Anchor>();

        #endregion

        public void AttachPlugin(IPlugin plugin)
        {
            if (!_anchorsByZindex.ContainsKey(plugin.Layer))
            {
                _anchorsByZindex[plugin.Layer] = new Anchor(plugin.Layer);
                RootElement.Children.Add(_anchorsByZindex[plugin.Layer].RootElement);
            }
            _anchorsByZindex[plugin.Layer].AttachPlugin(plugin, plugin.Position);
        }

        public void ForceAttachPlugin(IPlugin plugin, Position position, int layer)
        {
            if (!_anchorsByZindex.ContainsKey(layer))
            {
                _anchorsByZindex[layer] = new Anchor(layer);
                RootElement.Children.Add(_anchorsByZindex[layer].RootElement);
            }
            _anchorsByZindex[layer].AttachPlugin(plugin, position);
        }
    }
}