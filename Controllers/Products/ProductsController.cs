using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using RelatedEntities.Data;
using RelatedEntities.Dtos;
using RelatedEntities.Models;

namespace RelatedEntities.Controllers
{
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly DataContext _context;
        // Constructor para inyección de dependencias
        public ProductsController(DataContext context){

            _context = context;
        }

        [HttpGet]
        [Route("api/[controller]")]
        public IEnumerable<Product> Products()
        {
            return _context.Products.ToList();
        }

        //Obtener por Id de la PRODUCTO(PRODUCT)
        [HttpGet]
        [Route("api/[controller]/{Id}")]
        public async Task<ActionResult<Product>> GetProduct(int Id)
        {
             //Include() incliye las relaciones de ambas entidades (User,Product)
            var Product = await _context.Products.FirstOrDefaultAsync(o => o.Id == Id);

            if(Product == null){
              return BadRequest("Prodcto no existente actualmente.");
            }
            return Product;
        }

        // Eliminar por Id del Producto (Product)
        [HttpDelete]
        [Route("api/Product/{Id}")]
        public async Task<ActionResult> DeleteProduct(int Id)
        {
            // Buscar el producto por su Id
            var product = await _context.Products.FindAsync(Id);

            // Si el producto no existe, retornar NotFound
            if (product == null)
            {
                return NotFound("Producto no encontrado.");
            }

            // Verifica si el producto está referenciado en alguna orden
            bool hasOrders = _context.Orders.Any(o => o.ProductId == Id);

            // Si hay órdenes asociadas, retornar un BadRequest
            if (hasOrders)
            {
                return BadRequest("No se puede eliminar el producto porque está asociado con una o más órdenes.");
            }

            // Eliminar el producto si no está referenciado
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            // Retornar un mensaje de éxito
            return Ok("Producto eliminado exitosamente.");
        }

        //Agregar Prooductos(Product)
        [HttpPost]
        [Route("api/Product")]
        public IActionResult CreateProduct([FromBody] ProductDto ProductDto)
        {
            if (ProductDto == null)
            {
                return BadRequest("El cuerpo de la solicitud no puede estar vacío.");
            }

            // Mapeo del DTO a la entidad Product
            var Product = new Product
            {
                Name = ProductDto.Name,
                Description = ProductDto.Description,
                Price = ProductDto.Price
            };


            _context.Products.Add(Product);
            _context.SaveChanges();

            return Ok();
        }

        // PUT: api/Product/{id}
        // Este método actualiza un producto existente. Recibe el ID en la URL y un `ProductDto` en el cuerpo de la solicitud.
        [HttpPut]
        [Route("api/Product/{id}")]
        public IActionResult UpdateProduct(int id, [FromBody] ProductDto productDto)
        {
            // Validación inicial del DTO recibido
            if (productDto == null || id != productDto.Id)
            {
                return BadRequest("Datos inválidos proporcionados.");
            }

            // Verifica si el producto con el ID proporcionado existe.
            var existingProduct = _context.Products.Find(id);
            if (existingProduct == null)
            {
                // Retorna un mensaje claro si el ID no existe.
                return NotFound($"El producto con Id {id} no fue encontrado.");
            }

            // Actualiza las propiedades del producto existente usando los valores del DTO.
            existingProduct.Name = productDto.Name;
            existingProduct.Description = productDto.Description;
            existingProduct.Price = productDto.Price;

            try
            {
                // Llama al contexto para actualizar el producto en la base de datos.
                _context.Products.Update(existingProduct);
                
                // Guarda los cambios en la base de datos.
                _context.SaveChanges();

                // Retorna un mensaje de éxito con código 200 OK.
                return Ok("El producto fue actualizado exitosamente.");
            }
            catch (DbUpdateException ex)
            {
                // Si ocurre un error relacionado con la base de datos, captura la excepción y devuelve un mensaje específico.
                return StatusCode(500, $"Error al actualizar el producto en la base de datos: {ex.Message}");
            }
            catch (Exception ex)
            {
                // Si ocurre cualquier otro error, captura la excepción y devuelve un mensaje de error genérico.
                return StatusCode(500, $"Ocurrió un error inesperado: {ex.Message}");
            }
        }
    }
}