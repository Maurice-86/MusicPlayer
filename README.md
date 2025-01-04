# Music Player

一个基于 WPF 的现代化音乐播放器，使用 MVVM 架构模式开发。

## 项目结构
```
MusicPlayer/
├── App.xaml.cs # 应用程序入口，依赖注入配置
├── Models/ # 数据模型
├── ViewModels/ # 视图模型
│ ├── MainViewModel.cs # 主窗口视图模型
│ ├── PlayerViewModel.cs # 播放器视图模型
│ ├── LocalMusicViewModel.cs # 本地音乐视图模型
│ ├── SettingsViewModel.cs # 设置视图模型
│ └── ViewModelBase.cs # 视图模型基类
├── Views/ # 视图
│ ├── MainWindow.xaml # 主窗口
│ ├── PlayerView.xaml # 播放器控件
│ ├── LocalMusicView.xaml # 本地音乐视图
│ └── SettingsView.xaml # 设置视图
└── Services/ # 服务层
├── AudioService.cs # 音频播放服务
└── NavigationService.cs # 导航服务
```

## 主要功能

- 本地音乐播放
- 播放列表管理
- 进度条拖动控制
- 音量调节
- 播放模式切换（顺序播放/随机播放/单曲循环）
- 最小化到托盘

## 技术栈

- .NET 8.0
- WPF (Windows Presentation Foundation)
- MVVM 架构模式
- Material Design In XAML
- NAudio (音频处理)
- Microsoft.Extensions.DependencyInjection (依赖注入)
- CommunityToolkit.Mvvm (MVVM 工具包)

## 核心服务

### AudioService

音频播放核心服务，负责：
- 音频文件的播放/暂停/停止
- 播放进度控制
- 音量调节
- 自动播放下一首
- 播放状态管理

### NavigationService

导航服务，负责：
- 视图模型间的导航
- 当前视图模型的管理
- 视图切换事件通知

## 视图模型

### MainViewModel
- 主窗口逻辑
- 菜单项管理
- 导航控制

### PlayerViewModel
- 播放器控制逻辑
- 播放状态管理
- 进度条控制
- 音量控制

### LocalMusicViewModel
- 本地音乐列表管理
- 音乐文件扫描
- 播放列表操作

### SettingsViewModel
- 应用程序设置管理
- 播放模式设置
- 退出模式设置

## 使用说明

1. 启动应用后，点击"本地音乐"扫描本地音乐文件
2. 双击音乐列表中的歌曲开始播放
3. 使用底部播放控制栏控制播放
4. 在设置中可以配置播放模式和其他选项

## 开发说明

1. 克隆项目
2. 使用 Visual Studio 2022 打开解决方案
3. 还原 NuGet 包
4. 编译运行

## 依赖项

- MaterialDesignThemes
- NAudio
- Microsoft.Extensions.DependencyInjection
- CommunityToolkit.Mvvm