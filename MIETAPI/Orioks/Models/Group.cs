using System.Text.Json;

namespace MIETAPI.Orioks.Models
{
    public readonly struct Group
    {
        public readonly int Id;
        public readonly string? Name;

        public Group(JsonElement element)
        {
            Id = element.GetProperty("id").GetInt32();
            Name = element.GetProperty("name").GetString();
        }

        public override string ToString() => $"{Id} {Name}";
    }
}
