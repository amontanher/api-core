using API.Simple.Model;
using API.Simple.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Simple.Controllers
{
    [ApiController]
    [Route("v1/products")]
    public class ProductController : ControllerBase
    {
        private readonly ProductService _service;

        public ProductController(ProductService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("")]
        public async Task<ActionResult<List<Product>>> Get()
        {
            return await _service.Get();
        }

        [HttpPost]
        [Route("")]
        public ActionResult<Product> Post([FromBody]Product model)
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
        public IActionResult Update(int id, Product ProductIn)
        {
            var Product = _service.Get(id);

            if (Product == null)
                return NotFound();

            _service.Update(id, ProductIn);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var Product = _service.Get(id);

            if (Product == null)
                return NotFound();

            _service.Remove(id);

            return NoContent();
        }
    }
}
