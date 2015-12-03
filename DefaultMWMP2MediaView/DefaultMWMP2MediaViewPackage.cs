using System;
using System.ComponentModel.Composition;
using SharedProperties.Interfaces;

namespace DefaultMWMP2MediaView
{
    // ReSharper disable once InconsistentNaming
    [Export(typeof(IMediaViewerPackage))]
    public class DefaultMWMP2MediaViewPackage : IMediaViewerPackage
    {
        #region IMediaViewerPackage Implementation

        public string MediaViewerPackageName { get; }

        public IMediaViewer GetMediaViewerForFile()
        {
            throw new NotImplementedException();
        }

        #endregion


        public DefaultMWMP2MediaViewPackage()
        {
            MediaViewerPackageName = "DefaultMWMP2mediaview";
        }
    }
}