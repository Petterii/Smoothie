using System;
using System.Collections.Generic;
using MySmoothieTry2.Model;
using MySmoothieTry2.ViewModels;
using Xamarin.Forms;
using Plugin.DownloadManager;
using Plugin.DownloadManager.Abstractions;
using System.Threading.Tasks;

namespace MySmoothieTry2.Views
{
    public partial class MedicineListPage : ContentPage
    {


        public MedicineListPage()
        {
            InitializeComponent();

            ImageDownload.initDownload();
        //    string url = "https://firebasestorage.googleapis.com/v0/b/smoothieapp-e6257.appspot.com/o/XamarinMonkeys%2Fimage1.jpg?alt=media&token=adba0120-0f3a-4fff-a1fd-47b71ac9fe9a";

        //    ImageDownload.DownloadFile(url);
        }

    }
}
