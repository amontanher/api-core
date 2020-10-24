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

        public async Task<ActionResult<List<Product>>> Get()
        {
            return await _productCollection.Find(c => true).ToListAsync();
        }

        public Product Get(int id)
        {
            return _productCollection
                .Find(c => c.Id == id)
                .FirstOrDefault();
        }

        public Product Create(Product Product)
        {
            try
            {
                _productCollection.InsertOne(Product);
                return Product;
            }
            catch (MongoWriteException ex)
            {
                throw ex;
            }
        }

        public void Update(int id, Product Product)
        {
            _productCollection.ReplaceOne(c => c.Id == id, Product);
        }

        public void Remove(Product Product)
        {
            _productCollection.DeleteOne(c => c.Id == Product.Id);
        }

        public void Remove(int id)
        {
            _productCollection.DeleteOne(c => c.Id == id);
        }
    }
}
