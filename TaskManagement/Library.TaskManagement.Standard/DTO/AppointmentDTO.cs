using Library.TaskManagement.models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Library.TaskManagement.Standard.DTO
{
    public class AppointmentDTO : ItemDTO
    {
        [JsonProperty]
        public DateTimeOffset? Start { get; set; }
        [JsonProperty]
        public DateTimeOffset? End { get; set; }
        [JsonProperty]
        public ObservableCollection<string> Attendees { get; set; }

        public AppointmentDTO(Item i) : base(i)
        {
            Start = (i as Appointment)?.Start ?? new DateTimeOffset(DateTime.Now);
            End = (i as Appointment)?.End ?? new DateTimeOffset(DateTime.Now);
            Attendees = (i as Appointment)?.Attendees ?? new ObservableCollection<string>();
        }

        public AppointmentDTO()
        {
            Start = new DateTimeOffset(DateTime.Now);
            End = new DateTimeOffset(DateTime.Now);
            Attendees = new ObservableCollection<string>();
        }
    }
}
