using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using WorkoutGenerator.Models;

namespace WorkoutGenerator.Controllers
{
    public class GeneratorController : Controller
    {
        MongoClient _client;
        IMongoDatabase _database;

        public GeneratorController()
        {
            _client = new MongoClient("mongodb://localhost:27017");
            _database = _client.GetDatabase("WorkoutGenerator");
        }

        public List<Workout> GetWorkoutDatabase()
        {
            return _database.GetCollection<Workout>("Workouts").Find(FilterDefinition<Workout>.Empty).ToList();
        }

        public List<GeneratorViewModel> GetGeneratorDatabase()
        {
            return _database.GetCollection<GeneratorViewModel>("RandomWorkout").Find(FilterDefinition<GeneratorViewModel>.Empty).ToList();
        }

        public IActionResult Index()
        {
            var generatorDb = GetGeneratorDatabase();
            
            return View(generatorDb);
        }

        public IActionResult Random()
        {
            _database.GetCollection<GeneratorViewModel>("RandomWorkout").DeleteMany(FilterDefinition<GeneratorViewModel>.Empty);

            var workoutDb = GetWorkoutDatabase();

            var random = new Random();
            var r = random.Next(workoutDb.Count());
            var generator = new GeneratorViewModel();

            generator.Random_Workout = workoutDb[r];

            _database.GetCollection<GeneratorViewModel>("RandomWorkout").InsertOne(generator);

            return RedirectToAction("Index", generator);
        }

        //public IActionResult FinishedWorkout(Workout model)
        //{
        //    var filter = Builders<Workout>.Filter.Eq("Id", model.Id);

        //    model.Times_Completed++;
        //    var updater = Builders<Workout>.Update.Set("Times_Completed", model.Times_Completed);

        //    _database.GetCollection<Workout>("Workouts").UpdateOne(filter, updater);

        //    return RedirectToAction("Index");           
        //}

    }
}