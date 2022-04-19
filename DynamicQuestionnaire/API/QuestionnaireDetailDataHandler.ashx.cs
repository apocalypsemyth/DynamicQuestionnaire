using DynamicQuestionnaire.DynamicQuestionnaire.ORM;
using DynamicQuestionnaire.Models;
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
        private const string SINGLE_SELECT = "單選方塊";
        private const string MULTIPLE_SELECT = "複選方塊";
        private const string TEXT = "文字";

        // Session name
        private string _user = "User";
        private string _userAnswer = "UserAnswer";

        public void ProcessRequest(HttpContext context)
        {
            if (string.Compare("POST", context.Request.HttpMethod, true) == 0 && string.Compare("CREATE_USER", context.Request.QueryString["Action"], true) == 0)
            {
                bool isFirstCreate = (bool)(context.Session[_user] == null);

                if (isFirstCreate)
                {
                    this.CreateUserInSession(isFirstCreate, context);
                    return;
                }

                this.CreateUserInSession(isFirstCreate, context);
                return;
            }

            if (string.Compare("POST", context.Request.HttpMethod, true) == 0 && string.Compare("CREATE_USERANSWER", context.Request.QueryString["Action"], true) == 0)
            {
                string userAnswerStr = context.Request.Form["userAnswer"];
                List<string> userAnswerList = userAnswerStr.Split(';').ToList();
                CreateUserAnswerInSession(userAnswerList, context);

                return;
            }
        }

        private void CreateUserInSession(bool isFirstCreate, HttpContext context)
        {
            string userName = context.Request.Form["userName"];
            string phone = context.Request.Form["phone"];
            string email = context.Request.Form["email"];
            string ageStr = context.Request.Form["age"];
            string questionnaireIDStr = context.Request.Form["questionnaireID"];
            
            if (isFirstCreate)
            {
                User newUser = new User() 
                {
                    UserID = Guid.NewGuid(),
                    UserName = userName,
                    Phone = phone,
                    Email = email,
                    AnswerDate = DateTime.Now,
                };

                this.SameLogicOfCreateUserInSession(ageStr, questionnaireIDStr, newUser, context);
                return;
            }

            User user = context.Session[_user] as User;
            user.UserName = userName;
            user.Phone = phone;
            user.Email = email;
            user.AnswerDate = DateTime.Now;

            this.SameLogicOfCreateUserInSession(ageStr, questionnaireIDStr, user, context);
        }

        private void SameLogicOfCreateUserInSession(
            string ageStr, 
            string questionnaireIDStr, 
            User user, 
            HttpContext context
            )
        {
            if (int.TryParse(ageStr, out int age))
                user.Age = age;

            if (Guid.TryParse(questionnaireIDStr, out Guid questionnaireID))
                user.QuestionnaireID = questionnaireID;

            context.Session[_user] = user;
        }

        private void CreateUserAnswerInSession(List<string> userAnswerList, HttpContext context)
        {
            User user = context.Session[_user] as User;
            List<UserAnswerModel> userAnswerModelList = new List<UserAnswerModel>();

            foreach (var userQuestion in userAnswerList)
            {
                List<string> questionID_AnswerNum_Answer_QuestionTypingList = userQuestion.Split('_').ToList();

                UserAnswerModel newUserAnswerModel = new UserAnswerModel();
                newUserAnswerModel.UserID = user.UserID;

                string questionIDStr = questionID_AnswerNum_Answer_QuestionTypingList
                    .SingleOrDefault(item => Guid.TryParse(item, out Guid questionID));
                newUserAnswerModel.QuestionID = Guid.Parse(questionIDStr);
                questionID_AnswerNum_Answer_QuestionTypingList.Remove(questionIDStr);

                string questionTyping = questionID_AnswerNum_Answer_QuestionTypingList
                    .SingleOrDefault(item => item
                    .Contains(SINGLE_SELECT)
                        || item.Contains(MULTIPLE_SELECT)
                        || item.Contains(TEXT));
                newUserAnswerModel.QuestionTyping = questionTyping;
                questionID_AnswerNum_Answer_QuestionTypingList.Remove(questionTyping);

                string answerNumStr = questionID_AnswerNum_Answer_QuestionTypingList
                    .SingleOrDefault(item => int.TryParse(item, out int answerNum));
                newUserAnswerModel.AnswerNum = int.Parse(answerNumStr);
                questionID_AnswerNum_Answer_QuestionTypingList.Remove(answerNumStr);

                newUserAnswerModel.Answer = questionID_AnswerNum_Answer_QuestionTypingList.FirstOrDefault();

                userAnswerModelList.Add(newUserAnswerModel);
            }

            context.Session[_userAnswer] = userAnswerModelList.ToList();
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