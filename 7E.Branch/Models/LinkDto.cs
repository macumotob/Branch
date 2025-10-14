using _7E.Branch.Models;
using System.Text.Json;


namespace BranchLike.Api.Dtos;


public record LinkDto(
string ShortUrl,
string ShortCode,
string LongUrl,
long Clicks,
DateTime CreatedAt,
string? Title,
string? Description,
Dictionary<string, string>? Metadata,
Dictionary<string, string>? ControlParams
)
{
    public static LinkDto From(Link link, string baseUrl)
    {
        var md = string.IsNullOrWhiteSpace(link.MetadataJson)
        ? null
        : JsonSerializer.Deserialize<Dictionary<string, string>>(link.MetadataJson!);


        var cp = string.IsNullOrWhiteSpace(link.ControlParamsJson)
        ? null
        : JsonSerializer.Deserialize<Dictionary<string, string>>(link.ControlParamsJson!);


        return new LinkDto(
        ShortUrl: Combine(baseUrl, $"r/{link.ShortCode}"),
        ShortCode: link.ShortCode,
        LongUrl: link.LongUrl,
        Clicks: link.click_count,
        CreatedAt: link.CreatedAt,
        Title: link.Title,
        Description: link.Description,
        Metadata: md,
        ControlParams: cp
        );
    }


    private static string Combine(string baseUrl, string path)
    => baseUrl.TrimEnd('/') + "/" + path.TrimStart('/');
}