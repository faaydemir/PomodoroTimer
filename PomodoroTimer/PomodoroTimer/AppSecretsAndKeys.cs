using System;
using System.Collections.Generic;
using System.Text;

namespace PomodoroTimer
{
    /// <summary>
    /// KeyStorage for get keys and secrets. it is initial state. after keys added will not commited see usage of get for needed keys.
    /// </summary>
    internal class KeyStorage
    {
        private Dictionary<string, string> KeyDictionary;

        public KeyStorage()
        {
            KeyDictionary = new Dictionary<string, string>();
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
    public static class AppKeysAndSecrets
    {
        public static string Get(string key)
        {
            return new KeyStorage().Get(key);
        }
    }
}
