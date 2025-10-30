using gcam.Backend.Repositories.Interfaces;
using gcam.Backend.UnitsOfWork.Interfaces;
using gcam.Shared.Entities;
using gcam.Shared.Responses;

namespace gcam.Backend.UnitsOfWork.Implementations;

public class CountriesUnitOfWork : GenericUnitOfWork<Country>, ICountriesUnitOfWork
{
    private readonly ICountriesRepository _countriesRepository;

    public CountriesUnitOfWork(IGenericRepository<Country> repository, ICountriesRepository countriesRepository) : base(repository)
    {
        _countriesRepository = countriesRepository;
    }

    public override async Task<ActionResponse<IEnumerable<Country>>> GetAsync()
    {
        return await _countriesRepository.GetAsync();
    }

    public override async Task<ActionResponse<Country>> GetAsync(int id)
    {
        return await _countriesRepository.GetAsync(id);
    }
}