#if UNITY_EDITOR
using UnityEditor;
#endif
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace HNLib.ObjectPool
{
    [CreateAssetMenu(fileName = "PoolManager", menuName = "SO/PoolManager", order = 0)]
    public class PoolManagerSO : ScriptableObject
    {
        [SerializeField] private string prefabPath;
        
        public List<MonoBehaviour> poolList;
        private Dictionary<Type, Pool> _poolPairs;
        private Dictionary<Type, List<MethodInfo>> _resetItemPairs;
        
        public void Initialize(Transform parent)
        {
            _poolPairs = new Dictionary<Type, Pool>();
            _resetItemPairs = new Dictionary<Type, List<MethodInfo>>();

            foreach (MonoBehaviour monoBehaviour in poolList)
            {
                Type type = monoBehaviour.GetType();
                AddPool(parent, type, monoBehaviour);
                AddResetItem(type);
            }
        }

        private void AddResetItem(Type type)
        {
            MethodInfo[] methodInfos =
                type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            foreach (MethodInfo methodInfo in methodInfos)
            {
                if(methodInfo.GetParameters().Length > 0) continue;
                
                if (Attribute.IsDefined(methodInfo, typeof(ResetItemAttribute)))
                {
                    if (_resetItemPairs.TryGetValue(type, out List<MethodInfo> infos))
                    {
                        infos.Add(methodInfo);
                    }
                    else
                    {
                        _resetItemPairs.Add(type, new List<MethodInfo>(){methodInfo});
                    }
                }
            }
        }

        private void AddPool(Transform parent, Type type, MonoBehaviour monoBehaviour)
        {
            PoolableAttribute attribute = Attribute.GetCustomAttribute(type, typeof(PoolableAttribute)) as PoolableAttribute;
            
            if (attribute == null)
            {
                Debug.LogWarning($"{type} class attribute is null");
                return;
            }
            
            Pool pool = new Pool(monoBehaviour, parent, attribute.Count);
            _poolPairs.Add(type, pool);
        }

        public void Push(MonoBehaviour mono)
        {
            Type type = mono.GetType();

            if (_poolPairs.TryGetValue(type, out Pool pool))
            {
                pool.Push(mono);
            }
        }

        public T Pop<T>() where T : MonoBehaviour
        {
            if (_poolPairs.TryGetValue(typeof(T), out Pool pool))
            {
                MonoBehaviour item = pool.Pop();
                CallResetItem(item);

                return item as T;
            }

            return null;
        }

        private void CallResetItem(MonoBehaviour item)
        {
            Type type = item.GetType();
            if (_resetItemPairs.TryGetValue(type, out List<MethodInfo> methodInfos))
            {
                foreach (MethodInfo methodInfo in methodInfos)
                {
                    methodInfo.Invoke(item, null);
                }
            }
        }

#if UNITY_EDITOR
        [ContextMenu("GeneratePoolList")]
        private void GeneratePoolList()
        {
            poolList.Clear();
            
            string[] guids = AssetDatabase.FindAssets("t:Prefab", new[] { prefabPath });
            foreach (string guid in guids)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);

                GameObject gameObject = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
                MonoBehaviour[] monoBehaviours = gameObject.GetComponents<MonoBehaviour>();

                foreach (MonoBehaviour mono in monoBehaviours)
                {
                    Type type = mono.GetType();

                    if (Attribute.IsDefined(type, typeof(PoolableAttribute)))
                    {
                        poolList.Add(mono);
                        break;
                    }
                }
            }
        }
#endif
    }
}