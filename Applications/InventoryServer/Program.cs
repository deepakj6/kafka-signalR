using InventoryServer.Communication;
using InventoryServer.Extensions;

namespace InventoryServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.ConfigureKafkaDependencies(builder.Configuration);

            builder.Services.InjectServiceDependencies(builder.Configuration);

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowOrigin", builder =>
                {
                    builder.WithOrigins("http://localhost:4200")
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });


            // Register the hub context
            builder.Services.AddHttpContextAccessor(); // Required for accessing the HttpContext in the hub context
            builder.Services.AddSignalR(); // Add SignalR services
            //_=builder.Services.AddHub<InventoryHub>();



            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors("AllowOrigin");

            app.UseHttpsRedirection();



            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<InventoryHub>("/inventoryhub");
                // Map other endpoints as needed
            });


            app.Run();
        }
    }
}