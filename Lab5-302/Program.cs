using Azure.Storage.Blobs;
using Lab5_302.Data;
using Microsoft.EntityFrameworkCore;

namespace Lab5_302
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages();

            var connection = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<AnswerImageDataContext>(options => options.UseSqlServer(connection));

            var blobConnection = builder.Configuration.GetConnectionString("AzureBlobStorage");
            builder.Services.AddSingleton(new BlobServiceClient(blobConnection));


            var app = builder.Build();

/*            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<AnswerImageDataContext>();
                    //DbInitializer.Initialize(context);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while seeding the database.");
                }
            }*/

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                //app.UseHsts();
            }
            //app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();
            

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });

            app.Run();
        }
    }
}