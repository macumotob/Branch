
//namespace _7E.Branch
//{
//    public class Program
//    {
//        public static void Main(string[] args)
//        {
//            var builder = WebApplication.CreateBuilder(args);

//            // Add services to the container.

//            builder.Services.AddControllers();
//            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//            builder.Services.AddEndpointsApiExplorer();
//            builder.Services.AddSwaggerGen();

//            var app = builder.Build();

//            // Configure the HTTP request pipeline.
//            if (app.Environment.IsDevelopment())
//            {
//                app.UseSwagger();
//                app.UseSwaggerUI();
//            }

//            app.UseHttpsRedirection();

//            app.UseAuthorization();


//            app.MapControllers();

//            app.Run();
//        }
//    }
//}
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using BranchLike.Api;
using _7E.Branch.Helpers;

//var configuration = new ConfigurationBuilder()
//            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory) // Устанавливаем базовый путь
//            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true) // Чтение из файла
//            .Build();

//var len = configuration["Test:Vasyl:test"];

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.Configure<ForwardedHeadersOptions>(opts =>
{
    opts.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    opts.KnownNetworks.Clear();
    opts.KnownProxies.Clear();
});

var cnns = builder.Configuration.GetConnectionString("Default")!;
DB.use(cnns);

builder.Services.AddDbContext<AppDb>(options =>
{
    var cs = builder.Configuration.GetConnectionString("Default")!;

    options.UseMySql(cs, ServerVersion.AutoDetect(cs), mySqlOptions =>
    {
        // если нужно — можно указать поведение явно:
        // mySqlOptions.UseCharSet(CharSet.Utf8Mb4);
    });
});


var app = builder.Build();


app.UseForwardedHeaders();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


//using (var scope = app.Services.CreateScope())
//{
//    var db = scope.ServiceProvider.GetRequiredService<AppDb>();
//    db.Database.EnsureCreated(); // быстрый способ, но лучше миграции
//}


app.MapControllers();
app.Run();