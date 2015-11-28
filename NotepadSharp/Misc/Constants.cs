using OSIcon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace NotepadSharp
{
    public static class Constants
    {
        public const string CmdArgPrefix = "--";
        public const string LoremIpsum = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.";
        public const string KeyBindingCollectionSettingName = "KeyBindings";
        public static readonly BitmapImage Image_FolderOpen = IconReader.GetFolderIcon(IconSize.ExtraLarge, FolderState.Open).ToBitmapImage();
        public static readonly BitmapImage Image_FolderClosed = IconReader.GetFolderIcon(IconSize.ExtraLarge, FolderState.Closed).ToBitmapImage();
    }
}
