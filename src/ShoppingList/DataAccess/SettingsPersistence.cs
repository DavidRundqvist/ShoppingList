using System.IO;
using Newtonsoft.Json;
using ShoppingList.Models;

namespace ShoppingList.DataAccess {
    public class SettingsPersistence {
        private readonly DirectoryInfo _rootFolder;
        private readonly JsonSerializer _serializer;


        public SettingsPersistence(DirectoryInfo rootFolder, JsonSerializer serializer) {
            _rootFolder = rootFolder;
            _serializer = serializer;
        }

        FileInfo SettingsFile => new FileInfo(Path.Combine(_rootFolder.FullName, "Settings.json"));



        public void Save(Settings updatedSettings) {
            using (var fs = SettingsFile.CreateText()) {
                _serializer.Serialize(fs, updatedSettings);
            }
        }


        public Settings Load() {
            if (!SettingsFile.Exists) {
                return new Settings();
            }
            using (var fs = SettingsFile.OpenText()) {
                return (Settings) _serializer.Deserialize(fs, typeof(Settings));
            }
        }
    }
}