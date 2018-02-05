using System;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;

namespace BookingApi.Test.Common
{
    public static class Constants
    {

        public static readonly string TestProviderId =
            new Guid(Encoding.ASCII.GetBytes("dostuff").Concat(new Byte[16]).Take(16).ToArray()).ToString();
    }
}