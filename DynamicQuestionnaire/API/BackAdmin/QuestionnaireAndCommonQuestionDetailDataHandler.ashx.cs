using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace DynamicQuestionnaire.API.BackAdmin
{
    /// <summary>
    /// Summary description for QuestionnaireAndCommonQuestionDetailDataHandler
    /// </summary>
    public class QuestionnaireAndCommonQuestionDetailDataHandler : IHttpHandler, IRequiresSessionState
    {
        private string _textResponse = "text/plain";
        private string _successedResponse = "SUCCESSED";

        // Session name of QuestionnaireDetail or CommonQuestionDetail
        private string _isUpdateMode = "IsUpdateMode";

        // Session name of QuestionnaireDetail
        private string _questionnaire = "Questionnaire";
        private string _questionList = "QuestionList";
        private string _currentPagerIndex = "CurrentPagerIndex";
        private string _isSetCommonQuestionOnQuestionnaire = "IsSetCommonQuestionOnQuestionnaire";

        // Session name of CommonQuestionDetail
        private string _isPostBack = "IsPostBack";
        private string _commonQuestion = "CommonQuestion";
        private string _questionListOfCommonQuestion = "QuestionListOfCommonQuestion";

        public void ProcessRequest(HttpContext context)
        {
            if (string.Compare("GET", context.Request.HttpMethod, true) == 0
                && string.Compare("RESET_QUESTIONNAIRE_AND_COMMONQUESTIONDETAIL_SESSION", context.Request.QueryString["Action"], true) == 0)
            {
                // Session name of QuestionnaireDetail or CommonQuestionDetail
                context.Session.Remove(_isUpdateMode);

                // Session name of QuestionnaireDetail
                context.Session.Remove(_questionnaire);
                context.Session.Remove(_questionList);
                context.Session.Remove(_currentPagerIndex);
                context.Session.Remove(_isSetCommonQuestionOnQuestionnaire);

                // Session name of CommonQuestionDetail
                context.Session.Remove(_isPostBack);
                context.Session.Remove(_commonQuestion);
                context.Session.Remove(_questionListOfCommonQuestion);

                context.Response.ContentType = _textResponse;
                context.Response.Write(_successedResponse);
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