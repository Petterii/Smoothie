using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Firebase.Storage;
using Realms;
using Realms.Sync;
using static MySmoothieTry2.Constants;

namespace MySmoothieTry2.Model
{
    public class RealmFunctions
    {

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

        public static void DeleteItem(Realm _realm, RealmObject item)
        {
             _realm.Write(() =>
            {
                _realm.Remove((RealmObject)item);
            });
        }

        public static void SaveItem(Realm _realm, RealmObject item)
        {
            _realm.Write(() =>
            {
                _realm.Add(item, update: true);
            });
        }

        public static void AddIngredient(Realm _realm, Smoothie item, string value)
        {
            using (var trans = _realm.BeginWrite())
            {
                item.Ingredients.Add(new Ingredient() { Name = value });
                trans.Commit();
            }
        }


        public static void AddKcal(Realm _realm, Smoothie item, int kcal)
        {
            using (var trans = _realm.BeginWrite())
            {
                item.Kcal = kcal;
                trans.Commit();
            }
        }

        public static void AddImage(Realm _realm, Smoothie item, string imgurl)
        {
            using (var trans = _realm.BeginWrite())
            {
                item.UrlImageI = imgurl;
                trans.Commit();
            }
        }

        // should probbly be in a new class .. ex. FirebaseFunctions
        public static async Task<string> StoreImages(Stream imageStream)
        {
            var storageImage = await new FirebaseStorage("smoothieapp-e6257.appspot.com")
                .Child("Smoothies")
                .Child(Guid.NewGuid().ToString() + ".jpg")
                .PutAsync(imageStream, new CancellationToken(), "image/jpeg");


            string imgurl = storageImage;
            return imgurl;
        }
    }
}
