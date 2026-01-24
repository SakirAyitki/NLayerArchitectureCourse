using System.Reflection;
using Autofac;
using Autofac.Builder;
using Autofac.Features.Scanning;
using NLayer.Caching;
using Nlayer.Core.Models;
using Nlayer.Core.Repositories;
using Nlayer.Core.Services;
using Nlayer.Core.UnitOfWork;
using NLayer.Repository;
using NLayer.Repository.Repositories;
using NLayer.Repository.UnitOfWorks;
using NLayer.Service.Mapping;
using NLayer.Service.Services;
using Module = Autofac.Module;

namespace NLayer.API.Modules;

public class RepoServiceModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        var apiAssembly = Assembly.GetExecutingAssembly();
        var repoAssembly = Assembly.GetAssembly(typeof(AppDbContext));
        var serviceAssembly = Assembly.GetAssembly(typeof(MapProfile));
        
        builder.RegisterGeneric(typeof(GenericRepository<>))
            .As(typeof(IGenericRepository<>)).InstancePerLifetimeScope();

        builder.RegisterGeneric(typeof(Service<>))
            .As(typeof(IService<>)).InstancePerLifetimeScope();

        builder.RegisterType<UnitOfWork>().As<IUnitOfWork>();
        
        builder.RegisterAssemblyTypes(apiAssembly, repoAssembly, serviceAssembly)
            .Where(x => x.Name.EndsWith("Repository"))
            .AsImplementedInterfaces().InstancePerLifetimeScope();

        builder.RegisterAssemblyTypes(apiAssembly, repoAssembly, serviceAssembly)
            .Where(x => x.Name.EndsWith("Service"))
            .AsImplementedInterfaces().InstancePerLifetimeScope();
        
        builder.RegisterType<ProductServiceWithCaching>()
            .As<IProductService>();
        
    }
}