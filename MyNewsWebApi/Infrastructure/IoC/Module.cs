namespace MyNewsWebApi.Infrastructure.IoC;

/// <summary>
/// Abstraction that enables registered types to be logically grouped into Modules
/// </summary>
public abstract class Module
{
    public void Configure(IServiceCollection services)
    {
        Load(services);
    }

    protected abstract void Load(IServiceCollection services);
}