using System.IO;
using System.Threading.Tasks;

namespace Jaktloggen.Interfaces
{
    public interface ICamera
    {
        //Task<Stream> GetImageStreamAsync();
        void BringUpCamera();
        void BringUpPhotoGallery();
    }
}
