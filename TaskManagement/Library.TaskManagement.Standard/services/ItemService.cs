using Library.TaskManagement.helpers;
using Library.TaskManagement.models;
using Library.TaskManagement.Standard.DTO;
using Library.TaskManagement.Standard.persistence;
using Library.TaskManagement.Standard.utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Library.TaskManagement.services
{
    public class ItemService
    {
        private ObservableCollection<ItemDTO> items;
        private ListNavigator<Item> listNav;
        private int _idcount;
        //        private string persistencePath;
        private JsonSerializerSettings serializerSettings
            = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };

        private string urlRoot = "http://localhost:5000";

        private bool _useAPI = true;

        static private ItemService? instance;

        public ObservableCollection<ItemDTO> Items
        {
            get
            {
                if (UseAPI)
                {
                    try
                    {
                        Clear();
                        LoadFromServer();
                        return items;
                    }
                    catch (Exception e)
                    {
                        //                        UseAPI = false;
                        Console.WriteLine(e);
                        Clear();
                        LoadFromDisk();
                        return items;
                    }
                }
                else
                {   // local directory stuff is being used
                    Clear();
                    LoadFromDisk();
                    return items;
                }
            }
        }

        public int Count
        {
            get { return items.Count; }
        }

        public int IDCount
        {
            get
            {
                var local = Items;
                var check = local.Select(i => i.ID).Max() + 1;
                if (!(_idcount >= check))
                {
                    _idcount = check;
                }
                return _idcount;
            }
            set { _idcount = value; }
        }

        public bool UseAPI
        {
            get
            {
                return _useAPI;
            }
            set { _useAPI = value; }
        }

        public static ItemService Current
        {
            get
            {
                if (instance == null)
                {
                    instance = new ItemService();
                }
                return instance;
            }
        }

        private ItemService()
        {
            items = new ObservableCollection<ItemDTO>();
            try
            {
                LoadFromServer();
            }
            catch (Exception e)
            {
                UseAPI = false;
                LoadFromDisk();
                Console.WriteLine(e);
            }

            if (items.Any())
            {
                _idcount = items.Select(i => i.ID).Max() + 1;
            }
            else
            {
                _idcount = 1;
            }
        }

        private void LoadFromServer()
        {
            var payload = JsonConvert
                .DeserializeObject<List<ItemDTO>>(new WebRequestHandler()
                .Get($"{urlRoot}/Item").Result);

            if (payload != null)
            {
                payload.ToList().ForEach(items.Add);
            }
            // listNav = new ListNavigator<ItemDTO>(Items, 5);
        }

        private void LoadFromDisk()
        {
            var local = Filebase.Current;

            List<Appointment>? localAppts = local.Appointments;
            List<ToDo>? localToDos = local.ToDos;

            if (localAppts != null)
            {
                var payload = localAppts.Select(i => new AppointmentDTO(i));
                payload.ToList().ForEach(items.Add);
            }

            if (localToDos != null)
            {
                var payload = localToDos.Select(i => new ToDoDTO(i));
                payload.ToList().ForEach(items.Add);
            }
        }

        public async Task<ItemDTO> Add(ItemDTO i)
        {
            if (_useAPI)
            {
                string s;
                if (i is ToDoDTO) { s = $"{urlRoot}/ToDo/AddOrUpdate"; }
                else { s = $"{urlRoot}/Appointment/AddOrUpdate"; }
                var req = await new WebRequestHandler().Post(s, i);
                return JsonConvert.DeserializeObject<ItemDTO>(req);
            }
            else
            {
                var local = Filebase.Current;
                if (i is ToDoDTO)
                {
                    local.AddOrUpdate(new ToDo(i as ToDoDTO));
                }
                else if (i is AppointmentDTO)
                {
                    local.AddOrUpdate(new Appointment(i as AppointmentDTO));
                }
                else
                {
                    return null;
                }
                return i;
            }
        }

        public async Task<ItemDTO> Remove(ItemDTO i)
        {
            if (_useAPI)
            {
                return await Remove(i.ID);
            }
            else
            {
                var local = Filebase.Current;
                if (i is ToDoDTO)
                {
                    local.Delete("TODO", i.ID);
                }
                else if (i is AppointmentDTO)
                {
                    local.Delete("APPOINTMENT", i.ID);
                }
                else
                {
                    return null;
                }
                return i;
            }
        }

        public async Task<ItemDTO> Remove(int id)
        {
            if (_useAPI)
            {
                DeleteItemDTO deleteItem = new DeleteItemDTO { IDToDelete = id };
                var req = await new WebRequestHandler().Post($"{urlRoot}/ToDo/Delete", deleteItem);
                if (string.IsNullOrEmpty(req))
                {
                    req = await new WebRequestHandler().Post($"{urlRoot}/Appointment/Delete", deleteItem);
                }
                return JsonConvert.DeserializeObject<ItemDTO>(req);
            }
            else
            {
                var local = Filebase.Current;

                ToDo? IsToDo = local.Get("TODO", id) as ToDo;
                Appointment? IsAppt = local.Get("APPOINTMENT", id) as Appointment;

                if (IsToDo != null)
                {
                    local.Delete("TODO", id);
                    return new AppointmentDTO(IsAppt);
                }
                else if (IsAppt != null)
                {
                    local.Delete("APPOINTMENT", id);
                    return new ToDoDTO(IsToDo);
                }
                else
                {
                    return null;
                }
            }
        }

        public IEnumerable<ItemDTO> Search(string key)
        {
            if (_useAPI)
            {
                return JsonConvert.DeserializeObject<List<ItemDTO>>(
                    new WebRequestHandler().Search($"{urlRoot}/Item/Search", key).Result);
            }
            else
            {
                return Items.Where(i => string.IsNullOrEmpty(key)
                                || (i?.Name?.ToUpper()?.Contains(key.ToUpper()) ?? false)
                                || (i?.Description?.ToUpper()?.Contains(key.ToUpper()) ?? false)
                                || ((i as AppointmentDTO)?.Attendees?
                                .Select(t => t.ToUpper())?.Contains(key.ToUpper()) ?? false)).ToList();
            }
        }

        public void Clear()
        {
            items.Clear();
        }

        public ItemDTO? GetByID(int id)
        { return items.FirstOrDefault(t => t.ID == id); }

        public int NextID
        {
            get
            {
                return IDCount;
            }
        }
    }
}
