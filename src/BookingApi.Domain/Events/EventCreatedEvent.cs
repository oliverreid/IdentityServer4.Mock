namespace BookingApi.Domain.Events
{
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