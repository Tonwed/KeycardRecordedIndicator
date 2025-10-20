using Duckov.UI;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KeycardRecordedIndicator
{
    /// <summary>
    /// 管理已录入钥匙的 UI 标识
    /// 负责创建、销毁和管理标识 UI 元素
    /// </summary>
    public static class RecordedIndicatorUI
    {
        /// <summary>
        /// 缓存已添加标识的 ItemDisplay
        /// 用于避免重复添加标识
        /// </summary>
        private static HashSet<ItemDisplay> indicatedDisplays = new HashSet<ItemDisplay>();

        /// <summary>
        /// 为物品显示添加已录入标识
        /// </summary>
        /// <param name="itemDisplay">目标物品显示对象</param>
        public static void AddIndicator(ItemDisplay itemDisplay)
        {
            if (itemDisplay == null)
            {
                return;
            }

            // 如果已经添加过标识，不重复添加
            if (indicatedDisplays.Contains(itemDisplay))
            {
                return;
            }

            try
            {
                // 检查是否已经存在标识
                Transform existingIndicator = itemDisplay.transform.Find(Constants.INDICATOR_OBJECT_NAME);
                if (existingIndicator != null)
                {
                    indicatedDisplays.Add(itemDisplay);
                    return;
                }

                // 创建新的标识
                CreateIndicator(itemDisplay);
                indicatedDisplays.Add(itemDisplay);

                if (Constants.DEBUG_MODE)
                {
                    Debug.Log($"[{Constants.MOD_NAME}] Indicator added to item");
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"[{Constants.MOD_NAME}] Error adding indicator: {ex.Message}\n{ex.StackTrace}");
            }
        }

        /// <summary>
        /// 移除物品显示的已录入标识
        /// </summary>
        /// <param name="itemDisplay">目标物品显示对象</param>
        public static void RemoveIndicator(ItemDisplay itemDisplay)
        {
            if (itemDisplay == null)
            {
                return;
            }

            try
            {
                Transform indicatorTransform = itemDisplay.transform.Find(Constants.INDICATOR_OBJECT_NAME);

                if (indicatorTransform != null)
                {
                    UnityEngine.Object.Destroy(indicatorTransform.gameObject);

                    if (Constants.DEBUG_MODE)
                    {
                        Debug.Log($"[{Constants.MOD_NAME}] Indicator removed from item");
                    }
                }

                indicatedDisplays.Remove(itemDisplay);
            }
            catch (Exception ex)
            {
                Debug.LogError($"[{Constants.MOD_NAME}] Error removing indicator: {ex.Message}\n{ex.StackTrace}");
            }
        }

        /// <summary>
        /// 创建标识 UI 元素
        /// </summary>
        /// <param name="itemDisplay">目标物品显示对象</param>
        private static void CreateIndicator(ItemDisplay itemDisplay)
        {
            try
            {
                if (itemDisplay == null)
                {
                    Debug.LogError($"[{Constants.MOD_NAME}] ItemDisplay is null");
                    return;
                }

                // 创建一个新的 GameObject 作为标识容器
                GameObject indicatorGO = new GameObject(Constants.INDICATOR_OBJECT_NAME);
                indicatorGO.transform.SetParent(itemDisplay.transform, false);
                indicatorGO.transform.localScale = Vector3.one;

                // 确保有 RectTransform 组件
                RectTransform rectTransform = indicatorGO.AddComponent<RectTransform>();
                if (rectTransform == null)
                {
                    Debug.LogError($"[{Constants.MOD_NAME}] Failed to add RectTransform");
                    UnityEngine.Object.Destroy(indicatorGO);
                    return;
                }

                // 设置 RectTransform - 右上角位置
                rectTransform.anchorMin = new Vector2(1, 1);  // 右上角
                rectTransform.anchorMax = new Vector2(1, 1);  // 右上角
                rectTransform.pivot = new Vector2(1, 1);      // 右上角
                rectTransform.anchoredPosition = Constants.INDICATOR_ANCHOR_POSITION;
                rectTransform.sizeDelta = Constants.INDICATOR_SIZE;

                // ==================== 背景层：绿色圆形符号 ====================
                GameObject bgGO = new GameObject("Background");
                bgGO.transform.SetParent(indicatorGO.transform, false);
                bgGO.transform.localScale = Vector3.one;

                RectTransform bgRectTransform = bgGO.AddComponent<RectTransform>();
                if (bgRectTransform != null)
                {
                    bgRectTransform.anchorMin = Vector2.zero;
                    bgRectTransform.anchorMax = Vector2.one;
                    bgRectTransform.offsetMin = Vector2.zero;
                    bgRectTransform.offsetMax = Vector2.zero;
                }

                TextMeshProUGUI bgText = bgGO.AddComponent<TextMeshProUGUI>();
                if (bgText != null)
                {
                    bgText.text = Constants.INDICATOR_BACKGROUND_SYMBOL;
                    bgText.color = Constants.RECORDED_INDICATOR_BG_COLOR;
                    bgText.fontSize = Constants.INDICATOR_BACKGROUND_FONT_SIZE;
                    bgText.alignment = TextAlignmentOptions.Center;
                }

                // ==================== 文本层：白色勾 ====================
                GameObject textGO = new GameObject("Text");
                textGO.transform.SetParent(indicatorGO.transform, false);
                textGO.transform.localScale = Vector3.one;

                RectTransform textRectTransform = textGO.AddComponent<RectTransform>();
                if (textRectTransform != null)
                {
                    textRectTransform.anchorMin = Vector2.zero;
                    textRectTransform.anchorMax = Vector2.one;
                    textRectTransform.offsetMin = Vector2.zero;
                    textRectTransform.offsetMax = Vector2.zero;
                }

                TextMeshProUGUI indicatorText = textGO.AddComponent<TextMeshProUGUI>();
                if (indicatorText == null)
                {
                    Debug.LogError($"[{Constants.MOD_NAME}] Failed to add TextMeshProUGUI");
                    UnityEngine.Object.Destroy(indicatorGO);
                    return;
                }

                indicatorText.text = Constants.INDICATOR_TEXT_SYMBOL;
                indicatorText.color = Constants.RECORDED_INDICATOR_TEXT_COLOR;
                indicatorText.fontSize = Constants.INDICATOR_TEXT_FONT_SIZE;
                indicatorText.alignment = TextAlignmentOptions.Center;

                if (Constants.DEBUG_MODE)
                {
                    Debug.Log($"[{Constants.MOD_NAME}] Indicator UI element created successfully");
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"[{Constants.MOD_NAME}] Error creating indicator: {ex.Message}\n{ex.StackTrace}");
            }
        }

        /// <summary>
        /// 清空所有标识
        /// 在 Mod 卸载时调用
        /// </summary>
        public static void ClearAll()
        {
            try
            {
                indicatedDisplays.Clear();

                if (Constants.DEBUG_MODE)
                {
                    Debug.Log($"[{Constants.MOD_NAME}] All indicators cleared");
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"[{Constants.MOD_NAME}] Error clearing indicators: {ex.Message}\n{ex.StackTrace}");
            }
        }
    }
}

