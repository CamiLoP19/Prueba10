﻿using System;
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
        public async Task<ActionResult<IEnumerable<Boleto>>> Getboletos()
        {
            return await _context.boletos.ToListAsync();
        }

        // GET: api/Boletoes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Boleto>> GetBoleto(int id)
        {
            var boleto = await _context.boletos.FindAsync(id);

            if (boleto == null)
            {
                return NotFound();
            }

            return boleto;
        }

        // PUT: api/Boletoes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBoleto(int id, Boleto boleto)
        {
            if (id != boleto.BoletoId)
            {
                return BadRequest();
            }

            _context.Entry(boleto).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BoletoExists(id))
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
        // POST: api/Boletoes
        [HttpPost]
        public async Task<ActionResult<BoletoDTO>> PostBoleto(BoletoDTO boletodto)
        {
            // Validación explícita del enum (opcional, porque ya está tipado)
            if (!Enum.IsDefined(typeof(TipoBoletoEnum), boletodto.TipoBoleto))
            {
                return BadRequest("Tipo de boleto no válido. Opciones válidas: VIP, Preferencial, Comun.");
            }

            var boleto = new Boleto()
            {
                NombreComprador = boletodto.NombreComprador,
                TipoBoleto = boletodto.TipoBoleto,
                Descripcion = boletodto.Descripcion,
                Precio = boletodto.Precio,
                EstadoVenta = boletodto.EstadoVenta,
                CodigoQR = boletodto.CodigoQR,
                CodigoAN = boletodto.CodigoAN,
                EventoId = boletodto.EventoId,
                PersonaId = boletodto.PersonaId
            };

            _context.boletos.Add(boleto);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBoleto", new { id = boleto.BoletoId }, boletodto);
        }


        // DELETE: api/Boletoes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBoleto(int id)
        {
            var boleto = await _context.boletos.FindAsync(id);
            if (boleto == null)
            {
                return NotFound();
            }

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
