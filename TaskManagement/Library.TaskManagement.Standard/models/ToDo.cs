using Library.TaskManagement.Standard.DTO;
using Newtonsoft.Json;
using System;

namespace Library.TaskManagement.models
{
    public class ToDo : Item
    {
        [JsonProperty]
        public DateTimeOffset? Deadline { get; set; }
        [JsonProperty]
        public bool IsCompleted { get; set; }

        public override string ToString()
        {
            return $"ID: {ID} NAME: {Name}\nDESCRIPTION:\n{Description}\n"+
                $"DEADLINE: {Deadline}\nCOMPLETION: {IsCompleted}";
        }

        public ToDo() 
        { Deadline = new DateTimeOffset(DateTime.Now); }

        public ToDo(ToDoDTO dto) : base(dto)
        {
            Deadline = dto.Deadline;
            IsCompleted = dto.IsCompleted;
        }
    }
}
