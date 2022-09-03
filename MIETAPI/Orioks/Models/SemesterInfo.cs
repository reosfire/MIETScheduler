using System;
using System.Globalization;
using System.Text.Json;

namespace MIETAPI.Orioks.Models
{
    public readonly struct SemesterInfo
    {
        public readonly DateTime SemesterStart;
        public readonly DateTime SessionStart;
        public readonly DateTime SessionEnd;
        public readonly DateTime NextSemesterStart;

        public int CurrentWeekNumber => ((DateTime.Now - SemesterStart).Days / 7) + 1;
        public int CurrentWeekType => (CurrentWeekNumber - 1) % 4;

        public SemesterInfo(JsonElement element)
        {
            SemesterStart = AsDateTime(element.GetProperty("semester_start"));
            SessionStart = AsDateTime(element.GetProperty("session_start"));
            SessionEnd = AsDateTime(element.GetProperty("session_end"));
            NextSemesterStart = AsDateTime(element.GetProperty("next_semester_start"));
        }

        private static DateTime AsDateTime(JsonElement element)
        {
            string? value = element.GetString();
            if (string.IsNullOrWhiteSpace(value)) return DateTime.MinValue;

            return DateTime.ParseExact(value, "yyyy-MM-dd", CultureInfo.InvariantCulture);
        }
    }
}
