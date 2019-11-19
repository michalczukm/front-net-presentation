using System.Collections.Generic;
using System.Threading.Tasks;
using FrontNet.Common.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace FrontNet.Dashboard.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class SurveyController : ControllerBase
    {
        private IConfiguration _configuration;

        public SurveyController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IReadOnlyCollection<SurveyQuestion> Get()
        {
            return new List<SurveyQuestion>
            {
                new SurveyQuestion
                {
                    Id = 1,
                    Question = "Jak dobrze znasz C#?"
                },                
                new SurveyQuestion
                {
                    Id = 1,
                    Question = "Jak oceniasz WebAssembly?"
                },
                new SurveyQuestion
                {
                    Id = 1,
                    Question = "Jak oceniasz Blazora?"
                },
                new SurveyQuestion
                {
                    Id = 1,
                    Question = "Jak oceniasz prezentację?"
                },
            };
        }

        [HttpPost]
        public async Task CreateSurveyResponse(SurveyResponse response)
        {
            var filePath = _configuration.GetValue<string>("Survey:FilePath");
            using var stream = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite);
            var responses = await JsonSerializer.DeserializeAsync<List<SurveyResponse>>(stream);
            responses.Add(response);
            stream.Seek(0, SeekOrigin.Begin);
            await JsonSerializer.SerializeAsync(stream, responses);
        }
    }
}