using System.Collections.Generic;

namespace FrontNet.Dashboard.Models
{
    public class SurveyReportResponse
    {
        public string Answer { get; set; }
        public float Value { get; set; }
    }

    public class SurveyQuestionReport
    {
        public int QuestionId { get; set; }
        public string QuestionText { get; set; }
        public IReadOnlyList<SurveyReportResponse> Responses { get; set; }
    }

    public class SurveyReport
    {
        public IReadOnlyList<SurveyQuestionReport> QuestionsReports { get; set; }
        public IReadOnlyList<string> Comments { get; set; }
    }
}
