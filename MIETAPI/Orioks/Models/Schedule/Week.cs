using System;
using System.Collections.Generic;

namespace MIETAPI.Orioks.Models.Schedule
{
    public readonly struct Week
    {
        private readonly Day[] _days;
        public Week(IEnumerable<Subject> subjects)
        {
            _days = new Day[7];
            
            List<Subject>[] daySubjects = new List<Subject>[7];
            for (int i = 0; i < 7; i++)
            {
                daySubjects[i] = new List<Subject>();
            }

            foreach (Subject subject in subjects)
                daySubjects[subject.Day].Add(subject);

            for (int i = 0; i < 7; i++)
            {
                _days[i] = new Day(daySubjects[i]);
            }
        }

        public Day GetDay(int day)
        {
            if (day < 0 || day > 6) throw new Exception("Day must be in [0, 6]");

            return _days[day];
        }
    }
}
