using gcam.Backend.Repositories.Interfaces;
using gcam.Shared.Entities;
using gcam.Shared.Responses;

namespace gcam.Backend.UnitsOfWork.Implementations;

public class StatesUnitOfWork : GenericUnitOfWork<State>, IStatesUnitOfWork
{
    private readonly IStatesRepository _statesRepository;

    public StatesUnitOfWork(IGenericRepository<State> repository, IStatesRepository statesRepository) : base(repository)
    {
        _statesRepository = statesRepository;
    }

    public override async Task<ActionResponse<IEnumerable<State>>> GetAsync()
    {
        return await _statesRepository.GetAsync();
    }

    public override async Task<ActionResponse<State>> GetAsync(int id)
    {
        return await _statesRepository.GetAsync(id);
    }
}