using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using MySmoothieTry2.Model;
using MySmoothieTry2.Validations;
using Realms;
using Realms.Sync;
using Xamarin.Forms;

namespace MySmoothieTry2.ViewModels
{
    public class Singleton
        {
            private IEnumerable<Smoothie> smoothies;
            public IEnumerable<Smoothie> Smoothies
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
            
            private Smoothie selectedItem;
            public Smoothie SelectedItem
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
                get
                {
                    if (instance == null)
                        instance = new Singleton();
                    return instance;
                }
            }
        }
    }

