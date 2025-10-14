using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using BranchLike.Api;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<ForwardedHeadersOptions>(opts =>
{
    opts.ForwardedHeaders = ForwardedHeaders.XForwardedFor
     | ForwardedHeaders.XForwardedProto;
    opts.KnownNetworks.Clear();
    opts.KnownProxies.Clear();
});
builder.Services.AddDbContext<AppDb>(o =>
{
    var cs = builder.Configuration.GetConnectionString("Default")!;
    o.UseMySql(cs, ServerVersion.AutoDetect(cs));
});
var app = builder.Build();
app.UseForwardedHeaders();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); app.UseSwaggerUI();
}
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDb>(); db.Database.Migrate();
}
app.MapControllers(); 
app.Run();