namespace SharedProperties
{
    namespace Interfaces
    {
        public interface IMediaViewer
        {
                        
        }

        public interface IMediaViewerPackage
        {
            string MediaViewerPackageName { get; }

            IMediaViewer GetMediaViewerForFile();
        }
    }
}