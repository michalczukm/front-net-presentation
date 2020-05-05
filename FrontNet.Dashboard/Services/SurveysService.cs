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
        private IList<int> POSSIBLE_ANSWERS = Enumerable.Range(1, 5).ToList();
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
                    Question = "How well do you know C#?"
                },
                new SurveyQuestion
                {
                    Id = 2,
                    Question = "How do you rate WebAssembly?"
                },
                new SurveyQuestion
                {
                    Id = 3,
                    Question = "How do you rate Blazora?"
                },
                new SurveyQuestion
                {
                    Id = 4,
                    Question = "How do you rate the session?"
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
                .Select((group) => new SurveyQuestionReport
                {
                    QuestionId = group.Key,
                    QuestionText = questionsDict.GetValueOrDefault(group.Key, "==="),
                    Responses = CalculateResponses(group)
                }).ToList();

            return new SurveyReport
            {
                Comments = comments,
                QuestionsReports = questionsReports
            };
        }

        private List<SurveyReportResponse> CalculateResponses(IGrouping<int, SurveyQuestionResponse> group)
        {
            var answersDict = group.GroupBy(answer => answer.Score).Select((answersForScore) => new SurveyReportResponse
            {
                Answer = answersForScore.Key.ToString(),
                Value = answersForScore.Count()
            }).ToDictionary(report => report.Answer);

            return POSSIBLE_ANSWERS.Select(answer => answersDict.GetValueOrDefault<string, SurveyReportResponse>(answer.ToString(), new SurveyReportResponse
            {
                Answer = answer.ToString(),
                Value = 0
            })).ToList();
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