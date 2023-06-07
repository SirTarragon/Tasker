using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Library.TaskManagement.models;
using Library.TaskManagement.services;
using Library.TaskManagement.Standard.DTO;
using Newtonsoft.Json;

namespace Library.TaskManagement.Standard.persistence
{
    public class Filebase
    {
        private string _root;
        private string _appointmentRoot;
        private string _todoRoot;
        private static Filebase? _instance;

        public static Filebase Current
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Filebase();
                }

                return _instance;
            }
        }

        private Filebase()
        {
            _root = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            _appointmentRoot = $@"{_root}\Appointments";
            _todoRoot = $@"{_root}\ToDos";

            if (Directory.Exists(_root))
            {
                //                Directory.CreateDirectory(_root);
                if (!Directory.Exists(_appointmentRoot))
                {
                    Directory.CreateDirectory(_appointmentRoot);
                }

                if (!Directory.Exists(_todoRoot))
                {
                    Directory.CreateDirectory(_todoRoot);
                }
            }
        }

        public Item AddOrUpdate(Item item)
        {
            if (item.ID <= 0)
            {
                item.ID = ItemService.Current.NextID;
            }

            string path;
            if (item is ToDo)
            {
                path = $"{_todoRoot}/{item.ID}.json";
            }
            else
            {
                path = $"{_appointmentRoot}/{item.ID}.json";
            }

            if (File.Exists(path))
            {
                File.Delete(path);
            }

            File.WriteAllText(path, JsonConvert.SerializeObject(item));

            return item;
        }

        public Item? Get(string type, int id)
        {
            string path = null;
            bool IsToDo = false;
            bool IsAppointment = false;
            if (type.ToUpper().Equals("TODO"))
            {
                IsToDo = true;
                path = $"{_todoRoot}/{id}.json";
            }
            else if (type.ToUpper().Equals("APPOINTMENT"))
            {
                IsAppointment = true;
                path = $"{_appointmentRoot}/{id}.json";
            }

            if (!string.IsNullOrEmpty(path) && File.Exists(path))
            {
                if (IsToDo)
                {
                    return JsonConvert.DeserializeObject<ToDo>(
                    File.ReadAllText(path));
                }
                else if (IsAppointment)
                {
                    return JsonConvert.DeserializeObject<Appointment>(
                        File.ReadAllText(path));
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        public List<ToDo>? ToDos
        {
            get
            {
                var root = new DirectoryInfo(_todoRoot);
                List<ToDo>? _todos = new List<ToDo>();
                try
                {
                    foreach (var todoFile in root.GetFiles())
                    {
                        var todo = JsonConvert.DeserializeObject<ToDo>(
                            File.ReadAllText(todoFile.FullName));
                        _todos.Add(todo);
                    }

                    if (_todos.Count == 0)
                    {
                        _todos = null;
                    }
                }
                catch (Exception e)
                {  // nuke the exception
                    Console.WriteLine(e);
                    _todos = null;
                }
                return _todos;
            }
        }

        public List<Appointment>? Appointments
        {
            get
            {
                var root = new DirectoryInfo(_appointmentRoot);
                List<Appointment>? _apps = new List<Appointment>();
                try
                {
                    foreach (var appFile in root.GetFiles())
                    {
                        var app = JsonConvert.DeserializeObject<Appointment>(
                            File.ReadAllText(appFile.FullName));
                        _apps.Add(app);
                    }

                    if (_apps.Count == 0)
                    {
                        _apps = null;
                    }
                }
                catch (Exception e)
                {  // nuke the exception
                    Console.WriteLine(e);
                    _apps = null;
                }
                return _apps;
            }
        }

        public bool Delete(string type, int id)
        {
            string path;
            if (type.ToUpper().Equals("TODO"))
            {
                path = $"{_todoRoot}/{id}.json";
            }
            else if (type.ToUpper().Equals("APPOINTMENT"))
            {
                path = $"{_appointmentRoot}/{id}.json";
            }
            else
            {
                return false;
            }

            if (File.Exists(path))
            {
                File.Delete(path);
                return true;
            }

            return false;
        }
    }
}
