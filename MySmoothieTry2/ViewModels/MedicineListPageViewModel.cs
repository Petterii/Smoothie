using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using MySmoothieTry2.Model;
using MySmoothieTry2.Views;
using Realms;
using Realms.Sync;
using Xamarin.Forms;
using static MySmoothieTry2.Constants;

namespace MySmoothieTry2.ViewModels
{
    public class SmoothieListPageViewModel : BaseViewModel
    {

        private Realm _realm;

        public ICommand AddCommand { private set; get; }
        public ICommand DeleteCommand { private set; get; }

        private IEnumerable<Smoothie> smoothies;
        public IEnumerable<Smoothie> Smoothies
        {
            get
            {
                return smoothies;
            }
            set
            {
                smoothies = value;
                SetProperty(ref smoothies, value);
            }
        }

    
        public SmoothieListPageViewModel()
        {

            initICommands();

            Initialize().IgnoreResult();

        }


        private async Task Initialize()
        {
            _realm = await RealmFunctions.OpenRealm();
            Smoothies = _realm.All<Smoothie>().OrderBy(m => m.Name);
          //  string x = "hello";
        }




        private void initICommands()
        {
            DeleteCommand = new Command(
              execute: (item) =>
              {
                  // TODO Delete from realm
                  RealmFunctions.DeleteItem(_realm, (RealmObject)item);

              },
              canExecute: (item) => true
              );

            AddCommand = new Command(
                execute: () =>
                {
                    Singleton store = Singleton.Instance;
                    store.CURRENT_SMOOTHIE_ID = null;
                    Application.Current.MainPage.Navigation.PushAsync(new EditSmoothieItemPage());
                },
                canExecute: () => true
                );

          
        }
  
        Smoothie selectedItem;
        public Smoothie SelectedItem
        {
            get
            {
                return selectedItem;
            }
            set
            {
                Singleton store = Singleton.Instance;
                store.CURRENT_SMOOTHIE_ID = value.Id;
                selectedItem = value;
                if (value != null)
                {
                    Application.Current.MainPage.Navigation.PushAsync(new EditSmoothieItemPage());
                }
            }
        }

    }
 
}

