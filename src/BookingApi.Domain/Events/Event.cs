using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Amazon.DynamoDBv2.DataModel;

namespace BookingApi.Domain.Events
{
    public class Event: IEvent
    {
        [DynamoDBProperty("EventId")]
        private readonly string _eventId;
        [DynamoDBProperty("ProdivderId")]
        private readonly string _providerId;
        [DynamoDBProperty("Capacity")]
        private readonly Capacity _capacity;
        [DynamoDBProperty("StartTime")]
        private readonly DateTime _startTime;
        [DynamoDBProperty("EndTime")]
        private readonly DateTime _endTime;

        [DynamoDBProperty("Bookings")]
        private readonly IList<IEventBooking> _bookings = new List<IEventBooking>();

        public Event(string eventId, string providerId, Capacity capacity, DateTime startTime, DateTime endTime)
        {
            _eventId = eventId;
            _providerId = providerId;
            _capacity = capacity;
            _startTime = startTime;
            _endTime = endTime;
            DomainEvents.RaiseEventCreated(new EventCreatedEvent(eventId, providerId));
        }


        public bool CanTakeBooking(IEventBooking booking)
        {
            return !IsFull;
        }
        
        public void AddBooking(IEventBooking booking)
        {
            if (!CanTakeBooking(booking))
            {
                throw new EventBookingException("Cannot take this booking");
            }
            
            _bookings.Add(booking);
            DomainEvents.RaiseEventBookingCreated(new EventBookingCreated(_eventId, booking));
        }

        private bool IsFull => CurrentPlacesBooked >= _capacity;

        private uint CurrentPlacesBooked => (uint)_bookings.Sum(a => (int) a.NumberOfPlacesBooked);
        
    }

    public class EventCreatedEvent
    {
        public EventCreatedEvent(string eventId, string providerId)
        {
            EventId = eventId;
            ProviderId = providerId;
        }

        public string EventId { get; private set; }
        public string ProviderId { get; private set; }
    }
}