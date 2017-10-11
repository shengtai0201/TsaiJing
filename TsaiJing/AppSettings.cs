using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using Owin;
using System;
using System.Reflection;
using System.Web.Http;
using System.Web.Mvc;

namespace TsaiJing
{
    public class AppSettings
    {
        private AppSettings() { }

        private static readonly Lazy<ContainerBuilder> _builder = new Lazy<ContainerBuilder>(() => new ContainerBuilder());

        public static void RegisterAutofac(IAppBuilder app, Assembly clientExecutingAssembly)
        {
            var builder = _builder.Value;

            builder.RegisterModule(new DefaultModule());

            var executingAssembly = Assembly.GetExecutingAssembly();
            builder.RegisterControllers(executingAssembly, clientExecutingAssembly);
            builder.RegisterApiControllers(executingAssembly, clientExecutingAssembly);

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            app.UseAutofacMiddleware(container);
            app.UseAutofacMvc();
            app.UseAutofacWebApi(new HttpConfiguration());
        }
    }
}
