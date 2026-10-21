using Microsoft.EntityFrameworkCore;
using ticketManage.DTOs.Shared;

namespace ticketManage.Utils
{
    public class PaginationUtils
    {
        public static IQueryable<T> ApplyPagination<T>(
            IQueryable<T> query,
            int pageNumber,
            int pageSize
        )
        {
            return query.Skip((pageNumber - 1) * pageSize).Take(pageSize);
        }

        public static async Task<Page<T>> GetPaged<T>(
            IQueryable<T> filteredQuery,
            int pageNumber,
            int pageSize
        )
        {
            var totalElements = await filteredQuery.CountAsync();
            var content = await ApplyPagination(filteredQuery, pageNumber, pageSize).ToListAsync();
            return Page<T>.Create(content, pageNumber, pageSize, totalElements);
        }

        public static async Task<Page<T>> GetPaged<T>(
            IQueryable<T> filteredQuery,
            PageRequest request
        )
        {
            return await GetPaged(filteredQuery, request.PageNumber, request.PageSize);
        }
    }
}
