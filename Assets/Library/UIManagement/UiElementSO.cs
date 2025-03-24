using UnityEngine;

namespace Library.UIManagement
{
    [CreateAssetMenu(menuName = "SO/UiManagement/UiElement")]
    public class UiElementSO : ScriptableObject
    {
        public string canvasName;
        public UIMono ui;
        public string Key => ui.gameObject.name;
    }
}
