using System;
using System.Linq;
using BookingApi.Domain.Events;
using Xunit;

namespace BookingApi.Test.Events
{
    public class EventShould
    {
        [Fact]
        public void BroadcastEventCreatedEvent()
        {
            EventCreatedEvent ece = null;
            DomainEvents.OnEventCreated(e => ece = e);

            var testEventId = Guid.NewGuid().ToString();
            var testProviderId = Guid.NewGuid().ToString();
            
            var newEvent = new Event(testEventId, testProviderId, Capacity.Infinite, DateTime.Now, DateTime.Now, Enumerable.Empty<IEventBooking>());
            
            Assert.True(ece.EventId == testEventId && ece.ProviderId == testProviderId);
        }
        
        [Fact]
        public void BroadcastEventBookingCreatedEvent()
        {
            EventBookingCreated ece = null;
            DomainEvents.OnEventBookingCreated(e => ece = e);

            var testEventId = Guid.NewGuid().ToString();
            var testProviderId = Guid.NewGuid().ToString();
            
            var newEvent = new Event(testEventId, testProviderId, Capacity.Infinite, DateTime.Now, DateTime.Now, Enumerable.Empty<IEventBooking>());
            
            var eventBooking = new DummyEventBooking(5);
            newEvent.AddBooking(eventBooking);
            
            Assert.True(ece.EventId == testEventId && ece.EventBooking == eventBooking);
        }
        
        [Fact]
        public void NotTakeBookingForFullEvent()
        {
            EventBookingCreated ece = null;
            DomainEvents.OnEventBookingCreated(e => ece = e);

            var testEventId = Guid.NewGuid().ToString();
            var testProviderId = Guid.NewGuid().ToString();
            
            var newEvent = new Event(testEventId, testProviderId, Capacity.Finite(5), DateTime.Now, DateTime.Now, Enumerable.Empty<IEventBooking>());
            
            var eventBooking = new DummyEventBooking(5);
            newEvent.AddBooking(eventBooking);

            var nextBooking = new DummyEventBooking(2);

            Assert.False(newEvent.CanTakeBooking(nextBooking));
            Assert.Throws<EventBookingException>(() => newEvent.AddBooking(nextBooking));
        }

        class DummyEventBooking : IEventBooking
        {
            public DummyEventBooking(uint numberOfPlacesBooked)
            {
                NumberOfPlacesBooked = numberOfPlacesBooked;
            }

            public uint NumberOfPlacesBooked { get; }
        }
       
    }
}