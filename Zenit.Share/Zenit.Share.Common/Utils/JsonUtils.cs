using System.Text.Json;

namespace Zenit.Share.Common.Utils
{
    public class JsonUtils
    {
        public static T JsonDeserializeMessage<T>(string body)
        {
            return JsonSerializer.Deserialize<T>(body) 
                ?? throw new Exception("Failed to deserialize message.");
        }

        public static string JsonSerializeMessage<T>(T body)
        {
            return JsonSerializer.Serialize(body) 
                ?? throw new Exception("Failed to serialize message.");
        }
    }
}