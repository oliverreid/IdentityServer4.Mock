using System;
using System.Linq;
using System.Threading.Tasks;
using Amazon;
using Amazon.DynamoDBv2;
using BookingApi.Domain.Events;
using BookingApi.Test.Common;
using Xunit;

namespace BookingApi.IntegrationTest.Events
{
    public class DynamoEventRepositoryShould
    {
        [Fact]
        public async Task SaveAndRetrieveEvent()
        {
            var eventId = Guid.NewGuid().ToString();
            var providerId = Constants.TestProviderId;

            var someEvent = new Event(eventId, providerId, Capacity.Finite(5), DateTime.Now, DateTime.Now.AddHours(1), Enumerable.Empty<IEventBooking>());
            var client =
                new AmazonDynamoDBClient("AKIAIAIB7WQQUIRO4Z3Q", "9xsWrFZ2aosm1U+nbnYf6Oeh6lXmTD3Gwplgqc2F", RegionEndpoint.EUWest1);
            
            using (var repo = new DynamoEventRepository(client))
            {
                await repo.SaveEvent(someEvent);
                var saved = await repo.GetEvent(eventId, providerId);
                Assert.Equal(eventId, saved.EventId);
                Assert.Equal(providerId, saved.ProviderId);
                Assert.Equal(5u, saved.Capacity.Value.Value);
            }
        }
    }
}