using System.Linq.Expressions;
using System.Security.Claims;
using dataContext;
using dto;
using interfaces;
using Microsoft.EntityFrameworkCore;

namespace repositories;
public interface IEntityId
{
    int entityId { get; set; }
}

public class GlobalRepository<T> : IGlobalRepository<T> where T : class
{
    protected readonly DataContext _context;

    public GlobalRepository(DataContext context)
    {
        _context = context;
    }
    public string GetUserId(ClaimsPrincipal userIdClaims)
    {
        var userId = userIdClaims.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
        {
            throw new Exception("Acesso negado!");
        }

        return userId;
    }

    public async Task<IEnumerable<T>> GetAllByUserId()
    {
        return await _context.Set<T>().AsNoTracking().ToListAsync();
    }

    public async Task<T?> Get(Expression<Func<T, bool>> predicate)
    {
        return await _context.Set<T>().FirstOrDefaultAsync();
    }

    public async Task<T> AddAsync(T entity)
    {
        await _context.Set<T>().AddAsync(entity);
        return entity;
    }
    public T Update(T entity)
    {
        _context.Set<T>().Update(entity);
        return entity;
    }

    public T Remove(T entity)
    {
        _context.Set<T>().Remove(entity);
        return entity;
    }

    public IEnumerable<T> RemoveRange(IEnumerable<T> entity)
    {
        _context.Set<T>().RemoveRange(entity);
        return entity;
    }

    public PaginationResDTO<T> PaginationRes(int totalList, PageListDTO dtoQuery)
    {
        int totalPages = (int)Math.Ceiling((double)totalList / dtoQuery.PageSize);
        bool hasNextPage = dtoQuery.PageNumber < totalPages;

        PaginationResDTO<T> paginationRes = new()
        {
            PageNumber = dtoQuery.PageNumber,
            PageSize = dtoQuery.PageSize,
            TotalPages = totalPages,
            HasNextPage = hasNextPage,

        };
        return paginationRes;
    }


}