using NotepadSharp.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace NotepadSharp {
    public static class ArgsAndSettings
    {
        static Dictionary<string, Action<string[]>> _options = new Dictionary<string, Action<string[]>>() {
            { "logpath", Values => LogPath = Values.FirstOrDefault() }
        };

        static ArgsAndSettings() {
            SetDefaults();
            LoadSettings();
            LoadArgs();
        }

        public static string LogPath { get; private set; }
        public static Setting<double> Width { get; private set; }
        public static Setting<double> Height { get; private set; }
        public static Setting<double> Top { get; private set; }
        public static Setting<double> Left { get; private set; }
        public static Setting<WindowState> WindowState { get; private set; }
        public static KeyBindingCollection KeyBindings { get; private set; }

        public static void SaveSettings() {
            Settings.Default.Save();
        }

        private static void SetDefaults() {
            LogPath = "";
            KeyBindings = new KeyBindingCollection(new PersistentCollection<LuaKeyBinding>(Constants.KeyBindingCollectionSettingName));
        }

        private static void LoadSettings() {
            if (Settings.Default.UpdateRequired) {
                Settings.Default.Upgrade();
                Settings.Default.UpdateRequired = false;
            }

            Width = new Setting<double>(Settings.Default.Width, val => Settings.Default.Width = val);
            Height = new Setting<double>(Settings.Default.Height, val => Settings.Default.Height = val);
            Top = new Setting<double>(Settings.Default.Top, val => Settings.Default.Top = val);
            Left = new Setting<double>(Settings.Default.Left, val => Settings.Default.Left = val);
            WindowState = new Setting<WindowState>(Settings.Default.WindowState, val => Settings.Default.WindowState = val);
        }

        private static void LoadArgs() {
            var args = Environment.GetCommandLineArgs().SkipWhile(x => !x.StartsWith(Constants.CmdArgPrefix));
            var options = args.Aggregate(
                new List<List<string>>(),
                (list, value) => {
                    if (value.StartsWith(Constants.CmdArgPrefix)) list.Add(new List<string>());
                    list.Last().AddRange(value.Split(new char[] { '=' }, 2));
                    return list;
                }
            );

            foreach (var argSet in options) {
                var key = argSet.First().Remove(0, Constants.CmdArgPrefix.Length);
                Action<string[]> value;
                if (!_options.TryGetValue(key.ToLower(), out value)) continue;
                value(argSet.Skip(1).ToArray());
            }
        }
    }
}
