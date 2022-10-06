using GodlessBoard.Data;
using GodlessBoard.Pages.Account;
using GodlessBoard.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

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
/*builder.Services.AddCors(p => p.AddPolicy("clientPolicy", build =>
{
    build.WithOrigins("")
}));*/
builder.Services.AddDbContext<MyDbContext>(options =>
    options.UseNpgsql(connectionString));
var app = builder.Build();

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
app.UseAuthorization();
app.UseAuthentication();
app.MapRazorPages();
app.MapBlazorHub();

app.UseBlazorFrameworkFiles();
app.UseCors(policy =>
           policy.WithOrigins(app.Configuration["CorsOrigins"])
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
