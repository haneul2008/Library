using System;

namespace HNLib.ObjectPool
{
    [AttributeUsage(AttributeTargets.Class)]
    public class PoolableAttribute : Attribute
    {
        public int Count { get; private set; }

        public PoolableAttribute(int count)
        {
            Count = count;
        }
    }
}