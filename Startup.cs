using Microsoft.AspNetCore.Builder;

namespace GodlessBoard
{
    public class Startup
    {
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseBlazorFrameworkFiles();
        }
    }
}
