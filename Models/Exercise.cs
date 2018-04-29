using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WorkoutGenerator.Models
{
    public class Exercise
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Intervals can only be numbers")]
        public int Intervals { get; set; }
        [Required]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Reps can only be numbers")]
        public int Reps { get; set; }
        [Required]
        public string Weight { get; set; }

    }
}
