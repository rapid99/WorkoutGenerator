using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkoutGenerator.Models;

namespace WorkoutGenerator.Models
{
    public class Workout
    {
        public List<Exercise> Exercises { get; set; }
        public int Times_Completed { get; set; }
    }
}
