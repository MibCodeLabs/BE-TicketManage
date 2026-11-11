using ticketManage.Utils;

namespace ticketManage.DTOs.Shared
{
    public abstract class ChangeOperationRequest<IdType>
    {
        public IdType? Id { get; set; }
        public bool? ToDelete { get; set; }

        public ChangeOperation GetOperation()
        {
            if (Utility.IsNotNullOrEmpty(this.Id))
            {
                if (this.ToDelete.GetValueOrDefault(false))
                {
                    return ChangeOperation.Delete;
                }
                return ChangeOperation.Update;
            }
            return ChangeOperation.Create;
        }
    }
}
