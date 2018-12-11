using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace MySmoothieTry2.Views
{
    public partial class EditSmoothieItemPage : ContentPage
    {
        public EditSmoothieItemPage()
        {
            InitializeComponent();

            //smoothieNameCell.Keyboard = Keyboard.Create(KeyboardFlags.None);

            ingredientName.Keyboard = Keyboard.Create(KeyboardFlags.None);

            descriptionCell.Keyboard = Keyboard.Create(KeyboardFlags.None);
        }
    }
}
