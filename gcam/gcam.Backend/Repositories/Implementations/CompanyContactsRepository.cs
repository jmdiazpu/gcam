using gcam.Backend.Data;
using gcam.Backend.Helpers;
using gcam.Backend.Repositories.Interfaces;
using gcam.Shared.DTOs;
using gcam.Shared.Entities;
using gcam.Shared.Responses;
using Microsoft.EntityFrameworkCore;

namespace gcam.Backend.Repositories.Implementations
{
    public class CompanyContactsRepository : GenericRepository<CompanyContact>, ICompanyContactsRepository

    {
        private readonly DataContext _context;

        public CompanyContactsRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public override async Task<ActionResponse<IEnumerable<CompanyContact>>> GetAsync(PaginationDTO pagination)
        {
            var queryable = _context.CompanyContacts
                .Where(x => x.Company!.Id == pagination.Id)
                .AsQueryable();

            if (!string.IsNullOrEmpty(pagination.Filter))
            {
                queryable = queryable.Where(x => x.FullName.ToLower().Contains(pagination.Filter.ToLower()));
            }

            return new ActionResponse<IEnumerable<CompanyContact>>
            {
                WasSuccess = true,
                Result = await queryable
                .OrderBy(x => x.FullName)
                .Paginate(pagination)
                .ToListAsync()
            };
        }

        public override async Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination)
        {
            var queryable = _context.CompanyContacts
                .Where(x => x.Company!.Id == pagination.Id)
                .AsQueryable();

            if (!string.IsNullOrEmpty(pagination.Filter))
            {
                queryable = queryable.Where(x => x.FullName.ToLower().Contains(pagination.Filter.ToLower()));
            }

            double count = await queryable.CountAsync();
            return new ActionResponse<int>
            {
                WasSuccess = true,
                Result = (int)count
            };
        }

        public override async Task<ActionResponse<IEnumerable<CompanyContact>>> GetAsync()
        {
            var companyContacts = await _context.CompanyContacts
                .ToListAsync();
            return new ActionResponse<IEnumerable<CompanyContact>>
            {
                WasSuccess = true,
                Result = companyContacts
            };
        }

        public override async Task<ActionResponse<CompanyContact>> GetAsync(int id)
        {
            var companyContact = await _context.CompanyContacts
                .FirstOrDefaultAsync(x => x.Id == id);
            if (companyContact == null)
            {
                return new ActionResponse<CompanyContact>
                {
                    Message = "Estado no existe."
                };
            }
            return new ActionResponse<CompanyContact>
            {
                WasSuccess = true,
                Result = companyContact
            };
        }

        public async Task<IEnumerable<CompanyContact>> GetComboAsync(int companyId)
        {
            return await _context.CompanyContacts
                .Where(x => x.CompanyId == companyId)
                .OrderBy(x => x.FullName)
                .ToListAsync();
        }
    }
}