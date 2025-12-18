namespace ezyGo.Core.Notification.Email.Models;

public class EmailMessage
{
    public string To { get; set; } = default!;
    public string Subject { get; set; } = default!;
    public string Body { get; set; } = default!;
    public FileAttachment? Attachments { get; set; }
}

public class FileAttachment
{
    public string FileName { get; set; } = default!;
    public byte[] Content { get; set; } = default!;
    public string ContentType { get; set; } = "application/octet-stream";
}