using Library.TaskManagement.Standard.DTO;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;

namespace Library.TaskManagement.models
{
    public class Appointment : Item
    {
        [JsonProperty]
        public DateTimeOffset? Start { get; set; }
        [JsonProperty]
        public DateTimeOffset? End { get; set; }

        [JsonProperty]
        public ObservableCollection<string> Attendees { get; set; }

        public Appointment() {
            Start = new DateTimeOffset(DateTime.Now);
            End = new DateTimeOffset(DateTime.Now);
            Attendees = new ObservableCollection<string>();
        }

        public Appointment(AppointmentDTO dto) : base (dto)
        {
            Start = dto.Start;
            End = dto.End;
            Attendees = dto.Attendees;
        }

        public override string ToString() {
            return $"ID: {ID} NAME: {Name}\nDESCRIPTION:\n{Description}\n"+
                $"From {Start} to {End}\n{string.Join(", ", Attendees)}";
        }
    }
}
