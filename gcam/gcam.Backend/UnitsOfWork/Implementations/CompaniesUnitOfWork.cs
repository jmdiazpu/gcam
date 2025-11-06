using gcam.Backend.Repositories.Interfaces;
using gcam.Backend.UnitsOfWork.Interfaces;
using gcam.Shared.DTOs;
using gcam.Shared.Entities;
using gcam.Shared.Responses;

namespace gcam.Backend.UnitsOfWork.Implementations;

public class CompaniesUnitOfWork : GenericUnitOfWork<Company>, ICompaniesUnitOfWork
{
    private readonly ICompaniesRepository _CompaniesRepository;

    public CompaniesUnitOfWork(IGenericRepository<Company> repository, ICompaniesRepository CompaniesRepository) : base(repository)
    {
        _CompaniesRepository = CompaniesRepository;
    }

    public override async Task<ActionResponse<IEnumerable<Company>>> GetAsync(PaginationDTO pagination) => await _CompaniesRepository.GetAsync(pagination);

    public override async Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination) => await _CompaniesRepository.GetTotalRecordsAsync(pagination);

    public override async Task<ActionResponse<IEnumerable<Company>>> GetAsync() => await _CompaniesRepository.GetAsync();

    public override async Task<ActionResponse<Company>> GetAsync(int id) => await _CompaniesRepository.GetAsync(id);
}