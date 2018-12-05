using System;
using System.Collections.ObjectModel;
using Realms;

namespace MySmoothieTry2.Model
{
    //public class MedicineItem : RealmObject
    //{
    //    [PrimaryKey]
    //    public string Id { get; set; }

    //    public string BrandName { get; set; }
    //    public string Description { get; set; }
    //    public string SideEffects { get; set; }
    //    public string Dosage { get; set; }
    //}

    public class SmoothieItem : RealmObject
    {
        [PrimaryKey]
        public string Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        public ObservableCollection<Ingredient> IngredientList = new ObservableCollection<Ingredient>()
        {
            new Ingredient() { Name="Milk" },
            new Ingredient() { Name="Spinach" }
        };
    }

    public  class Ingredient
    {
        public string Name { get; set; }
        // public int kcal;
    }
}
