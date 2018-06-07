using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using MediatR;

namespace HPlusSports.App_Start
{
    public class Dependencies
    {
        public static void Register()
        {
            var builder = new ContainerBuilder();
            builder.RegisterControllers(typeof(Dependencies).Assembly);
            builder.RegisterType<HPlusSportsDbContext>()
                    .InstancePerRequest();

            builder.RegisterType<HPlusSports.Services.ProductUpdateService>()
                .InstancePerRequest();

            // mediator itself
            builder
              .RegisterType<Mediator>()
              .As<IMediator>()
              .InstancePerLifetimeScope();

            // request handlers
            builder
              .Register<SingleInstanceFactory>(ctx =>
              {
                  var c = ctx.Resolve<IComponentContext>();
                  return t => c.TryResolve(t, out var o) ? o : null;
              })
              .InstancePerLifetimeScope();

            // notification handlers
            builder
              .Register<MultiInstanceFactory>(ctx =>
              {
                  var c = ctx.Resolve<IComponentContext>();
                  return t => (IEnumerable<object>)c.Resolve(typeof(IEnumerable<>).MakeGenericType(t));
              })
              .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(typeof(Models.Product).Assembly)
                .AsImplementedInterfaces();

            var container = builder.Build();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

        }
    }
}