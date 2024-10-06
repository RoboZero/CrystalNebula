using System.IO;
using UnityEngine;

namespace Source.Serialization
{
    public class JsonSerializer
    {
        public string Serialize(object value)
        {
            return JsonUtility.ToJson(value);
        }

        public T? Deserialize<T>(string json)
        {
            return JsonUtility.FromJson<T>(json);
        }
    }
}