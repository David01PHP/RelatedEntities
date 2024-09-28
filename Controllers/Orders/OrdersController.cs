using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using RelatedEntities.Data;
using RelatedEntities.Models;
using RelatedEntities.Dtos;
using System.Linq;

namespace RelatedEntities.Controllers
{
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly DataContext _context;
        // Constructor para inyección de dependencias
        public OrdersController(DataContext context){

            _context = context;
        }


        // Acción para obtener todos los productos
        [HttpGet]
        [Route("api/[controller]")]
        public IEnumerable<Order> Orders()
        {
            //Include() incliye las relaciones de ambas entidades (User,Product)
            return _context.Orders.Include(o => o.User).Include(o => o.Product).ToList();
        }


        //Obtener por Id de la Orden(Order)
        [HttpGet]
        [Route("api/[controller]/{Id}")]
        public async Task<ActionResult<Order>> GetOrder(int Id)
        {
             //Include() incliye las relaciones de ambas entidades (User,Product)
            var order = await _context.Orders.Include(o => o.User).Include(o => o.Product).FirstOrDefaultAsync(o => o.Id == Id);

            if(order == null){
              return BadRequest("Orden no existente actualmente.");
            }
            return order;
        }
        //Eliminar por Id de la Orden(Order)
        [HttpDelete("api/Order/{Id}")]
        public async Task<ActionResult> DeleteOrder(int Id)
        {
            var order = await _context.Orders.FindAsync(Id);

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return Ok("Eliminado exitosaente.");
        }
        

        [HttpPost]
        [Route("api/Order")]
        public IActionResult CreateOrder([FromBody] OrderDto orderDto)
        {
            if (orderDto == null)
            {
                return BadRequest("El cuerpo de la solicitud no puede estar vacío.");
            }

            // Mapeo del DTO a la entidad Order
            var order = new Order
            {
                ProductId = orderDto.ProductId,
                UserId = orderDto.UserId
            };

            // Verificar si ProductId y UserId son válidos en la base de datos
            var product = _context.Products.SingleOrDefault(p => p.Id == orderDto.ProductId);
            var user = _context.Users.SingleOrDefault(u => u.Id == orderDto.UserId);

            if (product == null || user == null)
            {
                return NotFound("Producto o Usuario no encontrado.");
            }



            _context.Orders.Add(order);
            _context.SaveChanges();

            return Ok();
        }

        // PUT: api/Order/{id}
        // Este método actualiza una orden existente. Recibe el ID en la URL y un `OrderDto` en el cuerpo de la solicitud.
        [HttpPut]
        [Route("api/Order/{id}")]
        public IActionResult UpdateOrder(int id, [FromBody] OrderDto orderDto)
        {
            // Validación inicial del DTO recibido
            if (orderDto == null || id != orderDto.Id)
            {
                return BadRequest("Datos inválidos proporcionados.");
            }

            // Verifica si la orden con el ID proporcionado existe.
            var existingOrder = _context.Orders.Find(id);
            if (existingOrder == null)
            {
                // Retorna un mensaje claro si el ID no existe.
                return NotFound($"La orden con Id {id} no fue encontrada.");
            }

            // Actualiza las propiedades de la orden existente usando los valores del DTO.
            existingOrder.ProductId = orderDto.ProductId;
            existingOrder.UserId = orderDto.UserId;

            try
            {
                // Llama al contexto para actualizar la orden en la base de datos.
                _context.Orders.Update(existingOrder);
                
                // Guarda los cambios en la base de datos.
                _context.SaveChanges();

                // Retorna un mensaje de éxito con código 200 OK.
                return Ok("La orden fue actualizada exitosamente.");
            }
            catch (DbUpdateException ex)
            {
                // Si ocurre un error relacionado con la base de datos, captura la excepción y devuelve un mensaje específico.
                return StatusCode(500, $"Error al actualizar la orden en la base de datos: {ex.Message}");
            }
            catch (Exception ex)
            {
                // Si ocurre cualquier otro error, captura la excepción y devuelve un mensaje de error genérico.
                return StatusCode(500, $"Ocurrió un error inesperado: {ex.Message}");
            }
        }
    }
}