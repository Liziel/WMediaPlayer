using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SharedProperties.Interfaces;

namespace DefaultMWMP2MediaView
{
    /// <summary>
    /// Interaction logic for MediaViewer.xaml
    /// </summary>
    [Export(typeof(IMediaViewerPackage))]
    public partial class MediaViewer : UserControl, IMediaViewerPackage
    {
        public string MediaViewerPackageName { get; }


        public MediaViewer()
        {
            InitializeComponent();
            MediaViewerPackageName = "DefaultMWMP2mediaview";
        }
    }
}
