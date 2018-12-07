using System;
using Firebase.Storage;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using MySmoothieTry2.Model;
using MySmoothieTry2.Validations;
using Plugin.Media;
using Realms;
using Realms.Sync;
using Xamarin.Forms;
using static MySmoothieTry2.Constants;
using Plugin.Media.Abstractions;
using System.Threading;



namespace MySmoothieTry2.ViewModels
{
    public class EditMedicineItemPageViewModel : BaseViewModel
    {


        IsNotNullOrEmptyRule<string> rule = new IsNotNullOrEmptyRule<string>();

        private Realm _realm;

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
                    Console.WriteLine(CAMERAUNAVAILABLE);
                }
            }, 
            canExecute: () => true);

            SaveCommand = new Command(
                execute: () =>
                {
                     SaveToDatabase();
                   //  await getImage();

                },
                canExecute: () => true
                );

            Singleton store = Singleton.Instance;
            selectedItem = store.SelectedItem;
            if (selectedItem != null)
            {
                BrandNameE = selectedItem.Name;
                DescriptionE = selectedItem.Description;
                if (selectedItem.UrlImage == null)
                {
                    ThisImage = "ButtonCamera.png";
                }
                else ThisImage = selectedItem.UrlImage;

            }
            else ThisImage = "ButtonCamera.png";




            Initialize().IgnoreResult();

        }

   

        MediaFile file;

        public async void TakePhoto()
        {
            await CrossMedia.Current.Initialize();
            try
            {
                var options = new StoreCameraMediaOptions() { };
                file = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions
                {
                    PhotoSize = PhotoSize.Medium
                });
 //               var photo = await CrossMedia.Current.TakePhotoAsync(options);
                ThisImage = await StoreImages(file.GetStream());
              

            }
            catch (Exception e)
            {
                Console.WriteLine($"{e.ToString()}");
            }
        }

        public async Task<string> StoreImages(Stream imageStream)
        {
            var storageImage = await new FirebaseStorage("smoothieapp-e6257.appspot.com")
                .Child("Smoothies")
                .Child(Guid.NewGuid().ToString() + ".jpg")
                .PutAsync(imageStream, new CancellationToken(),"image/jpeg");


            string imgurl = storageImage;
            return imgurl;
        }

  


        internal void SaveToDatabase()
        {
            if (rule.Check(BrandNameE) && rule.Check(DescriptionE))
            {
                _realm.WriteAsync((tempRealm) =>
                {
                    OnPropertyChanged("BrandNameE");

                    SmoothieItem newItem = new SmoothieItem();

                    newItem.Name = BrandNameE;
                    newItem.Id = Guid.NewGuid().ToString();
                    newItem.Description = DescriptionE;

                    if (ThisImage != null)
                    {
                        newItem.UrlImage = ThisImage;
                    }


                    tempRealm.Add(newItem, true);
                });

                Application.Current.MainPage.Navigation.PopAsync();
            }
            else
            {

                Application.Current.MainPage.DisplayAlert(ERRORTITLE,
                                      ERRORPROMPT,
                                      OKBUTTONTITLE);
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

     
        private string thisImage;
        public string ThisImage
        {
            get
            {
                return thisImage;
            }
            set
            {
                if (!value.Equals(thisImage))
                {
                    SetProperty(ref thisImage, value);
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

