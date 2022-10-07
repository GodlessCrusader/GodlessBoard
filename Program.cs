using GodlessBoard.Data;
using GodlessBoard.Pages.Account;
using GodlessBoard.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.KnownProxies.Add(IPAddress.Parse("10.0.0.100"));
});
// Add services to the container.
builder.Services.AddSingleton<HashGenerator>();
builder.Services.AddRazorPages();
builder.Services.AddControllers();
builder.Services.AddServerSideBlazor();
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();
builder.Services.AddAuthentication("GodlessCookie").AddCookie("GodlessCookie", options =>
options.Cookie.Name = "GodlessCookie"
); 
var connectionString = builder.Configuration.GetConnectionString("MyDbContextConnection"); ;

builder.Services.AddDbContext<MyDbContext>(options =>
    options.UseNpgsql(connectionString));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();
app.UseAuthentication();
app.MapRazorPages();
app.MapBlazorHub();

app.UseBlazorFrameworkFiles();

app.UseCors(policy =>
           policy.WithOrigins(app.Configuration.GetSection("CORS:client").Value)
           .AllowAnyMethod()
           .AllowAnyHeader()
           
); ; ;



app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
});



app.Run();
