using System;
using Realms;

namespace MySmoothieTry2.Model
{
    public class MedicineItem : RealmObject
    {
        [PrimaryKey]
        public string Id { get; set; }

        public string BrandName { get; set; }
        public string Description { get; set; }
        public string SideEffects { get; set; }
        public string Dosage { get; set; }
    }

    public  class Ingredient
    {
        public string name;
        public int kcal;
    }
}
