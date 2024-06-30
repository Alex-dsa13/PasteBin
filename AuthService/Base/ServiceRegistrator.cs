namespace AuthService.Base
{
    public static class ServiceRegistrator
    {
        public static IServiceCollection RegisterServices(this IServiceCollection serviceCollection)
        {
            var services = InterfaceBindings.GetServiceBindings();
            foreach (var service in services)
            {
                serviceCollection.AddSingleton(service.Key, service.Value);
            }

            var repositories = InterfaceBindings.GetRepositoryBindings();
            foreach (var repository in repositories)
            {
                serviceCollection.AddSingleton(repository.Key, repository.Value);
            }

            return serviceCollection;
        }
    }
}
