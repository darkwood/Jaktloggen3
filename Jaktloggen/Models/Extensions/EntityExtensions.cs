using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jaktloggen.IO;
using Jaktloggen.Services;

namespace Jaktloggen.Models.Extensions
{
    public static class EntityExtensions
    {
        public static void Save<T>(this T entityList, string filename)
        {
            entityList.SaveToLocalStorage(filename);

            if (App.SyncWithServer)
            {
                entityList.UploadToServer(filename);
            }
        }
    }
}
