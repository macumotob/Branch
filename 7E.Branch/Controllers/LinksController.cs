using _7E.Branch.Dtos;
using _7E.Branch.Utils;
using BranchLike.Api.Dtos;
using _7E.Branch.Models;
using _7E.Branch.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using _7E.Branch.Helpers;


namespace BranchLike.Api.Controllers;



[ApiController]
[Route("api/[controller]/[action]")]
public class LinksController : ControllerBase
{
    private readonly AppDb _db;
    private readonly IConfiguration _cfg;


    public LinksController(AppDb db, IConfiguration cfg)
    {
        _db = db;
        _cfg = cfg;
    }


    private string BaseUrl => $"{Request.Scheme}://{Request.Host}";


    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateLinkRequest input, CancellationToken ct)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);


        var len = _cfg.GetValue<int>("ShortCode:Length", 7);


        string shortCode;
        if (!string.IsNullOrWhiteSpace(input.CustomCode))
        {
            var exists = await _db.Links.AnyAsync(x => x.ShortCode == input.CustomCode, ct);
            if (exists) return Conflict(new { error = "CustomCode already taken" });
            shortCode = input.CustomCode!;
        }
        else
        {
            shortCode = await ShortCodeGen.GenerateUniqueAsync(_db, len, ct);
        }


        var link = new Link
        {
            ShortCode = shortCode,
            LongUrl = input.LongUrl,
            Title = input.Title,
            Description = input.Description,
            MetadataJson = input.Metadata is null ? null : JsonSerializer.Serialize(input.Metadata),
            ControlParamsJson = input.ControlParams is null ? null : JsonSerializer.Serialize(input.ControlParams)
        };


        await _db.Links.AddAsync(link, ct);
        await _db.SaveChangesAsync(ct);


        return Ok(LinkDto.From(link, BaseUrl));
    }


    [HttpGet]
    public async Task<IActionResult> List([FromQuery] int skip = 0, [FromQuery] int take = 50, CancellationToken ct = default)
    {
        return LinkHelper.Get(take, skip);
        take = Math.Clamp(take <= 0 ? 50 : take, 1, 200);
        var items = await _db.Links
        .OrderByDescending(x => x.CreatedAt)
        .Skip(skip)
        .Take(take)
        .AsNoTracking()
        .ToListAsync(ct);


        return Ok(items.Select(x => LinkDto.From(x, BaseUrl)));
    }
    [HttpGet("{code}")]
    public async Task<IActionResult> Get(string code, CancellationToken ct)
    {
        var link = await _db.Links.AsNoTracking().FirstOrDefaultAsync(x => x.ShortCode == code, ct);
        if (link is null) return NotFound();


        var lastClicks = await _db.LinkClicks
        .Where(c => c.LinkId == link.Id)
        .OrderByDescending(c => c.CreatedAt)
        .Take(50)
        .AsNoTracking()
        .ToListAsync(ct);


        return Ok(new { link = LinkDto.From(link, BaseUrl), lastClicks });
    }
}