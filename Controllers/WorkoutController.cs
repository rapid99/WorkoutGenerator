using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MongoDB.Driver;
using WorkoutGenerator.Models;

namespace WorkoutGenerator.Controllers
{
    public class WorkoutController : Controller
    {
        MongoClient _client;
        IMongoDatabase _database;
        List<Exercise> _newWorkout;

        public WorkoutController()
        {
            _client = new MongoClient("mongodb://localhost:27017");
            _database = _client.GetDatabase("WorkoutGenerator");
        }
        public List<Workout> GetDatabase()
        {
            return _database.GetCollection<Workout>("Workouts").Find(FilterDefinition<Workout>.Empty).ToList();
        }
        public IActionResult Index()
        {
            var workouts = GetDatabase();

            return View(workouts);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var exercises = _database.GetCollection<Exercise>("Exercises").Find(FilterDefinition<Exercise>.Empty).ToList();

            return View(exercises);
        }

        [HttpPost]
        public IActionResult Create(Workout model)
        {
            var tempList = _database.GetCollection<Exercise>("TempList").Find(FilterDefinition<Exercise>.Empty).ToList();
            model.Exercises = new List<Exercise>();

            foreach (var exercise in tempList)
                model.Exercises.Add(exercise);

            model.Times_Completed = 0;

            _database.GetCollection<Workout>("Workouts").InsertOne(model);

            _database.GetCollection<Exercise>("TempList").DeleteMany(FilterDefinition<Exercise>.Empty);

            return RedirectToAction("Index", model);
        }

        public IActionResult AddExerciseToWorkout(string id)
        {
            var exercise_to_add = _database.GetCollection<Exercise>("Exercises").Find(i => i.Id == id).FirstOrDefault();

            _database.GetCollection<Exercise>("TempList").InsertOne(exercise_to_add);

            return RedirectToAction("Create");
        }

        [HttpGet]
        public IActionResult Details(string id)
        {
            if (id == null)
                return NotFound();

            var workout_to_view = _database.GetCollection<Workout>("Workouts").Find(i => i.Id == id).FirstOrDefault();

            if (workout_to_view == null)
                return NotFound();

            return View(workout_to_view);

        } 
    }
}