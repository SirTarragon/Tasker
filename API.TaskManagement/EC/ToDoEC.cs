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
    public class ToDoEC
    {
        public IEnumerable<ToDoDTO> Get()
        {
            return TaskDatabase.Current.ToDos.Select(t => new ToDoDTO(t));
        }

        public ToDoDTO AddOrUpdate(ToDoDTO todo)
        {
            if (todo.ID <= 0)
            {
                //CREATE
                var service = ItemService.Current;
                todo.ID = service.NextID;
                if (todo.Name == null) { todo.Name = "NULL"; }
                if (todo.Description == null) { todo.Description = "NULL"; }
                _ = TaskDatabase.Current.Add(new ToDo(todo), false);
            }
            else
            {
                //UPDATE
                _ = TaskDatabase.Current.Add(new ToDo(todo), true);
            }

            return todo;
        }

        public ToDoDTO Delete(int id)
        {
            return new ToDoDTO(
                TaskDatabase.Current.Delete("TODO", id));
        }
    }
}
