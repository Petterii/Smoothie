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

namespace MySmoothieTry2.ViewModels
{
    public class MedicineListPageViewModel : BaseViewModel
    {

        private Realm _realm;

        private IEnumerable<MedicineItem> smoothies;
        public IEnumerable<MedicineItem> Smoothies
        {
            get
            {
                return smoothies;
            }
            set
            {
                SetProperty(ref smoothies, value);
            }
        }

        private async Task Initialize()
        {
            _realm = await OpenRealm();
            Smoothies = _realm.All<MedicineItem>().OrderBy(m => m.BrandName);


        }

        private async Task<Realm> OpenRealm()
        {
            var user = User.Current;
            if (user != null)
            {
                var config = new FullSyncConfiguration(new Uri(Constants.RealmPath, UriKind.Relative), user);
                // User has already logged in, so we can just load the existing data in the Realm.
                return Realm.GetInstance(config);
            }
            var credentials = Credentials.UsernamePassword("test", "test", createUser: false);
            user = await User.LoginAsync(credentials, new Uri(Constants.AuthUrl));
            var configuration = new FullSyncConfiguration(new Uri(Constants.RealmPath, UriKind.Relative), user);
            // First time the user logs in, let's use GetInstanceAsync so we fully download the Realm
            // before letting them interract with the UI.
            var realm = await Realm.GetInstanceAsync(configuration);
            return realm;
        }

        public MedicineListPageViewModel()
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
                    Application.Current.MainPage.Navigation.PushAsync(new EditMedicineItemPage());
                },
                canExecute: () => true
                );

     

            Initialize().IgnoreResult();

        }

        MedicineItem selectedItem;
        public MedicineItem SelectedItem
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
                    Application.Current.MainPage.Navigation.PushAsync(new EditMedicineItemPage());
                }
            }
        }

        public ICommand AddCommand { private set; get; }
        public ICommand DeleteCommand { private set; get; }



        //Tell all buttons to check there canexecute status again
        private void RefreshCanExecute()
        {
     
            (AddCommand as Command).ChangeCanExecute();
        }
    }
 
}

