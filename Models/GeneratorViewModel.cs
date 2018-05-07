using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace WorkoutGenerator.Models
{
    public class GeneratorViewModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonElement("Random Workout")]
        public Workout Random_Workout { get; set; }
        public int Times_Completed { get; set; }
    }
}
