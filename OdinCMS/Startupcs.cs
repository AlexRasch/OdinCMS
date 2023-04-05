using Stripe;

namespace OdinCMS
{
    public class Startup
    {
        public IConfiguration configRoot
        {
            get;
        }



        public Startup(IConfiguration configuration)
        {
            configRoot = configuration;
        }
        public void ConfigureServices(IServiceCollection services, WebApplicationBuilder builder)
        {
            services.AddRazorPages();
            services.AddWebOptimizer(pipeline =>
            {
                pipeline.AddCssBundle("/css/default.css", "css/*.css");
            });

            // Stripe API key
            StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe:Secretkey").Get<string>();
        }

        public void Configure(WebApplication app, IWebHostEnvironment env)
        {
            app.UseWebOptimizer();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            // This need to be in this order
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapRazorPages();
            app.MapControllerRoute(
                name: "default",
                pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
