using MusicPlayer.Models;
using System.IO;
using System.Text;
using System.Text.Json;

namespace MusicPlayer.Helps
{
    public static class SettingsHelper
    {
        private static readonly string path = Path.Combine(AppContext.BaseDirectory, "settings.json");

        public static void SaveSettings(Settings settings)
        {
            // 创建 JsonSerializerOptions 实例
            var options = new JsonSerializerOptions
            {
                WriteIndented = true, // 格式化 JSON
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping // 直接保存中文字符
            };

            // 序列化对象为 JSON 字符串
            var json = JsonSerializer.Serialize(settings, options);

            // 保存 JSON 字符串到文件
            File.WriteAllText(path, json, Encoding.UTF8); // 指定 UTF-8 编码
        }

        public static Settings LoadSettings()
        {
            if (!File.Exists(path))
                return new Settings();

            var json = File.ReadAllText(path);
            return JsonSerializer.Deserialize<Settings>(json) ?? new Settings();
        }
    }
}
