using DynamicQuestionnaire.DynamicQuestionnaire.ORM;
using DynamicQuestionnaire.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace DynamicQuestionnaire.API.BackAdmin
{
    /// <summary>
    /// Summary description for CommonQuestionDetailDataHandler
    /// </summary>
    public class CommonQuestionDetailDataHandler : IHttpHandler, IRequiresSessionState
    {
        private string _textResponse = "text/plain";
        private string _jsonResponse = "application/json";
        private string _nullResponse = "NULL";
        //private string _successedResponse = "SUCCESSED";
        private string _failedResponse = "FAILED";

        // Session name
        private string _isUpdateMode = "IsUpdateMode";
        private string _commonQuestion = "CommonQuestion";
        private string _questionListOfCommonQuestion = "QuestionListOfCommonQuestion";

        public void ProcessRequest(HttpContext context)
        {
            if (string.Compare("POST", context.Request.HttpMethod, true) == 0 && string.Compare("CREATE_COMMONQUESTION", context.Request.QueryString["Action"], true) == 0)
            {
                bool isFirstCreate = (context.Session[_commonQuestion] == null);

                if (isFirstCreate)
                {
                    this.CreateCommonQuestionInSession(isFirstCreate, context);
                    return;
                }

                this.CreateCommonQuestionInSession(isFirstCreate, context);
                return;
            }

            if (string.Compare("POST", context.Request.HttpMethod, true) == 0 && string.Compare("GET_QUESTIONLIST_OF_COMMONQUESTION", context.Request.QueryString["Action"], true) == 0)
            {
                //bool isUpdateMode = (bool)context.Session[_isUpdateMode];

                //if (isUpdateMode)
                //{
                //    string questionnaireIDStr = context.Request.Form["questionnaireID"];
                //    if (!Guid.TryParse(questionnaireIDStr, out Guid questionnaireID))
                //    {
                //        context.Response.ContentType = _textResponse;
                //        context.Response.Write(_failedResponse);
                //        return;
                //    }

                //    List<QuestionModel> questionModelList = context.Session[_questionList] as List<QuestionModel>;
                //    if (questionModelList == null || questionModelList.Count == 0)
                //        context.Session[_questionList] = new List<QuestionModel>();

                //    var questionListInUpdateMode = this._questionMgr
                //        .GetQuestionListOfQuestionnaire(questionnaireID);
                //    var questionModelListInUpdateMode = this
                //        .BuildQuestionModelList(questionListInUpdateMode);
                //    context.Session[_questionList] = questionModelListInUpdateMode.ToList();
                //    string jsonText = Newtonsoft.Json.JsonConvert.SerializeObject(questionModelListInUpdateMode);

                //    context.Response.ContentType = _jsonResponse;
                //    context.Response.Write(jsonText);
                //    return;
                //}

                List<Question> questionListOfCommonQuestion = 
                    context.Session[_questionListOfCommonQuestion] as List<Question>;
                if (questionListOfCommonQuestion == null || questionListOfCommonQuestion.Count == 0)
                {
                    context.Session[_questionListOfCommonQuestion] = new List<Question>();

                    context.Response.ContentType = _textResponse;
                    context.Response.Write(_nullResponse);
                    return;
                }

                string jsonText = Newtonsoft.Json.JsonConvert.SerializeObject(context.Session[_questionListOfCommonQuestion]);

                context.Response.ContentType = _jsonResponse;
                context.Response.Write(jsonText);
                return;
            }

            if (string.Compare("POST", context.Request.HttpMethod, true) == 0 && string.Compare("CREATE_QUESTION_OF_COMMONQUESTION", context.Request.QueryString["Action"], true) == 0)
            {
                bool isUpdateMode = (bool)context.Session[_isUpdateMode];

                //if (isUpdateMode)
                //{
                //    List<QuestionModel> questionModelList = 
                //        context.Session[_questionListOfCommonQuestion] as List<QuestionModel>;
                //    if (questionModelList == null || questionModelList.Count == 0)
                //        context.Session[_questionListOfCommonQuestion] = new List<QuestionModel>();

                //    this.CreateQuestionOfCommonQuestionInSession(isUpdateMode, context);
                //    string modelJsonText = Newtonsoft.Json.JsonConvert.SerializeObject(context.Session[_questionListOfCommonQuestion]);

                //    context.Response.ContentType = _jsonResponse;
                //    context.Response.Write(modelJsonText);
                //    return;
                //}

                List<Question> questionListOfCommonQuestion =
                    context.Session[_questionListOfCommonQuestion] as List<Question>;
                bool isFirstCreate = 
                    (questionListOfCommonQuestion == null || questionListOfCommonQuestion.Count == 0);
                if (isFirstCreate)
                {
                    context.Session[_questionListOfCommonQuestion] = new List<Question>();
                    this.CreateQuestionOfCommonQuestionInSession(isUpdateMode, isFirstCreate, context);
                    string jsonText = Newtonsoft.Json.JsonConvert.SerializeObject(context.Session[_questionListOfCommonQuestion]);

                    context.Response.ContentType = _jsonResponse;
                    context.Response.Write(jsonText);
                    return;
                }

                this.CreateQuestionOfCommonQuestionInSession(isUpdateMode, isFirstCreate, context);
                string jsonText2 = Newtonsoft.Json.JsonConvert.SerializeObject(context.Session[_questionListOfCommonQuestion]);

                context.Response.ContentType = _jsonResponse;
                context.Response.Write(jsonText2);
                return;
            }

            if (string.Compare("POST", context.Request.HttpMethod, true) == 0 && string.Compare("DELETE_QUESTIONLIST_OF_COMMONQUESTION", context.Request.QueryString["Action"], true) == 0)
            {
                bool isUpdateMode = (bool)context.Session[_isUpdateMode];

                //if (isUpdateMode)
                //{
                //    if (context.Session[_questionListOfCommonQuestion] == null)
                //    {
                //        context.Session[_questionListOfCommonQuestion] = new List<QuestionModel>();

                //        context.Response.ContentType = _textResponse;
                //        context.Response.Write(_nullResponse);
                //        return;
                //    }

                //    List<QuestionModel> questionModelList = context.Session[_questionListOfCommonQuestion] as List<QuestionModel>;
                //    if (questionModelList.Count == 0)
                //    {
                //        context.Response.ContentType = _textResponse;
                //        context.Response.Write(_nullResponse);
                //        return;
                //    }
                //}
                //else
                //{
                    if (context.Session[_questionListOfCommonQuestion] == null)
                    {
                        context.Session[_questionListOfCommonQuestion] = new List<Question>();

                        context.Response.ContentType = _textResponse;
                        context.Response.Write(_nullResponse);
                        return;
                    }

                    List<Question> questionListOfCommonQuestion = 
                        context.Session[_questionListOfCommonQuestion] as List<Question>;
                    if (questionListOfCommonQuestion.Count == 0)
                    {
                        context.Response.ContentType = _textResponse;
                        context.Response.Write(_nullResponse);
                        return;
                    }
                //}

                string checkedQuestionIDListOfCommonQuestion = 
                    context.Request.Form["checkedQuestionIDListOfCommonQuestion"];
                if (string.IsNullOrEmpty(checkedQuestionIDListOfCommonQuestion))
                {
                    context.Response.ContentType = _textResponse;
                    context.Response.Write(_failedResponse);

                    return;
                }

                string[] checkedQuestionIDStrArr = checkedQuestionIDListOfCommonQuestion.Split(',');
                Guid[] checkedQuestionIDGuidArr = 
                    checkedQuestionIDStrArr.Select(item => Guid.Parse(item)).ToArray();
                this.DeleteQuestionListOfCommonQuestionInSession(
                    isUpdateMode, 
                    checkedQuestionIDGuidArr, 
                    context
                    );
                string jsonText = Newtonsoft.Json.JsonConvert.SerializeObject(context.Session[_questionListOfCommonQuestion]);

                context.Response.ContentType = _jsonResponse;
                context.Response.Write(jsonText);
                return;
            }
        }

        private void CreateCommonQuestionInSession(bool isFirstCreate, HttpContext context)
        {
            string commonQuestionName = context.Request.Form["commonQuestionName"];

            if (isFirstCreate)
            {
                CommonQuestion newCommonQuestion = new CommonQuestion()
                {
                    CommonQuestionID = Guid.NewGuid(),
                    CommonQuestionName = commonQuestionName.Trim(),
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now,
                };

                context.Session[_commonQuestion] = newCommonQuestion;
                return;
            }

            CommonQuestion commonQuestion = context.Session[_commonQuestion] as CommonQuestion;
            commonQuestion.CommonQuestionName = commonQuestionName.Trim();
            commonQuestion.CreateDate = DateTime.Now;
            commonQuestion.UpdateDate = DateTime.Now;

            context.Session[_commonQuestion] = commonQuestion;
        }

        private void CreateQuestionOfCommonQuestionInSession(
            bool isUpdateMode, 
            bool isFirstCreate, 
            HttpContext context
            )
        {
            string questionName = context.Request.Form["questionName"];
            string questionAnswer = context.Request.Form["questionAnswer"];
            string questionCategory = context.Request.Form["questionCategory"];
            string questionTyping = context.Request.Form["questionTyping"];
            string questionRequiredStr = context.Request.Form["questionRequired"];
            CommonQuestion commonQuestion = context.Session[_commonQuestion] as CommonQuestion;

            //if (isUpdateMode)
            //{
            //    List<QuestionModel> questionModelList = context.Session[_questionListOfCommonQuestion] as List<QuestionModel>;
            //    QuestionModel newQuestionModel = new QuestionModel()
            //    {
            //        QuestionID = Guid.NewGuid(),
            //        QuestionnaireID = commonQuestion.QuestionnaireID,
            //        QuestionCategory = questionCategory,
            //        QuestionTyping = questionTyping,
            //        QuestionName = questionName.Trim(),
            //        QuestionAnswer = questionAnswer.Trim(),
            //        CreateDate = DateTime.Now,
            //        UpdateDate = DateTime.Now,
            //        IsCreated = true,
            //        IsUpdated = false,
            //        IsDeleted = false,
            //    };
            //    if (bool.TryParse(questionRequiredStr, out bool questionRequiredInUpdateMode))
            //        newQuestionModel.QuestionRequired = questionRequiredInUpdateMode;
            //    else
            //        newQuestionModel.QuestionRequired = false;

            //    questionModelList.Add(newQuestionModel);
            //    context.Session[_questionListOfCommonQuestion] = questionModelList
            //        .OrderByDescending(item => item.UpdateDate)
            //        .ToList();
            //    return;
            //}

            List<Question> questionListOfCommonQuestion =
                context.Session[_questionListOfCommonQuestion] as List<Question>;

            if (isFirstCreate)
            {
                Question newQuestionOfCommonQuestion = new Question()
                {
                    QuestionID = Guid.NewGuid(),
                    QuestionnaireID = Guid.NewGuid(),
                    QuestionCategory = questionCategory,
                    QuestionTyping = questionTyping,
                    QuestionName = questionName.Trim(),
                    QuestionAnswer = questionAnswer.Trim(),
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now,
                    CommonQuestionID = commonQuestion.CommonQuestionID,
                };
                if (bool.TryParse(questionRequiredStr, out bool questionRequired))
                    newQuestionOfCommonQuestion.QuestionRequired = questionRequired;
                else
                    newQuestionOfCommonQuestion.QuestionRequired = false;

                questionListOfCommonQuestion.Add(newQuestionOfCommonQuestion);
                context.Session[_questionListOfCommonQuestion] = questionListOfCommonQuestion;
                return;
            }

            var questionOfCommonQuestion = 
                questionListOfCommonQuestion.FirstOrDefault();
            Guid questionnaireIDOfQuestionOfCommonQuestion = questionOfCommonQuestion.QuestionnaireID;
            Question afterFirstNewQuestionOfCommonQuestion = new Question()
            {
                QuestionID = Guid.NewGuid(),
                QuestionnaireID = questionnaireIDOfQuestionOfCommonQuestion,
                QuestionCategory = questionCategory,
                QuestionTyping = questionTyping,
                QuestionName = questionName.Trim(),
                QuestionAnswer = questionAnswer.Trim(),
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now,
                CommonQuestionID = commonQuestion.CommonQuestionID,
            };
            if (bool.TryParse(questionRequiredStr, out bool questionRequired2))
                afterFirstNewQuestionOfCommonQuestion.QuestionRequired = questionRequired2;
            else
                afterFirstNewQuestionOfCommonQuestion.QuestionRequired = false;

            questionListOfCommonQuestion.Add(afterFirstNewQuestionOfCommonQuestion);
            context.Session[_questionListOfCommonQuestion] = questionListOfCommonQuestion;
            return;
        }

        private void DeleteQuestionListOfCommonQuestionInSession(
            bool isUpdateMode,
            Guid[] questionIDOfCommonQuestionArr,
            HttpContext context
            )
        {
            //if (isUpdateMode)
            //{
            //    List<QuestionModel> questionModelList = context.Session[_questionListOfCommonQuestion] as List<QuestionModel>;
            //    var toDeleteQuestionModelList = questionIDOfCommonQuestionArr
            //        .Select(questionID => questionModelList
            //        .Where(questionModel => questionModel.QuestionID == questionID)
            //        .FirstOrDefault());
            //    foreach (var questionItem in toDeleteQuestionModelList)
            //        questionItem.IsDeleted = true;

            //    context.Session[_questionListOfCommonQuestion] = questionModelList.ToList();
            //    return;
            //}

            List<Question> questionListOfCommonQuestion = 
                context.Session[_questionListOfCommonQuestion] as List<Question>;
            var toDeleteQuestionListOfCommonQuestion = questionIDOfCommonQuestionArr
                .Select(questionIDOfCommonQuestion => questionListOfCommonQuestion
                .Where(questionOfCommonQuestion => questionOfCommonQuestion.QuestionID 
                == questionIDOfCommonQuestion)
                .FirstOrDefault());
            foreach (var questionOfCommonQuestion in toDeleteQuestionListOfCommonQuestion)
                questionListOfCommonQuestion.Remove(questionOfCommonQuestion);

            context.Session[_questionListOfCommonQuestion] = questionListOfCommonQuestion;
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