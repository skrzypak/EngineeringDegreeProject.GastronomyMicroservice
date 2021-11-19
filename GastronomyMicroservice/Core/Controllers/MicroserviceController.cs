using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GastronomyMicroservice.Core.Controllers
{
    [ApiController]
    [Route("/api/gastronomy/1.0.0/msv")]
    public class MicroserviceController : ControllerBase
    {
        private readonly ILogger<MicroserviceController> _logger;

        public MicroserviceController(ILogger<MicroserviceController> logger)
        {
            _logger = logger;
        }
    }
}
