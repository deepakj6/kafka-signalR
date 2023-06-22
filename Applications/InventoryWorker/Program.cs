// See https://aka.ms/new-console-template for more information
using Hangfire;
using Hangfire.SqlServer;
using InventoryWorker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;



var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

var kafkaConfig = configuration.GetSection("KafkaConfig");

var bootstrapServers = kafkaConfig["BootstrapServers"];
var saslUsername = kafkaConfig["SaslUsername"];
var saslPassword = kafkaConfig["SaslPassword"];
var securityProtocol = kafkaConfig["SecurityProtocol"];
var saslMechanism = kafkaConfig["SaslMechanism"];

var kafkaHelper = new KafkaHelper(bootstrapServers, saslUsername, saslPassword);

var consumerBuilder = kafkaHelper.CreateConsumerBuilder();
var producerBuilder = kafkaHelper.CreateProducerBuilder();



// Configure Hangfire with SQL Server storage
string hangfireConnectionString = "YOUR_HANGFIRE_CONNECTION_STRING";
GlobalConfiguration.Configuration.UseSqlServerStorage(hangfireConnectionString);

// Register Hangfire background job
RecurringJob.AddOrUpdate<CartEventConsumer>("cart-event-consumer", consumer => consumer.StartConsuming(), Cron.Minutely);


// Run Hangfire server
using (var server = new BackgroundJobServer())
{
    Console.WriteLine("Hangfire server started. Press any key to exit...");
    Console.ReadKey();
}
