using MediAid.Models;
using MediAid.ViewModels;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MediAid.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AddDrug : ContentPage
	{
        private AddDrugViewModel viewModel;
        private bool save = false;

		public AddDrug()
		{
			InitializeComponent ();

            BindingContext = viewModel = new AddDrugViewModel();

        }

        public async void Save()
        {
            if (!Validate()) return;
            save = true;
            viewModel.Drug.DrugTypeEnum = DrugTypeConverter.FromString(viewModel.DrugTypeName).Id;
            MessagingCenter.Send(this, "AddDrug", viewModel.Drug);
            await Navigation.PopToRootAsync();
        }

        private bool Validate()
        {
            var drug = viewModel.Drug;
            return (!String.IsNullOrEmpty(drug.Name) && viewModel.Drug.DrugType != null);
        }

        async void Take_PictureAsync(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();
            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                Debug.WriteLine("Unable to take picture");
                return;
            }

            var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
            {
                Directory = "temp",
                Name = "temp.jpg"
            });


            Debug.WriteLine("Done:" + file.Path);
            ImageSource newImage = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();
                file.Dispose();
                return stream;
            });
            Debug.WriteLine("Image Taken:" + newImage);

            //Set Drug Image
            viewModel.Drug.ImageFile = file.Path;

            Image.Source = newImage;
        }


        protected override bool OnBackButtonPressed()
        {
            //Delete image if drug not created
            Debug.WriteLine("Check delete");
            if(!save)File.Delete(viewModel.Drug.ImageFile);
            return base.OnBackButtonPressed();
        }

    }
}