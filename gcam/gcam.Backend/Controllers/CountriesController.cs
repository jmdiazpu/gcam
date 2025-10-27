using gcam.Backend.Data;
using gcam.Shared.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace gcam.Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CountriesController : ControllerBase
{
    private readonly DataContext _context;

    public CountriesController(DataContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAsync()
    {
        return Ok(await _context.Countries.ToListAsync());
    }

    [HttpPost]
    public async Task<IActionResult> PostAsync(Country country)
    {
        _context.Countries.Add(country);
        await _context.SaveChangesAsync();
        return Ok(country);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> PutAsync(int id, Country country)
    {
        if (id != country.Id)
        {
            return BadRequest("El ID del país no coincide con el ID de la URL.");
        }
        _context.Entry(country).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return Ok(country);
    }
}