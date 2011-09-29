using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Castle.Facilities.TypedFactory;
using Castle.MicroKernel.Registration;
using Castle.Windsor;

namespace WindsorFactoryTest
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication, IContainerAccessor 
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);


            container = new WindsorContainer();
            container.AddFacility<TypedFactoryFacility>();
            container.Register(
                Component.For<IBuilderFactory>().AsFactory().LifeStyle.PerWebRequest,
                Component.For<IBuilder>().ImplementedBy<ConcreteBuilder>().LifeStyle.Transient
            );
        }


        private static IWindsorContainer container;

        public IWindsorContainer Container
        {
            get { return container; }
        }
    }


    public interface IBuilderFactory
    {
        IBuilder GetBuilder();
    }

    public interface IBuilder
    {
        void Build();
    }

    public class ConcreteBuilder : IBuilder, IDisposable
    {
        public void Build()
        {
            HttpContext.Current.Response.Write("Building something");
        }

        public void Dispose()
        {
            HttpContext.Current.Response.Write("Disposing..." + GetHashCode());
        }
    }

}