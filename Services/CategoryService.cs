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

        internal async Task<ActionResult<List<Category>>> Find()
        {
            return await _categoryCollection.Find(new BsonDocument()).ToListAsync();
        }

        internal void Create(Category model)
        {
            _categoryCollection.InsertOne(model);
        }
    }
}
