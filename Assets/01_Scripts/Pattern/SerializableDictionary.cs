using System;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Pattern
{
    [Serializable]
    public class SerializableDictionary<TKey, TValue> : ISerializationCallbackReceiver
    {
        [SerializeField]
        private List<SerializableKeyValuePair<TKey, TValue>> keyValuePairs = new List<SerializableKeyValuePair<TKey, TValue>>();

        private Dictionary<TKey, TValue> dictionary = new Dictionary<TKey, TValue>();

        public void OnBeforeSerialize()
        {
            keyValuePairs.Clear();
            foreach (var pair in dictionary)
            {
                keyValuePairs.Add(new SerializableKeyValuePair<TKey, TValue>(pair.Key, pair.Value));
            }
        }

        public void OnAfterDeserialize()
        {
            dictionary.Clear();
            foreach (var pair in keyValuePairs)
            {
                dictionary[pair.Key] = pair.Value;
            }
        }

        public void Add(TKey key, TValue value)
        {
            dictionary[key] = value;
        }

        public bool Contains(TKey key)
        {
            bool result = false;
            foreach (var pair in keyValuePairs)
            {
                if (!pair.Key.Equals(key))
                    continue;
                result = true;
                break;
            }
            return result;
        }
        public bool Contains(TValue value)
        {
            bool result = false;
            foreach (var pair in keyValuePairs)
            {
                if (!pair.Value.Equals(value))
                    continue;
                result = true;
                break;
            }
            return result;
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return dictionary.TryGetValue(key, out value);
        }

        public Dictionary<TKey, TValue> ToDictionary()
        {
            return dictionary;
        }
    }
}


