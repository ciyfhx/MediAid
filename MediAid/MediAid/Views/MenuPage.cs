using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace MediAid.Views
{
    public class MenuPage : ContentPage
    {
        public MenuPage()
        {
            Content = new StackLayout
            {
                Children = {
                    new Label { Text = "Settings" }
                }
            };
        }


    }
}
