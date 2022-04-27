using DynamicQuestionnaire.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynamicQuestionnaire.API
{
    /// <summary>
    /// Summary description for QuestionnaireStatisticsDataHandler
    /// </summary>
    public class QuestionnaireStatisticsDataHandler : IHttpHandler
    {
        private string _textResponse = "text/plain";
        private string _jsonResponse = "application/json";
        private string _failedResponse = "FAILED";

        private QuestionManager _questionMgr = new QuestionManager();
        private UserAnswerManager _userAnswerMgr = new UserAnswerManager();

        public void ProcessRequest(HttpContext context)
        {
            if (string.Compare("POST", context.Request.HttpMethod, true) == 0 && string.Compare("GET_QUESTIONNAIRE_STATISTICS", context.Request.QueryString["Action"], true) == 0)
            {
                string questionnaireIDStr = context.Request.Form["questionnaireID"];
                if (!Guid.TryParse(questionnaireIDStr, out Guid questionnaireID))
                {
                    context.Response.ContentType = _textResponse;
                    context.Response.Write(_failedResponse);
                    return;
                }

                var questionList = this._questionMgr.GetQuestionListOfQuestionnaire(questionnaireID);
                var questionModelList = this._questionMgr.BuildQuestionModelList(questionList);
                var userAnswerList = this._userAnswerMgr.GetUserAnswerList(questionnaireID);
                var userAnswerModelList = this._userAnswerMgr.BuildUserAnswerModelList(userAnswerList);
                object[] statisticsArr = { questionModelList, userAnswerModelList };
                string jsonText = Newtonsoft.Json.JsonConvert.SerializeObject(statisticsArr);

                context.Response.ContentType = _jsonResponse;
                context.Response.Write(jsonText);
                return;
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}