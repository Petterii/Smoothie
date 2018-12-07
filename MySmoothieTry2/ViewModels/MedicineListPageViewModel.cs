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
                SetProperty(ref smoothies, value);
            }
        }

    
        public MedicineListPageViewModel()
        {

            initICommands();

            Initialize().IgnoreResult();

        }


        private async Task Initialize()
        {
            _realm = await RealmFunctions.OpenRealm();
            Smoothies = _realm.All<Smoothie>().OrderBy(m => m.Name);
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
                    Application.Current.MainPage.Navigation.PushAsync(new EditMedicineItemPage());
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
                    Application.Current.MainPage.Navigation.PushAsync(new EditMedicineItemPage());
                }
            }
        }



    }
 
}

