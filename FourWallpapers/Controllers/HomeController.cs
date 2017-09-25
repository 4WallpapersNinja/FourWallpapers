using Microsoft.AspNetCore.Mvc;

namespace FourWallpapers.Controllers {
    public class HomeController : Controller {
        public ActionResult Index() {
            return File("~/index.html", "text/html");
        }
    }
}