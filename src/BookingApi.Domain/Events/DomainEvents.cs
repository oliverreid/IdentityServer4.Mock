using System;
using System.Collections.Generic;
using System.Linq;

namespace BookingApi.Domain.Events
{
    public static class DomainEvents
    {
        [ThreadStatic]
        private static readonly List<object> Events;

        static DomainEvents()
        {
            Events = new List<object>();
        }

        public static IDisposable OnEventBookingCreated(Action<EventBookingCreated> handler) =>
            AddHandler(handler);

        public static void RaiseEventBookingCreated(EventBookingCreated @event) => RaiseEvent(@event);

        public static IDisposable OnEventCreated(Action<EventCreatedEvent> handler) => AddHandler(handler);

        public static void RaiseEventCreated(EventCreatedEvent @event) => RaiseEvent(@event);
        
        private static IDisposable AddHandler<T>(Action<T> handler)
        {
            var indexToRemove = Events.Count;
            Events.Add(handler);
            return new Unscubscriber(() => Events.RemoveAt(indexToRemove));
        }

        private static void RaiseEvent<T>(T @event)
        {
            foreach (var handler in Events.OfType<Action<T>>())
            {
                handler(@event);
            }
        }
        
        private class Unscubscriber : IDisposable
        {
            private readonly Action _dispose;

            public Unscubscriber(Action dispose)
            {
                _dispose = dispose;
            }

            public void Dispose() => _dispose();
        }
    }
}