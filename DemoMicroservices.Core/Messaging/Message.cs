using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoMicroservices.Core.Messaging
{
    public class Message : IMessage
    {
        public Guid Id { get; set; }

        public string ToJson()
        {
            return System.Text.Json.JsonSerializer.Serialize(this);
        }

        public static T FromJson<T>(string json) where T : IMessage
        {
            return System.Text.Json.JsonSerializer.Deserialize<T>(json);
        }
    }
}
