using Microsoft.AspNetCore.Mvc.Rendering;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WorkoutGenerator.Models;

namespace WorkoutGenerator.Models
{
    public class Workout
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [Required]
        public List<Exercise> Exercises { get; set; }
        [Required]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Times completed can only be a number!")]
        [Display(Name ="Times Completed")]
        public int Times_Completed { get; set; }
        [Required]
        public int Sets { get; set; }
    }
}
