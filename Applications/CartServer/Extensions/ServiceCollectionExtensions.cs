using AutoMapper;
using CartServer.Configuration;
using CartServer.Models;
using CartServer.Respository;
using CartServer.Services;
using CartServer.Services.Contracts;
using CartServer.Services.Publishers;
using Confluent.Kafka;
using Microsoft.EntityFrameworkCore;


namespace CartServer.Extensions
{
    public static class ServiceCollectionExtensions
    {

        public static void AddScrutor(this IServiceCollection services)
        {
            // Scan the assembly and register types automatically
            services.Scan(scan => scan
                .FromAssemblyOf<Program>()
                .AddClasses()
                .AsImplementedInterfaces()
                .WithScopedLifetime());
        }


        public static void AddAutoMapper(this IServiceCollection services)
        {
            var mapperConfig = new MapperConfiguration(mapperConfiguration =>
                mapperConfiguration.AddProfile(new MappingProfile()));

            var mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);
        }
        public static void ConfigureKafkaDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            // Load Kafka configuration from appsettings.json
            IConfigurationSection kafkaConfigSection = configuration.GetSection("KafkaConfig");
            KafkaConfig kafkaConfig = kafkaConfigSection.Get<KafkaConfig>();

            // Register the Kafka producer and consumer
            services.AddSingleton<ProducerConfig>(sp => new ProducerConfig
            {
                BootstrapServers = kafkaConfig.BootstrapServers,
                SaslUsername = kafkaConfig.SaslUsername,
                SaslPassword = kafkaConfig.SaslPassword,
                SecurityProtocol = SecurityProtocol.SaslSsl,
                SaslMechanism = SaslMechanism.Plain
            });

            services.AddSingleton<ConsumerConfig>(sp => new ConsumerConfig
            {
                BootstrapServers = kafkaConfig.BootstrapServers,
                SaslUsername = kafkaConfig.SaslUsername,
                SaslPassword = kafkaConfig.SaslPassword,
                SecurityProtocol = SecurityProtocol.SaslSsl,
                SaslMechanism = SaslMechanism.Plain,
                GroupId = "my-group"
            });

            services.AddSingleton<IConsumer<string, string>>(sp =>
            {
                var consumerConfig = sp.GetRequiredService<ConsumerConfig>();
                return new ConsumerBuilder<string, string>(consumerConfig).Build();
            });

            services.AddSingleton<IProducer<string, string>>(sp =>
            {
                var producerConfig = sp.GetRequiredService<ProducerConfig>();
                return new ProducerBuilder<string, string>(producerConfig).Build();
            });
        }
        public static void InjectServiceDependencies(this IServiceCollection services, IConfiguration configuration)
        {

            // Register the CartEventSubscriber
            services.AddScoped<ICartEventPublisher, CartEventPublisher>();
            services.AddScoped<ICartRepository, SqlCartRepository>();
            services.AddScoped<ICartService, CartService>();

            // Add services to the container.
            services.AddDbContext<CartDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });



        }

    }
}
