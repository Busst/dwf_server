using System.Drawing;
using server.Resources.Enums;

namespace server.models
{
    public class Picture
    {
        public Image Image {get; set;}
        public int OffsetX {get; set;} = 0;
        public int OffsetY {get; set;} = 0;
        public string FilePath {get; set;}
        public ImageEnum.Extension exstension {get; set;} = ImageEnum.Extension.JPEG;
        public string FileName {get; set;}
        
    }
}