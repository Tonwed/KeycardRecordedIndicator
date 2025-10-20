using UnityEngine;

namespace KeycardRecordedIndicator
{
    /// <summary>
    /// Mod 常量定义
    /// </summary>
    public static class Constants
    {
        // ==================== Mod 信息 ====================
        
        /// <summary>
        /// Mod 唯一标识符
        /// </summary>
        public const string MOD_ID = "KeycardRecordedIndicator";

        /// <summary>
        /// Mod 显示名称
        /// </summary>
        public const string MOD_NAME = "已录入钥匙标识";

        /// <summary>
        /// Mod 版本
        /// </summary>
        public const string MOD_VERSION = "1.0.0";

        // ==================== UI 样式 ====================

        /// <summary>
        /// 已录入标识的背景颜色（绿色）
        /// </summary>
        public static readonly Color RECORDED_INDICATOR_BG_COLOR = new Color(0.2f, 0.8f, 0.2f, 1);

        /// <summary>
        /// 已录入标识的文本颜色（白色）
        /// </summary>
        public static readonly Color RECORDED_INDICATOR_TEXT_COLOR = new Color(1, 1, 1, 1);

        /// <summary>
        /// 已录入标识的背景符号（绿色圆形）
        /// </summary>
        public const string INDICATOR_BACKGROUND_SYMBOL = "●";

        /// <summary>
        /// 已录入标识的文本符号（白色勾）
        /// </summary>
        public const string INDICATOR_TEXT_SYMBOL = "✓";

        /// <summary>
        /// 标识背景符号的字体大小
        /// </summary>
        public const int INDICATOR_BACKGROUND_FONT_SIZE = 32;

        /// <summary>
        /// 标识文本符号的字体大小
        /// </summary>
        public const int INDICATOR_TEXT_FONT_SIZE = 20;

        /// <summary>
        /// 标识的锚点位置（右上角，距离 5 像素）
        /// </summary>
        public static readonly Vector2 INDICATOR_ANCHOR_POSITION = new Vector2(-5, -5);

        /// <summary>
        /// 标识的大小（28x28 像素）
        /// </summary>
        public static readonly Vector2 INDICATOR_SIZE = new Vector2(28, 28);

        // ==================== 调试 ====================

        /// <summary>
        /// 是否启用调试模式
        /// </summary>
        public const bool DEBUG_MODE = true;

        // ==================== 标签 ====================

        /// <summary>
        /// 钥匙标签
        /// </summary>
        public const string KEY_TAG = "Key";

        /// <summary>
        /// 标识 GameObject 的名称
        /// </summary>
        public const string INDICATOR_OBJECT_NAME = "RecordedIndicator";
    }
}

