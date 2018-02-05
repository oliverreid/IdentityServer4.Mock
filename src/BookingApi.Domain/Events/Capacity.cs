using System;
using Newtonsoft.Json;

namespace BookingApi.Domain.Events
{
    public class Capacity : IComparable<Capacity>
    {
        private Capacity()
        {
        }

        private Capacity(uint value)
        {
            Type = CapacityType.Finite;
            Value = value;
        }

        [JsonProperty]
        public CapacityType Type { get; private set; } 

        [JsonProperty]
        public uint? Value { get; private set; }

        public static implicit operator Capacity(uint i)
        {
            return new Capacity(i);
        }
       

        public bool Equals(Capacity other)
        {
            return Type == other.Type && Value == other.Value;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Capacity && Equals((Capacity) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((int) Type * 397) ^ Value.GetHashCode();
            }
        }

        public static bool operator ==(Capacity left, Capacity right)
        {
            return (CapacityType.Finite == left.Type && left.Type == right.Type) || left.Value == right.Value;
        }      

        
        public static bool operator !=(Capacity left, Capacity right)
        {
            return !(left == right);
        }
        
        public static bool operator <(Capacity left, Capacity right)
        {
            return left.CompareTo(right) < 0;
        }
        public static bool operator >(Capacity left, Capacity right)
        {
            return left.CompareTo(right) > 0;
        }
        
        public static bool operator <=(Capacity left, Capacity right)
        {
            return left.CompareTo(right) < 0 || left.Equals(right);
        }
        public static bool operator >=(Capacity left, Capacity right)
        {
            return left.CompareTo(right) > 0 || left.Equals(right);
        }
        
        public bool HasSpaceFor(uint i) => this.Type == CapacityType.Infinite ? true : i <= this.Value.Value;
        
        public static Capacity Infinite { get; } = new Capacity();
        
        public static Capacity Finite(uint i) => new Capacity(i);

        public int CompareTo(Capacity other)
        {
            if (this.Type == CapacityType.Infinite)
            {
                if (other.Type == CapacityType.Infinite)
                {
                    return 0;
                }

                return 1;
            }

            if (other.Type == CapacityType.Infinite)
            {
                return -1;
            }

            return this.Value.Value.CompareTo(other.Value.Value);
        }

    }
}