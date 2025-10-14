using _7E.Branch.Models;
using _7E.Branch.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Web;

namespace BranchLike.Api.Controllers;


[ApiController]
public class RedirectController : ControllerBase
{
    private readonly AppDb _db;


    public RedirectController(AppDb db)
    {
        _db = db;
    }


    [HttpGet("r/{code}")]
    public async Task<IActionResult> RedirectByCode(string code, CancellationToken ct)
    {
        var link = await _db.Links.FirstOrDefaultAsync(x => x.ShortCode == code, ct);
        if (link is null) return NotFound();


        Dictionary<string, string>? cp = null;
        if (!string.IsNullOrWhiteSpace(link.ControlParamsJson))
        {
            try { cp = JsonSerializer.Deserialize<Dictionary<string, string>>(link.ControlParamsJson!); }
            catch { /* ignore */ }
        }


        var target = new Uri(link.LongUrl);
        var nvc = HttpUtility.ParseQueryString(target.Query);
        if (cp is not null)
        {
            foreach (var kv in cp)
            {
                if (string.IsNullOrWhiteSpace(kv.Key)) continue;
                if (nvc.Get(kv.Key) is null) nvc.Add(kv.Key, kv.Value);
            }
        }
        var builderUri = new UriBuilder(target) { Query = nvc.ToString() ?? string.Empty };
        var finalUrl = builderUri.Uri.ToString();


        var click = new LinkClick
        {
            LinkId = link.Id,
            UserAgent = Request.Headers.UserAgent.ToString(),
            Referer = Request.Headers.Referer.ToString(),
            Ip = HttpHelpers.GetClientIp(HttpContext) ?? string.Empty,
            Query = Request.QueryString.HasValue ? Request.QueryString.Value : null
        };
        await _db.LinkClicks.AddAsync(click, ct);


        link.click_count++;
        await _db.SaveChangesAsync(ct);


        return Redirect(finalUrl);
    }
}