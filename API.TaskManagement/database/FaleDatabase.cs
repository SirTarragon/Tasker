using Library.TaskManagement.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.TaskManagement.database
{
    public class FaleDatabase
    {
        public static List<Item> Appointments = new List<Item>
        {
            //          new Appointment{Name = "Appointment 1", Description="Appointment 1 Desc", Start=DateTime.Today, ID = 1},
        };

        public static List<Item> ToDos = new List<Item>
        {
            //          new ToDo{Name = "ToDo 1", Description="ToDo 1 Desc", IsCompleted=false, ID = 2, Priority = 5}
        };
    }
}
