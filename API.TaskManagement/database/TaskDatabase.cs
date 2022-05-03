using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library.TaskManagement.models;
using Library.TaskManagement.services;
using Library.TaskManagement.Standard.utilities;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;

namespace API.TaskManagement.database
{
    public class TaskDatabase
    {
        private string connection;
        private static TaskDatabase _instance;

        public static TaskDatabase Current
        {
            get
            {
                if(_instance == null)
                {
                    _instance = new TaskDatabase();
                }

                return _instance;
            }
        }

        private TaskDatabase()
        {   // VALID MONGODB CONNECTION STRING NEEDED
            connection = "";
        }

        public Item Add(Item item, bool update)
        {
            if (item.ID <= 0)
            {
                item.ID = ItemService.Current.NextID;
            }

            MongoClient dbClient = new MongoClient(connection);

            bool IsToDo = item is ToDo;
            bool IsAppt = item is Appointment;

            var database = dbClient.GetDatabase("Tasks");

            IMongoCollection<BsonDocument> collection;

            if (IsToDo)
            {
                collection = database.GetCollection<BsonDocument>("todos");
            }
            else if (IsAppt)
            {
                collection = database.GetCollection<BsonDocument>("appointments");
            }
            else
            {
                return null;
            }

            try
            {
                if (update)
                {
                    var filter = Builders<BsonDocument>.Filter.Eq("ID", item.ID);
                    _ = collection.ReplaceOne(filter,
                        BsonDocument.Parse(JsonConvert.SerializeObject(item)));
                }
                else
                {
                    collection.InsertOne(BsonDocument.Parse(
                        JsonConvert.SerializeObject(item)));
                }

                return item;
            }
            catch
            {
                return null;
            }
        }

        public Item Delete(string type, int id)
        {
            MongoClient dbClient = new MongoClient(connection);
            var filter = Builders<BsonDocument>.Filter.Eq("ID", id);
            var database = dbClient.GetDatabase("Tasks");
            IMongoCollection<BsonDocument> collection;
            if (type.ToUpper().Equals("TODO"))
            {
                collection = database.GetCollection<BsonDocument>("todos");
            }
            else if (type.ToUpper().Equals("APPOINTMENT"))
            {
                collection = database.GetCollection<BsonDocument>("appointments");
            }
            else
            {
                return null;
            }

            try
            {
                var projection = Builders<BsonDocument>.Projection.Exclude("_id");
                var document = collection.Find(filter).Project(projection).FirstOrDefault();
                var conversion = BsonTypeMapper.MapToDotNetValue(document);
                var new_json = JsonConvert.SerializeObject(conversion);
                var found = JsonConvert.DeserializeObject<Item>(new_json);
                _ = collection.DeleteOne(filter);
                return found;
            }
            catch
            {
                return null;
            }
        }

        public List<ToDo> ToDos
        {
            get
            {
                List<ToDo> _todos = new List<ToDo>();
                MongoClient dbClient = new MongoClient(connection);
                var database = dbClient.GetDatabase("Tasks");
                var collection = database.GetCollection<BsonDocument>("todos");
                var projection = Builders<BsonDocument>.Projection.Exclude("_id");
                var documents = collection.Find(new BsonDocument()).Project(projection).ToList();
                var conversion = documents.ConvertAll(BsonTypeMapper.MapToDotNetValue);
                var new_json = JsonConvert.SerializeObject(conversion);
                var payload = JsonConvert.DeserializeObject<List<ToDo>>(new_json);
                if (payload != null)
                {
                    payload.ToList().ForEach(_todos.Add);
                }
                return _todos;
            }
        }

        public List<Appointment> Appointments
        {
            get
            {
                List<Appointment> _appts = new List<Appointment>();
                MongoClient dbClient = new MongoClient(connection);
                var database = dbClient.GetDatabase("Tasks");
                var collection = database.GetCollection<BsonDocument>("appointments");
                var projection = Builders<BsonDocument>.Projection.Exclude("_id");
                var documents = collection.Find(new BsonDocument()).Project(projection).ToList();
                var conversion = documents.ConvertAll(BsonTypeMapper.MapToDotNetValue);
                var new_json = JsonConvert.SerializeObject(conversion);
                var payload = JsonConvert.DeserializeObject<List<Appointment>>(new_json);
                if (payload != null)
                {
                    payload.ToList().ForEach(_appts.Add);
                }
                return _appts;
            }
        }
    }
}
