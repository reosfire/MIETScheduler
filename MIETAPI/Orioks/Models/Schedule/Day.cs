using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MIETAPI.Orioks.Models.Schedule
{
    public class Day : IEnumerable<Subject>
    {
        private readonly Dictionary<int, List<Subject>> _subjects = new Dictionary<int, List<Subject>>();

        public Day(IEnumerable<Subject> subjects)
        {
            foreach (Subject subject in subjects)
            {
                if (!_subjects.ContainsKey(subject.Class)) _subjects.Add(subject.Class, new List<Subject>());
                _subjects[subject.Class].Add(subject);
            }
        }

        public IEnumerator<Subject> GetEnumerator()
        {
            return _subjects.Values.SelectMany(subjects => subjects).GetEnumerator();
        }
        
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}