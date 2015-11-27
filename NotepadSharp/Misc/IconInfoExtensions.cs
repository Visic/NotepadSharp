using OSIcon;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media.Imaging;

namespace NotepadSharp {
    public static class IconInfoExtensions {
        public static BitmapImage ToBitmapImage(this IconInfo iconInfo) {
            if (iconInfo == null) return null;
            using(var ms = new MemoryStream()) {
                iconInfo.Bitmap.Save(ms, ImageFormat.Png);
                ms.Seek(0, SeekOrigin.Begin);
                var img = new BitmapImage();
                img.BeginInit();
                img.CacheOption = BitmapCacheOption.OnLoad;
                img.StreamSource = ms;
                img.EndInit();
                return img;
            }
        }
    }
}
