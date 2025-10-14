namespace _7E.Branch.Models;
public class LinkClick
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string LinkId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string? UserAgent { get; set; }
    public string? Referer { get; set; }
    public string? Ip { get; set; }
    public string? Query { get; set; }
}