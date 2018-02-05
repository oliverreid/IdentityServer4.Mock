﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
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

    public class DynamoEventRepository : IEventRepository, IDisposable
    {
        private readonly DynamoDBContext _context;

        public DynamoEventRepository(AmazonDynamoDBClient client)
        {
            _context = new DynamoDBContext(client);
        }

        public Task SaveEvent(Event evt) => _context.SaveAsync(EventDto.FromEvent(evt));
        
        public Task<Event> GetEvent(string eventId, string providerId) =>
            _context.LoadAsync<EventDto>(eventId, providerId).ContinueWith(t => t.Result.ToEvent());

        public void Dispose()
        {
            _context?.Dispose();
        }

        [DynamoDBTable("Events")]
        public class EventDto
        {
            [DynamoDBHashKey("EventId")]
            public string EventId { get; set; }

            [DynamoDBProperty("ProdivderId")]
            public string ProviderId { get; set; }

            [DynamoDBProperty("Capacity", typeof(JsonPropertyConverter<Capacity>))]
            public Capacity Capacity { get; set; }

            [DynamoDBProperty("StartTime")]
            public DateTime StartTime { get; set; }

            [DynamoDBProperty("EndTime")]
            public DateTime EndTime { get; set; }

            [DynamoDBProperty("Bookings", typeof(JsonPropertyConverter<IEnumerable<IEventBooking>>))]
            public IEnumerable<IEventBooking> Bookings { get; set; }

            public static EventDto FromEvent(Event @event) => new EventDto()
            {
                EventId = @event.EventId,
                ProviderId = @event.ProviderId,
                Capacity = @event.Capacity,
                StartTime = @event.StartTime,
                EndTime = @event.EndTime,
                Bookings = @event.Bookings
            };
            
            public Event ToEvent() => new Event(EventId, ProviderId, Capacity, StartTime, EndTime, Bookings);
        }
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