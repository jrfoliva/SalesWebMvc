using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SalesWebMvc.Data;
using Pomelo.EntityFrameworkCore;
using SalesWebMvc.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add to Context
builder.Services.AddDbContext<SalesWebMvcContext>
    (options => options.UseMySql(
        "server=localhost;port=3306;user=root;password=masterkey;database=webseller",
        Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.31-mysql")));

// Add Injeção de dependências
builder.Services.AddScoped<SeedingService>();
builder.Services.AddScoped<SellerService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
{
    app.Services.CreateScope().ServiceProvider.GetRequiredService<SeedingService>().Seed();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
