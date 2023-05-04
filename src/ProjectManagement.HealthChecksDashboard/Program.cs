using ProjectManagement.HealthChecksDashboard.Data;
using ProjectManagement.HealthChecksDashboard.Extensions;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddApplicationConfiguration();
builder.Logging.AddApplicationLogging(builder.Configuration);
builder.Services.RegisterDependencies(builder.Configuration);

WebApplication app = builder.Build();

// Add services to the container.
// builder.Services.AddRazorPages();
// builder.Services.AddServerSideBlazor();
// builder.Services.AddSingleton<WeatherForecastService>();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
