namespace BookingApi.Domain.Events
{
    public interface IEventBooking
    {
        uint NumberOfPlacesBooked { get; }
    }
}