using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Library.UIManagement.Editor
{
    [CustomEditor(typeof(UIManager))]
    public class CustomUIManagement : UnityEditor.Editor
    {
        public VisualTreeAsset treeAsset = default;

        [SerializeField] private string uiElementPath = "Assets/Library/UIManagement/UiElements";

        public override VisualElement CreateInspectorGUI()
        {
            if (treeAsset == null) return base.CreateInspectorGUI();

            VisualElement root = new VisualElement();
            treeAsset.CloneTree(root);

            root.Q<Button>("btn_generateUiSO").clickable.clicked += HandleGenerateUISO;
            root.Q<Button>("btn_generateUiGO").clickable.clicked += HandleGenerateUIGameObject;
            root.Q<Button>("btn_generateEnum").clickable.clicked += HandleGenerateEnum;
            root.Q<Button>("btn_clearEnum").clickable.clicked += HandleClearEnum;

            root.Q<TextField>("SOField").value = uiElementPath;

            return root;
        }

        private void HandleClearEnum()
        {
            GenerateUiElementsEnumFile("");
        }

        private void HandleGenerateEnum()
        {
            StringBuilder codeBuilder = new StringBuilder();
            foreach (UiElementSO item in UIManager.Instance.Uis)
            {
                codeBuilder.Append(item.Key);
                codeBuilder.Append(",");
            }

            GenerateUiElementsEnumFile(codeBuilder.ToString());
        }

        private void GenerateUiElementsEnumFile(string codeBuilder)
        {
            string code = CodeFormat.FormatCode(codeBuilder);
            string path = $"{Application.dataPath}/Library/UIManagement/Uitype.cs";
            Debug.Log(path);
            File.WriteAllText(path, code);

            AssetDatabase.Refresh();
        }

        private void HandleGenerateUIGameObject()
        {
            UIManager.Instance.GenerateUiList();
        }

        private void HandleGenerateUISO()
        {
            UIManager.Instance.GenerateUiList(CreatAssetDatabase());
        }

        private List<UiElementSO> CreatAssetDatabase()
        {
            List<UiElementSO> list = new List<UiElementSO>();

            string[] assetGuids = AssetDatabase.FindAssets("", new[] { uiElementPath });

            foreach (var guid in assetGuids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                UiElementSO item = AssetDatabase.LoadAssetAtPath<UiElementSO>(path);

                if (item != null)
                {
                    list.Add(item);
                }
            }

            return list;
        }

    }
}
