using gcam.Backend.Repositories.Interfaces;
using gcam.Backend.UnitsOfWork.Interfaces;
using gcam.Shared.Responses;

namespace gcam.Backend.UnitsOfWork.Implementations;

public class GenericUnitOfWork<T> : IGenericUnitOfWork<T> where T : class
{
    private readonly IGenericRepository<T> _countriesRepository;

    public GenericUnitOfWork(IGenericRepository<T> repository)
    {
        _countriesRepository = repository;
    }

    public virtual async Task<ActionResponse<T>> AddAsync(T entity) => await _countriesRepository.AddAsync(entity);

    public virtual async Task<ActionResponse<T>> DeleteAsync(int id) => await _countriesRepository.DeleteAsync(id);

    public virtual async Task<ActionResponse<T>> GetAsync(int id) => await _countriesRepository.GetAsync(id);

    public virtual async Task<ActionResponse<IEnumerable<T>>> GetAsync() => await _countriesRepository.GetAsync();

    public virtual async Task<ActionResponse<T>> UpdateAsync(T entity) => await _countriesRepository.UpdateAsync(entity);
}