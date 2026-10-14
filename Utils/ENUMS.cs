namespace ticketManage.Utils
{
    public enum AccountType
    {
        Employee,
        Manager,
        Support,
        QA,
        Developer,
        Admin,
    }

    public enum TicketStatus
    {
        Unassigned,
        InProgress,
        OnHold,
        Failed,
        Completed,
        Reopened,
        Expired,
    }

    public enum StorageProvider
    {
        Database,
        S3,
    }

    public enum FileAssetPurpose
    {
        ProfilePicture,
        TicketAttachment,
        CommentAttachment,
        RichTextImage,
    }

    public enum TokenType
    {
        Access,
        Refresh,
    }
}
