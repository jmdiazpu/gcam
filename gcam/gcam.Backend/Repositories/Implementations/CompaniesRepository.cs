using gcam.Backend.Data;
using gcam.Backend.Helpers;
using gcam.Backend.Repositories.Interfaces;
using gcam.Shared.DTOs;
using gcam.Shared.Entities;
using gcam.Shared.Responses;
using Microsoft.EntityFrameworkCore;

namespace gcam.Backend.Repositories.Implementations;

public class CompaniesRepository : GenericRepository<Company>, ICompaniesRepository
{
    private readonly DataContext _context;

    public CompaniesRepository(DataContext context) : base(context)
    {
        _context = context;
    }

    public override async Task<ActionResponse<IEnumerable<Company>>> GetAsync(PaginationDTO pagination)
    {
        var queryable = _context.Companies
            .Include(c => c.City)
            .Include(x => x.CompanyContacts)
            .AsQueryable();

        if (!string.IsNullOrEmpty(pagination.Filter))
        {
            queryable = queryable.Where(x => x.Name.ToLower().Contains(pagination.Filter.ToLower()));
        }

        return new ActionResponse<IEnumerable<Company>>
        {
            WasSuccess = true,
            Result = await queryable
            .OrderBy(x => x.Name)
            .Paginate(pagination)
            .ToListAsync()
        };
    }

    public override async Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination)
    {
        var queryable = _context.Companies.AsQueryable();
        if (!string.IsNullOrEmpty(pagination.Filter))
        {
            queryable = queryable.Where(x => x.Name.ToLower().Contains(pagination.Filter.ToLower()));
        }

        double count = await queryable.CountAsync();
        return new ActionResponse<int>
        {
            WasSuccess = true,
            Result = (int)count
        };
    }

    public override async Task<ActionResponse<IEnumerable<Company>>> GetAsync()
    {
        var companies = await _context.Companies
            .Include(c => c.City)
            .Include(x => x.CompanyContacts)
            .OrderBy(x => x.Name)
            .ToListAsync();

        return new ActionResponse<IEnumerable<Company>>
        {
            WasSuccess = true,
            Result = companies
        };
    }

    public override async Task<ActionResponse<Company>> GetAsync(int id)
    {
        var company = await _context.Companies
            .Include(c => c.City)
            .ThenInclude(city => city!.State)
            .ThenInclude(state => state!.Country)
            .Include(x => x.CompanyContacts)
            .FirstOrDefaultAsync(x => x.Id == id);
        if (company == null)
        {
            return new ActionResponse<Company>
            {
                Message = "Empresa no existe."
            };
        }
        return new ActionResponse<Company>
        {
            WasSuccess = true,
            Result = company
        };
    }

    public async Task<IEnumerable<Company>> GetComboAsync()
    {
        return await _context.Companies
            .OrderBy(x => x.Name)
            .ToListAsync();
    }
}