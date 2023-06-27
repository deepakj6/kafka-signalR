namespace CartServer.Models
{
    public class KafkaConfig
    {
        public string BootstrapServers { get; set; }
        public string SaslUsername { get; set; }
        public string SaslPassword { get; set; }
    }

}
