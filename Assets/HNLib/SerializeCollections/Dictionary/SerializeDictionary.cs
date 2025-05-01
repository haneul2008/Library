using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SerializableKeyValuePair<TKey, TValue>
{
    public TKey key;
    public TValue value;
}

namespace HNLib.SerializeCollections.Dictionary
{
    [Serializable]
    public class SerializeDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
    {
        [SerializeField]
        private List<SerializableKeyValuePair<TKey, TValue>> keyValuePairs =
            new List<SerializableKeyValuePair<TKey, TValue>>();

        public void OnBeforeSerialize()
        {
            keyValuePairs.Clear();

            foreach (var pair in this)
            {
                keyValuePairs.Add(new SerializableKeyValuePair<TKey, TValue>()
                    {
                        key = pair.Key, 
                        value = pair.Value
                    });
            }
        }

        public void OnAfterDeserialize()
        {
            Clear();

            foreach (var pair in keyValuePairs)
            {
                if (!ContainsKey(pair.key)) Add(pair.key, pair.value);
            }
        }
    }
}
