using API.Simple.Data.Configuration;
using API.Simple.Model;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Simple.Services
{
    public class CategoryService
    {
        readonly IMongoCollection<Category> _categoryCollection;

        public CategoryService(IDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _categoryCollection = database.GetCollection<Category>(settings.CategoryCollectionName);
        }

        public async Task<ActionResult<List<Category>>> Get()
        {
            return await _categoryCollection.Find(c => true).ToListAsync();
        }

        public Category Get(string id)
        {
            return _categoryCollection
                .Find(c => c.Id.ToString() == id)
                .FirstOrDefault();
        }

        public Category Create(Category category)
        {
            _categoryCollection.InsertOne(category);
            return category;
        }

        public void Update(string id, Category category)
        {
            _categoryCollection.ReplaceOne(c => c.Id.ToString() == id, category);
        }

        public void Remove(Category category)
        {
            _categoryCollection.DeleteOne(c => c.Id.ToString() == category.Id.ToString());
        }

        public void Remove(string id)
        {
            _categoryCollection.DeleteOne(c => c.Id.ToString() == id);
        }
    }
}
