using gcam.Shared.DTOs;
using gcam.Shared.Entities;
using gcam.Shared.Responses;

namespace gcam.Backend.UnitsOfWork.Interfaces;

public interface ICompanyContactsUnitOfWork
{
    Task<ActionResponse<IEnumerable<CompanyContact>>> GetAsync(PaginationDTO pagination);

    Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination);

    Task<ActionResponse<CompanyContact>> GetAsync(int id);

    Task<ActionResponse<IEnumerable<CompanyContact>>> GetAsync();
}