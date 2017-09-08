using MediAid.Helpers;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MediAid.Services
{
    [Obsolete]
    public class TakeCameraPicture
    {
        public static async Task<ImageSource> TakePicture()
        {
            await CrossMedia.Current.Initialize();
            Debug.WriteLine("Init");
            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                Debug.WriteLine("Unable to take picture");
                return null;
            }

            var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
            {
                Directory = "temp",
                Name = "temp.jpg"
            });

            if (file == null) return null;

            Debug.WriteLine("Done:"+file.Path);
            ImageSource newImage = ImageSource.FromStream(()  => {
                var stream = file.GetStream();
                file.Dispose();
                return stream;
            });
            Debug.WriteLine("Image Taken:"+newImage);


            return newImage;

        }
    }

}
