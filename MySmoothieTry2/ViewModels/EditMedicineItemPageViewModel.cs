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
            _realm = await RealmFunctions.OpenRealm();


             Singleton store = Singleton.Instance;
             CURRENT_SMOOTHIE_ID = store.CURRENT_SMOOTHIE_ID;
             store.CURRENT_SMOOTHIE_ID = null;

            Smoothie = _realm.Find<Smoothie>(CURRENT_SMOOTHIE_ID);
            if (CURRENT_SMOOTHIE_ID != null)
            {
                Smoothie = _realm.Find<Smoothie>(CURRENT_SMOOTHIE_ID);

                if (Smoothie.UrlImage == null)
                {
                    ThisImage = CAMERABUTTONIMAGE;
                }
                else ThisImage = Smoothie.UrlImage; 
            }
            else 
            {
                Smoothie = new Smoothie();
                Smoothie.Id = Guid.NewGuid().ToString();
             
                ThisImage = CAMERABUTTONIMAGE;
            }
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

