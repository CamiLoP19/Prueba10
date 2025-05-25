using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiTuEvento_.Models;
using Microsoft.AspNetCore.Authorization;
using Humanizer;

namespace ApiTuEvento_.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventoesController : ControllerBase
    {
        private readonly ContextDB _context;

        public EventoesController(ContextDB context)
        {
            _context = context;
        }

        // GET: api/Eventoes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Evento>>> Geteventos()
        {
            return await _context.eventos.ToListAsync();
        }

        // GET: api/Eventoes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Evento>> GetEvento(int id)
        {
            var evento = await _context.eventos.FindAsync(id);

            if (evento == null)
            {
                return NotFound();
            }

            return evento;
        }

        // PUT: api/Eventoes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEvento(int id, Evento evento)
        {
            if (id != evento.EventoId)
            {
                return BadRequest();
            }

            _context.Entry(evento).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EventoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Eventoes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<EventoDTO>> PostEvento(EventoDTO eventodto)
        {
            if (eventodto.Imagen != null)
            {
               
                var filePath = Path.Combine("wwwroot/imagenes", eventodto.Imagen.FileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await eventodto.Imagen.CopyToAsync(stream);
                }
            }


            var evento = new Evento
            {
                NombreEvento = eventodto.NombreEvento,
                FechaEvento = eventodto.FechaEvento,
                LugarEvento = eventodto.LugarEvento,
                Aforo = eventodto.Aforo,
                CategoriaEventoId = eventodto.CategoriaEventoId,
                DescripcionEvento = eventodto.DescripcionEvento,
                ImagenUrl = "/imagenes/" + eventodto.Imagen.FileName,
                EstadoEventoActivo = eventodto.EstadoEventoActivo
            };
            _context.eventos.Add(evento);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEvento", new { id = eventodto.EventoId }, eventodto);
        }

        // DELETE: api/Eventoes/5
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvento(int id)
        {
            var evento = await _context.eventos.FindAsync(id);
            if (evento == null)
            {
                return NotFound();
            }

            _context.eventos.Remove(evento);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EventoExists(int id)
        {
            return _context.eventos.Any(e => e.EventoId == id);
        }
    }
}
