using gcam.Shared.DTOs;
using gcam.Shared.Entities;
using gcam.Shared.Responses;

namespace gcam.Backend.Repositories.Interfaces;

public interface ICountriesRepository
{
    Task<IEnumerable<Country>> GetComboAsync();

    Task<ActionResponse<IEnumerable<Country>>> GetAsync(PaginationDTO pagination);

    Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination);

    Task<ActionResponse<Country>> GetAsync(int id);

    Task<ActionResponse<IEnumerable<Country>>> GetAsync();
}