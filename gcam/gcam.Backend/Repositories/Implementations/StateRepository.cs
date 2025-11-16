using gcam.Backend.Data;
using gcam.Backend.Helpers;
using gcam.Backend.Repositories.Interfaces;
using gcam.Shared.DTOs;
using gcam.Shared.Entities;
using gcam.Shared.Responses;
using Microsoft.EntityFrameworkCore;

namespace gcam.Backend.Repositories.Implementations;

public class StateRepository : GenericRepository<State>, IStatesRepository

{
    private readonly DataContext _context;

    public StateRepository(DataContext context) : base(context)
    {
        _context = context;
    }

    public override async Task<ActionResponse<IEnumerable<State>>> GetAsync(PaginationDTO pagination)
    {
        var queryable = _context.States
            .Include(x => x.Cities)
            .Where(x => x.Country!.Id == pagination.Id)
            .AsQueryable();

        if (!string.IsNullOrEmpty(pagination.Filter))
        {
            queryable = queryable.Where(x => x.Name.ToLower().Contains(pagination.Filter.ToLower()));
        }

        return new ActionResponse<IEnumerable<State>>
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
        var queryable = _context.States
            .Where(x => x.Country!.Id == pagination.Id)
            .AsQueryable();

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

    public override async Task<ActionResponse<IEnumerable<State>>> GetAsync()
    {
        var states = await _context.States
            .Include(x => x.Cities)
            .ToListAsync();
        return new ActionResponse<IEnumerable<State>>
        {
            WasSuccess = true,
            Result = states
        };
    }

    public override async Task<ActionResponse<State>> GetAsync(int id)
    {
        var state = await _context.States
            .Include(x => x.Cities)
            .FirstOrDefaultAsync(x => x.Id == id);
        if (state == null)
        {
            return new ActionResponse<State>
            {
                Message = "Estado no existe."
            };
        }
        return new ActionResponse<State>
        {
            WasSuccess = true,
            Result = state
        };
    }

    public async Task<IEnumerable<State>> GetComboAsync(int countryId)
    {
        return await _context.States
            .Where(s => s.CountryId == countryId)
            .OrderBy(s => s.Name)
            .ToListAsync();
    }
}