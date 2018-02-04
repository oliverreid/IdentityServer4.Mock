namespace BookingApi.Domain.Bookings
{
    public struct Booking
    {
        public Booking(string bookingId, string eventId)
        {
            BookingId = bookingId;
            EventId = eventId;
        }

        public string BookingId { get; private set; }
        public string EventId { get; private set; }

        public bool Equals(Booking other)
        {
            return string.Equals(BookingId, other.BookingId) && string.Equals(EventId, other.EventId);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Booking && Equals((Booking) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((BookingId != null ? BookingId.GetHashCode() : 0) * 397) ^ (EventId != null ? EventId.GetHashCode() : 0);
            }
        }
    }
}