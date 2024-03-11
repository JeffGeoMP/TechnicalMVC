using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechnicalMVC.Data;
using TechnicalMVC.Models.Entities;
using TechnicalMVC.Models.Product;

namespace TechnicalMVC.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult ListProduct()
        {
            return View();
        }

        [HttpGet]
        public IActionResult AddProduct()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> EditProduct(int productId)
        {
            var product = await _context.Products.Include(p => p.FamilyProduct).Where(x => x.Id == productId).FirstOrDefaultAsync();

            var model = new ProductViewModel
            {
                Id = product.Id,
                SKU = product.SKU.Replace("SKU", ""),
                Description = product.Description,
                Price = Math.Round(product.Price, 2),
                Category = new CategoryViewModel
                {
                    Id = product.FamilyProductId,
                    Name = product.FamilyProduct.Name
                }
            };

            return View(model);
        }

        /// <summary>
        /// Get for all products.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _context.Products.Include(p => p.FamilyProduct).Select(x => new ProductViewModel
            {
                Id = x.Id,
                SKU = x.SKU,
                Description = x.Description,
                Price = Math.Round(x.Price, 2),
                Category = new CategoryViewModel
                {
                    Id = x.FamilyProduct.Id,
                    Name = x.FamilyProduct.Name
                }
            }).ToListAsync();

            return Ok(products);
        }

        /// <summary>
        /// Get family product all.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetFamilyProducts()
        {
            var familyProducts = await _context.FamilyProducts
                .Select(x => new CategoryViewModel
                {
                    Id = x.Id,
                    Name = x.Name
                }).ToListAsync();

            return Ok(familyProducts);
        }

        /// <summary>
        /// Add family product.
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> AddFamilyProduct([FromBody] CategoryViewModel category)
        {
            try
            {
                await _context.FamilyProducts.AddAsync(new FamilyProducts
                {
                    Name = category.Name
                });

                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            
        }

        /// <summary>
        /// Add new product.
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> AddProduct([FromBody] ProductViewModel product)
        {
            try
            {
                await _context.Products.AddAsync(new Product
                {
                    SKU = "SKU" + product.SKU,
                    Description = product.Description,
                    FamilyProductId = product.Category.Id,
                    Price = product.Price
                });

                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Delete product.
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> DeleteProduct(int productId) 
        {
            try
            {
               _context.Products.Remove(await _context.Products.FindAsync(productId));
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> EditProduct([FromBody] ProductViewModel product)
        {
            try
            {
                var productEntity = await _context.Products.FindAsync(product.Id);
                productEntity.SKU = "SKU" + product.SKU;
                productEntity.Description = product.Description;
                productEntity.FamilyProductId = product.Category.Id;
                productEntity.Price = product.Price;

                _context.Products.Update(productEntity);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
