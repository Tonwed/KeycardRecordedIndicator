
# Better Key Indicator

## 功能介绍

这个 Mod 为已录入的钥匙和卡添加视觉标识，帮助玩家快速识别已经录入过的物品。

### 主要特性

- ✅ 自动检测已录入的钥匙
- ✅ 显示绿色"✓"标记
- ✅ 实时更新
- ✅ 性能优化
- ✅ 完全兼容其他 Mod

## 使用方法

### Steam 平台

1. 打开 Steam 创意工坊页面（链接待补充）
2. 点击"订阅"按钮
3. 启动游戏
4. 在主菜单进入 Mods 界面
5. 勾选"已录入钥匙标识"启用 Mod

### 非 Steam 平台

1. 下载 Mod 文件
2. 解压到游戏安装目录的 `Duckov_Data/Mods/` 文件夹
3. 启动游戏
4. 在主菜单进入 Mods 界面
5. 勾选"已录入钥匙标识"启用 Mod

## 工作原理

当你打开容器或背包时，Mod 会自动检查每个物品：

1. 检查物品是否为钥匙
2. 检查钥匙是否已被录入
3. 如果已录入，在物品上显示绿色"✓"标记


## 开发信息

### 技术栈

- C# (.NET Standard 2.1)
- Harmony (Mod 框架)
- Unity (游戏引擎)

### 项目结构

```
KeycardRecordedIndicator/
├── ModBehaviour.cs              # 主 Mod 类
├── PatchItemDisplaySetup.cs     # Harmony Patch
├── RecordedIndicatorUI.cs       # UI 管理
├── Constants.cs                 # 常量定义
├── KeycardRecordedIndicator.csproj
├── KeycardRecordedIndicator.sln
├── info.ini                     # Mod 配置
└── README.md                    # 本文件
```

### 编译方法

1. 打开 Visual Studio
2. 打开 `KeycardRecordedIndicator.sln`
3. 在项目属性中设置 `DuckovPath` 为游戏安装目录
4. 选择 Release 配置
5. Build → Build Solution
6. DLL 文件将生成在 `bin/Release/` 目录

## 许可证

MIT License

## 致谢

感谢 Escape From Duckov 的开发者提供的 Mod 系统。

参考了以下项目：
- [Duckov Modding 示例](https://github.com/xvrsl/duckov_modding)
- [ItemLevelAndSearchSoundMod](https://github.com/dzj0821/ItemLevelAndSearchSoundMod)

---
本项目全程采用Claude Code进行开发

**祝你游戏愉快！** 🎮


