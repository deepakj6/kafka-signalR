using Confluent.Kafka;
using InventoryServer.Models;
using InventoryServer.Repository;
using InventoryServer.Services.Subscribers;
using Microsoft.EntityFrameworkCore;

namespace InventoryServer.Extensions
{
    public static class ServiceCollectionExtension
    {
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
            services.AddScoped<ICartEventSubscriber, CartEventSubscriber>();

            // Add services to the container.
            services.AddDbContext<InventoryDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddScoped<IInventoryRepository, SqlInventoryRepository>();
        }


    }
}
