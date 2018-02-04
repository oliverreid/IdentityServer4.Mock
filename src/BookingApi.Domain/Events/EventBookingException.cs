
using System;

namespace BookingApi.Domain.Events
{
    public class EventBookingException : Exception
    {
        public EventBookingException(string message) : base(message)
        {
        }
    }
}