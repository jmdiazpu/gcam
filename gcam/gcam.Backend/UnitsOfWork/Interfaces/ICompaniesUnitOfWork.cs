using gcam.Shared.DTOs;
using gcam.Shared.Entities;
using gcam.Shared.Responses;

namespace gcam.Backend.UnitsOfWork.Interfaces;

public interface ICompaniesUnitOfWork
{
    Task<ActionResponse<IEnumerable<Company>>> GetAsync(PaginationDTO pagination);

    Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination);

    Task<ActionResponse<Company>> GetAsync(int id);

    Task<ActionResponse<IEnumerable<Company>>> GetAsync();
}