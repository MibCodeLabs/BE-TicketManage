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

    public enum SortDirection
    {
        Ascending,
        Descending,
    }

    public enum AllowedMimeType
    {
        Pdf,
        Svg,
        Png,
        Jpeg,
        Webp,
        PlainText,
        Csv,
        Zip,
    }

    public enum CommentType
    {
        Comment,
        Reply,
    }

    public enum ChangeOperation
    {
        Create,
        Update,
        Delete,
    }
}
