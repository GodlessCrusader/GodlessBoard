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
builder.Services.AddSingleton<MediaUploadRouter>();
builder.Services.AddSingleton<HashGenerator>();
builder.Services.AddRazorPages();
builder.Services.AddControllers();
builder.Services.AddServerSideBlazor();
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();
builder.Services.AddAuthentication("GodlessCookie").AddCookie("GodlessCookie", options =>
{ 
    options.Cookie.Name = "GodlessCookie";
    options.Cookie.
} 

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

var corsSection = app.Configuration.GetSection("CORS");

var cors = new List<string>();
foreach (var c in corsSection.AsEnumerable())
    cors.Add(c.Value);
cors.RemoveAll(x => x == null);
app.UseCors(policy =>
           policy.WithOrigins(cors.ToArray())
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
