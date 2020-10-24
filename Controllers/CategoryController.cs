using API.Simple.Data.Configuration;
using API.Simple.Model;
using API.Simple.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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

        private readonly ILogger<CategoryController> _logger;

        public CategoryController(CategoryService service,
                                  ILogger<CategoryController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        [Route("")]
        public async Task<ActionResult<List<Category>>> Get()
        {
            try
            {
                var rng = new Random();

                if (rng.Next(0, 5) < 2)
                {
                    throw new Exception("Ooops what happened?");
                }
                _logger.LogInformation("OK DONE!");
                _logger.LogDebug("DEBUGANDO");
                return Ok(await _service.Get());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Something bag happenned");
                return new StatusCodeResult(400);
            }
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
        public IActionResult Update(int id, Category categoryIn)
        {
            var category = _service.Get(id);

            if (category == null)            
                return NotFound();            

            _service.Update(id, categoryIn);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var category = _service.Get(id);

            if (category == null)
                return NotFound();

            _service.Remove(id);

            return NoContent();
        }
    }
}
