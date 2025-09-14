using Api.Data;
using Api.Filters;
using Api.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : ControllerBase
    {
    

        private readonly ILogger<ProductsController> _logger;
        private readonly ApplicationDbContext _context;

        public ProductsController(ILogger<ProductsController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }


        [HttpPost]
        [Route("")]
        public IActionResult CreateProduct(Product product)
        {
            //product.Id = 0;
            _context.Set<Product>().Add(product);
            _context.SaveChanges();

            return Ok((product.Id,product.Name).ToString());
        }

        [HttpGet]
        [Route("{id}")]
        [CheckPermission(Permision.Read)]

        public IActionResult GetProduct(int id)
        {

            var product = _context.Set<Product>().Find(id);
           

            return Ok(product);
        }

        [HttpGet]
        [Route("")]
        [CheckPermission(Permision.Read)]
        public IActionResult GetAllProducts()
        {

            var product = _context.Set<Product>().ToList();


            return Ok(product);
        }




        [HttpPut]
        [Route("")]
        public IActionResult UpdateProduct(Product product) {

            var Dbproduct = _context.Set<Product>().Find(product.Id);

            if (product.Id == null)
                return NotFound();

            Dbproduct.Name = product.Name;
            Dbproduct.Sku = product.Sku;



            _context.Set<Product>().Update(Dbproduct);
            _context.SaveChanges();

            return Ok();
        
        
        }



        [HttpDelete]
        [Route("{id}")]

        public IActionResult DeleteProduct(int id)
        {
            Product product = _context.Set<Product>().Find(id);
            _context.Remove(product);
            _context.SaveChanges();

            return Ok();


        }









    }
}
