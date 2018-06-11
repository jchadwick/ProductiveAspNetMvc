using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;

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

            var container = builder.Build();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

        }
    }
}