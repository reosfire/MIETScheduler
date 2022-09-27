using System;
using System.Text.Json;

namespace MIETAPI.Orioks.Models.Schedule
{
    public readonly struct Subject
    {
        public readonly string Name;
        public readonly SubjectType Type;
        public readonly int Day;
        public readonly int Class;
        public readonly int Week;
        public readonly int WeekRecurrence;
        public readonly string Location;
        public readonly string Teacher;

        public Subject(JsonElement element)
        {
            Name = element.GetProperty("name").GetString()!;
            string typeString = element.GetProperty("type").GetString()!;
            Type = typeString switch
            {
                "Лек" => SubjectType.Lecture,
                "Пр" => SubjectType.Practice,
                "Лаб" => SubjectType.Laboratory,
                "" => SubjectType.Another,
                _ => throw new Exception($"Unknown type: {typeString}")
            };

            Day = element.GetProperty("day").GetInt32();
            Class = element.GetProperty("class").GetInt32();
            Week = element.GetProperty("week").GetInt32();
            WeekRecurrence = element.GetProperty("week_recurrence").GetInt32();
            Location = element.GetProperty("location").GetString()!;
            Teacher = element.GetProperty("teacher").GetString()!;
        }

        public override string ToString()
        {
            return $"Name:{Name} Type:{Type} Day:{Day} Class:{Class} Week:{Week} WeekRecurrence:{WeekRecurrence} Location:{Location} Teacher:{Teacher}";
        }
    }
}