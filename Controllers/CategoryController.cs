using API.Simple.Data.Configuration;
using API.Simple.Model;
using API.Simple.Services;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Simple.Controllers
{
    [ApiController]
    [Route("v1/categories")]
    public class CategoryController : ControllerBase
    {
        private readonly CategoryService _service;

        public CategoryController(CategoryService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("")]
        public async Task<ActionResult<List<Category>>> Get()
        {
            return await _service.Get();            
        }

        [HttpPost]
        [Route("")]
        public ActionResult<Category> Post([FromBody]Category model)
        {
            if (ModelState.IsValid)
            {
                _service.Create(model);                
                return model;
            }
            else
                return BadRequest(ModelState);            
        }

        [HttpPut("{id}")]
        public IActionResult Update(string id, Category categoryIn)
        {
            var category = _service.Get(id);

            if (category == null)            
                return NotFound();            

            _service.Update(id, categoryIn);

            return NoContent();
        }

        [HttpDelete]
        public IActionResult Delete(string id)
        {
            var category = _service.Get(id);

            if (category == null)
                return NotFound();

            _service.Remove(id);

            return NoContent();
        }
    }
}
