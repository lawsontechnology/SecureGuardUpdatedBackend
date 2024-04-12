using Microsoft.EntityFrameworkCore;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Visitor_Management_System.Core.Application.Interface.Repositories;
using Visitor_Management_System.Core.Domain.Entities;
using Visitor_Management_System.Infrastructure.Context;

namespace Visitor_Management_System.Infrastructure.Repositories
{
    public class TokenRepositories : BaseRepositories<Token>, IToken
    {
        public TokenRepositories(VisitorManagementSystem context)
        {
            _context = context;
        }

        public Task<Token> Get(string id)
        {
            return _context.Tokens.SingleOrDefaultAsync(x => x.Id == id);
        }

        public Task<Token> Get(Expression<Func<Token, bool>> predicate)
        {
            return _context.Tokens.SingleOrDefaultAsync(predicate);
        }

        public void RemoveToken(string token)
        {
            var tokenToRemove = _context.Tokens.SingleOrDefault(x => x.Tokens == token);

            if (tokenToRemove != null)
            {
                _context.Tokens.Remove(tokenToRemove);
            }
        }
    }
}
