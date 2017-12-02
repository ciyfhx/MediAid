using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MediAid.Services;
using Xamarin.Forms;
using MediAid.Droid;
using System.Threading.Tasks;

[assembly: Dependency(typeof(AndroidMediaService))]
namespace MediAid.Droid
{
    class AndroidMediaService : IMediaService
    {
        public async Task<bool> PlayVideoAsync(string url)
        {
            Context context = Android.App.Application.Context;

            Android.Net.Uri uri = Android.Net.Uri.Parse(url);

            Intent intent = new Intent(Intent.ActionView, uri);
            intent.SetDataAndType(uri, "video/mp4");
            context.StartActivity(intent);

            return await Task.FromResult(true);

        }
    }
}