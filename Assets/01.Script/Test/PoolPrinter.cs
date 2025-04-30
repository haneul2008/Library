using System;
using System.Collections.Generic;
using HNLib.ObjectPool;
using UnityEngine;

namespace _01.Script.Test
{
    public class PoolPrinter : MonoBehaviour
    {
        [SerializeField] private PoolManagerSO poolManager;

        private Stack<TestPoolable> _poolables = new Stack<TestPoolable>();
        
        private void Awake()
        {
            poolManager.Initialize(transform);
        }

        [ContextMenu("Pop")]
        private void Pop()
        {
            TestPoolable poolable = poolManager.Pop<TestPoolable>();
            _poolables.Push(poolable);
        }

        [ContextMenu("Push")]
        private void Push()
        {
            if (_poolables.Count == 0) return;
            
            TestPoolable poolable = _poolables.Pop();
            poolManager.Push(poolable);
        }
    }
}