using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Amazon.DynamoDBv2.DataModel;

namespace BookingApi.Domain.Events
{
    public class Event: IEvent
    {
        public string EventId { get; }
        public string ProviderId { get; }
        public Capacity Capacity { get; }
        public DateTime StartTime { get; }
        public DateTime EndTime { get; }
        public IEnumerable<IEventBooking> Bookings { get; }

        public Event(
            string eventId, 
            string providerId, 
            Capacity capacity, 
            DateTime startTime, 
            DateTime endTime, 
            IEnumerable<IEventBooking> bookings)
        {
            EventId = eventId;
            ProviderId = providerId;
            Capacity = capacity;
            StartTime = startTime;
            EndTime = endTime;
            Bookings = (bookings ?? Enumerable.Empty<IEventBooking>()).ToList();
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
            
            ((List<IEventBooking>)Bookings).Add(booking);
            DomainEvents.RaiseEventBookingCreated(new EventBookingCreated(EventId, booking));
        }

        private bool IsFull => CurrentPlacesBooked >= Capacity;

        private uint CurrentPlacesBooked => (uint)Bookings.Sum(a => (int) a.NumberOfPlacesBooked);
    }
}