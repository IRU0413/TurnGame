using System;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Pattern
{
    [Serializable]
    public class SerializableDictionary<TKey, TValue> : ISerializationCallbackReceiver
    {
        [SerializeField]
        private List<SerializableKeyValuePair<TKey, TValue>> _keyValuePairs = new List<SerializableKeyValuePair<TKey, TValue>>();

        private Dictionary<TKey, TValue> _dictionary = new Dictionary<TKey, TValue>();

        public int Count
        {
            get
            {
                if (_keyValuePairs == null)
                    _keyValuePairs = new();
                return _keyValuePairs.Count;
            }
        }

        public void OnBeforeSerialize()
        {
            _keyValuePairs.Clear();
            foreach (var pair in _dictionary)
            {
                _keyValuePairs.Add(new SerializableKeyValuePair<TKey, TValue>(pair.Key, pair.Value));
            }
        }
        public void OnAfterDeserialize()
        {
            _dictionary.Clear();
            foreach (var pair in _keyValuePairs)
            {
                _dictionary[pair.Key] = pair.Value;
            }
        }

        public void Add(TKey key, TValue value)
        {
            _dictionary[key] = value;
        }

        public bool Contains(TKey key)
        {
            bool result = false;
            foreach (var pair in _keyValuePairs)
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
            foreach (var pair in _keyValuePairs)
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
            return _dictionary.TryGetValue(key, out value);
        }

        public Dictionary<TKey, TValue> ToDictionary()
        {
            return _dictionary;
        }
    }
}


