using System;
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
            public List<Lazy<IToolBar>> ListToolBars { get; set; }

            [ImportMany(typeof (IMediaDisplay), AllowRecomposition = true)]
            public IEnumerable<Lazy<IMediaDisplay>> ListMediaDisplays { get; set; }

            [ImportMany(typeof (IExternalView), AllowRecomposition = true)]
            public IEnumerable<Lazy<IExternalView>> ListExternalViews { get; set; }

            #endregion

            #region WPFContent

            private IMediaDisplay _display = null;

            public IMediaDisplay Display
            {
                get { return _display; }
                private set
                {
                    _display = value;
                    RaisePropertyChanged(nameof(Display));
                }
            }

            private IToolBar _toolbar = null;

            public IToolBar ToolBar
            {
                get { return _toolbar; }
                private set
                {
                    _toolbar = value;
                    RaisePropertyChanged(nameof(ToolBar));
                }
            }

            private IExternalView _externalView = null;

            public IExternalView ExternalView
            {
                get { return _externalView; }
                private set
                {
                    _externalView = value;
                    RaisePropertyChanged(nameof(ExternalView));
                }
            }

            #endregion

            #region INotifyPropertyChanged Members

            public event PropertyChangedEventHandler PropertyChanged;

            #endregion

            #region Methods

            public void SetupBaseConfiguration()
            {
                _pluginConfiguration = new Configuration.PluginConfiguration();
                Display = ListMediaDisplays.FirstOrDefault(i => i.Value.Name == _pluginConfiguration.MediaViewPlugin)?.Value;
                ToolBar = ListToolBars.FirstOrDefault(i => i.Value.Name == _pluginConfiguration.ToolBarPlugin)?.Value;
                System.Diagnostics.Debug.WriteLine("ToolBars dll :" + ListToolBars.Count);
            }

            public void LoadPlugin(string path, string pattern)
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
            protected virtual void RaisePropertyChanged([CallerMemberName] string propertyName = null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
