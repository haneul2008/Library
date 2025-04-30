using System.Collections.Generic;
using UnityEngine;

namespace HNLib.ObjectPool
{
    public class Pool
    {
        private readonly Stack<MonoBehaviour> _pools;
        private readonly MonoBehaviour _mono;
        private readonly Transform _parent;
        
        public Pool(MonoBehaviour mono, Transform parent, int count)
        {
            _mono = mono;
            _parent = parent;
            _pools = new Stack<MonoBehaviour>();

            for (int i = 0; i < count; ++i)
            {
                MonoBehaviour newObj = GameObject.Instantiate(mono, parent);
                newObj.gameObject.SetActive(false);
                _pools.Push(newObj);
            }
        }

        public MonoBehaviour Pop()
        {
            MonoBehaviour newObj;
            
            if (_pools.Count == 0)
            {
                newObj = GameObject.Instantiate(_mono, _parent);
            }
            else
            {
                newObj = _pools.Pop();
                newObj.gameObject.SetActive(true);
            }

            return newObj;
        }

        public void Push(MonoBehaviour mono)
        {
            _pools.Push(mono);
            mono.gameObject.SetActive(false);
        }
    }
}
