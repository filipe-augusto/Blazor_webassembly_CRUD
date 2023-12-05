using Microsoft.AspNetCore.Mvc;

namespace Blazor_WASM_CRUD.API.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
