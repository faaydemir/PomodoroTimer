using System;
using System.Collections.Generic;
using System.Text;

namespace PomodoroTimer
{
    /// <summary>
    /// KeyStorage for get keys and secrets. it is initial state. after keys added,this file  will not commited.
    /// </summary>
    internal class KeyStorage
    {
        private Dictionary<string, string> KeyDictionary;

        public KeyStorage()
        {
            KeyDictionary = new Dictionary<string, string>();
            KeyDictionary.Add("firebaseUrl", "YourFireBaseKey");
            //TODO Add needed keys 
        }

        public string Get(string Key)
        {
            if (KeyDictionary.ContainsKey(Key))
            {
                return KeyDictionary[Key];
            }
            else
            {
                return null;
            }
        }
    }

    /// <summary>
    /// AppKeysAndSecrets
    /// </summary>
    public static class AppKeysAndSecrets
    {
        public static string Get(string key)
        {
            return new KeyStorage().Get(key);
        }
    }
}
