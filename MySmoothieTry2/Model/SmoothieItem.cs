using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Realms;

namespace MySmoothieTry2.Model
{
    public class Smoothie : RealmObject
    {
        [PrimaryKey]
        public string Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public string UrlImage { get; set; }
        public int Kcal { get; set; }

        public IList<Ingredient> Ingredients { get; }

        [Ignored]
        public string UrlImageI
        {
            get
            {
                if (UrlImage == null)
                {
                    return "smoothie.png";
                }
                return UrlImage;
            }
            set
            {
                UrlImage = value;
            }
        }
    }

    public  class Ingredient: RealmObject
    {
        public string Name { get; set; }
        // public int kcal;
    }
}
