using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

using AVFoundation;

using CoreGraphics;

using Foundation;

using Jaktloggen.iOS.IO;
using Jaktloggen.Interfaces;

using UIKit;

using Xamarin.Forms;

[assembly: Dependency(typeof(CameraIOS))]


namespace Jaktloggen.iOS.IO
{
    public class CameraIOS : ICamera
    {
        public async void BringUpCamera()
        {
            //Check if we have permission to use the camera
            var authorizationStatus = AVCaptureDevice.GetAuthorizationStatus(AVMediaType.Video);

            //If we don't have access, and have never asked before, prompt them
            if (authorizationStatus != AVAuthorizationStatus.Authorized)
            {
                var access = await AVCaptureDevice.RequestAccessForMediaTypeAsync(AVMediaType.Video);

                //If access was granted we can proceed, if not, you can add an else statement and implement an error message or something more helpful
                if (access)
                {
                    GotAccessToCamera();
                }
            }
            else
            {
                //We've already been given access
                GotAccessToCamera();
            }
        }

        public void BringUpPhotoGallery()
        {
            var imagePicker = new UIImagePickerController { SourceType = UIImagePickerControllerSourceType.PhotoLibrary, MediaTypes = UIImagePickerController.AvailableMediaTypes(UIImagePickerControllerSourceType.PhotoLibrary) };
            imagePicker.AllowsEditing = true;

            //Make sure we have the root view controller which will launch the photo gallery 
            var window = UIApplication.SharedApplication.KeyWindow;
            var vc = window.RootViewController;
            while (vc.PresentedViewController != null)
            {
                vc = vc.PresentedViewController;
            }

            //Show the image gallery
            vc.PresentViewController(imagePicker, true, null);

            //call back for when a picture is selected and finished editing
            imagePicker.FinishedPickingMedia += (sender, e) =>
            {
                UIImage originalImage = e.Info[UIImagePickerController.EditedImage] as UIImage;
                if (originalImage != null)
                {
    				var pngImage = originalImage.AsPNG();
    				byte[] myByteArray = new byte[pngImage.Length];
    				System.Runtime.InteropServices.Marshal.Copy(pngImage.Bytes, myByteArray, 0, Convert.ToInt32(pngImage.Length));

    				MessagingCenter.Send<byte[]>(myByteArray, "ImageSelected");
    			
                }

                //Close the image gallery on the UI thread
                Device.BeginInvokeOnMainThread(() =>
                {
                    vc.DismissViewController(true, null);
                });
            };

            //Cancel button callback from the image gallery
            imagePicker.Canceled += (sender, e) => vc.DismissViewController(true, null);
        }

        private void GotAccessToCamera()
        {
            //Create an image picker object
            var imagePicker = new UIImagePickerController { 
                SourceType = UIImagePickerControllerSourceType.Camera,
                
            };

            //Make sure we can find the top most view controller to launch the camera
            var window = UIApplication.SharedApplication.KeyWindow;
            var vc = window.RootViewController;
            while (vc.PresentedViewController != null)
            {
                vc = vc.PresentedViewController;
            }

            vc.PresentViewController(imagePicker, true, null);

            //Callback method for when the user has finished taking the picture
            imagePicker.FinishedPickingMedia += (sender, e) =>
            {
                //Grab the image
                UIImage image = (UIImage)e.Info.ObjectForKey(new NSString("UIImagePickerControllerOriginalImage"));

                image.SaveToPhotosAlbum((img, error) => {
					UIImage rotateImage = RotateImage(image, image.Orientation);
					rotateImage = rotateImage.Scale(new CGSize(rotateImage.Size.Width, rotateImage.Size.Height), 0.5f);

					var jpegImage = rotateImage.AsJPEG();

					byte[] myByteArray = new byte[jpegImage.Length];
					System.Runtime.InteropServices.Marshal.Copy(jpegImage.Bytes, myByteArray, 0, Convert.ToInt32(jpegImage.Length));

					MessagingCenter.Send<byte[]>(myByteArray, "ImageSelected");

					Device.BeginInvokeOnMainThread(() =>
					{
						vc.DismissViewController(true, null);
					});
                });
            };

            imagePicker.Canceled += (sender, e) => vc.DismissViewController(true, null);
        }

        //Method that will take in a photo and rotate it based on the orientation that the image was taken in
        double radians(double degrees) { return degrees * Math.PI / 180; }

        private UIImage RotateImage(UIImage src, UIImageOrientation orientation)
        {
            UIGraphics.BeginImageContext(src.Size);

            if (orientation == UIImageOrientation.Right)
            {
                CGAffineTransform.MakeRotation((nfloat)radians(90));
            }
            else if (orientation == UIImageOrientation.Left)
            {
                CGAffineTransform.MakeRotation((nfloat)radians(-90));
            }
            else if (orientation == UIImageOrientation.Down)
            {
                // NOTHING
            }
            else if (orientation == UIImageOrientation.Up)
            {
                CGAffineTransform.MakeRotation((nfloat)radians(90));
            }

            src.Draw(new CGPoint(0, 0));
            UIImage image = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();
            return image;
        }
    }
}
