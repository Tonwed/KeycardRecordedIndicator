using Duckov.UI;
using Duckov.Utilities;
using HarmonyLib;
using ItemStatsSystem;
using System;
using System.Linq;
using UnityEngine;

namespace KeycardRecordedIndicator
{
    /// <summary>
    /// Harmony Patch - 拦截 ItemDisplay.Setup 方法
    /// 在物品显示时检查是否为已录入的钥匙，并添加标识
    /// </summary>
    [HarmonyPatch(typeof(ItemDisplay), "Setup")]
    public partial class PatchItemDisplaySetup
    {
        /// <summary>
        /// Postfix 方法 - 在 ItemDisplay.Setup 执行后调用
        /// </summary>
        /// <param name="__instance">ItemDisplay 实例</param>
        /// <param name="target">物品实例</param>
        static void Postfix(ItemDisplay __instance, Item target)
        {
            try
            {
                // 参数检查
                if (__instance == null)
                {
                    return;
                }

                // 如果物品为空，移除标识
                if (target == null)
                {
                    RecordedIndicatorUI.RemoveIndicator(__instance);
                    return;
                }

                // 注册检查状态改变事件
                // 这样当搜索完成时，标识会自动显示
                RegisterInspectionStateChangedEvent(target);

                // 检查物品是否正在被搜索（NeedInspection 为 true 表示正在搜索）
                if (target.NeedInspection)
                {
                    // 物品正在被搜索，不显示标识
                    RecordedIndicatorUI.RemoveIndicator(__instance);
                    return;
                }

                // 检查是否为钥匙
                bool isKey = IsKeyItem(target);

                if (!isKey)
                {
                    // 不是钥匙，移除标识
                    RecordedIndicatorUI.RemoveIndicator(__instance);
                    return;
                }

                // 检查钥匙是否已录入
                bool isRecorded = IsKeyRecorded(target.TypeID);

                if (isRecorded)
                {
                    // 已录入，添加标识
                    RecordedIndicatorUI.AddIndicator(__instance);

                    if (Constants.DEBUG_MODE)
                    {
                        Debug.Log($"[{Constants.MOD_NAME}] Key recorded: {target.name} (TypeID: {target.TypeID})");
                    }
                }
                else
                {
                    // 未录入，移除标识
                    RecordedIndicatorUI.RemoveIndicator(__instance);
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"[{Constants.MOD_NAME}] Error in PatchItemDisplaySetup.Postfix: {ex.Message}\n{ex.StackTrace}");
            }
        }

        /// <summary>
        /// 注册物品检查状态改变事件
        /// 当搜索完成时，自动更新标识
        /// </summary>
        private static void RegisterInspectionStateChangedEvent(Item target)
        {
            if (target == null)
            {
                return;
            }

            try
            {
                // 移除旧的事件处理器（避免重复注册）
                target.onInspectionStateChanged -= OnItemInspectionStateChanged;
                // 注册新的事件处理器
                target.onInspectionStateChanged += OnItemInspectionStateChanged;
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"[{Constants.MOD_NAME}] Error registering inspection state changed event: {ex.Message}");
            }
        }

