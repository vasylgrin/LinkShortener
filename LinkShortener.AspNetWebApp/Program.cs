namespace LinkShortener.AspNetWebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages();
            builder.Services.AddControllersWithViews();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            app.UseMiddleware<Middlewares.ExceptionHandlerMiddleware>();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
            }
            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapRazorPages();

            //app.MapFallback(async (HttpContext httpContext) =>
            //{
            //    var path = httpContext.Request.Path.ToUriComponent();
            //    string shortUrl = httpContext.Request.Scheme + "://" + httpContext.Request.Host + path;

            //    var urlRequest = new URLRequest { Url = shortUrl };
            //    var longLinkResponse = await new UrlShortenerService().FindShortUrlAsync(urlRequest);

            //    var uri = new Uri(longLinkResponse.LongUrl);
            //    return Results.Redirect(uri.AbsoluteUri);
            //});

            app.Run();
        }
    }
}