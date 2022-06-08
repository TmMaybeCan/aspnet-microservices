using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Discount.gRPC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        [HttpGet]
        public string Get()
        {
            return "Test";
        }
    }
}
