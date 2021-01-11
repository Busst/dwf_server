using System.Drawing.Imaging;

namespace server.Resources.Enums
{
    public class ImageEnum
    {
        public enum Extension {
            PNG,
            JPEG
        }

        public static string ToString(Extension extension){
            switch(extension){
                case(Extension.JPEG):
                    return ".jpeg";
                case(Extension.PNG):
                    return ".png";
                default:
                    return "";
            }
        }
        public static Extension ToEnum(string extension){
            switch(extension.ToLower()){
                case(".jpeg"):
                    return Extension.JPEG;
                case(".png"):
                    return Extension.PNG;
                case("jpeg"):
                    return Extension.JPEG;
                case("png"):
                    return Extension.PNG;
                default:
                    return Extension.JPEG;
            }
        }

        public static ImageFormat ToEncoding(Extension extension){
            switch(extension){
                case(Extension.JPEG):
                    return ImageFormat.Jpeg;
                case(Extension.PNG):
                    return ImageFormat.Png;
                default:
                    return null;
            }
        }
    }
}