
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Autenticacion.Data;
using Autenticacion.Models;

namespace Autenticacion.Controllers;

[ApiController]
[Route("api/persona")]
public class PersonaController : ControllerBase
{
    private readonly AppDbContext _context;

    public PersonaController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/persona
    [HttpGet]

    public async Task<ActionResult<IEnumerable<Persona>>> GetPersona()
    {
        var token = HttpContext.Session.GetString("GitHubToken");

        if (token == null)
            return Unauthorized("No hay token guardado");

        return await _context.Personas.ToListAsync();
    }

    // GET: api/persona/1
    [HttpGet("{id}")]
    public async Task<ActionResult<Persona>> ObtenerPorId(int id)
    {
        var persona = await _context.Personas.FindAsync(id);

        if (persona == null)
            return NotFound();

        return persona;
    }

    // POST: api/persona
    [HttpPost]
    public async Task<ActionResult<Persona>> Crear(Persona persona)
    {
        _context.Personas.Add(persona);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(ObtenerPorId),
            new { id = persona.Id },
            persona
        );
    }

    // PUT: api/persona/1
    [HttpPut("{id}")]
    public async Task<IActionResult> Actualizar(int id, Persona persona)
    {
        if (id != persona.Id)
            return BadRequest();

        var existe = await _context.Personas.AnyAsync(p => p.Id == id);

        if (!existe)
            return NotFound();

        _context.Entry(persona).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/persona/1
    [HttpDelete("{id}")]
    public async Task<IActionResult> Eliminar(int id)
    {
        var persona = await _context.Personas.FindAsync(id);

        if (persona == null)
            return NotFound();

        _context.Personas.Remove(persona);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
