using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MediAid.Helpers
{
    public class EnvironmentUtils
    {
        public static string GetPlatformEnironmentPath()
        {
#if __ANDROID__
            return Environment.GetFolderPath(Environment.SpecialFolder.Personal);
#elif __IOS__
            return null;
#endif

        }

    }
}
