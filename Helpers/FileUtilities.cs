using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace Kanopy.Helpers
{
    public class FileUtilities
    {
        public static bool AllowedFileType(string path)
        {
            var ext = Path.GetExtension(path);
            return WebConfigurationManager.AppSettings["AllowedFileExtensions"].Contains(ext) ||
                WebConfigurationManager.AppSettings["AllowedImageExtensions"].Contains(ext);
        }
    }
}
