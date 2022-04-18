using DynamicQuestionnaire.DynamicQuestionnaire.ORM;
using DynamicQuestionnaire.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace DynamicQuestionnaire.API
{
    /// <summary>
    /// Summary description for QuestionnaireDetailDataHandler1
    /// </summary>
    public class QuestionnaireDetailDataHandler1 : IHttpHandler, IRequiresSessionState
    {
        private string _textResponse = "text/plain";
        private string _jsonResponse = "application/json";
        //private string _nullResponse = "NULL";
        //private string _successedResponse = "SUCCESSED";
        private string _failedResponse = "FAILED";
        private const string SINGLE_SELECT = "單選方塊";
        private const string MULTIPLE_SELECT = "複選方塊";
        private const string TEXT = "文字";

        // Session name
        //private string _user = "User";
        private string _userQuestion = "UserQuestion";

        //private QuestionnaireManager _questionnaireMgr = new QuestionnaireManager();
        //private QuestionManager _questionMgr = new QuestionManager();

        public void ProcessRequest(HttpContext context)
        {
            if (string.Compare("POST", context.Request.HttpMethod, true) == 0 && string.Compare("CREATE_USERQUESTION", context.Request.QueryString["Action"], true) == 0)
            {
                string userQuestionStr = context.Request.Form["userQuestion"];
                if (string.IsNullOrWhiteSpace(userQuestionStr))
                {
                    context.Response.ContentType = _textResponse;
                    context.Response.Write(_failedResponse);
                    return;
                }

                if (context.Session[_userQuestion] == null)
                    context.Session[_userQuestion] = new List<UserQuestionTemp>();

                List<string> userQuestionList = userQuestionStr.Split(';').ToList();
                CreateUserQuestionInSession(userQuestionList, context);
                string jsonText = Newtonsoft.Json.JsonConvert.SerializeObject(context.Session[_userQuestion]);

                context.Response.ContentType = _jsonResponse;
                context.Response.Write(jsonText);
                return;
            }
        }

        private void CreateUserQuestionInSession(List<string> userQuestionList, HttpContext context)
        {
            List<UserQuestionTemp> userQuestionTempList = new List<UserQuestionTemp>();

            foreach (var userQuestion in userQuestionList)
            {
                List<string> questionID_AnswerNum_Answer_QuestionTypingList = userQuestion.Split('_').ToList();
                UserQuestionTemp newUserQuestionTemp = new UserQuestionTemp();

                string questionIDStr = questionID_AnswerNum_Answer_QuestionTypingList
                    .SingleOrDefault(item => Guid.TryParse(item, out Guid questionID));
                newUserQuestionTemp.QuestionID = Guid.Parse(questionIDStr);
                questionID_AnswerNum_Answer_QuestionTypingList.Remove(questionIDStr);

                string questionTyping = questionID_AnswerNum_Answer_QuestionTypingList
                    .SingleOrDefault(item => item
                    .Contains(SINGLE_SELECT)
                        || item.Contains(MULTIPLE_SELECT)
                        || item.Contains(TEXT));
                newUserQuestionTemp.QuestionTyping = questionTyping;
                questionID_AnswerNum_Answer_QuestionTypingList.Remove(questionTyping);

                string answerNumStr = questionID_AnswerNum_Answer_QuestionTypingList
                    .SingleOrDefault(item => int.TryParse(item, out int answerNum));
                newUserQuestionTemp.AnswerNum = int.Parse(answerNumStr);
                questionID_AnswerNum_Answer_QuestionTypingList.Remove(answerNumStr);

                newUserQuestionTemp.Answer = questionID_AnswerNum_Answer_QuestionTypingList.FirstOrDefault();

                userQuestionTempList.Add(newUserQuestionTemp);
            }

            context.Session[_userQuestion] = userQuestionTempList.ToList();
        }

        public class UserQuestionTemp
        {
            public Guid QuestionID { get; set; }
            public int AnswerNum { get; set; }
            public string Answer { get; set; }
            public string QuestionTyping { get; set; }
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