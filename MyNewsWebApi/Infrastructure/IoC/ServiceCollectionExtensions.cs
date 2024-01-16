namespace MyNewsWebApi.Infrastructure.IoC;

/// <summary>
/// Extension methods for IServiceCollection
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Allows all Modules to be registered in a single call 
    /// </summary>
    /// <param name="services"></param>
    public static void RegisterAllModules(this IServiceCollection services)
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();

        foreach (var assembly in assemblies)
        {
            foreach (var tp in assembly.GetTypes().Where(t => t.IsSubclassOf(typeof(Module))))
            {
                if (Activator.CreateInstance(tp) is Module module)
                    module.Configure(services);
            }
        }
    }

    /// <summary>
    /// Allows a specified Module to be registered
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="services"></param>
    public static void RegisterModule<T>(this IServiceCollection services) where T : Module
    {
        if (Activator.CreateInstance(typeof(T)) is Module module)
            module.Configure(services);
    }
}