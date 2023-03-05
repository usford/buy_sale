using buy_sale.database;
using buy_sale.database.Interfaces;
using buy_sale.database.Models;
using buy_sale.database.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

ConfigureServices(builder.Services);

var app = builder.Build();

//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

//app.UseSwaggerUI(options =>
//{
//    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
//    options.RoutePrefix = string.Empty;
//});

app.MapGet("/index.html", async (IRepository<Product> dsa) => await dsa.GetAllAsync());

app.Run();

void ConfigureServices(IServiceCollection services)
{
    services.AddDbContext<BuySaleDbContext>(opt =>
    {
        opt.UseSqlServer(config.GetConnectionString("Default"));
    });
    services.AddTransient<IRepository<Product>, ProductRepository>();
    services.AddControllers();

    //services.AddSwaggerGen(options =>
    //{
    //    options.SwaggerDoc("v1", new OpenApiInfo
    //    {
    //        Version = "v1",
    //        Title = "BUY/SALE API",
    //        Description = "API для покупки/продажи всякого",
    //    });
    //});  
}
