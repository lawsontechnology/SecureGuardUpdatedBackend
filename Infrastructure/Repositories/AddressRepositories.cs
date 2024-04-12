using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Visitor_Management_System.Core.Application.Interface.Repositories;
using Visitor_Management_System.Core.Domain.Entities;
using Visitor_Management_System.Infrastructure.Context;

namespace Visitor_Management_System.Infrastructure.Repositories
{
    public class AddressRepositories : BaseRepositories<Address>, IAddressRepo
    {
        public AddressRepositories(VisitorManagementSystem context)
        {
            _context = context;
        }
        public async Task<Address> Get(string id)
        {
            var address = await _context.Addresses
             .Include(x => x.Profile)
             .SingleOrDefaultAsync(a => a.Id == id && a.IsDeleted == false);
            return address;
        }

        public async Task<Address> Get(Expression<Func<Address, bool>> predicate)
        {
            var address = await _context.Addresses
            .Include(x => x.Profile)
            .Where(x => x.IsDeleted == false)
            .SingleOrDefaultAsync(predicate);
            return address;
        }

        public async Task<ICollection<Address>> GetAll()
        {
            return await _context.Addresses
            .Include(x => x.Profile)
            .Where(a => a.IsDeleted == false)
            .ToListAsync();
        }
    }
}
