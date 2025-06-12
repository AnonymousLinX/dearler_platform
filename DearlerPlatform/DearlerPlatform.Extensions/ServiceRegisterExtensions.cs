using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using DearlerPlatform.Core.Repository;

namespace DearlerPlatform.Extensions;

public static class ServiceRegisterExtensions
{
    public static IServiceCollection RepositoryRegister(this IServiceCollection service)
    {
        var asmCore = typeof(Repository<>).Assembly;
        var implementationType = asmCore.GetTypes().FirstOrDefault( m => m.Name == "Repository`1");
        var interfaceType = implementationType?.GetInterface("IRepository`1");
        if (interfaceType != null && implementationType != null)
        {
            service.AddTransient(interfaceType, implementationType);
        }
        return service;
    }
}
