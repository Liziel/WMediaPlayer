using System;
using System.Collections.Generic;
using System.Linq;
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
                ColumnDefinitions = { new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) } },
                RowDefinitions = { new RowDefinition {Height = new GridLength(1, GridUnitType.Star)} },
            };

            private readonly Grid _divisedGrid = new Grid
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
                Grid.SetColumn(_allControl, 0);
                Grid.SetRow(_allControl, 0);
                Grid.SetColumn(_divisedGrid, 0);
                Grid.SetRow(_divisedGrid, 0);

                Grid.SetColumn(_leftControl, 0);
                Grid.SetColumn(_centerControl, 1);
                Grid.SetColumn(_rightControl, 2);

                _grid.Children.Add(_allControl);
                _grid.Children.Add(_divisedGrid);

                _divisedGrid.Children.Add(_leftControl);
                _divisedGrid.Children.Add(_centerControl);
                _divisedGrid.Children.Add(_rightControl);

                Panel.SetZIndex(RootElement, zindex);
            }

            #endregion

            #region Attach Plugin

            public bool Empty() =>
                _allControl.Content == null && _centerControl.Content == null && _leftControl.Content == null &&
                _rightControl.Content == null;

            public bool RemovePlugin(IViewPlugin viewPlugin)
            {
                _allControl.Content = _allControl.Content == viewPlugin ? null : _allControl.Content;
                _rightControl.Content = _rightControl.Content == viewPlugin ? null : _rightControl.Content;
                _centerControl.Content = _centerControl.Content == viewPlugin ? null : _centerControl.Content;
                _leftControl.Content = _leftControl.Content == viewPlugin ? null : _leftControl.Content;
                return Empty();
            }

            public void AttachPlugin(IViewPlugin viewPlugin, Position position)
            {
                switch (position)
                {
                    case Position.All:
                        _allControl.Content = viewPlugin;
                        break;
                    case Position.Center:
                        _centerControl.Content = viewPlugin;
                        break;
                    case Position.Left:
                        _leftControl.Content = viewPlugin;
                        break;
                    case Position.Right:
                        _rightControl.Content = viewPlugin;
                        break;
                }
            }

            #endregion
        }

        #region RootElement

        public Grid RootElement { get; } = new Grid();

        #endregion

        #region Properties

        private readonly Dictionary<int, Anchor> _anchorsByZindex = new Dictionary<int, Anchor>();

        #endregion

        public void AttachPlugin(IViewPlugin viewPlugin)
        {
            if (!_anchorsByZindex.ContainsKey(viewPlugin.Layer))
            {
                _anchorsByZindex[viewPlugin.Layer] = new Anchor(viewPlugin.Layer);
                RootElement.Children.Add(_anchorsByZindex[viewPlugin.Layer].RootElement);
            }
            _anchorsByZindex[viewPlugin.Layer].AttachPlugin(viewPlugin, viewPlugin.Position);
        }

        public void ForceAttachPlugin(IViewPlugin viewPlugin, Position position, int layer)
        {
            if (!_anchorsByZindex.ContainsKey(layer))
            {
                _anchorsByZindex[layer] = new Anchor(layer);
                RootElement.Children.Add(_anchorsByZindex[layer].RootElement);
            }
            _anchorsByZindex[layer].AttachPlugin(viewPlugin, position);
        }

        public void PutPluginOnTop(IViewPlugin viewPlugin)
        {
            int highest = _anchorsByZindex.Aggregate((kv, rkv) => kv.Key > rkv.Key ? kv : rkv).Key + 1;
            ForceAttachPlugin(viewPlugin, viewPlugin.Position, highest);
        }

        public void DesattachPlugin(IViewPlugin viewPlugin)
        {
            foreach (var kv in _anchorsByZindex.Where(kv => kv.Value.RemovePlugin(viewPlugin)).ToList())
                _anchorsByZindex.Remove(kv.Key);
        }
    }
}