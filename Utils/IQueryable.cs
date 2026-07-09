using Microsoft.EntityFrameworkCore;

namespace dotnet_starter.Utils
{
    public class PaginatedResponse<T>
    {
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int TotalRows { get; set; }
        public List<T> Data { get; set; } = new();
    }
    public static class IQueryableExtensions
    {


        public static async Task<PaginatedResponse<T>> ToPaginatedListAsync<T>(
            this IQueryable<T> source,
            int currentPage,
            int pageSize)
        {
            var count = await source.CountAsync();
            var totalPages = (int)Math.Ceiling(count / (double)pageSize);
            var items = await source.Skip((currentPage - 1) * pageSize).Take(pageSize).ToListAsync();

            return new PaginatedResponse<T>
            {
                CurrentPage = currentPage,
                PageSize = pageSize,
                TotalPages = totalPages,
                TotalRows = count,
                Data = items
            };
        }
    }

}