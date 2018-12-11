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
                //smoothies = value;
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
            _realm = await OpenRealm();
            Smoothies = _realm.All<Smoothie>().OrderBy(m => m.Name);
          //  string x = "hello";
        }

        public static async Task<Realm> OpenRealm()
        {
            var user = User.Current;
            if (user != null)
            {
                var config = new FullSyncConfiguration(new Uri(REALMPATH, UriKind.Relative), user);
                // User has already logged in, so we can just load the existing data in the Realm.
                return Realm.GetInstance(config);
            }
            var credentials = Credentials.UsernamePassword(USERNAME, PASSWORD, createUser: false);
            user = await User.LoginAsync(credentials, new Uri(Constants.AuthUrl));
            var configuration = new FullSyncConfiguration(new Uri(REALMPATH, UriKind.Relative), user);
            // First time the user logs in, let's use GetInstanceAsync so we fully download the Realm
            // before letting them interract with the UI.
            var realm = await Realm.GetInstanceAsync(configuration);
            return realm;
        }


        private void initICommands()
        {
            DeleteCommand = new Command(
              execute: (item) =>
              {
                    // TODO Delete from realm
                    _realm.Write(() =>
                  {
                      _realm.Remove((RealmObject)item);
                  });

              
              },
              canExecute: (item) => true
              );

            AddCommand = new Command(
                execute: () =>
                {
                    Singleton store = Singleton.Instance;
                    store.SelectedItem = null;
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
                store.SelectedItem = value;
                selectedItem = value;
                if (value != null)
                {
                    Application.Current.MainPage.Navigation.PushAsync(new EditSmoothieItemPage());
                }
            }
        }



    }
 
}

