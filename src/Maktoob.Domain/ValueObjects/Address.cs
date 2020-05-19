using System;
using System.Collections.Generic;
using System.Text;

namespace Maktoob.Domain.ValueObjects
{
    public class Address : ValueObject
    {
        public string City { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string Street { get; set; }
        public string ZipCode { get; set; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Street;
            yield return Country;
            yield return ZipCode;
            yield return State;
            yield return City;
        }
    }
}
