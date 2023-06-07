using Library.TaskManagement.interfaces;
using Library.TaskManagement.Standard.DTO;
using Library.TaskManagement.Standard.utilities;
using Newtonsoft.Json;

namespace Library.TaskManagement.models
{
    [JsonConverter(typeof(ItemJsonConverter))]
    public class Item : IItem
    {
        [JsonProperty]
        public int ID { get; set; }

        [JsonProperty]
        public string? Name { get; set; }

        [JsonProperty]
        public string? Description { get; set; }

        [JsonProperty]
        public int Priority { get; set; }

        public override string ToString()
        { return $"{ID} {Name}\n{Description}"; }

        public Item() { }

        public Item(ItemDTO i)
        {
            Name = i?.Name ?? string.Empty;
            Description = i?.Description ?? string.Empty;
            ID = i?.ID ?? 0;
            Priority = i?.Priority ?? 1;
        }
    }
}
