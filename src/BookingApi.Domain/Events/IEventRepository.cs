using System.Threading.Tasks;
using Amazon.DynamoDBv2.DataModel;

namespace BookingApi.Domain.Events
{
    public interface IEventRepository
    {
        Task SaveEvent(Event evt);
    }

//    public class DynamoEventRepository : IEventRepository
//    {
//        DynamoDBContext ctx = new DynamoDBContext();
//        public Task SaveEvent<TBooking, TDetails>(Event<TBooking, TDetails> evt) where TBooking : IEventBooking where TDetails : IEventDetails
//        {
//            throw new System.NotImplementedException();
//        }
//    }
}