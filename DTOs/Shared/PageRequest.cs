using ticketManage.Utils;

namespace ticketManage.DTOs.Shared
{
    public abstract class PageRequest
    {
        public int PageNumber { get; set; } = Constants.DEFAULT_START_PAGE_NUMBER;

        private int _pageSize = Constants.DEFAULT_PAGE_SIZE;

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = Math.Clamp(value, 1, Constants.DEFAULT_MAX_PAGE_SIZE);
        }
        public Sort[]? Sort { get; set; }
        public Filter[]? Filters { get; set; }
        public abstract string?[] AllowedSortFields { get; }
        public abstract string?[] AllowedFilterFields { get; }
    }
}
