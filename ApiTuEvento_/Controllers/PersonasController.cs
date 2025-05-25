using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiTuEvento_.Models;
using ApiTuEvento_.Helpers;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using Microsoft.AspNetCore.Authorization;


namespace ApiTuEvento_.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonasController : ControllerBase
    {
        private readonly ContextDB _context;
        private readonly JwtHelper _jwtHelper;

        public PersonasController(ContextDB context, JwtHelper jwtHelper)
        {
            _jwtHelper = jwtHelper;
            _context = context;

        }

        // GET: api/Personas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Persona>>> Getpersonas()
        {
            return await _context.personas.ToListAsync();
        }

        // GET: api/Personas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Persona>> GetPersona(int id)
        {
            var persona = await _context.personas.FindAsync(id);

            if (persona == null)
            {
                return NotFound();
            }

            return persona;
        }

        // PUT: api/Personas/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = "Administrador")]
        [HttpPut("{id}")]
    public async Task<IActionResult> PutPersona(int id, Persona persona)
    {
        if (id != persona.PersonaId)
        {
            return BadRequest("Persona no encontrada");
        }

        var personaExistente = await _context.personas.FindAsync(id);
        if (personaExistente == null)
        {
            return NotFound();
        }

        // Actualizar campos excepto contraseña directamente
        personaExistente.NombreUsuario = persona.NombreUsuario;
        personaExistente.Correo = persona.Correo;
        personaExistente.FechaNacimiento = persona.FechaNacimiento;
        personaExistente.Rol = persona.Rol;

        // Verificar si la contraseña cambió (comparar con la encriptada)
        if (!BCrypt.Net.BCrypt.Verify(persona.Contraseña, personaExistente.Contraseña))
        {
            personaExistente.Contraseña = BCrypt.Net.BCrypt.HashPassword(persona.Contraseña);
        }

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!PersonaExists(id))
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

        // POST: api/Personas
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754

        [HttpPost] //login
        public async Task<IActionResult> PostPersona(LoginDTO logindto)
        {
            var login = await _context.personas
                .FirstOrDefaultAsync(l => l.NombreUsuario == logindto.NombreUsuario);
            if (login == null || !BCrypt.Net.BCrypt.Verify(logindto.Contraseña, login.Contraseña))
            {
                return Unauthorized("Credenciales inválidas");
            }

            var token = _jwtHelper.GenerateToken(login.NombreUsuario, login.Rol);

            return Ok(new
            {
                token,
                user = new { login.NombreUsuario, login.Correo, login.Rol }
            });
        }

        [HttpPost ("Register")]
        public async Task<IActionResult> Register(RegisterDTO registerdto)
        {
            if (_context.personas.Any(u => u.NombreUsuario == registerdto.NombreUsuario))
                return BadRequest("El usuario ya existe.");

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(registerdto.Contraseña);

            var persona = new Persona
            {
                NombreUsuario = registerdto.NombreUsuario,
                Correo = registerdto.Correo,
                FechaNacimiento = registerdto.FechaNacimiento,
                Rol = registerdto.Rol,
                Contraseña = hashedPassword
            };

            _context.personas.Add(persona);
            await _context.SaveChangesAsync();

            return Ok("Usuario registrado correctamente.");
        }
        // DELETE: api/Personas/5
        [Authorize (Roles = "Administrador")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePersona(int id)
        {
            var persona = await _context.personas.FindAsync(id);
            if (persona == null)
            {
                return NotFound();
            }

            _context.personas.Remove(persona);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PersonaExists(int id)
        {
            return _context.personas.Any(e => e.PersonaId == id);
        }
    }
}