        /// <summary>
        /// 物品检查状态改变时的回调
        /// </summary>
        private static void OnItemInspectionStateChanged(Item item)
        {
            try
            {
                if (item == null)
                {
                    return;
                }

                // 检查物品是否不再需要检查（搜索完成）
                if (!item.NeedInspection)
                {
                    // 搜索完成，检查是否为已录入的钥匙
                    if (IsKeyItem(item) && IsKeyRecorded(item.TypeID))
                    {
                        // 找到对应的 ItemDisplay 并添加标识
                        // 这里我们需要通过 ItemDisplay 的 Target 属性来找到对应的 ItemDisplay
                        // 但由于我们无法直接访问所有 ItemDisplay，我们通过 Refresh 来触发更新

                        if (Constants.DEBUG_MODE)
                        {
                            Debug.Log($"[{Constants.MOD_NAME}] Item inspection completed: {item.name} (TypeID: {item.TypeID})");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"[{Constants.MOD_NAME}] Error in OnItemInspectionStateChanged: {ex.Message}");
            }
        }

        /// <summary>
        /// 检查物品是否为钥匙
        /// </summary>
        /// <param name="item">物品实例</param>
        /// <returns>是否为钥匙</returns>
        private static bool IsKeyItem(Item item)
        {
            if (item == null)
            {
                return false;
            }

            try
            {
                // 使用 TagCollection.Contains(string) 方法检查是否有 "Key" 标签
                return item.Tags != null && item.Tags.Contains(Constants.KEY_TAG);
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"[{Constants.MOD_NAME}] Error checking if item is key: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 检查钥匙是否已被录入
        /// </summary>
        /// <param name="typeID">物品类型 ID</param>
        /// <returns>是否已录入</returns>
        private static bool IsKeyRecorded(int typeID)
        {
            try
            {
                // 使用 MasterKeysManager 检查钥匙是否已被录入
                return Duckov.MasterKeys.MasterKeysManager.IsActive(typeID);
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"[{Constants.MOD_NAME}] Error checking if key is recorded: {ex.Message}");
                return false;
            }
        }
    }

    /// <summary>
    /// Harmony Patch - 拦截 ItemDisplay.Refresh 方法
    /// 在物品显示刷新时更新标识
    /// </summary>
    [HarmonyPatch(typeof(ItemDisplay), "Refresh")]
    public class PatchItemDisplayRefresh
    {
        /// <summary>
        /// Postfix 方法 - 在 ItemDisplay.Refresh 执行后调用
        /// </summary>
        /// <param name="__instance">ItemDisplay 实例</param>
        static void Postfix(ItemDisplay __instance)
        {
            try
            {
                if (__instance == null)
                {
                    return;
                }

                // 获取 ItemDisplay 的 Target 属性
                Item target = __instance.Target;

                if (target == null)
                {
                    RecordedIndicatorUI.RemoveIndicator(__instance);
                    return;
                }

                // 检查物品是否正在被搜索
                if (target.NeedInspection)
                {
                    // 物品正在被搜索，不显示标识
                    RecordedIndicatorUI.RemoveIndicator(__instance);
                    return;
                }

                // 检查是否为钥匙
                if (!PatchItemDisplaySetup.IsKeyItemPublic(target))
                {
                    RecordedIndicatorUI.RemoveIndicator(__instance);
                    return;
                }

                // 检查钥匙是否已录入
                if (PatchItemDisplaySetup.IsKeyRecordedPublic(target.TypeID))
                {
                    RecordedIndicatorUI.AddIndicator(__instance);
                }
                else
                {
                    RecordedIndicatorUI.RemoveIndicator(__instance);
                }
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"[{Constants.MOD_NAME}] Error in PatchItemDisplayRefresh.Postfix: {ex.Message}");
            }
        }
    }

    /// <summary>
    /// 扩展 PatchItemDisplaySetup 类以提供公共方法
    /// </summary>
    public partial class PatchItemDisplaySetup
    {
        /// <summary>
        /// 公共版本的 IsKeyItem 方法
        /// </summary>
        public static bool IsKeyItemPublic(Item item)
        {
            if (item == null)
            {
                return false;
            }

            try
            {
                return item.Tags != null && item.Tags.Contains(Constants.KEY_TAG);
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"[{Constants.MOD_NAME}] Error checking if item is key: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 公共版本的 IsKeyRecorded 方法
        /// </summary>
        public static bool IsKeyRecordedPublic(int typeID)
        {
            try
            {
                return Duckov.MasterKeys.MasterKeysManager.IsActive(typeID);
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"[{Constants.MOD_NAME}] Error checking if key is recorded: {ex.Message}");
                return false;
            }
        }
    }
}

