using HNLib.ObjectPool;
using UnityEngine;

namespace _01.Script.Test
{
    [Poolable(3)]
    public class TestPoolable : MonoBehaviour
    {
        [ResetItem]
        public void ResetItem()
        {
            print("reset test poolable");
        }
    }
}