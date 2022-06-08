using Catalog.API.Entities;
using Catalog.API.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Catalog.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger<CatalogController> _logger;

        public CatalogController(IProductRepository productRepository, ILogger<CatalogController> logger)
        {
            _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger)); ;
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<IEnumerable<Product>>> GetAll()
        {
            var result = await _productRepository.GetAll();

            if (result is null)
                return NotFound();
            return Ok(result);
        }

        [HttpGet("{id:length(24)}", Name = "GetProduct")]
        //[Route("[action]")]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<Product>> GetById(string id)
        { 
            var result = await _productRepository.GetById(id);

            if (result is null)
                return NotFound();
            return Ok(result);
        }

        [HttpGet]
        [Route("[action]/{name}")]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<Product>> GetByName(string name)
        { 
            var result = await _productRepository.GetByName(name);
            //string ak = "jmkfg";
            
            if (result is null)
                return NotFound();
            return Ok(result);
        }

        [HttpGet]
        [Route("[action]/{category}", Name ="GetProductByCategory")]
        [ProducesResponseType(typeof(Product), 200)]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<Product>> GetByCategory(string category)
        {
            var result = await _productRepository.GetByCategory(category);

            if (result is null)
                return NotFound();
            return Ok(result);

            //var products = await _productRepository.GetByCategory(category);
            //return Ok(products);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Product>> Create([FromBody] Product product)
        {
            await _productRepository.Create(product);
            return CreatedAtRoute("GetProduct", new { id = product.Id }, product);
        }

        [HttpPut]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Product>> Update([FromBody] Product product)
        {
            var result = await _productRepository.Update(product);

            return Ok(result);
        }

        [HttpDelete("{id:length(24)}", Name = "DeleteProduct")]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> Delete(string id)
        { 
            var result = await _productRepository.Delete(id);

            return Ok(result);
        }
    }
}
