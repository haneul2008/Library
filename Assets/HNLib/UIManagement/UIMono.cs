using System;
using UnityEngine;

namespace Library.UIManagement
{
    public class UIMono : MonoBehaviour
    {
        public event Action<bool> OnActiveUi;
        [SerializeField] private Canvas myCanvas;

        public Canvas MyCanvas => myCanvas;
        
        public virtual void OnActive(bool isActive)
        {
            OnActiveUi?.Invoke(isActive);
        }
    }
}
