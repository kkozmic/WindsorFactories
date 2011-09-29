using System.Web.Mvc;
using Castle.Windsor;

namespace WindsorFactoryTest.Controllers
{
    public class HomeController : Controller
    {
        public static IBuilderFactory GetFactory()
        {
            return ((IContainerAccessor) System.Web.HttpContext.Current.ApplicationInstance).Container.Resolve<IBuilderFactory>();
        }

        public ActionResult Index()
        {
            ViewBag.Message = "Just created 100 builders with Windsor 3.1";

            for (int x = 1; x < 100; x++)
            {
                GetFactory().GetBuilder().Build();
            }

            return View();
        }

        public ActionResult About()
        {
            return View();
        }
    }
}
