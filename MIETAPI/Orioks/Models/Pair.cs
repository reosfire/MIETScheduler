using System;
using System.Globalization;
using System.Linq;
using System.Text.Json;

namespace MIETAPI.Orioks.Models
{
    public readonly struct Pair
    {
        public readonly int Id;
        public readonly DateTime StartTime;
        public readonly DateTime EndTime;
        public TimeSpan Duration => EndTime - StartTime;

        public Pair(JsonProperty property)
        {
            Id = int.Parse(property.Name);
            JsonElement[] elements = property.Value.EnumerateArray().ToArray();
            StartTime = AsDateTime(elements[0]);
            EndTime = AsDateTime(elements[1]);
        }

        private static DateTime AsDateTime(JsonElement element)
        {
            string? value = element.GetString();
            if (string.IsNullOrWhiteSpace(value)) return DateTime.MinValue;

            return DateTime.ParseExact(value, "HH:mm", CultureInfo.InvariantCulture);
        }

        public override string ToString() => $"{Id} {StartTime:HH:mm} - {EndTime:HH:mm}";
    }
}
