using gcam.Backend.Repositories.Interfaces;
using gcam.Backend.UnitsOfWork.Interfaces;
using gcam.Shared.DTOs;
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

    public override async Task<ActionResponse<IEnumerable<State>>> GetAsync(PaginationDTO pagination) => await _statesRepository.GetAsync(pagination);

    public override async Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination) => await _statesRepository.GetTotalRecordsAsync(pagination);

    public override async Task<ActionResponse<IEnumerable<State>>> GetAsync() => await _statesRepository.GetAsync();

    public override async Task<ActionResponse<State>> GetAsync(int id) => await _statesRepository.GetAsync(id);

    public async Task<IEnumerable<State>> GetComboAsync(int id) => await _statesRepository.GetComboAsync(id);
}