using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using WorkoutGenerator.Models;

namespace WorkoutGenerator.Controllers
{
    public class WeightTrainingController : Controller
    {
        MongoClient _client;
        IMongoDatabase _database;

        public WeightTrainingController()
        {
            _client = new MongoClient("mongodb://localhost:27017");
            _database = _client.GetDatabase("WorkoutGenerator");
        }

        public List<WeightTrainingViewModel> GetWeightTrainingDatabase()
        {
            return _database.GetCollection<WeightTrainingViewModel>("WeightTrainingCards").Find(FilterDefinition<WeightTrainingViewModel>.Empty).ToList();
        }

        public IActionResult Index()
        {
            var filter = Builders<Exercise>.Filter.Eq(e => e.Is_Weight_Training, true);

            ViewBag.TotalExercises = _database.GetCollection<Exercise>("Exercises").Find(filter).Count();

            return View();
        }
    }
}