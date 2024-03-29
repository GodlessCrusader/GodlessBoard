using GodlessBoard.Data;
using GodlessBoard.Hubs;
using GodlessBoard.Pages.Account;
using GodlessBoard.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Net;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.KnownProxies.Add(IPAddress.Parse("10.0.0.100"));
});
// Add services to the container.
builder.Services.AddSignalR();
builder.Services.AddScoped<Auth>();
builder.Services.AddScoped<JwtHandler>();
builder.Services.AddSingleton<MediaUploadRouter>();
builder.Services.AddSingleton<HashGenerator>();
builder.Services.AddRazorPages();
builder.Services.AddControllers();
builder.Services.AddServerSideBlazor();
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();
builder.Services.AddAuthentication(AuthHandler.PolicyName)
    .AddScheme<AuthOptions, AuthHandler>(AuthHandler.PolicyName, null, null);

var connectionString = builder.Configuration.GetConnectionString("MyDbContextConnection"); ;


builder.Services.AddDbContext<MyDbContext>(options =>
    options.UseNpgsql(connectionString));

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

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

//app.MapHub<GameHub>("gamehub");

app.UseEndpoints(endpoints =>
{
    endpoints.MapDefaultControllerRoute();
    endpoints.MapHub<GameHub>("gamehub");
});



app.Run();
