using System.IO;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ShoppingList.Models;

namespace ShoppingList.DataAccess {
    public class SettingsPersistence {
        private readonly DirectoryInfo _rootFolder;
        private readonly JsonSerializer _serializer;
        private readonly ILogger<SettingsPersistence> _log;

        public SettingsPersistence(DirectoryInfo rootFolder, JsonSerializer serializer, ILogger<SettingsPersistence> log) {
            _rootFolder = rootFolder;
            _serializer = serializer;
            this._log = log;
        }

        FileInfo SettingsFile => new FileInfo(Path.Combine(_rootFolder.FullName, "Settings.json"));



        public void Save(Settings updatedSettings) {
            using (var fs = SettingsFile.CreateText()) {
                _serializer.Serialize(fs, updatedSettings);
            }
        }


        public Settings Load() {
            var exists = SettingsFile.Exists;
            _log.LogInformation($"Checking Settings file. Does {SettingsFile.FullName} exist? {exists}");

            if (!exists) {
                return new Settings();
            }
            using (var fs = SettingsFile.OpenText()) {
                return (Settings) _serializer.Deserialize(fs, typeof(Settings));
            }
        }
    }
}