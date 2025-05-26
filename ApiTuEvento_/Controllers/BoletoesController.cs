using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiTuEvento_.Models;

namespace ApiTuEvento_.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BoletoesController : ControllerBase
    {
        private readonly ContextDB _context;

        public BoletoesController(ContextDB context)
        {
            _context = context;
        }

        // GET: api/Boletoes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BoletoDTO>>> Getboletos()
        {
            var boletos = await _context.boletos
                .Select(b => new BoletoDTO
                {
                    BoletoId = b.BoletoId,
                    NombreComprador = b.NombreComprador,
                    TipoBoleto = b.TipoBoleto,
                    Descripcion = b.Descripcion,
                    EstadoVenta = b.EstadoVenta,
                    CodigoQR = b.CodigoQR,
                    CodigoAN = b.CodigoAN,
                    EventoId = b.EventoId,
                    PersonaId = b.PersonaId
                })
                .ToListAsync();

            return boletos;
        }

        // GET: api/Boletoes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BoletoDTO>> GetBoleto(int id)
        {
            var b = await _context.boletos.FindAsync(id);

            if (b == null)
                return NotFound();

            var dto = new BoletoDTO
            {
                BoletoId = b.BoletoId,
                NombreComprador = b.NombreComprador,
                TipoBoleto = b.TipoBoleto,
                Descripcion = b.Descripcion,
                EstadoVenta = b.EstadoVenta,
                CodigoQR = b.CodigoQR,
                CodigoAN = b.CodigoAN,
                EventoId = b.EventoId,
                PersonaId = b.PersonaId
            };

            return dto;
        }

        // PUT: api/Boletoes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBoleto(int id, BoletoDTO boletoDto)
        {
            if (id != boletoDto.BoletoId)
                return BadRequest();

            var boleto = await _context.boletos.FindAsync(id);
            if (boleto == null)
                return NotFound();

            // Actualizar los campos permitidos
            boleto.NombreComprador = boletoDto.NombreComprador;
            boleto.TipoBoleto = boletoDto.TipoBoleto;
            boleto.Descripcion = boletoDto.Descripcion;
            boleto.EstadoVenta = boletoDto.EstadoVenta;
            boleto.CodigoQR = boletoDto.CodigoQR;
            boleto.CodigoAN = boletoDto.CodigoAN;
            boleto.EventoId = boletoDto.EventoId;
            boleto.PersonaId = boletoDto.PersonaId;

            _context.Entry(boleto).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BoletoExists(id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        // POST: api/Boletoes
        [HttpPost]
        public async Task<ActionResult<BoletoDTO>> PostBoleto([FromBody] BoletoDTO boletodto)
        {
            try
            {
                // Generar Código Alfanumérico irrepetible
                string codigoAN;
                do
                {
                    codigoAN = Guid.NewGuid().ToString("N").Substring(0, 10).ToUpper();
                }
                while (_context.boletos.Any(b => b.CodigoAN == codigoAN));

                // Simular un código QR (en una app real puedes usar QRCoder para una imagen)
                string codigoQR = $"QR-{codigoAN}";

                var boleto = new Boleto()
                {
                    NombreComprador = boletodto.NombreComprador,
                    TipoBoleto = boletodto.TipoBoleto,
                    Descripcion = boletodto.Descripcion,
                    EstadoVenta = boletodto.EstadoVenta,
                    CodigoQR = codigoQR,
                    CodigoAN = codigoAN,
                    EventoId = boletodto.EventoId,
                    PersonaId = boletodto.PersonaId
                };

                _context.boletos.Add(boleto);
                await _context.SaveChangesAsync();

                // Devolver el DTO con los códigos generados
                var resultDto = new BoletoDTO
                {
                    BoletoId = boleto.BoletoId,
                    NombreComprador = boleto.NombreComprador,
                    TipoBoleto = boleto.TipoBoleto,
                    Descripcion = boleto.Descripcion,
                    EstadoVenta = boleto.EstadoVenta,
                    CodigoQR = boleto.CodigoQR,
                    CodigoAN = boleto.CodigoAN,
                    EventoId = boleto.EventoId,
                    PersonaId = boleto.PersonaId
                };

                return CreatedAtAction(nameof(GetBoleto), new { id = boleto.BoletoId }, resultDto);
            }
            catch(Exception ex)
            {
                return StatusCode(500, $"Error al guardar el boleto: {ex.Message}");


            }
        }
        // DELETE: api/Boletoes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBoleto(int id)
        {
            var boleto = await _context.boletos.FindAsync(id);
            if (boleto == null)
                return NotFound();

            _context.boletos.Remove(boleto);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BoletoExists(int id)
        {
            return _context.boletos.Any(e => e.BoletoId == id);
        }
    }
}