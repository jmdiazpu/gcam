using gcam.Shared.Entities;
using gcam.Shared.Responses;

namespace gcam.Backend.UnitsOfWork.Implementations;

public interface IStatesUnitOfWork
{
    Task<ActionResponse<State>> GetAsync(int id);

    Task<ActionResponse<IEnumerable<State>>> GetAsync();
}