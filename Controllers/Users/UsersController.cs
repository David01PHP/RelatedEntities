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
    public class UsersController : ControllerBase
    {
        private readonly DataContext _context;
        // Constructor para inyección de dependencias
        public UsersController(DataContext context){

            _context = context;
        }

        [HttpGet]
        [Route("api/[controller]")]
        public IEnumerable<User> Users()
        {
            return _context.Users.ToList();
        }
        //Obtener por Id de la Usuario(User)
        [HttpGet]
        [Route("api/[controller]/{Id}")]
        public async Task<ActionResult<User>> GetUser(int Id)
        {
             //Include() incliye las relaciones de ambas entidades (User,User)
            var User = await _context.Users.FirstOrDefaultAsync(o => o.Id == Id);

            if(User == null){
              return BadRequest("Usuario no existente actualmente.");
            }
            return User;
        }

        //Eliminar por Id de la Usero(User)
        [HttpDelete]
        [Route("api/User/{Id}")]
        public async Task<ActionResult> DeleteUser(int Id)
        {
            var User = await _context.Users.FindAsync(Id);

            if (User == null)
            {
                return NotFound("Usero no encontrado.");
            }

            // Verifica si el Usero está referenciado en alguna orden
            bool hasOrders = _context.Orders.Any(o => o.UserId == Id);

            if (hasOrders)
            {
                return BadRequest("No se puede eliminar el Usero porque está asociado con una o más órdenes.");
            }

            _context.Users.Remove(User);
            await _context.SaveChangesAsync();

            return Ok("Usero eliminado exitosamente.");
        }

        //agregar
        [HttpPost]
        [Route("api/User")]
        public IActionResult CreateUser([FromBody] UserDto UserDto)
        {
            if (UserDto == null)
            {
                return BadRequest("El cuerpo de la solicitud no puede estar vacío.");
            }

            // Mapeo del DTO a la entidad User
            var User = new User
            {
                Names = UserDto.Names,
                LastNames = UserDto.LastNames,
                Email = UserDto.Email
            };


            _context.Users.Add(User);
            _context.SaveChanges();

            return Ok();
        }

        // PUT: api/User/{id}
// Este método actualiza un usuario existente. Recibe el ID en la URL y un `UserDto` en el cuerpo de la solicitud.
[HttpPut]
[Route("api/User/{id}")]
public IActionResult UpdateUser(int id, [FromBody] UserDto userDto)
{
    // Validación inicial del DTO recibido
    if (userDto == null || id != userDto.Id)
    {
        return BadRequest("Datos inválidos proporcionados.");
    }

    // Verifica si el usuario con el ID proporcionado existe.
    var existingUser = _context.Users.Find(id);
    if (existingUser == null)
    {
        // Retorna un mensaje claro si el ID no existe.
        return NotFound($"El usuario con Id {id} no fue encontrado.");
    }

    // Actualiza las propiedades del usuario existente usando los valores del DTO.
    existingUser.Names = userDto.Names;
    existingUser.LastNames = userDto.LastNames;
    existingUser.Email = userDto.Email;

    try
    {
        // Llama al contexto para actualizar el usuario en la base de datos.
        _context.Users.Update(existingUser);
        
        // Guarda los cambios en la base de datos.
        _context.SaveChanges();

        // Retorna un mensaje de éxito con código 200 OK.
        return Ok("El usuario fue actualizado exitosamente.");
    }
    catch (DbUpdateException ex)
    {
        // Si ocurre un error relacionado con la base de datos, captura la excepción y devuelve un mensaje específico.
        return StatusCode(500, $"Error al actualizar el usuario en la base de datos: {ex.Message}");
    }
    catch (Exception ex)
    {
        // Si ocurre cualquier otro error, captura la excepción y devuelve un mensaje de error genérico.
        return StatusCode(500, $"Ocurrió un error inesperado: {ex.Message}");
    }
}

        
    }
}