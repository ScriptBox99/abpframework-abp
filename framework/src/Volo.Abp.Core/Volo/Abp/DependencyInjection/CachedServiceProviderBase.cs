using System;
using System.Collections.Concurrent;
using Microsoft.Extensions.DependencyInjection;

namespace Volo.Abp.DependencyInjection;

public abstract class CachedServiceProviderBase
{
    protected IServiceProvider ServiceProvider { get; }
    protected ConcurrentDictionary<Type, Lazy<object>> CachedServices { get; }

    protected CachedServiceProviderBase(IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
        CachedServices = new ConcurrentDictionary<Type, Lazy<object>>();
        CachedServices.TryAdd(typeof(IServiceProvider), new Lazy<object>(() => ServiceProvider));
    }

    public virtual object GetService(Type serviceType)
    {
        return CachedServices.GetOrAdd(
            serviceType,
            _ => new Lazy<object>(() => ServiceProvider.GetService(serviceType))
        ).Value;
    }
    
    public virtual object GetRequiredService(Type serviceType)
    {
        return CachedServices.GetOrAdd(
            serviceType,
            _ => new Lazy<object>(() => ServiceProvider.GetRequiredService(serviceType))
        ).Value;
    }
}
