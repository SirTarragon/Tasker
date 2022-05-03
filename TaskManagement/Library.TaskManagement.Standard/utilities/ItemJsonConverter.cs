using Library.TaskManagement.models;
using Library.TaskManagement.Standard.DTO;
using Newtonsoft.Json.Linq;
using System;

namespace Library.TaskManagement.Standard.utilities
{
    public class ItemDTOJsonConverter : JsonCreationConverter<ItemDTO>
    {
        protected override ItemDTO Create(Type objectType, JObject jObject)
        {
            if (jObject == null) throw new ArgumentNullException("jObject");

            if (jObject["isCompleted"] != null || jObject["IsCompleted"] != null)
            {
                return new ToDoDTO();
            }
            else if (jObject["attendees"] != null || jObject["Attendees"] != null)
            {
                return new AppointmentDTO();
            }
            else
            {
                return new ItemDTO();
            }
        }
    }

    public class ItemJsonConverter : JsonCreationConverter<Item>
    {
        protected override Item Create(Type objectType, JObject jObject)
        {
            if (jObject == null) throw new ArgumentNullException("jObject");

            if (jObject["isCompleted"] != null || jObject["IsCompleted"] != null)
            {
                return new ToDo();
            }
            else if (jObject["attendees"] != null || jObject["Attendees"] != null)
            {
                return new Appointment();
            }
            else
            {
                return new Item();
            }
        }
    }
}
