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

        //public ObservableCollection<Ingredient> Ingredients;
        public IList<Ingredient> Ingredients { get; }
        //public RealmList<Ingredient> Ingredients { get; }

    }

    public  class Ingredient: RealmObject
    {
        public string Name { get; set; }
        // public int kcal;
    }
}
