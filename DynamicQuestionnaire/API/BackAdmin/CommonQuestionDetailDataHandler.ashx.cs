using DynamicQuestionnaire.DynamicQuestionnaire.ORM;
using DynamicQuestionnaire.Managers;
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
        private string _failedResponse = "FAILED";

        // Session name
        private string _isUpdateMode = "IsUpdateMode";
        private string _commonQuestion = "CommonQuestion";
        private string _questionListOfCommonQuestion = "QuestionListOfCommonQuestion";

        private CommonQuestionManager _commonQuestionMgr = new CommonQuestionManager();
        private QuestionManager _questionMgr = new QuestionManager();

        public void ProcessRequest(HttpContext context)
        {
            if (string.Compare("POST", context.Request.HttpMethod, true) == 0 && string.Compare("GET_COMMONQUESTION", context.Request.QueryString["Action"], true) == 0)
            {
                string commonQuestionIDStr = context.Request.Form["commonQuestionID"];
                if (!Guid.TryParse(commonQuestionIDStr, out Guid commonQuestionID))
                {
                    context.Response.ContentType = _textResponse;
                    context.Response.Write(_failedResponse);
                    return;
                }

                if (context.Session[_commonQuestion] == null)
                {
                    var commonQuestion = this._commonQuestionMgr.GetCommonQuestion(commonQuestionID);
                    context.Session[_commonQuestion] = commonQuestion;
                    return;
                }

                return;
            }

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

            if (string.Compare("POST", context.Request.HttpMethod, true) == 0 && string.Compare("UPDATE_COMMONQUESTION", context.Request.QueryString["Action"], true) == 0)
            {
                bool isUpdated = this.UpdateCommonQuestionInSession(context);
                if (isUpdated)
                {
                    string updatedJsonText = 
                        Newtonsoft
                        .Json
                        .JsonConvert
                        .SerializeObject(context.Session[_commonQuestion]);

                    context.Response.ContentType = _jsonResponse;
                    context.Response.Write(updatedJsonText);
                    return;
                }

                string notUpdatedJsonText =
                        Newtonsoft
                        .Json
                        .JsonConvert
                        .SerializeObject(context.Session[_commonQuestion]);

                context.Response.ContentType = _jsonResponse;
                context.Response.Write(notUpdatedJsonText);
                return;
            }

            if (string.Compare("POST", context.Request.HttpMethod, true) == 0 && string.Compare("GET_QUESTIONLIST_OF_COMMONQUESTION", context.Request.QueryString["Action"], true) == 0)
            {
                bool isUpdateMode = (bool)context.Session[_isUpdateMode];

                if (isUpdateMode)
                {
                    string commonQuestionIDStr = context.Request.Form["commonQuestionID"];
                    if (!Guid.TryParse(commonQuestionIDStr, out Guid commonQuestionID))
                    {
                        context.Response.ContentType = _textResponse;
                        context.Response.Write(_failedResponse);
                        return;
                    }

                    List<QuestionModel> questionModelListOfCommonQuestion = 
                        context.Session[_questionListOfCommonQuestion] as List<QuestionModel>;
                    if (questionModelListOfCommonQuestion == null 
                        || questionModelListOfCommonQuestion.Count == 0)
                        context.Session[_questionListOfCommonQuestion] = new List<QuestionModel>();

                    var questionListOfCommonQuestionInUpdateMode = 
                        this._questionMgr.GetQuestionListOfCommonQuestion(commonQuestionID);
                    var questionModelListOfCommonQuestionInUpdateMode = 
                        this._questionMgr.BuildQuestionModelList(
                            true,
                            false,
                            questionListOfCommonQuestionInUpdateMode
                            );
                    context.Session[_questionListOfCommonQuestion] = questionModelListOfCommonQuestionInUpdateMode.ToList();
                    string jsonTextInUpdateMode = 
                        Newtonsoft
                        .Json
                        .JsonConvert
                        .SerializeObject(questionModelListOfCommonQuestionInUpdateMode);

                    context.Response.ContentType = _jsonResponse;
                    context.Response.Write(jsonTextInUpdateMode);
                    return;
                }

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

                if (isUpdateMode)
                {
                    List<QuestionModel> questionModelListOfCommonQuestion =
                        context.Session[_questionListOfCommonQuestion] as List<QuestionModel>;
                    if (questionModelListOfCommonQuestion == null 
                        || questionModelListOfCommonQuestion.Count == 0)
                        context.Session[_questionListOfCommonQuestion] = new List<QuestionModel>();

                    this.CreateQuestionOfCommonQuestionInSession(isUpdateMode, false, context);
                    string questionModelJsonText = 
                        Newtonsoft
                        .Json
                        .JsonConvert
                        .SerializeObject(context.Session[_questionListOfCommonQuestion]);

                    context.Response.ContentType = _jsonResponse;
                    context.Response.Write(questionModelJsonText);
                }
                else
                {
                    List<Question> questionListOfCommonQuestion =
                        context.Session[_questionListOfCommonQuestion] as List<Question>;
                    bool isFirstCreate = 
                        (questionListOfCommonQuestion == null 
                        || questionListOfCommonQuestion.Count == 0);
                    if (isFirstCreate)
                    {
                        context.Session[_questionListOfCommonQuestion] = new List<Question>();
                        this.CreateQuestionOfCommonQuestionInSession(isUpdateMode, isFirstCreate, context);
                        string firstJsonText = 
                            Newtonsoft
                            .Json
                            .JsonConvert
                            .SerializeObject(context.Session[_questionListOfCommonQuestion]);

                        context.Response.ContentType = _jsonResponse;
                        context.Response.Write(firstJsonText);
                        return;
                    }

                    this.CreateQuestionOfCommonQuestionInSession(isUpdateMode, isFirstCreate, context);
                    string afterFirstJsonText = 
                        Newtonsoft
                        .Json
                        .JsonConvert
                        .SerializeObject(context.Session[_questionListOfCommonQuestion]);

                    context.Response.ContentType = _jsonResponse;
                    context.Response.Write(afterFirstJsonText);
                }

                return;
            }

            if (string.Compare("POST", context.Request.HttpMethod, true) == 0 && string.Compare("DELETE_QUESTIONLIST_OF_COMMONQUESTION", context.Request.QueryString["Action"], true) == 0)
            {
                bool isUpdateMode = (bool)context.Session[_isUpdateMode];

                if (isUpdateMode)
                {
                    List<QuestionModel> questionModelListOfCommonQuestion = 
                        context.Session[_questionListOfCommonQuestion] as List<QuestionModel>;
                    if (questionModelListOfCommonQuestion == null 
                        || questionModelListOfCommonQuestion.Count == 0)
                    {
                        context.Response.ContentType = _textResponse;
                        context.Response.Write(_nullResponse);
                        return;
                    }
                }
                else
                {
                    List<Question> questionListOfCommonQuestion = 
                        context.Session[_questionListOfCommonQuestion] as List<Question>;
                    if (questionListOfCommonQuestion == null 
                        || questionListOfCommonQuestion.Count == 0)
                    {
                        context.Response.ContentType = _textResponse;
                        context.Response.Write(_nullResponse);
                        return;
                    }
                }

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
                object[] leftQuestionListOfCommonQuestionAndIsUpdateModeArr = 
                    { context.Session[_questionListOfCommonQuestion], isUpdateMode };
                string jsonText = 
                    Newtonsoft
                    .Json
                    .JsonConvert
                    .SerializeObject(leftQuestionListOfCommonQuestionAndIsUpdateModeArr);

                context.Response.ContentType = _jsonResponse;
                context.Response.Write(jsonText);
                return;
            }

            if (string.Compare("POST", context.Request.HttpMethod, true) == 0 && string.Compare("SHOW_TO_UPDATE_QUESTION_OF_COMMONQUESTION", 
                context.Request.QueryString["Action"], true) == 0)
            {
                string clickedQuestionIDOfCommonQuestion = 
                    context.Request.Form["clickedQuestionIDOfCommonQuestion"];
                if (!Guid.TryParse(clickedQuestionIDOfCommonQuestion, 
                    out Guid questionIDOfCommonQuestion))
                {
                    context.Response.ContentType = _textResponse;
                    context.Response.Write(_failedResponse);
                    return;
                }

                List<QuestionModel> questionModelListOfCommonQuestion = 
                    context.Session[_questionListOfCommonQuestion] as List<QuestionModel>;
                if (questionModelListOfCommonQuestion == null 
                    || questionModelListOfCommonQuestion.Count == 0)
                {
                    context.Session[_questionListOfCommonQuestion] = new List<QuestionModel>();

                    context.Response.ContentType = _textResponse;
                    context.Response.Write(_nullResponse);
                    return;
                }

                var toUpdateQuestionModelOfCommonQuestion = 
                    questionModelListOfCommonQuestion
                    .SingleOrDefault(questionModel => questionModel.QuestionID
                    == questionIDOfCommonQuestion);
                if (toUpdateQuestionModelOfCommonQuestion == null)
                {
                    context.Response.ContentType = _textResponse;
                    context.Response.Write(_nullResponse);
                    return;
                }
                string jsonText = 
                    Newtonsoft
                    .Json
                    .JsonConvert
                    .SerializeObject(toUpdateQuestionModelOfCommonQuestion);

                context.Response.ContentType = _jsonResponse;
                context.Response.Write(jsonText);
                return;
            }

            if (string.Compare("POST", context.Request.HttpMethod, true) == 0 && string.Compare("UPDATE_QUESTION_OF_COMMONQUESTION", context.Request.QueryString["Action"], true) == 0)
            {
                string clickedQuestionIDOfCommonQuestion = 
                    context.Request.Form["clickedQuestionIDOfCommonQuestion"];
                if (!Guid.TryParse(clickedQuestionIDOfCommonQuestion, 
                    out Guid questionIDOfCommonQuestion))
                {
                    context.Response.ContentType = _textResponse;
                    context.Response.Write(_failedResponse);
                    return;
                }

                this.UpdateQuestionOfCommonQuestionInSession(questionIDOfCommonQuestion, context);
                string jsonText = 
                    Newtonsoft
                    .Json
                    .JsonConvert
                    .SerializeObject(context.Session[_questionListOfCommonQuestion]);

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

        private bool UpdateCommonQuestionInSession(HttpContext context)
        {
            string commonQuestionName = context.Request.Form["commonQuestionName"];
            CommonQuestion commonQuestion = context.Session[_commonQuestion] as CommonQuestion;

            if (commonQuestionName != commonQuestion.CommonQuestionName)
            {
                commonQuestion.CommonQuestionName = commonQuestionName.Trim();
                commonQuestion.UpdateDate = DateTime.Now;

                context.Session[_commonQuestion] = commonQuestion;
                return true;
            }

            return false;
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

            if (isUpdateMode)
            {
                List<QuestionModel> questionModelListOfCommonQuestion = 
                    context.Session[_questionListOfCommonQuestion] as List<QuestionModel>;
                Guid questionModelItsQuestionnaireID = 
                    questionModelListOfCommonQuestion
                    .Select(item => item.QuestionnaireID)
                    .FirstOrDefault();
                QuestionModel newQuestionModel = new QuestionModel()
                {
                    QuestionID = Guid.NewGuid(),
                    QuestionnaireID = questionModelItsQuestionnaireID,
                    QuestionCategory = questionCategory,
                    QuestionTyping = questionTyping,
                    QuestionName = questionName.Trim(),
                    QuestionAnswer = questionAnswer.Trim(),
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now,
                    CommonQuestionID = commonQuestion.CommonQuestionID,
                    IsCreated = true,
                    IsUpdated = false,
                    IsDeleted = false,
                };
                if (bool.TryParse(questionRequiredStr, out bool questionRequiredInUpdateMode))
                    newQuestionModel.QuestionRequired = questionRequiredInUpdateMode;
                else
                    newQuestionModel.QuestionRequired = false;

                questionModelListOfCommonQuestion.Add(newQuestionModel);
                context.Session[_questionListOfCommonQuestion] = 
                    questionModelListOfCommonQuestion
                    .OrderByDescending(item => item.UpdateDate)
                    .ToList();
            }
            else
            {
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
            }
        }

        private void DeleteQuestionListOfCommonQuestionInSession(
            bool isUpdateMode,
            Guid[] questionIDOfCommonQuestionArr,
            HttpContext context
            )
        {
            if (isUpdateMode)
            {
                List<QuestionModel> questionModelListOfCommonQuestion = 
                    context.Session[_questionListOfCommonQuestion] as List<QuestionModel>;
                var toDeleteQuestionModelListOfCommonQuestion = 
                    questionIDOfCommonQuestionArr
                    .Select(questionIDOfCommonQuestion => questionModelListOfCommonQuestion
                    .Where(questionModel => questionModel.QuestionID 
                    == questionIDOfCommonQuestion)
                    .FirstOrDefault());
                foreach (var questionOfCommonQuestion in toDeleteQuestionModelListOfCommonQuestion)
                    questionOfCommonQuestion.IsDeleted = true;

                context.Session[_questionListOfCommonQuestion] = questionModelListOfCommonQuestion.ToList();
                return;
            }

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

        private void UpdateQuestionOfCommonQuestionInSession(
            Guid questionIDOfCommonQuestion, 
            HttpContext context
            )
        {
            string questionName = context.Request.Form["questionName"];
            string questionAnswer = context.Request.Form["questionAnswer"];
            string questionCategory = context.Request.Form["questionCategory"];
            string questionTyping = context.Request.Form["questionTyping"];
            string questionRequiredStr = context.Request.Form["questionRequired"];
            List<QuestionModel> questionModelListOfCommonQuestion = 
                context.Session[_questionListOfCommonQuestion] as List<QuestionModel>;

            var toEditQuestionModel = 
                questionModelListOfCommonQuestion
                .SingleOrDefault(questionModel => questionModel.QuestionID 
                == questionIDOfCommonQuestion);
            toEditQuestionModel.QuestionCategory = questionCategory;
            toEditQuestionModel.QuestionTyping = questionTyping;
            toEditQuestionModel.QuestionName = questionName.Trim();
            toEditQuestionModel.QuestionAnswer = questionAnswer.Trim();
            toEditQuestionModel.UpdateDate = DateTime.Now;
            toEditQuestionModel.IsUpdated = true;
            if (bool.TryParse(questionRequiredStr, out bool questionRequired))
                toEditQuestionModel.QuestionRequired = questionRequired;
            else
                toEditQuestionModel.QuestionRequired = false;

            context.Session[_questionListOfCommonQuestion] = 
                questionModelListOfCommonQuestion
                .OrderByDescending(item => item.UpdateDate)
                .ToList();
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