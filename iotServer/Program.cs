namespace iotServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Zorgt ervoor dat de decimal separator een punt is
            System.Globalization.CultureInfo.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
            System.Globalization.CultureInfo.CurrentUICulture = System.Globalization.CultureInfo.InvariantCulture;

            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // add logging
            builder.Logging.AddConsole();


            // dependency injection
            builder.Services.AddSingleton<NewsLetter.NewsLetter>();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                //app.UseHsts();
            }

            // app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            // websockets instellen

            var websocketsOptions = new WebSocketOptions
            {
                KeepAliveInterval = TimeSpan.FromSeconds(2)
            };

            app.UseWebSockets(websocketsOptions);

            app.Run();
        }
    }
}