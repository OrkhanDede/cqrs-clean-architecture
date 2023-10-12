using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using Newtonsoft.Json;

namespace Core.Utilities
{
    public static class Base64Util
    {
        public static string ObjectToString(object obj)
        {
            using MemoryStream ms = new MemoryStream();
            new BinaryFormatter().Serialize(ms, obj);
            return Convert.ToBase64String(ms.ToArray());
        }
        public static T StringToObject<T>(string base64String)
        {
            byte[] bytes = Convert.FromBase64String(base64String);
            using MemoryStream ms = new MemoryStream(bytes, 0, bytes.Length);
            ms.Write(bytes, 0, bytes.Length);
            ms.Position = 0;
            return (T)new BinaryFormatter().Deserialize(ms);
        }
        public static string ToBase64(this object obj)
        {
            string json = JsonConvert.SerializeObject(obj);

            byte[] bytes = Encoding.Default.GetBytes(json);

            return Convert.ToBase64String(bytes);
        }
        public static T FromBase64<T>(this string base64Text)
        {
            byte[] bytes = Convert.FromBase64String(base64Text);

            string json = Encoding.Default.GetString(bytes);

            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
