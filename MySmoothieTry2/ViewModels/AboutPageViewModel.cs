using System.Windows.Input;
using Xamarin.Forms;
using MySmoothieTry2.Data;
using System.Collections;
using System.Collections.Generic;
using MySmoothieTry2.Model;

namespace MySmoothieTry2.ViewModels
{
    public class AboutPageViewModel : BaseViewModel
    {

        public ICommand FetchSmoothieRestItemCommand { private set; get; }

        private IEnumerable<SmoothieRestItem> smoothie;
        public IEnumerable<SmoothieRestItem> Smoothie
        {
            get
            {
                return smoothie;
            }
            set
            {
                SetProperty(ref smoothie, value);
            }
        }

        public AboutPageViewModel()
        {


            FetchSmoothieRestItemCommand = new Command(
            execute: async () =>
            {
            // you code
            var Manager = new SmoothieRestItemManager(new RestService());
                var result = await Manager.GetTasksAsync();

                Smoothie = result;

            });
        }

    }
}
