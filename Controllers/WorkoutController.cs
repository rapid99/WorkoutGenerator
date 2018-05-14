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
            //temporary list to hold exercises in the workout currently being created
            var tempList = _database.GetCollection<Exercise>("TempList").Find(FilterDefinition<Exercise>.Empty).ToList();

            //instantiate model prop
            model.Exercises = new List<Exercise>();

            //now that model prop is instantiated, add all exercises in temp list to model prop
            foreach (var exercise in tempList)
                model.Exercises.Add(exercise);

            model.Times_Completed = 0;
            model.Sets = 1;

            //add model to db
            _database.GetCollection<Workout>("Workouts").InsertOne(model);

            //delete temp list contents for next use
            _database.GetCollection<Exercise>("TempList").DeleteMany(FilterDefinition<Exercise>.Empty);

            return RedirectToAction("Index", model);
        }

        public IActionResult AddExerciseToWorkout(string id)
        {
            var exercise_to_add = _database.GetCollection<Exercise>("Exercises").Find(i => i.Id == id).FirstOrDefault();
            var tempList = _database.GetCollection<Exercise>("TempList").Find(FilterDefinition<Exercise>.Empty).ToList();

            foreach(var e in tempList)
                if (e.Id == id)
                    return BadRequest("Cannot add an exercise more than once to the same workout");

            _database.GetCollection<Exercise>("TempList").InsertOne(exercise_to_add);

            return RedirectToAction("Create");
        }

        public IActionResult DeleteTempList()
        {
            _database.GetCollection<Exercise>("TempList").DeleteMany(FilterDefinition<Exercise>.Empty);

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

        [HttpPost]
        public IActionResult Update(Workout model)
        {
            try
            {
                var filter = Builders<Workout>.Filter.Eq("Id", model.Id); 

                var updater = Builders<Workout>.Update.Set("Sets", model.Sets);
                updater = updater.Set("Times_Completed", model.Times_Completed);

                var result = _database.GetCollection<Workout>("Workouts").UpdateOne(filter, updater);

                if (result.IsAcknowledged == false)
                    return BadRequest("Unable to update workout");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Delete(string id)
        {
            if (id == null)
                return NotFound();

            var workout_to_delete = _database.GetCollection<Workout>("Workouts").Find(i => i.Id == id).FirstOrDefault();

            if (workout_to_delete == null)
                return NotFound();

            return View(workout_to_delete);
        }

        [HttpPost]
        public IActionResult Delete(Workout model)
        {
            try
            {
                var result = _database.GetCollection<Workout>("Workouts").DeleteOne(i => i.Id == model.Id);

                if (result.IsAcknowledged == false)
                    return BadRequest("Unable to delete workout");

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return RedirectToAction("Index");
        }

    }
}