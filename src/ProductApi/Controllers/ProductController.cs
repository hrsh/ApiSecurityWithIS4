using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace ProductApi.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ProductController : ControllerBase
    {
        public IActionResult Index() => Ok("Product Api");

        [HttpGet("secret")]
        [Authorize]
        public IActionResult Secret()
        {
            var t = from c in User.Claims
                    select new
                    {
                        c.Type,
                        c.Value
                    };

            return new JsonResult(t);
        }
    }
}
