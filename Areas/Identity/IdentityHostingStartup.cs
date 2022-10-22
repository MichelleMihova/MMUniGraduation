using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(MMUniGraduation.Areas.Identity.IdentityHostingStartup))]
namespace MMUniGraduation.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
            });
        }
    }
}