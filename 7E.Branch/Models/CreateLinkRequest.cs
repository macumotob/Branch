using System.ComponentModel.DataAnnotations;


namespace _7E.Branch.Dtos;


public class CreateLinkRequest
{
    [Required]
    [Url]
    public string LongUrl { get; set; } = null!;


    public string? CustomCode { get; set; }


    public string? Title { get; set; }
    public string? Description { get; set; }


    public Dictionary<string, string>? Metadata { get; set; }
    public Dictionary<string, string>? ControlParams { get; set; }
}