using gcam.Backend.Repositories.Interfaces;
using gcam.Backend.UnitsOfWork.Interfaces;
using gcam.Shared.DTOs;
using gcam.Shared.Entities;
using gcam.Shared.Responses;

namespace gcam.Backend.UnitsOfWork.Implementations;

public class CompanyContactsUnitOfWork : GenericUnitOfWork<CompanyContact>, ICompanyContactsUnitOfWork
{
    private readonly ICompanyContactsRepository _companyContacts;

    public CompanyContactsUnitOfWork(IGenericRepository<CompanyContact> repository, ICompanyContactsRepository companyContacts) : base(repository)
    {
        _companyContacts = companyContacts;
    }

    public override async Task<ActionResponse<IEnumerable<CompanyContact>>> GetAsync(PaginationDTO pagination) => await _companyContacts.GetAsync(pagination);

    public override async Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination) => await _companyContacts.GetTotalRecordsAsync(pagination);

    public override async Task<ActionResponse<IEnumerable<CompanyContact>>> GetAsync() => await _companyContacts.GetAsync();

    public override async Task<ActionResponse<CompanyContact>> GetAsync(int id) => await _companyContacts.GetAsync(id);

    public async Task<IEnumerable<CompanyContact>> GetComboAsync(int companyId) => await _companyContacts.GetComboAsync(companyId);
}