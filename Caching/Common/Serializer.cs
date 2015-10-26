using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Caching.Common
{
    public static class Serializer
    {
        public static byte[] Serialize(object o)
        {
            if (o == null)
            {
                return null;
            }

            var binaryFormatter = new BinaryFormatter();
            using (var memoryStream = new MemoryStream())
            {
                binaryFormatter.Serialize(memoryStream, o);
                byte[] objectDataAsStream = memoryStream.ToArray();
                return objectDataAsStream;
            }
        }

        public static T Deserialize<T>(byte[] stream)
        {
            if (stream == null)
            {
                return default(T);
            }

            var binaryFormatter = new BinaryFormatter();
            using (var memoryStream = new MemoryStream(stream))
            {
                T result = (T) binaryFormatter.Deserialize(memoryStream);
                return result;
            }
        }

        public static string XmlSerialize<T>(T t) where T : new()
        {
            var sb = new StringBuilder();
            var ser = new XmlSerializer(typeof(T));
            using (var swriter = new StringWriter(sb))
            {
                ser.Serialize(swriter, t);
            }
            return sb.ToString();
        }

        public static T XmlDeserialize<T>(string data) where T : new()
        {
            var customType = new T();
            if (string.IsNullOrEmpty(data)) return customType;
            var serializer = new XmlSerializer(typeof(T));
            using (var reader = new StringReader(data))
            {
                customType = (T)serializer.Deserialize(reader);
            }
            return customType;
        }

        public static string JsonSerialize<T>(T t) where T : new()
        {
            return JsonConvert.SerializeObject(t);
        }

        public static T JsonDeserialize<T>(string data) where T : new()
        {
            return JsonConvert.DeserializeObject<T>(data);
        }

    }
}