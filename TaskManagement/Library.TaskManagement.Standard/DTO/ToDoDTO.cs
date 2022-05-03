using Library.TaskManagement.models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Library.TaskManagement.Standard.DTO
{
    public class ToDoDTO : ItemDTO
    {
        [JsonProperty]
        public DateTimeOffset? Deadline { get; set; }
        [JsonProperty]
        public bool IsCompleted { get; set; }
        public ToDoDTO(Item i) : base(i)
        {
            Deadline = (i as ToDo)?.Deadline ?? new DateTimeOffset(DateTime.Now);
            IsCompleted = (i as ToDo)?.IsCompleted ?? false;
        }
        
        public ToDoDTO()
        { Deadline = new DateTimeOffset(DateTime.Now); }
    }
}
