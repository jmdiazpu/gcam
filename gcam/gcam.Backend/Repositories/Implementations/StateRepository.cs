using gcam.Backend.Data;
using gcam.Backend.Repositories.Interfaces;
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
}