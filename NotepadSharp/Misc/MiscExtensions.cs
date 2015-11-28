using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media.Imaging;

namespace NotepadSharp {
    public static class MiscExtensions {
        public static BitmapImage ToBitmapImage(this Icon icon) {
            return icon?.ToBitmap().ToBitmapImage();
        }

        public static BitmapImage ToBitmapImage(this Bitmap bitmap) {
            if (bitmap == null) return null;
            using(var ms = new MemoryStream()) {
                bitmap.Save(ms, ImageFormat.Png);
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
