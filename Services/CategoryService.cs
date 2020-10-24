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

        public Category Get(int id)
        {
            return _categoryCollection
                .Find(c => c.Id == id)
                .FirstOrDefault();
        }

        public Category Create(Category category)
        {
            try
            {
                _categoryCollection.InsertOne(category);
                return category;
            }
            catch (MongoWriteException ex)
            {
                throw ex;
            }
        }

        public void Update(int id, Category category)
        {
            _categoryCollection.ReplaceOne(c => c.Id == id, category);
        }

        public void Remove(Category category)
        {
            _categoryCollection.DeleteOne(c => c.Id == category.Id);
        }

        public void Remove(int id)
        {
            _categoryCollection.DeleteOne(c => c.Id == id);
        }
    }
}
