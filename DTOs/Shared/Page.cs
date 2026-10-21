using System.Text.Json.Serialization;

namespace ticketManage.DTOs.Shared
{
    public class Page<T>
    {
        public IReadOnlyList<T> Content { get; init; } = [];
        public int PageNumber { get; init; }
        public int PageSize { get; init; }

        [JsonIgnore]
        public long TotalElements { get; init; }
        public int TotalPages => (int)Math.Ceiling((double)TotalElements / PageSize);

        public static Page<T> Create(
            IReadOnlyList<T> content,
            int pageNumber,
            int pageSize,
            long totalElements
        )
        {
            return new Page<T>
            {
                Content = content,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalElements = totalElements,
            };
        }
    }
}
