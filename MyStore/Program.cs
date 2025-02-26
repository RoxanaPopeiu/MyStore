using Microsoft.EntityFrameworkCore;
using MyStore;
using MyStore.Interfaces.Services;
using MyStore.Services;
using System;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresConnection")));
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IBrandService, BrandService>();
builder.Services.AddScoped<ICategoryService,CategoryService>();
builder.Services.AddScoped<ISizeService,SizeService>();
builder.Services.AddScoped<IPromotionService, PromotionService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICartItemService, CartItemService>();
builder.Services.AddScoped<ICartService, CartService>(); 


// Add services to the container.
var serviceProvider = builder.Services.BuildServiceProvider();


builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
