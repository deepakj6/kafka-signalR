namespace CartServer.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void ConfigureSessionServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Required for session state
            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {                
                options.Cookie.Name = "OpenBookStore_" + Guid.NewGuid().ToString();
               
            });           
        }

    }
}
