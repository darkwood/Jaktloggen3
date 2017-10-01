using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Jaktloggen.Helpers;
using Jaktloggen.Interfaces;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace Jaktloggen.Services
{
    public static class DataService
    {
        private const string ServicePath = "http://192.168.110.1/JaktDataService/api/DataService/?filename={0}";

        public static List<T> LoadFromServer<T>(string filename)
        {
            var remoteObj = (List<T>)Activator.CreateInstance(typeof(List<T>));
            
            using (var httpClient = new HttpClient())
            {
                try
                {
                    //todo make async method or go full syncronized
                    var responseTask = httpClient.GetStringAsync(string.Format(ServicePath, filename));
                    if (filename.EndsWith(".json"))
                    {
                        remoteObj = JsonConvert.DeserializeObject<List<T>>(responseTask.Result);
                    }
                    else
                    {
                        using (var reader = new StringReader(responseTask.Result))
                        {
                            XmlSerializer serializer = new XmlSerializer(typeof(List<T>));
                            remoteObj = (List<T>)serializer.Deserialize(reader);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Utils.LogError(ex);
                }
            }
            return remoteObj;
        }

        public static async void UploadToServer<T>(this T objToSerialize, string filename)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(objToSerialize.GetType());
            var serializedObjectString = "";
            try
            {
                if (filename.EndsWith(".json"))
                {
                    serializedObjectString = JsonConvert.SerializeObject(objToSerialize);
                }
                else
                {
                    using (StringWriter textWriter = new StringWriter())
                    {
                        xmlSerializer.Serialize(textWriter, objToSerialize);
                        serializedObjectString = textWriter.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                Utils.LogError(ex);
            }
            

            using (var httpClient = new HttpClient())
            {
                try
                {
                    var content = new StringContent(serializedObjectString);
                    var response = await httpClient.PostAsync(string.Format(ServicePath, filename), content);
                    var result = response.Content.ReadAsStringAsync().Result;
                }
                catch (Exception ex)
                {
                    Utils.LogError(ex);
                }
            }
        }
    }
}
