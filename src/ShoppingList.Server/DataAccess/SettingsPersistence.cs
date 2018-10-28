//using System.IO;
//using Newtonsoft.Json;
//using Shared.Data;
//using ShoppingList.Models;

//namespace Server.DataAccess {
//    public class SettingsPersistence {
//        private readonly DirectoryInfo _rootFolder;
//        private readonly JsonSerializer _serializer;


//        public SettingsPersistence(DirectoryInfo rootFolder, JsonSerializer serializer) {
//            _rootFolder = rootFolder;
//            _serializer = serializer;
//        }

//        FileInfo SettingsFile => new FileInfo(Path.Combine(_rootFolder.FullName, "Settings.json"));



//        public void Save(SettingsDTO updatedSettings) {
//            using (var fs = SettingsFile.CreateText()) {
//                _serializer.Serialize(fs, updatedSettings);
//            }
//        }


//        public SettingsDTO Load() {
//            if (!SettingsFile.Exists) {
//                return new SettingsDTO();
//            }
//            using (var fs = SettingsFile.OpenText()) {
//                return (SettingsDTO) _serializer.Deserialize(fs, typeof(SettingsDTO));
//            }
//        }
//    }
//}