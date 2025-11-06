using gcam.Shared.Entities;

namespace gcam.Backend.Data;

public class SeedDb
{
    private readonly DataContext _context;

    public SeedDb(DataContext context)
    {
        _context = context;
    }

    public async Task SeedDbAsync()
    {
        await _context.Database.EnsureCreatedAsync();
        await CheckCountriesAsync();
        await CheckStatesAsync();
        await CheckCitiesAsync();
    }

    private async Task CheckCitiesAsync()
    {
        if (!_context.Cities.Any())
        {
            _context.Cities.Add(new City { Name = "Acacías", StateId = 1 });
            await _context.SaveChangesAsync();
        }
    }

    private async Task CheckStatesAsync()
    {
        if (!_context.States.Any())
        {
            _context.States.Add(new State { Name = "Meta", CountryId = 1 });
            await _context.SaveChangesAsync();
        }
    }

    private async Task CheckCountriesAsync()
    {
        if (!_context.Countries.Any())
        {
            _context.Countries.Add(new Country { Name = "Colombia" });
            await _context.SaveChangesAsync();
        }
    }
}