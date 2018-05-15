﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using WorkoutGenerator.Models;
using PagedList;

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

        public List<Exercise> GetDatabase()
        {
            return _database.GetCollection<Exercise>("Exercises").Find(FilterDefinition<Exercise>.Empty).ToList();
        }

        public IActionResult Index(string search)
        {
            var exercises = _database.GetCollection<Exercise>("Exercises").Find(FilterDefinition<Exercise>.Empty).SortBy(x => x.Title).ToEnumerable();

            if (!String.IsNullOrEmpty(search))
            {
                exercises = exercises.Where(a => a.Title.ToUpper().Contains(search.ToUpper()) || a.Type.ToUpper().Contains(search.ToUpper()) || a.Reps.ToUpper().Contains(search.ToUpper()) || a.Weight.ToUpper().Contains(search.ToUpper()));
            }

            return View("Index", exercises.ToList());
        }

        public IActionResult SortByType(string search)
        {
            var exercises = _database.GetCollection<Exercise>("Exercises").Find(FilterDefinition<Exercise>.Empty).SortBy(x => x.Type).Limit(7).ToEnumerable();

            if (!String.IsNullOrEmpty(search))
            {
                exercises = exercises.Where(a => a.Title.ToUpper().Contains(search.ToUpper()) || a.Type.ToUpper().Contains(search.ToUpper()) || a.Reps.ToUpper().Contains(search.ToUpper()) || a.Weight.ToUpper().Contains(search.ToUpper()));
            }

            return View("Index", exercises.ToList());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Exercise model)
        {
            _database.GetCollection<Exercise>("Exercises").InsertOne(model);

            return RedirectToAction("Index", model);
        }

        [HttpGet]
        public IActionResult Details(string id)
        {
            if (id == null)
                return NotFound();

            var exercise_to_view = _database.GetCollection<Exercise>("Exercises").Find(i => i.Id == id).FirstOrDefault();

            if (exercise_to_view == null)
                return NotFound();
             
            return View(exercise_to_view);

        }

        [HttpGet]
        public IActionResult Edit(string id)
        {
            if (id == null)
                return NotFound();

            var exercise_to_edit = _database.GetCollection<Exercise>("Exercises").Find(i => i.Id == id).FirstOrDefault();

            if (exercise_to_edit == null)
                return NotFound();

            return View(exercise_to_edit);

        }

        [HttpPost]
        public IActionResult Edit(Exercise model)
        {
            try
            {
                var filter = Builders<Exercise>.Filter.Eq("Id", model.Id);

                var updater = Builders<Exercise>.Update.Set("Title", model.Title);
                updater = updater.Set("Reps", model.Reps);
                updater = updater.Set("Weight", model.Weight);
                updater = updater.Set("Type", model.Type);

                var result = _database.GetCollection<Exercise>("Exercises").UpdateOne(filter, updater);

                if (result.IsAcknowledged == false)
                    return BadRequest("Unable to update " + model.Title);
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

            var exercise_to_delete = _database.GetCollection<Exercise>("Exercises").Find(i => i.Id == id).FirstOrDefault();

            if (exercise_to_delete == null)
                return NotFound();

            return View(exercise_to_delete);
        }

        [HttpPost]
        public IActionResult Delete(Exercise model)
        {
            try
            {
                var result = _database.GetCollection<Exercise>("Exercises").DeleteOne(i => i.Id == model.Id);

                if (result.IsAcknowledged == false)
                    return BadRequest("Unable to delete " + model.Title);

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return RedirectToAction("Index");
        }
    }
}