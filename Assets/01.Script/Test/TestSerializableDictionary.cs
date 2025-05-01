using System;
using System.IO;
using UnityEngine;
using HNLib.SerializeCollections.Dictionary;

namespace _01.Script.Test
{
    public class TestSerializableDictionary : MonoBehaviour
    {
        [SerializeField] private SerializeDictionary<int, int> testDictionary;
    }
}