using UnityEngine;

namespace Library.UIManagement
{
    public enum UIPos
            {
                Center,
                LeftCenter,
                RightCenter,
                TopCenter,
                BottomCenter,
                LeftTop,
                RightTop,
                LeftBottom,
                RightBottom
            }

        public static class UIManagementUtility
        {
            /// <summary>
            /// ��� UI�� Ȱ��ȭ/��Ȱ��ȭ ��
            /// </summary>
            /// <param name="active">Ȱ��ȭ/��Ȱ��ȭ</param>
            public static void ActiveAllUI(bool active)
            {
                int uiCount = UIManager.Instance.Uis.Count;

                for(int i = 0; i < uiCount; i++)
                {
                    UIManager.Instance.ActiveUI((UiType)i, active);
                }
            }
            /// <summary>
            /// UI�� ������Ʈ�� ������
            /// </summary>
            /// <typeparam name="T">������ Ÿ��</typeparam>
            /// <param name="uiType">Ÿ���� ������ UI�� Ű</param>
            /// <param name="setActiveTrue">UI�� ��Ȱ��ȭ �Ǿ����� �� Ȱ��ȭ ��ų��</param>
            /// <returns></returns>
            public static T GetComponent<T>(UiType uiType, bool setActiveTrue = false) where T : Component
            {
                if(UIManager.Instance.UiPairs[uiType].TryGetComponent(out T component))
                {
                    if(!UIManager.Instance.UiInstantiated[(int)uiType] && setActiveTrue)
                        UIManager.Instance.ActiveUI(uiType, true);

                    return component;
                }

                return null;
            }
            /// <summary>
            /// UI������Ʈ�� ������ ������
            /// </summary>
            /// <param name="uiType">������ ���� UI�� Ű</param>
            /// <param name="siblingIndex">���� ����</param>
            public static void SetSiblingIndex(UiType uiType, int siblingIndex)
            {
                UIManager.Instance.UiPairs[uiType].transform.SetSiblingIndex(siblingIndex);
            }

            /// <summary>
            /// UI�� ��ġ�� ��������
            /// </summary>
            /// <param name="uiType">��ġ�� ���� UI�� Ű</param>
            /// <param name="type">������ ��ġ Ÿ��</param>
            /// <param name="offset">��ġ Ÿ���� �������� �� ������ ��</param>
            public static void SetUiPos(UiType uiType, UIPos type, Vector2 offset)
            {
                Vector2 pos = Vector2.zero;
                Vector2 defOffSet = Vector2.zero;

                switch (type)
                {
                    case UIPos.Center:
                        pos = new Vector2(0.5f, 0.5f);
                        break;

                    case UIPos.LeftCenter:
                        pos = new Vector2(0, 0.5f);
                        defOffSet = new Vector2(1, 0);
                        break;

                    case UIPos.RightCenter:
                        pos = new Vector2(1, 0.5f);
                        defOffSet = new Vector2(-1, 0);
                        break;

                    case UIPos.TopCenter:
                        pos = new Vector2(0.5f, 1);
                        defOffSet = new Vector2(0, -1);
                        break;

                    case UIPos.BottomCenter:
                        pos = new Vector2(0.5f, 0);
                        defOffSet = new Vector2(0, 1);
                        break;

                    case UIPos.LeftTop:
                        pos = new Vector2(0, 1);
                        defOffSet = new Vector2(1, -1);
                        break;

                    case UIPos.RightTop:
                        pos = new Vector2(1, 1);
                        defOffSet = new Vector2(-1, -1);
                        break;

                    case UIPos.LeftBottom:
                        pos = new Vector2(0, 0);
                        defOffSet = new Vector2(1, 1);
                        break;

                    case UIPos.RightBottom:
                        pos = new Vector2(1, 0);
                        defOffSet = new Vector2(-1, 1);
                        break;
                }

                if (UIManager.Instance.UiPairs[uiType].TryGetComponent(out RectTransform rectTrm))
                {
                    defOffSet *= new Vector2(rectTrm.sizeDelta.x / 2, rectTrm.sizeDelta.y / 2);
                }

                SetUIPos(uiType, pos, defOffSet + offset, false, rectTrm);
            }
            
            //pos = ��Ŀ ��ġ, offset = ��Ŀ�� �������� ������ ��ġ, worldPos = ���� ����������, uiRectTrm = ��ġ�� ������ ui�� rectTrm�� �ִ� ��� �Ҵ�
            private static void SetUIPos(UiType uiType, Vector2 pos, Vector2 offset, bool worldPos = true, RectTransform uiRectTrm = null)
            {
                if (worldPos)
                    Camera.main.WorldToScreenPoint(pos);

                if (!UIManager.Instance.UiInstantiated[(int)uiType])
                    UIManager.Instance.ActiveUI(uiType, true);

                GameObject ui = UIManager.Instance.UiPairs[uiType].gameObject;

                RectTransform rectTrmToEdit = null;

                if(uiRectTrm is null)
                {
                    if (ui.TryGetComponent(out RectTransform rectTrm))
                    {
                        rectTrmToEdit = rectTrm;
                    }
                    if (rectTrmToEdit is null) return;
                }
                else
                    rectTrmToEdit = uiRectTrm;

                rectTrmToEdit.anchorMax = pos;
                rectTrmToEdit.anchorMin = pos;
                rectTrmToEdit.anchoredPosition = offset;
            }
        }
}
