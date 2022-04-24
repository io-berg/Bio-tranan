using Infrastructure;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using MovieTheaterCore.Interfaces;
using MovieTheaterCore.Services;
using WebApp.Services;
using WebApp.Workers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDbContext<MovieTheaterContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("MovieTheaterConnection")));

builder.Services.AddScoped<MovieViewingViewModelService>();
builder.Services.AddScoped<MovieViewModelService>();
builder.Services.AddScoped<SalonViewModelService>();
builder.Services.AddScoped<ReservationViewModelService>();
builder.Services.AddScoped<CreateReviewViewModelService>();
builder.Services.AddScoped<MovieService>();
builder.Services.AddScoped<SalonService>();
builder.Services.AddScoped<MovieViewingService>();
builder.Services.AddScoped<ReservationService>();
builder.Services.AddScoped<ReviewService>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddHttpClient();
builder.Services.AddHostedService<ReservationCleaningWorker>();


var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "AdminAPI");
    });
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<MovieTheaterContext>();
    await MovieTheaterContextSeeding.SeedAsync(context);
}

app.Run();
