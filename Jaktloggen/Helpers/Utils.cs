using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jaktloggen.Interfaces;
using Xamarin.Forms;




namespace Jaktloggen.Helpers
{
    public static class Utils
    {
        public static int Rnd(int from, int to)
        {
            return new Random().Next(from, to);
        }

        public static void SetProperty(string key, object value)
        {
            var keyExists = Application.Current.Properties.ContainsKey(key);

            if (keyExists)
            {
                Application.Current.Properties[key] = value;
            }
            else
            {
                Application.Current.Properties.Add(key, value);                
            }
        }
        public static object GetProperty(string key)
        {
            object val;
            Application.Current.Properties.TryGetValue(key, out val);
            return val;
        }
        public static void ClearProperty(string key)
        {
            if (Application.Current.Properties.ContainsKey(key))
            {
                Application.Current.Properties.Remove(key);
            }
        }

        public static void LogError(Exception ex)
        {
            //DependencyService.Get<IFileUtility>().LogError(ex.Message + "; " + ex.StackTrace);
            //throw new Exception(ex.Message, ex);
        }
    }
}
