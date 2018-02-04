namespace BookingApi.Domain.Events
{
    public interface IEvent
    {
        bool CanTakeBooking(IEventBooking booking);
        void AddBooking(IEventBooking booking);
    }
}