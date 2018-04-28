using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using WorkoutGenerator.Models;

namespace WorkoutGenerator.Controllers
{
    public class ExerciseController : Controller
    {
        MongoClient _client;
        IMongoDatabase _database;

        public ExerciseController()
        {
            _client = new MongoClient("mongodb://localhost:27017");
            _database = _client.GetDatabase("WorkoutGenerator");
        }

        public IActionResult Index()
        {
            return View("Index", GetDatabase());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Exercise model)
        {
            if (ModelState.IsValid)
                _database.GetCollection<Exercise>("Exercises").InsertOne(model);

            return RedirectToAction("Index", model);
        }

        public List<Exercise> GetDatabase()
        {
            return _database.GetCollection<Exercise>("Exercises").Find(FilterDefinition<Exercise>.Empty).ToList();
        }
    }
}