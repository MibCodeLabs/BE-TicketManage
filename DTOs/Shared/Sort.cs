using ticketManage.Utils;

namespace ticketManage.DTOs.Shared
{
    public class Sort
    {
        public string Field { get; set; } = "";
        public SortDirection SortDirection { get; set; } = SortDirection.Ascending;
    }
}
