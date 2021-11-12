using GastronomyMicroservice.Core.Interfaces.Services;
using GastronomyMicroservice.Core.Models.Dto.Participant;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GastronomyMicroservice.Core.Controllers.Single
{
    [ApiController]
    [Route("/api/gastronomy/1.0.0/{enterpriseId}/participants")]
    public class ParticipantController : ControllerBase
    {
        private readonly ILogger<ParticipantController> _logger;
        private readonly IParticipantService _participantService;

        public ParticipantController(ILogger<ParticipantController> logger, IParticipantService participantService)
        {
            _logger = logger;
            _participantService = participantService;
        }

        [HttpGet]
        public ActionResult<object> Get([FromRoute] int enterpriseId)
        {
            var response = _participantService.Get(enterpriseId);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public ActionResult<object> GetById([FromRoute] int enterpriseId, [FromRoute] int id)
        {
            var response = _participantService.GetById(enterpriseId, id);
            return Ok(response);
        }

        [HttpPost]
        public ActionResult Create([FromRoute] int enterpriseId, [FromBody] ParticipantCoreDto<int> dto)
        {
            int id = _participantService.Create(enterpriseId, dto);
            return CreatedAtAction(nameof(GetById), new { enterpriseId = enterpriseId, id = id }, null);
        }

        [HttpDelete("{id}")]
        public ActionResult Delete([FromRoute] int enterpriseId, [FromRoute] int id)
        {
            _participantService.Delete(enterpriseId, id);
            return NoContent();
        }
    }
}
