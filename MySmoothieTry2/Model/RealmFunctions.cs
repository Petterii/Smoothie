using System;
using System.Threading.Tasks;
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

        public static async void DeleteItem(Realm _realm, RealmObject item)
        {
            await _realm.WriteAsync((t) =>
            {
                t.Remove((RealmObject)item);
            });
        }
    }
}
