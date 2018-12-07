using System;
using System.Threading.Tasks;
using Plugin.DownloadManager;
using Plugin.DownloadManager.Abstractions;
using Xamarin.Forms;

namespace MySmoothieTry2.Model
{
    public class ImageDownload
    {
       // public static IDownloadFile File;
        static bool isDownloading = true;

        //string url = ""https://firebasestorage.googleapis.com/v0/b/smoothieapp-e6257.appspot.com/o/XamarinMonkeys%2Fimage1.jpg?alt=media&token=adba0120-0f3a-4fff-a1fd-47b71ac9fe9a"";

        //ImageDownload.DownloadFile(url);

        public static void initDownload() {

            CrossDownloadManager.Current.CollectionChanged += (sender, e) =>
            System.Diagnostics.Debug.Write(
            "[DownloadManager] " + e.Action +
            " -> New Item: " + (e.NewItems?.Count ?? 0) +
            " at " + e.NewStartingIndex +
            " || old items: " + (e.OldItems?.Count ?? 0) +
            " at " + e.OldStartingIndex
                );
        }

        public static async Task DownloadFile(string FileName)
        {

            await Task.Yield();
         
            await Task.Run(() =>
            {
                var downloadManager = CrossDownloadManager.Current;
                var file = downloadManager.CreateDownloadFile(FileName);
                downloadManager.Start(file, true);

                while (isDownloading)
                {
                    isDownloading = IsDownloading(file);
                }

            });

            if (!isDownloading)
            {
                await Application.Current.MainPage.DisplayAlert("File Status", "File Downloaded", "OK");
            }
        }

        private static bool IsDownloading(IDownloadFile File)
        {
          
            if (File == null) return false;

            switch (File.Status)
            {
                case DownloadFileStatus.INITIALIZED:
                case DownloadFileStatus.PAUSED:
                case DownloadFileStatus.PENDING:
                case DownloadFileStatus.RUNNING: return true;

                case DownloadFileStatus.COMPLETED:
                case DownloadFileStatus.CANCELED:
                case DownloadFileStatus.FAILED: return false;

                default: throw new ArgumentOutOfRangeException();

            }
        }

        //public void AbortDownloading()
        //{
        //    CrossDownloadManager.Current.Abort(File);
        //}
    }
}
