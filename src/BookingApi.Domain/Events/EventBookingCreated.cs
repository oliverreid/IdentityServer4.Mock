namespace BookingApi.Domain.Events
{
    public class EventBookingCreated 
    {
        public EventBookingCreated(string eventId, IEventBooking eventBooking)
        {
            EventId = eventId;
            EventBooking = eventBooking;
        }

        public string EventId { get; private set; }
        public IEventBooking EventBooking { get; private set; }
    }
}