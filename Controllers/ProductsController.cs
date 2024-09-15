using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Product_Creation.Controllers.Data;
using Product_Creation.Controllers.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
using Product_Creation.Controllers.DTOs;

using Newtonsoft.Json;

namespace Product_Creation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public ProductsController(ApplicationDbContext context, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            return await _context.Products.ToListAsync();
        }

        // POST: api/Products
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct([FromForm] ProductCreateDto productDto, [FromForm] List<IFormFile> images)
        {
            Console.WriteLine($"CategoryId from request: {productDto.CategoryId}");

            if (!_context.Categories.Any(c => c.Id == productDto.CategoryId))
            {
                return BadRequest("Invalid CategoryId.");
            }

            var imagePaths = new List<string>();
            var webRootPath = _hostingEnvironment.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");

            var uploadsDirectory = Path.Combine(webRootPath, "uploads");
            if (!Directory.Exists(uploadsDirectory))
            {
                Directory.CreateDirectory(uploadsDirectory);
            }

            foreach (var image in images)
            {
                if (image.Length > 0)
                {
                    var fileName = Path.GetFileName(image.FileName);
                    var filePath = Path.Combine(uploadsDirectory, fileName);

                    try
                    {
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await image.CopyToAsync(stream);
                        }

                        imagePaths.Add($"/uploads/{fileName}");
                    }
                    catch (IOException ioEx)
                    {
                        Console.WriteLine($"Error saving image: {ioEx.Message}");
                        return StatusCode(StatusCodes.Status500InternalServerError, "Error saving image: " + ioEx.Message);
                    }
                }
            }

            var product = new Product
            {
                Name = productDto.Name,
                Price = productDto.Price,
                Description = productDto.Description,
                CategoryId = productDto.CategoryId,
                StartTime = productDto.StartTime,
                EndTime = productDto.EndTime,
                Thumbnail = imagePaths.FirstOrDefault()
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, new { product, images = imagePaths });
        }

        [HttpGet("categories")]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
        {
            var categories = await _context.Categories.ToListAsync();
            return Ok(categories);
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductCreateDto>> GetProduct(int id)
        {
            var product = await _context.Products
                .Where(p => p.Id == id)
                .Select(p => new ProductCreateDto
                {
                    Name = p.Name,
                    Price = p.Price,
                    Description = p.Description,
                    CategoryId = p.CategoryId,
                    StartTime = p.StartTime,
                    EndTime = p.EndTime
                })
                .FirstOrDefaultAsync();

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
