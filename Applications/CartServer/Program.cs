using CartServer.Extensions;

namespace CartServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //builder.Services.AddScrutor();
            // Add services to the container.
            builder.Services.AddAutoMapper();

            builder.Services.ConfigureKafkaDependencies(builder.Configuration);

            builder.Services.InjectServiceDependencies(builder.Configuration);


            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            /*            builder.Services.Scan(scan => scan
                        .FromExecutingAssembly()
                        .AddClasses()
                        .AsImplementedInterfaces()
                        .WithScopedLifetime());*/


            var app = builder.Build();


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}