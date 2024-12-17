using System;
using System.Reflection;
using AutoFixture;
using AutoFixture.Kernel;
using Castle.Components.DictionaryAdapter.Xml;

namespace MechanicApp.Tests
{
    public class DateTimeRangeSpecimenBuilder : ISpecimenBuilder
    {
        private readonly DateTime _min = new DateTime(1910, 01, 01);
        private readonly DateTime _max = new DateTime(2020, 01, 01);
        private Random gen = new Random();
        public object Create(object request, ISpecimenContext context)
        {
            if (request is PropertyInfo property && property == typeof(DateTime))
            {
                int range = (_max - _min).Days;
                return _min.AddDays(gen.Next(range));
            }

            return new NoSpecimen();
        }
    }
    
}
