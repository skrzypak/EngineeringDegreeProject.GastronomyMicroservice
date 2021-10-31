using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GastronomyMicroservice.Core.Interfaces.Services;
using GastronomyMicroservice.Core.Models.Dto.Participant;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GastronomyMicroservice.Core.Controllers.Single
{
    [ApiController]
    [Route("[controller]")]
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
        public ActionResult<object> Get()
        {
            var response = _participantService.Get();
            return Ok(response);
        }

        [HttpGet("{id}")]
        public ActionResult<object> GetById([FromRoute] int id)
        {
            var response = _participantService.GetById(id);
            return Ok(response);
        }

        [HttpPost]
        public ActionResult Create([FromBody] ParticipantCoreDto<int> dto)
        {
            int id = _participantService.Create(dto);
            return CreatedAtAction(nameof(GetById), new { id = id }, null);
        }

        [HttpDelete("{id}")]
        public ActionResult Delete([FromRoute] int id)
        {
            _participantService.Delete(id);
            return NoContent();
        }
    }
}
