using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using MySmoothieTry2.Model;
using MySmoothieTry2.Validations;
using Plugin.Media;
using Realms;
using Realms.Sync;
using Xamarin.Forms;

namespace MySmoothieTry2.ViewModels
{
    public class EditMedicineItemPageViewModel : BaseViewModel
    {

        const string SAVETITLE = "Save Smoothie";
        const string SAVEPROMPT = "Proceed and save changes?";
        const string OKBUTTONTITLE = "OK";
        const string CANCELBUTTONTITLE = "Cancel";

        const string ERRORTITLE = "Error";
        const string ERRORPROMPT = "Name and Description are required.";

        const string SAVEBUTTONTITLE = "Save";


        IsNotNullOrEmptyRule<string> rule = new IsNotNullOrEmptyRule<string>();

        private Realm _realm;

        // private IEnumerable<MedicineItem> smoothie;
        // public IEnumerable<MedicineItem> Smoothie
        private IEnumerable<SmoothieItem> smoothie;
        public IEnumerable<SmoothieItem> Smoothie
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

        private async Task Initialize()
        {
            _realm = await OpenRealm();
            //Smoothie = _realm.All<MedicineItem>().OrderBy(m => m.BrandName);
            Smoothie = _realm.All<SmoothieItem>().OrderBy(m => m.Name);

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
            var credentials = Credentials.UsernamePassword("test", "testt", createUser: false);
            user = await User.LoginAsync(credentials, new Uri(Constants.AuthUrl));
            var configuration = new FullSyncConfiguration(new Uri(Constants.RealmPath, UriKind.Relative), user);
            // First time the user logs in, let's use GetInstanceAsync so we fully download the Realm
            // before letting them interract with the UI.
            var realm = await Realm.GetInstanceAsync(configuration);
            return realm;
        }

        public EditMedicineItemPageViewModel()
        {
            UseCameraCommand = new Command(execute: () => {
                if (CrossMedia.Current.IsCameraAvailable)
                {
                    TakePhoto();
                }
                else 
                {
                    Console.WriteLine("Camera isn't available...");
                }
            }, 
            canExecute: () => true);

            SaveCommand = new Command(
                execute: () =>
                {
                    if (rule.Check(BrandNameE) && rule.Check(DescriptionE))
                    //if (App.SelectedSmoothie != null && !string.IsNullOrEmpty(App.SelectedSmoothie.Name) && !string.IsNullOrEmpty(App.SelectedSmoothie.Description))
                    {
                        _realm.WriteAsync((tempRealm) =>
                        {
                            OnPropertyChanged("BrandNameE");
                            //MedicineItem newItem = new MedicineItem();
                            SmoothieItem newItem = new SmoothieItem();
                            //newItem.BrandName = BrandNameE;
                            //newItem.Id = Guid.NewGuid().ToString();
                            //newItem.Description = DescriptionE;
                            newItem.Name = BrandNameE;
                            newItem.Id = Guid.NewGuid().ToString();
                            newItem.Description = DescriptionE;
                            //newItem.SideEffects = SideEffectsE;
                            //newItem.Dosage = "Every Second";
                            //       newItem.DateDoseTaken = new DateTimeOffset();
                            tempRealm.Add(newItem, true);
                        });

                        Application.Current.MainPage.Navigation.PopAsync();
                    }
                    else
                    {
                        // TODO Alert Window

                        Application.Current.MainPage.DisplayAlert(ERRORTITLE,
                                              ERRORPROMPT,
                                              OKBUTTONTITLE);
                    }



                },
                canExecute: () => true
                );

            Singleton store = Singleton.Instance;
            selectedItem = store.SelectedItem;
            if (selectedItem != null)
            {
                BrandNameE = selectedItem.Name;
                DescriptionE = selectedItem.Description;
            }


            Initialize().IgnoreResult();

        }

        internal async void TakePhoto()
        {
            try
            {
                //Console.WriteLine("Entered camera method!");
                var options = new Plugin.Media.Abstractions.StoreCameraMediaOptions() { };
                var photo = await CrossMedia.Current.TakePhotoAsync(options);
                
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e.ToString()}");
            }
        }

        //MedicineItem selectedItem;
        //public MedicineItem SelectedItem
        SmoothieItem selectedItem;
        public SmoothieItem SelectedItem
        {
            get
            {
                return selectedItem;
            }
            set
            {
                SetProperty(ref selectedItem, value);

                RefreshCanExecute();
            }
        }

        private string brandNameE;
        public string BrandNameE
        {
            get
            {
                return brandNameE;
            }
            set
            {
                if (!value.Equals(brandNameE))
                {
                    SetProperty(ref brandNameE, value);
                    RefreshCanExecute();
                }
            }
        }

        private string descriptionE;
        public string DescriptionE
        {
            get
            {
                return descriptionE;
            }
            set
            {
                if (!value.Equals(descriptionE))
                {
                    SetProperty(ref descriptionE, value);
                    RefreshCanExecute();
                }
            }
        }

        //private string sideEffectsE;
        //public string SideEffectsE
        //{
        //    get
        //    {
        //        return sideEffectsE;
        //    }
        //    set
        //    {
        //        if (!value.Equals(sideEffectsE))
        //        {
        //            SetProperty(ref sideEffectsE, value);
        //            RefreshCanExecute();
        //        }
        //    }
        //}

        //private string dosageE;
        //public string DosageE
        //{
        //    get
        //    {
        //        return dosageE;
        //    }
        //    set
        //    {
        //        if (!value.Equals(dosageE))
        //        {
        //            SetProperty(ref dosageE, value);
        //            RefreshCanExecute();
        //        }
        //    }
        //}

        public ICommand AddCommand { private set; get; }
        public ICommand SaveCommand { private set; get; }
        public ICommand UseCameraCommand { private set; get; }

        // Tell all buttons to check their canexecute status again
        private void RefreshCanExecute()
        {
            (SaveCommand as Command).ChangeCanExecute();
        }
    }
    public static class Extensions
    {
        public static void IgnoreResult(this Task task)
        {
            // This just silences the warnings when tasks are not awaited.
        }
    }
}

