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
    public partial class SmoothieListPage : ContentPage
    {


        public SmoothieListPage()
        {
            InitializeComponent();

            ImageDownload.initDownload();
   
        }

    }
}
