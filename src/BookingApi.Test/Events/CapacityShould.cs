using System;
using BookingApi.Domain;
using BookingApi.Domain.Events;
using Xunit;

namespace BookingApi.Test
{
    public class CapacityShould
    {
        [Fact]
        public void BeEqualForTheSameValue()
        {
            Capacity c1 = 1;
            Capacity c2 = 1;
            Assert.Equal(c2, c1);
        }
        
        [Fact]
        public void ShouldCompareProperlyForDifferentValues()
        {
            Capacity c1 = 1;
            Capacity c2 = 2;
            Assert.True(c2 > c1);
        }
        
        [Fact]
        public void ShouldCompareProperlyForTheSameValues()
        {
            Capacity c1 = 1;
            Capacity c2 = 1;
            Assert.True(c2 == c1);
        }
        
        [Fact]
        public void BeGreaterForInfiniteThanFinite()
        {
            Capacity c1 = Capacity.Infinite;
            Capacity c2 = 1;
            Assert.True(c1 > c2);
        }
        
        [Fact]
        public void BeGreaterOrEqualForTwoInfinites()
        {
            Capacity c1 = Capacity.Infinite;
            Capacity c2 =  Capacity.Infinite;
            Assert.True(c1 <= c2);
        }
        
        [Fact]
        public void NotBeGreaterOrEqualForFinateAndInfinite()
        {
            Capacity c1 = 1;
            Capacity c2 =  Capacity.Infinite;
            Assert.True(c1 <= c2);
        }
    }
}