using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using SimpleAspNetApp.Models;

namespace SimpleAspNetApp.Services {
    public static class MessagesStorage {
        private static List<UserMessage> _storage = new List<UserMessage>();
        private const string _fileName = "storage.bin";
        private static readonly string SavePath;

        static MessagesStorage() {
            var execPath = Assembly.GetExecutingAssembly().CodeBase.Replace(@"file:///", "");
            var dir = Path.GetDirectoryName(execPath);
            SavePath = dir + @"\" + _fileName;
        }

        public static void LoadFromFile() {
            if (!File.Exists(SavePath)) {
                return;
            }
            using (Stream stream = File.Open(SavePath, FileMode.Open)) {
                _storage = (List<UserMessage>) new BinaryFormatter().Deserialize(stream);
            }
        }

        private static void AutoSave() {
            using (Stream stream = File.Open(SavePath, FileMode.Create)) {
                new BinaryFormatter().Serialize(stream, _storage);
            }
        }

        public static void PutNewMessage(UserMessage msg) {
            _storage.Add(msg);
            AutoSave();
        }

        public static List<string> GetLastTenMessagesForUser(string userId) {
            var msgs = _storage.Where(msg => msg.UserId == userId).ToList();
            var result = new List<string>();
            for (var i = msgs.Count - 1; i >= 0; i--) {
                if (result.Count >= 10) {
                    break;
                }
                result.Add(msgs[i].Message);
            }
            return result;
        }

        public static List<UserMessage> GetLast20Messages() {
            var result = new List<UserMessage>();
            for (var i = _storage.Count - 1; i >= 0; i--) {
                if (result.Count >= 20) {
                    break;
                }
                result.Add(_storage[i]);
            }
            return result;
        }
    }
}