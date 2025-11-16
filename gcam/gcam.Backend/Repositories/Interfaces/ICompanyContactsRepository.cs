using gcam.Shared.DTOs;
using gcam.Shared.Entities;
using gcam.Shared.Responses;

namespace gcam.Backend.Repositories.Interfaces;

public interface ICompanyContactsRepository
{
    Task<ActionResponse<IEnumerable<CompanyContact>>> GetAsync(PaginationDTO pagination);

    Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination);

    Task<ActionResponse<CompanyContact>> GetAsync(int id);

    Task<ActionResponse<IEnumerable<CompanyContact>>> GetAsync();

    Task<IEnumerable<CompanyContact>> GetComboAsync(int companyId);
}