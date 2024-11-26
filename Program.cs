using Microsoft.EntityFrameworkCore;
using ComicSystem.Models;
using ComicSystem.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Cấu hình DbContext với MySQL
builder.Services.AddDbContext<ComicSystemContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("ComicSystemDatabase"),
                     new MySqlServerVersion(new Version(8, 0, 32))));

// Cấu hình CORS nếu cần thiết
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder => builder.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});

// Thêm Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Sử dụng CORS policy nếu đã cấu hình
app.UseCors("AllowAll");

app.UseAuthorization();

// Sử dụng Swagger
app.UseSwagger();
app.UseSwaggerUI();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=ComicBooks}/{action=Index}/{id?}");

// Map các API controllers
app.MapControllers();

app.Run();
