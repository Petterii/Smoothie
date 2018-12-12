namespace MySmoothieTry2
{
    public static class Constants
    {
        // Realm
        public static string AuthUrl = "https://petes-bananas-workshop.de1a.cloud.realm.io";
        public static string REALMPATH = "Movies";
        public static string USERNAME = "test";
        public static string PASSWORD = "test";

        // Add/Edit Smoothie
        public const string SAVETITLE = "Save Smoothie";
        public const string SAVEPROMPT = "Proceed and save changes?";
        public const string OKBUTTONTITLE = "OK";
        public const string CANCELBUTTONTITLE = "Cancel";

        public const string ERRORTITLE = "Error";
        public const string ERRORPROMPT = "Name is required!";

        public const string SAVEBUTTONTITLE = "Save";

        public const string CAMERABUTTONIMAGE = "ButtonCamera.png";
        public const string CAMERAUNAVAILABLE = "Camera isn't available...";

        public const string SMOOTHIEIMAGE = "smoothie.png";

        // Image downloading
        public const string DOWNLOADTITLE = "File Status";
        public const string DOWNLOADPROMPT = "File Downloaded";

        // App description in About tab
        public static string APPDESC = "This app allows you to upload pictures of your favourite smoothies to Firebase.\nYou must choose a photo from your gallery.\nSave the ingredients used for future reference!";

        // URL of REST service for SmoothieRestItem
        //public static string RestUrl = "https://developer.xamarin.com:8081/api/todoitems/{0}";
        public static string RestUrl = "https://smoothieexpress-82203.firebaseapp.com/smoothies";
        // Credentials that are hard coded into the REST service
        public static string Username = "Xamarin";
        public static string Password = "Pa$$w0rd";
    }
}
