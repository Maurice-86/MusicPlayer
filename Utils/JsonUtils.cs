using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MusicPlayer.Models;

namespace MusicPlayer.Utils
{
    public static class JsonUtils<T>
    where T : class, new()
    {
        private static readonly JsonSerializerOptions _options = new()
        {
            WriteIndented = true,
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };

        /// <summary>
        /// 保存信息到 Json
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="model"></param>
        public static void SaveInfoToJson(T model, string filePath)
        {
            var json = JsonSerializer.Serialize(model, _options);
            File.WriteAllText(filePath, json, Encoding.UTF8);
        }

        /// <summary>
        /// 从 Json 读取信息
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static T? ReadInfoFromJson(string filePath)
        {
            if (!File.Exists(filePath)) return null;

            var file = File.ReadAllText(filePath);
            try
            {
                return JsonSerializer.Deserialize<T>(file, _options);
            }
            catch (JsonException)
            {
                return null;
            }
        }
    }
}
