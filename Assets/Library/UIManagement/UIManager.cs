using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Serialization;

namespace Library.UIManagement
{
    public class UIManager : MonoBehaviour
        {
            [field: SerializeField] public bool UseGameObject { get; private set; }
            [field: SerializeField] public List<UiElementSO> Uis { get; private set; }

            public Dictionary<UiType, UIMono> UiPairs = new Dictionary<UiType, UIMono>();
            public List<bool> UiInstantiated { get; private set; } = new List<bool>();

            [SerializeField] private List<Canvas> canvasList = new List<Canvas>();
            
            private static UIManager _instance = null;
            
            public static UIManager Instance
            {
                get
                {
                    if (_instance is null)
                    {
                        _instance = FindObjectOfType<UIManager>();

                        if (_instance is null)
                            Debug.LogWarning("UiManager singleton is not exits");
                    }

                    return _instance;
                }
            }

            private void Awake()
            {
                Initialize();
            }

            public UIMono ActiveUI(UiType uiType, bool isActive)
            {
                if (!UiPairs.TryGetValue(uiType, out UIMono ui) || ui == null)
                {
                    Debug.LogWarning($"{uiType} is null");
                    return null;
                }

                int seq = (int)uiType;

                bool isInstantiateSo = !UiInstantiated[seq] && !UseGameObject && isActive;
                
                if (isInstantiateSo)
                {
                    UiInstantiated[seq] = true;

                    UIMono uiObject = Instantiate(ui, ui.MyCanvas.transform);
                    uiObject.gameObject.name = UiPairs[uiType].gameObject.name;
                    UiPairs[uiType] = uiObject;
                }

                ui.gameObject.SetActive(isActive);
                
                ui.OnActive(isActive);

                return ui;
            }

            private void Initialize()
            {
                canvasList = FindObjectsByType<Canvas>(FindObjectsSortMode.None).ToList();
                
                UiPairs.Clear();

                for (int i = 0; i < Uis.Count; i++)
                {
                    UiElementSO uiElementSo = Uis[i];
                    UIMono uiMono = uiElementSo.ui;
                    
                    if (uiMono.MyCanvas == null)
                    {
                        if (string.IsNullOrEmpty(uiElementSo.canvasName))
                        {
                            Debug.LogError($"No canvas specified : {uiElementSo.Key}");
                            continue;
                        }

                        Canvas canvas = canvasList.Find(canvas => canvas.name == uiElementSo.canvasName);

                        if (canvas == null)
                        {
                            Debug.LogError($"No canvas specified : {uiElementSo.Key}");
                            continue;
                        }
                            
                        SetUiMonoCanvas(uiMono, canvas);
                    }
                    
                    UiPairs.Add((UiType)i, uiMono);

                    UiInstantiated.Add(UseGameObject ? true : false);
                }
            }

            public void GenerateUiList(List<UiElementSO> list)
            {
                UseGameObject = false;
                Uis = list;
            }

            public void GenerateUiList()
            {
                canvasList = FindObjectsByType<Canvas>(FindObjectsSortMode.None).ToList();

                if (canvasList.Count == 0)
                {
                    Debug.LogWarning("There is no canvas");
                }
                
                List<UIMono> uis = new List<UIMono>();
                
                foreach (Canvas canvas in canvasList)
                {
                    foreach (UIMono uiMono in canvas.GetComponentsInChildren<UIMono>())
                    {
                        uis.Add(uiMono);

                        SetUiMonoCanvas(uiMono, canvas);
                    }
                }
                
                List<UiElementSO> list = new List<UiElementSO>();

                foreach (var ui in uis)
                {
                    UiElementSO uiElementSO = ScriptableObject.CreateInstance<UiElementSO>();
                    uiElementSO.name = ui.name;
                    uiElementSO.ui = ui;

                    list.Add(uiElementSO);
                }

                UseGameObject = true;
                Uis = list;
            }

            private static void SetUiMonoCanvas(UIMono uiMono, Canvas canvas)
            {
                Type uiMonoType = typeof(UIMono);
                FieldInfo canvasField = uiMonoType.GetField("myCanvas", BindingFlags.Instance | BindingFlags.NonPublic);
                canvasField.SetValue(uiMono, canvas);
            }
        }
}

