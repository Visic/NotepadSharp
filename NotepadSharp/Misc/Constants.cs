using NotepadSharp.Properties;
using System;
using System.IO;
using System.Windows.Media.Imaging;

namespace NotepadSharp {
    public static class Constants
    {
        public const string CmdArgPrefix = "--";
        public const string LoremIpsum = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.";
        public static readonly string KeyBindingCollectionSettingName = nameof(Settings.KeyBindings);
        public static readonly string CachedFileCollectionSettingName = nameof(Settings.CachedFiles);
        public static readonly string FavoritedLocationsCollectionSettingName = nameof(Settings.FavoritedLocations);
        public static readonly BitmapImage Image_FolderOpen = Resources.OpenFolder.ToBitmapImage();
        public static readonly BitmapImage Image_FolderClosed = Resources.ClosedFolder.ToBitmapImage();

        public static readonly string DesktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        public static readonly string AppData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        public static readonly string FileCachePath = Path.Combine(AppData, "NotepadSharp");
    }
}
