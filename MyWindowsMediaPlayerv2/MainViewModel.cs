﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using MyWindowsMediaPlayerv2.Annotations;
using SharedProperties.Interfaces;

namespace MyWindowsMediaPlayerv2
{
    namespace ViewModel
    {
        class MainViewModel : INotifyPropertyChanged
        {
            #region PluginsList

            private Configuration.PluginConfiguration _pluginConfiguration;

            [ImportMany(typeof (IToolBar), AllowRecomposition = true)]
            // ReSharper disable once UnusedAutoPropertyAccessor.Local
            private List<Lazy<IToolBar>> ListToolBars { get; set; }

            [ImportMany(typeof (IMediaViewerPackage), AllowRecomposition = true)]
            private IEnumerable<Lazy<IMediaViewerPackage>> ListMediaDisplays { get; set; }

            [ImportMany(typeof (IExternalView), AllowRecomposition = true)]
            private IEnumerable<Lazy<IExternalView>> ListExternalViews { get; set; }

            #endregion

            #region WPFContent

            private IMediaViewerPackage _viewerPackage = null;

            public IMediaViewerPackage ViewerPackage
            {
                get { return _viewerPackage; }
                private set
                {
                    _viewerPackage = value;
                    OnPropertyChanged(nameof(ViewerPackage));
                }
            }

            private IToolBar _toolbar = null;

            public IToolBar ToolBar
            {
                get { return _toolbar; }
                private set
                {
                    _toolbar = value;
                    OnPropertyChanged(nameof(ToolBar));
                }
            }

            private IExternalView _externalView = null;

            public IExternalView ExternalView
            {
                get { return _externalView; }
                private set
                {
                    _externalView = value;
                    OnPropertyChanged(nameof(ExternalView));
                }
            }

            #endregion

            #region INotifyPropertyChanged Members

            public event PropertyChangedEventHandler PropertyChanged;

            #endregion

            #region Methods

            private void SetupBaseConfiguration()
            {
                _pluginConfiguration = new Configuration.PluginConfiguration();
                ViewerPackage = ListMediaDisplays.FirstOrDefault(i => i.Value.MediaViewerPackageName == _pluginConfiguration.MediaViewPlugin)?.Value;
                ToolBar = ListToolBars.FirstOrDefault(i => i.Value.ToolbarName == _pluginConfiguration.ToolBarPlugin)?.Value;
            }

            private void LoadPlugin(string path, string pattern)
            {
                DirectoryCatalog directoryCatalog = new DirectoryCatalog(path, pattern);
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                AttributedModelServices.ComposeParts(new CompositionContainer(directoryCatalog), this);
            }

            public MainViewModel()
            {
                LoadPlugin(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Plugins"), "*.dll");
                SetupBaseConfiguration();
            }

            #endregion

            [NotifyPropertyChangedInvocator]
            private void OnPropertyChanged([CallerMemberName] string propertyName = null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
