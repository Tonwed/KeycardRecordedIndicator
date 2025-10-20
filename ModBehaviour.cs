using Duckov.Modding;
using HarmonyLib;
using System;
using System.Reflection;
using UnityEngine;

namespace KeycardRecordedIndicator
{
    /// <summary>
    /// 已录入钥匙标识 Mod 的主类
    /// 负责初始化 Harmony Patch 和事件订阅
    /// </summary>
    public class ModBehaviour : Duckov.Modding.ModBehaviour
    {
        private Harmony? harmony;

        /// <summary>
        /// Mod 加载时调用
        /// </summary>
        void Awake()
        {
            Debug.Log($"[{Constants.MOD_NAME}] Loaded!!!");
        }

        /// <summary>
        /// Mod 启用时调用
        /// 初始化 Harmony Patch 和事件订阅
        /// </summary>
        void OnEnable()
        {
            Debug.Log($"[{Constants.MOD_NAME}] OnEnable");

            try
            {
                // 创建 Harmony 实例
                harmony = new Harmony(Constants.MOD_ID);

                // 注册所有 Patch
                harmony.PatchAll(Assembly.GetExecutingAssembly());

                Debug.Log($"[{Constants.MOD_NAME}] Harmony patches applied successfully");

                // 订阅钥匙录入事件
                // 当钥匙被录入时，更新相关的 UI 标识
                try
                {
                    var masterKeysManagerType = Type.GetType("Duckov.MasterKeys.MasterKeysManager, Assembly-CSharp");
                    if (masterKeysManagerType != null)
                    {
                        var onMasterKeyUnlockedField = masterKeysManagerType.GetField(
                            "OnMasterKeyUnlocked",
                            BindingFlags.Public | BindingFlags.Static
                        );

                        if (onMasterKeyUnlockedField != null)
                        {
                            // 订阅事件
                            // 这里可以添加额外的逻辑来处理钥匙录入事件
                            Debug.Log($"[{Constants.MOD_NAME}] Successfully subscribed to OnMasterKeyUnlocked event");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogWarning($"[{Constants.MOD_NAME}] Could not subscribe to OnMasterKeyUnlocked: {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"[{Constants.MOD_NAME}] Error in OnEnable: {ex.Message}\n{ex.StackTrace}");
            }
        }

        /// <summary>
        /// Mod 禁用时调用
        /// 卸载 Harmony Patch
        /// </summary>
        void OnDisable()
        {
            Debug.Log($"[{Constants.MOD_NAME}] OnDisable");

            try
            {
                if (harmony != null)
                {
                    harmony.UnpatchAll(Constants.MOD_ID);
                    Debug.Log($"[{Constants.MOD_NAME}] Harmony patches removed successfully");
                }

                // 清空所有标识
                RecordedIndicatorUI.ClearAll();
            }
            catch (Exception ex)
            {
                Debug.LogError($"[{Constants.MOD_NAME}] Error in OnDisable: {ex.Message}\n{ex.StackTrace}");
            }
        }

        /// <summary>
        /// Mod 销毁时调用
        /// </summary>
        void OnDestroy()
        {
            Debug.Log($"[{Constants.MOD_NAME}] OnDestroy");
        }
    }
}

