using API.TaskManagement.database;
using Library.TaskManagement.models;
using Library.TaskManagement.services;
using Library.TaskManagement.Standard.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.TaskManagement.EC
{
    public class AppointmentEC
    {
        public IEnumerable<AppointmentDTO> Get()
        {
            return TaskDatabase.Current.Appointments.Select(t => new AppointmentDTO(t));
        }

        public AppointmentDTO AddOrUpdate(AppointmentDTO appoint)
        {
            if (appoint.ID <= 0)
            {
                //CREATE
                appoint.ID = ItemService.Current.NextID;
                if (appoint.Name == null) { appoint.Name = "NULL"; }
                if (appoint.Description == null) { appoint.Description = "NULL"; }
                _ = TaskDatabase.Current.Add(new Appointment(appoint), false);
            }
            else
            {
                //UPDATE
                _ = TaskDatabase.Current.Add(new Appointment(appoint), true);
            }

            return appoint;
        }

        public AppointmentDTO Delete(int id)
        {
            return new AppointmentDTO(
                TaskDatabase.Current.Delete("APPOINTMENT", id));
        }
    }
}
