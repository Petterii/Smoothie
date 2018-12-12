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
using System.ComponentModel;
using System.Runtime.CompilerServices;
using MySmoothieTry2.ServiceHandlers;

namespace MySmoothieTry2.ViewModels
{
    public class EditSmoothieItemPageViewModel : BaseViewModel
    {
        public string CURRENT_SMOOTHIE_ID;

        IsNotNullOrEmptyRule<string> rule = new IsNotNullOrEmptyRule<string>();

        Realm _realm;


        // MILJA TEST START
        IngredientServices _ingredientServices = new IngredientServices();
        NutritionModelPOST _nutritionModelPOST = new NutritionModelPOST();
        // MILJA TEST END


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
                },
                canExecute: () => false);

            //MILJA TEST START
            AddIngredientToSmoothieCommand = new Command(
                execute: async () =>
                {
                    await InitializeGetIngredientAsync();   // Fetches FoodURI
                    NutritionPOST post = new NutritionPOST();
                    post.foodURI = FoodURI;
                    // TODO: Add and bind quantity (now hardcoded to 100)
                    _nutritionModelPOST.ingredients.Add(post);
                },
                canExecute: () => true);

            GetNutritionInfoCommand = new Command(
                execute: async () =>
                {
                    var jsonContent = _nutritionModelPOST;
                    long kcal = 0;

                    foreach (var i in _nutritionModelPOST.ingredients)
                    {
                        NutritionModelPOST model = new NutritionModelPOST();
                        model.ingredients.Add(i);
                        // just to check contents of model object, can be removed
                        NutritionPostReply = await _ingredientServices.GetNutritionDetails(model).ConfigureAwait(false);
                        kcal = kcal + NutritionPostReply.Calories;
                    }
                    Kcal = kcal;
                });
            //MILJA TEST END

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


        // MILJA TEST START
        private IngredientMainModel _ingredientMainModel; // xaml Binding
        public IngredientMainModel IngredientMainModel
        {
            get { return _ingredientMainModel; }
            set
            {
                _ingredientMainModel = value;
                OnPropertyChanged();
            }
        }

        private string _singleIngredient;
        public string SingleIngredient
        {
            get { return _singleIngredient; }
            set
            {
                _singleIngredient = value;
                //Task.Run(async () =>
                //{
                //    await InitializeGetIngredientAsync();
                //});
                OnPropertyChanged();
            }
        }

        private string _foodURI;
        public string FoodURI
        {
            get { return _foodURI; }
            set
            {
                _foodURI = value;
                OnPropertyChanged();
            }
        }

        private bool _isBusy;   // loader
        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                _isBusy = value;
                OnPropertyChanged();
            }
        }

        // Fire this when Add Ingredient button pressed
        // This code searches the API for the given ingredient (for retrieving FoodURI)
        private async Task InitializeGetIngredientAsync()
        {
            IngredientMainModel = await _ingredientServices.GetIngredientDetails(_singleIngredient);
            FoodURI = IngredientMainModel.Parsed[0].Food.Uri;
        }

        private NutritionPOSTReply _nutritionPostReply;
        public NutritionPOSTReply NutritionPostReply
        {
            get { return _nutritionPostReply; }
            set
            {
                _nutritionPostReply = value;
                OnPropertyChanged();
            }
        }

        private long _kcal;
        public long Kcal
        {
            get { return _kcal; }
            set
            {
                SetProperty(ref _kcal, value);
            }
        }
        // MILJA TEST END

        public ICommand AddCommand { private set; get; }
        public ICommand AddIngredientCommand { private set; get; }
        public ICommand SaveCommand { private set; get; }
        public ICommand UseCameraCommand { private set; get; }


        // MILJA TEST START
        public ICommand AddIngredientToSmoothieCommand { private set; get; }
        public ICommand GetNutritionInfoCommand { private set; get; }
        // MILJA TEST END


        // Tell all buttons to check their canexecute status again
        private void RefreshCanExecute()
        {
            (SaveCommand as Command).ChangeCanExecute();
            (AddIngredientCommand as Command).ChangeCanExecute();
            (AddIngredientToSmoothieCommand as Command).ChangeCanExecute();

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

