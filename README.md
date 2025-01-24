# 音乐播放器 (Music Player)

一个使用 WPF (.NET 9) 开发的简单音乐播放器应用，采用 MVVM 架构模式。

## 功能特点

- 支持播放本地音频文件
- 播放控制（播放/暂停/上一首/下一首）
- 进度条显示和拖动控制
- 音量调节
- 播放列表管理（添加、删除、保存）
- 自动记忆播放位置
- 支持播放列表持久化存储

## 技术栈

- .NET 9
- WPF (Windows Presentation Foundation)
- C#
- CSCore (音频处理库)
- TagLib# (音频标签解析)
- CommunityToolkit.Mvvm (MVVM 框架)

## 项目结构 
```
MusicPlayer/
├── Models/ # 数据模型
│ ├── Song.cs # 歌曲模型
│ └── Playlist.cs # 播放列表模型
├── ViewModels/ # 视图模型
│ ├── ViewModelBase.cs # 视图模型基类
│ └── MainViewModel.cs # 主窗口视图模型
├── Services/ # 服务层
│ ├── AudioPlayerService.cs # 音频播放服务
│ └── PlaylistService.cs # 播放列表服务
├── Utils/ # 工具类
│ └── JsonUtils.cs # JSON 序列化工具
├── Enum/ # 枚举定义
│ └── PlaybackOperation.cs # 播放操作枚举
├── Converters/ # 值转换器
│ ├── BaseValueConverter.cs
│ ├── BooleanToVisibilityConverter.cs
│ ├── IndexToColorConverter.cs
│ ├── SecondsToTimeSpan.cs
│ └── TimeSpanToSeconds.cs
├── Helpers/ # 辅助类
│ └── AudioHelper.cs # 音频文件解析辅助
└── Constants/ # 常量定义
└── FileConstants.cs # 文件路径常量
```

## 核心功能实现

### 音频播放
- 使用 CSCore 实现音频文件的播放、暂停、停止等功能
- 支持进度条拖动和音量调节
- 自动切换下一首

### 播放列表管理
- 支持播放列表的增删改查
- 使用 JSON 文件持久化存储播放列表
- 自动保存播放位置和播放列表状态

### MVVM 架构
- 使用 CommunityToolkit.Mvvm 实现 MVVM 模式
- 视图与逻辑分离，提高代码可维护性
- 使用值转换器处理数据绑定转换

## 开发环境要求

- Visual Studio 2022
- .NET 6 SDK
- Windows 操作系统

## 依赖项

- CSCore：音频处理核心库
- TagLib#：音频文件标签读取
- CommunityToolkit.Mvvm：MVVM 框架支持
- System.Text.Json：JSON 序列化

## 如何运行

1. 克隆仓库到本地
2. 使用 Visual Studio 2022 打开解决方案
3. 还原 NuGet 包
4. 编译并运行项目

## 使用说明

1. 点击"打开文件"按钮选择音频文件
2. 使用播放控制按钮控制音乐播放
3. 通过进度条拖动调整播放位置
4. 使用音量滑块调节音量
5. 双击播放列表中的歌曲直接播放
6. 程序会自动保存播放列表和播放位置

## 许可证

MIT License

## 贡献指南

欢迎提交 Issue 和 Pull Request 来帮助改进这个项目。

## 联系方式

如有问题或建议，请通过 Issue 与我们联系。