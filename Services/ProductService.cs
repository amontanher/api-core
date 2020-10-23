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
    public class ProductService
    {
        readonly IMongoCollection<Product> _productCollection;

        public ProductService(IDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _productCollection = database.GetCollection<Product>(settings.ProductCollectionName);
        }

        public async Task<ActionResult<List<Product>>> Find()
        {
            return await _productCollection.Find(new BsonDocument()).ToListAsync();
        }

        public Product GetById(int id)
        {
            return _productCollection.Find(p => p.Id == id).FirstOrDefault();
        }

        public void Create(Product model)
        {
            _productCollection.InsertOne(model);
        }
    }
}
