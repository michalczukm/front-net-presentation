using System.Collections.Generic;

namespace FrontNet.Common.Models
{
    public class SurveyResponse
    {
        public IReadOnlyCollection<SurveyQuestionResponse> QuestionResponses { get; set; }
        public string Author { get; set; }
        public string AdditionalComment { get; set; }
    }
}
