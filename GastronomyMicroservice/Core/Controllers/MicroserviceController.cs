using GastronomyMicroservice.Core.Fluent;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GastronomyMicroservice.Core.Controllers
{
    [ApiController]
    [Route("/api/gastronomy/1.0.0/msv")]
    public class MicroserviceController : ControllerBase
    {
        private readonly ILogger<MicroserviceController> _logger;
        private readonly MicroserviceContext _context;

        public MicroserviceController(ILogger<MicroserviceController> logger, MicroserviceContext context)
        {
            _logger = logger;
            _context = context;
        }
    }
}
