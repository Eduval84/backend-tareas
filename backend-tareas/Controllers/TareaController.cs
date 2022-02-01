using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using backend_tareas.Context;
using backend_tareas.Models;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace backend_tareas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TareaController : ControllerBase
    {
        //TODO: Revisar patron repository e implementar los interfaces y las capas necesarias 
        private readonly AplicationDbContext _context;

        public TareaController(AplicationDbContext context)
        {
            this._context = context;

        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var listTareas = await _context.Tareas.ToListAsync();
                return Ok(listTareas);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Tarea tarea)
        {
            try
            {
                _context.Add(tarea);
                await _context.SaveChangesAsync();
                return Ok("Tarea creada");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete ("{id}") ]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var tarea = await _context.Tareas.FindAsync(id);
                if (tarea == null)
                {
                    return NotFound();
                }
                _context.Remove(tarea);
                await _context.SaveChangesAsync();
                return Ok("Tarea eliminada");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
       
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id,[FromBody] Tarea tarea)
        {
            try
            {
                if (id != tarea.Id)
                {
                    return BadRequest();
                }

                tarea.Estado = !tarea.Estado;
                _context.Entry(tarea).State= EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok("Tarea actualizada");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }

    }
}
