using System;
using System.Collections.Generic;

namespace MySmoothieTry2.Model
{
    public class Singleton
    {
        //private IEnumerable<MedicineItem> smoothies;
        //public IEnumerable<MedicineItem> Smoothies { 
        //get { return smoothies; }

        //set {
        //    if (smoothies== null)
        //        {
        //            smoothies = value;       
        //        }
        //    }
        //}

        private IEnumerable<SmoothieItem> smoothies;
        public IEnumerable<SmoothieItem> Smoothies
        {
            get { return smoothies; }

            set
            {
                if (smoothies == null)
                {
                    smoothies = value;
                }
            }
        }

        //private MedicineItem selectedItem;
        //public MedicineItem SelectedItem
        //{
        //    get { return selectedItem; }
        //    set
        //    {
        //        selectedItem = value;
        //    }
        //}

        private SmoothieItem selectedItem;
        public SmoothieItem SelectedItem
        {
            get { return selectedItem; }
            set
            {
                selectedItem = value;
            }
        }

        private static Singleton instance;
  
        private Singleton() { }

        public static Singleton Instance
        {
            get {
                if (instance == null)
                    instance = new Singleton();
                return instance;
            }
        }
    }
}
