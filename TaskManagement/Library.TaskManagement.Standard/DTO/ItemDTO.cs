using Library.TaskManagement.interfaces;
using Library.TaskManagement.models;
using Library.TaskManagement.Standard.utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Library.TaskManagement.Standard.DTO
{
    [JsonConverter(typeof(ItemDTOJsonConverter))]
    public class ItemDTO
    {
        [JsonProperty]
        public string? Name { get; set; }

        [JsonProperty]
        public string? Description { get; set; }
        [JsonProperty]
        public int ID { get; set; }
        [JsonProperty]
        public int Priority { get; set; }

        public override string ToString()
        { return $"{ID} {Name}\n{Description}"; }

        public ItemDTO() { }

        public ItemDTO(Item i)
        {
            Name = i?.Name ?? string.Empty;
            Description = i?.Description ?? string.Empty;
            ID = i?.ID ?? 0;
            Priority = i?.Priority ?? 1;
        }
    }
}
