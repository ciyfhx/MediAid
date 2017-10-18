using MediAid.Models;
using MediAid.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
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
            InitializeComponent();

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
            DeleteImage();
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

            //Get Predictions
            //var predictions = await SendImageForPredictionAsync(file.Path);
            //var highest = predictions.GetHighest();
            //viewModel.DrugTypeName = highest.Item1.Name;
            //DrugsPicker.SelectedItem = highest.Item1.Name;

            Image.Source = newImage;

        }

        private async Task<Predictions> SendImageForPredictionAsync(string file)
        {
            WebRequest webrequest = WebRequest.Create(new Uri(""));
            webrequest.ContentType = "application/x-www-form-urlencoded";
            webrequest.Method = "POST";

            //Send Image
            byte[] imgData = File.ReadAllBytes(file);
            using (Stream stream = webrequest.GetRequestStream())
            {
                await stream.WriteAsync(imgData, 0, imgData.Length);
            }

            //Get Response
            var res = (HttpWebResponse)webrequest.GetResponse();
            var resStream = res.GetResponseStream();
            var reader = new JsonTextReader(new StreamReader(resStream));

            return new JsonSerializer().Deserialize<Predictions>(reader);

        }


        protected override bool OnBackButtonPressed()
        {
            DeleteImage();
            return base.OnBackButtonPressed();
        }

        private void DeleteImage()
        {
            //Delete image if drug not created
            if (!save && viewModel.Drug.ImageFile != null) File.Delete(viewModel.Drug.ImageFile);
        }

    }

    public class Predictions
    {
        public double capsules { get; set; }
        public double syrup { get; set; }
        public double pills { get; set; }
        public double inhaler { get; set; }




    }

    public static class PredictionsExtension{

        public static Tuple<DrugType, double> GetHighest(this Predictions predictions)
        {
            if (predictions.GetType() != typeof(Predictions)) throw new InvalidCastException("Pass a Predictions type!");

            var type = predictions.GetType();



            //var list = type.GetProperties().Select(prop => (double)type.GetProperty(prop.Name).GetValue(predictions, null)).ToList();
            //list.Sort();
            //Debug.WriteLine(list);


            double highest = 0;
            string name = null;

            //Use Reflection
            foreach (var prop in type.GetProperties())
            {
                double current = ((double)type.GetProperty(prop.Name).GetValue(predictions, null));
                if (current >= highest)
                {
                    highest = current;
                    name = prop.Name;
                }
            }
            
            return Tuple.Create(DrugType.FromString(name), highest);
        }

    }


}