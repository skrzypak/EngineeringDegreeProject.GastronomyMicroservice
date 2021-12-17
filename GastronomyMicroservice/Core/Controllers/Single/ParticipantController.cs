using Authentication;
using GastronomyMicroservice.Core.Interfaces.Services;
using GastronomyMicroservice.Core.Models.Dto.Participant;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GastronomyMicroservice.Core.Controllers.Single
{
    [ApiController]
    [Route("/api/gastronomy/1.0.0/participants")]
    public class ParticipantController : ControllerBase
    {
        private readonly ILogger<ParticipantController> _logger;
        private readonly IParticipantService _participantService;
        private readonly IHeaderContextService _headerContextService;

        public ParticipantController(ILogger<ParticipantController> logger, IParticipantService participantService, IHeaderContextService headerContextService)
        {
            _logger = logger;
            _participantService = participantService;
            _headerContextService = headerContextService;
        }

        [HttpGet]
        public ActionResult<object> Get([FromQuery] int espId)
        {
            var response = _participantService.Get(espId);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public ActionResult<object> GetById([FromQuery] int espId, [FromRoute] int id)
        {
            var response = _participantService.GetById(espId, id);
            return Ok(response);
        }

        [HttpPost]
        public ActionResult Create([FromQuery] int espId, [FromBody] ParticipantCoreDto<int> dto)
        {
            int eudId = _headerContextService.GetEudId();
            int id = _participantService.Create(espId, eudId, dto);
            return CreatedAtAction(nameof(GetById), new { espId = espId, id = id }, null);
        }

        [HttpPut("{id}")]
        public ActionResult Update([FromQuery] int espId, [FromBody] ParticipantDto<int> dto)
        {
            int eudId = _headerContextService.GetEudId();
            _participantService.Update(espId, eudId, dto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete([FromQuery] int espId, [FromRoute] int id)
        {
            int eudId = _headerContextService.GetEudId();
            _participantService.Delete(espId, eudId, id);
            return NoContent();
        }
    }
}
