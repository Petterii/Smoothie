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
    public class EditSmoothieItemPageViewModel : BaseViewModel
    {
        public string CURRENT_SMOOTHIE_ID;

        IsNotNullOrEmptyRule<string> rule = new IsNotNullOrEmptyRule<string>();

        Realm _realm;

        private Smoothie smoothie;
        public Smoothie Smoothie
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

        async Task Initialize()
        {
            _realm = await OpenRealm();
            Smoothie = _realm.Find<Smoothie>(CURRENT_SMOOTHIE_ID);

            Singleton store = Singleton.Instance;
            selectedSmoothie = store.SelectedItem;
            if (selectedSmoothie != null)
            {
                CURRENT_SMOOTHIE_ID = selectedSmoothie.Id;
                Smoothie = _realm.Find<Smoothie>(CURRENT_SMOOTHIE_ID);

                if (selectedSmoothie.UrlImage == null)
                {
                    ThisImage = CAMERABUTTONIMAGE;
                }
                else ThisImage = selectedSmoothie.UrlImage; 
                // ThisImage = Smoothie.UrlImage ist. för ovan?

            }
            else 
            {
                Smoothie = new Smoothie();
                Smoothie.Id = Guid.NewGuid().ToString();
                //Smoothie.Ingredients = new IList<Ingredient>();
                ThisImage = CAMERABUTTONIMAGE;
            }
        }

        private async Task<Realm> OpenRealm()
        {
            var user = User.Current;
            if (user != null)
            {
                var config = new FullSyncConfiguration(new Uri(REALMPATH, UriKind.Relative), user);
                // User has already logged in, so we can just load the existing data in the Realm.
                return Realm.GetInstance(config);
            }
            var credentials = Credentials.UsernamePassword(USERNAME, 
                                                           PASSWORD, 
                                                           createUser: false);

            user = await User.LoginAsync(credentials, new Uri(Constants.AuthUrl));
            var configuration = new FullSyncConfiguration(new Uri(REALMPATH, UriKind.Relative), user);
            // First time the user logs in, let's use GetInstanceAsync so we fully download the Realm
            // before letting them interract with the UI.
            var realm = await Realm.GetInstanceAsync(configuration);
            return realm;
        }

        public EditSmoothieItemPageViewModel()
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

            //SaveCommand = new Command(
                //execute: () =>
                //{
                //    SaveToDatabase();
                //    //  await getImage();

                //},
                //canExecute: () => true
                //);

            AddIngredientCommand = new Command(
                execute: () =>
                {
                    using (var trans = _realm.BeginWrite())
                    {
                        Smoothie.Ingredients.Add(new Ingredient() { Name = NewIngredientName });
                        trans.Commit();
                    }
                });

            SaveCommand = new Command(

                execute: () =>
                {
                    if (rule.Check(Smoothie.Name) && rule.Check(Smoothie.Description))
                    
                    {   
                        _realm.Write(() =>
                        {

                            Smoothie.UrlImage = ThisImage;
                            _realm.Add(Smoothie, update: true);
                        });

                        Application.Current.MainPage.Navigation.PopAsync();
                    }
                    else
                    {
                        Application.Current.MainPage.DisplayAlert(ERRORTITLE,
                                              ERRORPROMPT,
                                              OKBUTTONTITLE);
                    }
                },
                canExecute: () => true
                );
                
                   //  SaveToDatabase();
                   //  await getImage();

            //Singleton store = Singleton.Instance;
            //selectedItem = store.SelectedItem;
            //if (selectedItem != null)
            //{
            //    BrandNameE = selectedItem.Name;
            //    DescriptionE = selectedItem.Description;
            //    if (selectedItem.UrlImage == null)
            //    {
            //        ThisImage = "ButtonCamera.png";
            //    }
            //    else ThisImage = selectedItem.UrlImage;

            //}
            //else ThisImage = "ButtonCamera.png";

            Initialize().IgnoreResult();
        }

        MediaFile file;

        public async void TakePhoto()
        {
            await CrossMedia.Current.Initialize();
            try
            {
                file = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions
                {
                    PhotoSize = PhotoSize.Medium
                });

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

        //internal void SaveToDatabase()
        //{
        //    if (rule.Check(BrandNameE) && rule.Check(DescriptionE))
        //    {
        //        _realm.WriteAsync((tempRealm) =>
        //        {
        //            OnPropertyChanged("BrandNameE");

        //            SmoothieItem newItem = new SmoothieItem();
        //            newItem.Name = BrandNameE;
        //            newItem.Id = Guid.NewGuid().ToString();
        //            newItem.Description = DescriptionE;

        //            tempRealm.Add(newItem, true);
        //        });

        //        Application.Current.MainPage.Navigation.PopAsync();
        //    }
        //    else
        //    {

        //        Application.Current.MainPage.DisplayAlert(ERRORTITLE,
        //                              ERRORPROMPT,
        //                              OKBUTTONTITLE);
        //    }
        //}

        Smoothie selectedSmoothie;
        public Smoothie SelectedSmoothie
        {
            get
            {
                return selectedSmoothie;
            }
            set
            {
                SetProperty(ref selectedSmoothie, value);

                RefreshCanExecute();
            }
        }

        private string newIngredientName;
        public string NewIngredientName
        {
            get
            {
                return newIngredientName;
            }
            set
            {
                if (!value.Equals(newIngredientName))
                {
                    SetProperty(ref newIngredientName, value);
                    RefreshCanExecute();
                }
            }
        }
     
        private string thisImage;
        public string ThisImage
        {
            get
            {
                if (thisImage == null)
                {
                    thisImage = "smoothie.png";
                }
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

        public ICommand AddCommand { private set; get; }
        public ICommand AddIngredientCommand { private set; get; }
        public ICommand SaveCommand { private set; get; }
        public ICommand UseCameraCommand { private set; get; }

        // Tell all buttons to check their canexecute status again
        private void RefreshCanExecute()
        {
            (SaveCommand as Command).ChangeCanExecute();
            (AddIngredientCommand as Command).ChangeCanExecute();
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

