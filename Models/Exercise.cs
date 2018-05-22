using Microsoft.AspNetCore.Mvc.Rendering;
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
        public string Reps { get; set; }
        [Required]
        [Display(Name ="Weight / Speed")]
        public string Weight{ get; set; }
        [Required]
        [Display(Name = "Type")]
        public string Type { get; set; }
        [Display(Name = "Weight Training?")]
        public bool Is_Weight_Training { get; set; }
        public List<Exercise> TempList { get; set; }

    }
}
