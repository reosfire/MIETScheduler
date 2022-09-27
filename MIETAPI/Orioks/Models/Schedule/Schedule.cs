using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace MIETAPI.Orioks.Models.Schedule
{
    public readonly struct Schedule
    {
        private readonly Week[] _weeks;

        public Schedule(JsonElement element)
        {
            int weeksCount = element.EnumerateArray().Select(e => e.GetProperty("week").GetInt32()).Max() + 1;

            Subject[] subjects = element.EnumerateArray().Select(e => new Subject(e)).ToArray();

            List<Subject>[] weeksSubjects = new List<Subject>[weeksCount];
            for (int i = 0; i < weeksCount; i++)
            {
                weeksSubjects[i] = new List<Subject>();
            }

            foreach (Subject subject in subjects)
                weeksSubjects[subject.Week].Add(subject);

            _weeks = weeksSubjects.Select(e => new Week(e)).ToArray();
        }

        public Week GetForWeek(int number) => _weeks[number % _weeks.Length];
    }
}