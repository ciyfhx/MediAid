using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MediAid.Services
{
    interface IMediaService
    {

        Task<bool> PlayVideoAsync(string url);
        

    }
}
