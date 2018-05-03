﻿using MongoDB.Bson;
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
        public string Sets { get; set; }
        [Required]
        public string Reps { get; set; }
        [Required]
        [Display(Name ="Weight / Speed")]
        public string Weight{ get; set; }
        public List<Exercise> TempList { get; set; }

    }
}
