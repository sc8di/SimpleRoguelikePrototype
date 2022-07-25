using System;
using System.Collections.Generic;
using System.Linq;

namespace TestingTest
{
    public static class RandomUtils
    {
        private static Random _random = new Random();

        public static int Range(int min, int max) 
            => _random.Next(min, max);

        public static T GetRandomEnumValue<T>(Type enumType, List<T> excludedValues)
        {
            var values = ((T[])Enum.GetValues(enumType)).ToList().FindAll(value => !excludedValues.Contains(value));
            return values[Range(0, values.Count)];
        }
    }
}