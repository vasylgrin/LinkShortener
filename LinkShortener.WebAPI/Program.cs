using LinkShortener.Domain.DTO.Request;
using LinkShortener.Domain.Mapper;
using LinkShortener.Service.UrlShortener;
using LinkShortener.WebAPI.Middlewares;

namespace LinkShortener.WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            app.UseMiddleware<ExceptionHandlerMiddleware>();
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseRouting();

            app.UseCors();
            app.UseCors(bldr =>
            {
                bldr.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
            });

            app.UseAuthorization();


            app.MapControllers();

            app.MapFallback(async (HttpContext httpContext) =>
            {
                var path = httpContext.Request.Path.ToUriComponent();
                string shortUrl = httpContext.Request.Scheme + "://" + httpContext.Request.Host + path;

                var urlRequest = new URLRequest { Url = shortUrl };
                var longLinkResponse = await new UrlShortenerService().FindShortUrlAsync(urlRequest);

                var uri = new Uri(longLinkResponse.LongUrl);
                return Results.Redirect(uri.AbsoluteUri);
            });

            app.Run();
        }
    }
}