using gcam.Shared.DTOs;
using gcam.Shared.Entities;
using gcam.Shared.Responses;

namespace gcam.Backend.UnitsOfWork.Interfaces;

public interface IStatesUnitOfWork
{
    Task<ActionResponse<IEnumerable<State>>> GetAsync(PaginationDTO pagination);

    Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination);

    Task<ActionResponse<State>> GetAsync(int id);

    Task<ActionResponse<IEnumerable<State>>> GetAsync();

    Task<IEnumerable<State>> GetComboAsync(int id);
}