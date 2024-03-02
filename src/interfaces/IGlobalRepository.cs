using System.Linq.Expressions;
using System.Security.Claims;
using dto;

namespace interfaces;
public interface IGlobalRepository<T>
{
    Task<IEnumerable<T>> GetAllByUserId();
    Task<T?> Get(Expression<Func<T, bool>> predicate);
    Task<T> AddAsync(T entity);
    T Update(T entity);
    T Remove(T entity);
    IEnumerable<T> RemoveRange(IEnumerable<T> entity);
    string GetUserId(ClaimsPrincipal userIdClaims);
    PaginationResDTO<T> PaginationRes(int totalList, PageListDTO dtoQuery);

}