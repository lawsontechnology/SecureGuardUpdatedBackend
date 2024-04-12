

using System.Linq.Expressions;
using Visitor_Management_System.Core.Domain.Entities;

namespace Visitor_Management_System.Core.Application.Interface.Repositories
{
    public interface IToken : IBaseResponse<Token>
    {
        Task<Token> Get(string id);
        Task<Token> Get(Expression<Func<Token, bool>> predicate);
        public void RemoveToken(string token);
    }
}
