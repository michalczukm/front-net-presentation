using System.Collections.Generic;
using System.Threading.Tasks;
using FrontNet.Common.Models;
using FrontNet.Dashboard.Services;
using Microsoft.AspNetCore.Mvc;

namespace FrontNet.Dashboard.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class SurveyController : ControllerBase
    {
        private SurveysService _surveysService;

        public SurveyController(SurveysService surveysService)
        {
            _surveysService = surveysService;
        }

        public ActionResult<IReadOnlyCollection<SurveyQuestion>> Get()
        {
            return Ok(_surveysService.GetQuestions());
        }

        [HttpPost]
        public async Task<IActionResult> CreateSurveyResponse(SurveyResponse response)
        {
            // try {
                await _surveysService.AddSurveyResponse(response);
                return Ok();
            // } catch {
                // return StatusCode(500);
            // }
        }
    }
}