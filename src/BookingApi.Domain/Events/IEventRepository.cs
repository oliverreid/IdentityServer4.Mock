using System;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Newtonsoft.Json;

namespace BookingApi.Domain.Events
{
    public interface IEventRepository
    {
        Task SaveEvent(Event evt);
        Task<Event> GetEvent(string eventId, string providerId);
    }

    public class JsonPropertyConverter<T> : IPropertyConverter
    {
        public DynamoDBEntry ToEntry(object value)
        {
            DynamoDBEntry entry = new Primitive
            {
                Value = JsonConvert.SerializeObject(value)
            };
            return entry;
        }

        public object FromEntry(DynamoDBEntry entry)
        {
            Primitive primitive = entry as Primitive;
            if (primitive == null || !(primitive.Value is string))
                throw new ArgumentOutOfRangeException();

            var val = (string) primitive.Value;
            
            return string.IsNullOrEmpty(val) ? default(T) : JsonConvert.DeserializeObject<T>(val);
        }
    }
}