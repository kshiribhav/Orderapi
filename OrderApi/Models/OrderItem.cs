using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace OrderApi.Models
{
    public class OrderItem
    {
        [BsonId(IdGenerator =typeof(StringObjectIdGenerator))]
        [BsonElement("id")]
        public string Id { get; set; }

        [BsonElement("user")]
        public string User { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("price")]
        public string Price { get; set; }

        [BsonElement("restaurant")]
        public string Restaurant { get; set; }

        [BsonElement("type")]
        public string Type { get; set; }

        [BsonElement("location")]
        public string Location { get; set; }
        
    }
}
