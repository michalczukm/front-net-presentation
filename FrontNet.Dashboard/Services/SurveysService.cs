using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using FrontNet.Common.Models;
using FrontNet.Dashboard.Models;
using Microsoft.Extensions.Configuration;

namespace FrontNet.Dashboard.Services
{
    public class SurveysService
    {
        private IConfiguration _configuration;

        public SurveysService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IReadOnlyList<SurveyQuestion> GetQuestions()
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
                    Id = 2,
                    Question = "Jak oceniasz WebAssembly?"
                },
                new SurveyQuestion
                {
                    Id = 3,
                    Question = "Jak oceniasz Blazora?"
                },
                new SurveyQuestion
                {
                    Id = 4,
                    Question = "Jak oceniasz prezentację?"
                },
            };
        }

        public async Task<SurveyReport> GetReport()
        {
            var filePath = _configuration.GetValue<string>("Survey:FilePath");
            using var stream = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite);
            var responses = await JsonSerializer.DeserializeAsync<IList<SurveyResponse>>(stream);

            var questionsDict = GetQuestions().ToDictionary(question => question.Id, question => question.Question);

            var comments = responses.Select(response => response.AdditionalComment).ToList();
            var questionsReports = responses
                .SelectMany(response => response.QuestionResponses)
                .GroupBy(questionResponse => questionResponse.QuestionId)
                .Select((group, questionId) => new SurveyQuestionReport
                {
                    QuestionId = questionId,
                    QuestionText = questionsDict.GetValueOrDefault<int, string>(questionId, "==="),
                    Responses = group.GroupBy(answer => answer.Score).Select((answersForScore, score) => new SurveyReportResponse
                    {
                        Answer = score.ToString(),
                        Value = answersForScore.Sum(answer => answer.Score)
                    }).ToList()
                }).ToList();

            return new SurveyReport
            {
                Comments = comments,
                QuestionsReports = questionsReports
            };
        }

        public async Task AddSurveyResponse(SurveyResponse response)
        {
            var filePath = _configuration.GetValue<string>("Survey:FilePath");

            using var stream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            var responses = new List<SurveyResponse>();

            try
            {
                responses = await JsonSerializer.DeserializeAsync<List<SurveyResponse>>(stream);
            }
            catch 
            {
                // broken json, or empty file. We don't care.
            }
            finally
            {
                responses.Add(response);
            }

            stream.Seek(0, SeekOrigin.Begin);
            await JsonSerializer.SerializeAsync(stream, responses);
        }
    }
}