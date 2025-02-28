using Microsoft.EntityFrameworkCore;
using MyStore;
using MyStore.Interfaces.Repositories;
using MyStore.Interfaces.Services;
using MyStore.Repositories;
using MyStore.Services;
using System;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresConnection")));

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IBrandRepository, BrandRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ISizeRepository, SizeRepository>();
builder.Services.AddScoped<IPromotionRepository, PromotionRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ICartItemRepository, CartItemRepository>();
builder.Services.AddScoped<ICartRepository, CartRepository>();


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
