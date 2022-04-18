using DynamicQuestionnaire.DynamicQuestionnaire.ORM;
using DynamicQuestionnaire.Managers;
using DynamicQuestionnaire.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace DynamicQuestionnaire.API
{
    /// <summary>
    /// Summary description for QuestionnaireDetailDataHandler
    /// </summary>
    public class QuestionnaireDetailDataHandler : IHttpHandler, IRequiresSessionState
    {
        private string _textResponse = "text/plain";
        private string _jsonResponse = "application/json";
        private string _nullResponse = "NULL";
        private string _successedResponse = "SUCCESSED";
        private string _failedResponse = "FAILED";

        // Session name
        private string _isUpdateMode = "IsUpdateMode";
        private string _questionnaire = "Questionnaire";
        private string _questionList = "QuestionList";

        private QuestionnaireManager _questionnaireMgr = new QuestionnaireManager();
        private QuestionManager _questionMgr = new QuestionManager();

        public void ProcessRequest(HttpContext context)
        {
            if (string.Compare("POST", context.Request.HttpMethod, true) == 0 && string.Compare("GET_QUESTIONNAIRE", context.Request.QueryString["Action"], true) == 0)
            {
                string questionnaireIDStr = context.Request.Form["questionnaireID"];
                if (!Guid.TryParse(questionnaireIDStr, out Guid questionnaireID))
                {
                    context.Response.ContentType = _textResponse;
                    context.Response.Write(_failedResponse);
                    return;
                }

                if (context.Session[_questionnaire] == null)
                {
                    var questionnaire = this._questionnaireMgr.GetQuestionnaire(questionnaireID);
                    context.Session[_questionnaire] = questionnaire;
                    return;
                }

                return;
            }

            if (string.Compare("POST", context.Request.HttpMethod, true) == 0 && string.Compare("CREATE_QUESTIONNAIRE", context.Request.QueryString["Action"], true) == 0)
            {
                bool isFirstCreate = (bool)(context.Session[_questionnaire] == null);

                if (isFirstCreate)
                {
                    this.CreateQuestionnaireInSession(isFirstCreate, context);
                    return;
                }

                this.CreateQuestionnaireInSession(isFirstCreate, context);
                return;
            }
            
            if (string.Compare("POST", context.Request.HttpMethod, true) == 0 && string.Compare("UPDATE_QUESTIONNAIRE", context.Request.QueryString["Action"], true) == 0)
            {
                string updateState = this.UpdateQuestionnaireInSession(context);
                if (updateState == _failedResponse)
                {
                    context.Response.ContentType = _textResponse;
                    context.Response.Write(_failedResponse);
                    return;
                }

                return;
            }

            if (string.Compare("POST", context.Request.HttpMethod, true) == 0 && string.Compare("GET_QUESTIONLIST", context.Request.QueryString["Action"], true) == 0)
            {
                bool isUpdateMode = (bool)context.Session[_isUpdateMode];

                if (isUpdateMode)
                {
                    string questionnaireIDStr = context.Request.Form["questionnaireID"];
                    if (!Guid.TryParse(questionnaireIDStr, out Guid questionnaireID))
                    {
                        context.Response.ContentType = _textResponse;
                        context.Response.Write(_failedResponse);
                        return;
                    }

                    List<QuestionModel> questionModelList = context.Session[_questionList] as List<QuestionModel>;
                    if (questionModelList == null || questionModelList.Count == 0)
                        context.Session[_questionList] = new List<QuestionModel>();

                    var questionListInUpdateMode = this._questionMgr
                        .GetQuestionListOfQuestionnaire(questionnaireID);
                    var questionModelListInUpdateMode = this
                        .CreateQuestionModelListForShowTheyInUpdateMode(questionListInUpdateMode);
                    context.Session[_questionList] = questionModelListInUpdateMode.ToList();
                    string jsonText = Newtonsoft.Json.JsonConvert.SerializeObject(questionModelListInUpdateMode);

                    context.Response.ContentType = _jsonResponse;
                    context.Response.Write(jsonText);
                    return;
                }

                List<Question> questionList = context.Session[_questionList] as List<Question>;
                if (questionList == null || questionList.Count == 0)
                {
                    context.Session[_questionList] = new List<Question>();

                    context.Response.ContentType = _textResponse;
                    context.Response.Write(_nullResponse);
                    return;
                }

                return;
            }

            if (string.Compare("POST", context.Request.HttpMethod, true) == 0 && string.Compare("CREATE_QUESTION", context.Request.QueryString["Action"], true) == 0)
            {
                bool isUpdateMode = (bool)context.Session[_isUpdateMode];

                if (isUpdateMode)
                {
                    List<QuestionModel> questionModelList = context.Session[_questionList] as List<QuestionModel>;
                    if (questionModelList == null || questionModelList.Count == 0)
                        context.Session[_questionList] = new List<QuestionModel>();

                    this.CreateQuestionInSession(isUpdateMode, context);
                    string modelJsonText = Newtonsoft.Json.JsonConvert.SerializeObject(context.Session[_questionList]);

                    context.Response.ContentType = _jsonResponse;
                    context.Response.Write(modelJsonText);
                    return;
                }

                if (context.Session[_questionList] == null)
                    context.Session[_questionList] = new List<Question>();

                this.CreateQuestionInSession(isUpdateMode, context);
                string jsonText = Newtonsoft.Json.JsonConvert.SerializeObject(context.Session[_questionList]);

                context.Response.ContentType = _jsonResponse;
                context.Response.Write(jsonText);
                return;
            }

            if (string.Compare("POST", context.Request.HttpMethod, true) == 0 && string.Compare("DELETE_QUESTIONLIST", context.Request.QueryString["Action"], true) == 0)
            {
                bool isUpdateMode = (bool)context.Session[_isUpdateMode];

                if (isUpdateMode)
                {
                    if (context.Session[_questionList] == null)
                    {
                        context.Session[_questionList] = new List<QuestionModel>();

                        context.Response.ContentType = _textResponse;
                        context.Response.Write(_nullResponse);
                        return;
                    }

                    List<QuestionModel> questionModelList = context.Session[_questionList] as List<QuestionModel>;
                    if (questionModelList.Count == 0)
                    {
                        context.Response.ContentType = _textResponse;
                        context.Response.Write(_nullResponse);
                        return;
                    }
                }
                else
                {
                    if (context.Session[_questionList] == null)
                    {
                        context.Session[_questionList] = new List<Question>();

                        context.Response.ContentType = _textResponse;
                        context.Response.Write(_nullResponse);
                        return;
                    }

                    List<Question> questionList = context.Session[_questionList] as List<Question>;
                    if (questionList.Count == 0)
                    {
                        context.Response.ContentType = _textResponse;
                        context.Response.Write(_nullResponse);
                        return;
                    }
                }

                string checkedQuestionIDList = context.Request.Form["checkedQuestionIDList"];
                if (string.IsNullOrEmpty(checkedQuestionIDList))
                {
                    context.Response.ContentType = _textResponse;
                    context.Response.Write(_failedResponse);

                    return;
                }

                string[] checkedBookIDStrArr = checkedQuestionIDList.Split(',');
                Guid[] checkedBookIDGuidArr = checkedBookIDStrArr.Select(item => Guid.Parse(item)).ToArray();
                this.DeleteQuestionListInSession(isUpdateMode, checkedBookIDGuidArr, context);
                string jsonText = Newtonsoft.Json.JsonConvert.SerializeObject(context.Session[_questionList]);

                context.Response.ContentType = _jsonResponse;
                context.Response.Write(jsonText);
                return;
            }

            if (string.Compare("POST", context.Request.HttpMethod, true) == 0 && string.Compare("SHOW_TO_UPDATE_QUESTION", context.Request.QueryString["Action"], true) == 0)
            {
                string clickedQuestionID = context.Request.Form["clickedQuestionID"];
                if (!Guid.TryParse(clickedQuestionID, out Guid questionID))
                {
                    context.Response.ContentType = _textResponse;
                    context.Response.Write(_failedResponse);
                    return;
                }

                List<QuestionModel> questionModelList = context.Session[_questionList] as List<QuestionModel>;
                if (questionModelList == null || questionModelList.Count == 0)
                {
                    context.Session[_questionList] = new List<QuestionModel>();

                    context.Response.ContentType = _textResponse;
                    context.Response.Write(_nullResponse);
                    return;
                }

                var toUpdateQuestionModel = questionModelList
                    .SingleOrDefault(questionModel => questionModel.QuestionID
                    == questionID);
                if (toUpdateQuestionModel == null)
                {
                    context.Response.ContentType = _textResponse;
                    context.Response.Write(_nullResponse);
                    return;
                }
                string jsonText = Newtonsoft.Json.JsonConvert.SerializeObject(toUpdateQuestionModel);

                context.Response.ContentType = _jsonResponse;
                context.Response.Write(jsonText);
                return;
            }

            if (string.Compare("POST", context.Request.HttpMethod, true) == 0 && string.Compare("UPDATE_QUESTION", context.Request.QueryString["Action"], true) == 0)
            {
                string clickedQuestionID = context.Request.Form["clickedQuestionID"];
                if (!Guid.TryParse(clickedQuestionID, out Guid questionID))
                {
                    context.Response.ContentType = _textResponse;
                    context.Response.Write(_failedResponse);
                    return;
                }

                this.UpdateQuestionInSession(questionID, context);
                string jsonText = Newtonsoft.Json.JsonConvert.SerializeObject(context.Session[_questionList]);

                context.Response.ContentType = _jsonResponse;
                context.Response.Write(jsonText);
                return;
            }
        }

        private void CreateQuestionnaireInSession(bool isFirstCreate, HttpContext context)
        {
            string caption = context.Request.Form["caption"];
            string description = context.Request.Form["description"];
            string startDateStr = context.Request.Form["startDate"];
            string endDateStr = context.Request.Form["endDate"];
            string isEnableStr = context.Request.Form["isEnable"];

            if (isFirstCreate)
            {
                Questionnaire newQuestionnaire = new Questionnaire()
                {
                    QuestionnaireID = Guid.NewGuid(),
                    Caption = caption.Trim(),
                    Description = description.Trim(),
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now,
                };

                this.SameLogicOfCreateQuestionnaireInSession(
                    startDateStr,
                    endDateStr,
                    isEnableStr,
                    newQuestionnaire,
                    context
                );
                return;
            }

            Questionnaire questionnaire = context.Session[_questionnaire] as Questionnaire;
            questionnaire.Caption = caption.Trim();
            questionnaire.Description = description.Trim();
            questionnaire.CreateDate = DateTime.Now;
            questionnaire.UpdateDate = DateTime.Now;

            this.SameLogicOfCreateQuestionnaireInSession(
                startDateStr,
                endDateStr,
                isEnableStr,
                questionnaire,
                context
            );
        }

        private void SameLogicOfCreateQuestionnaireInSession(
            string startDateStr,
            string endDateStr,
            string isEnableStr,
            Questionnaire questionnaire,
            HttpContext context
            )
        {
            if (DateTime.TryParse(startDateStr, out DateTime startDate))
                questionnaire.StartDate = startDate;

            if (DateTime.TryParse(endDateStr, out DateTime endDate))
                questionnaire.EndDate = endDate;

            if (bool.TryParse(isEnableStr, out bool isEnable))
                questionnaire.IsEnable = isEnable;
            else
                questionnaire.IsEnable = true;

            context.Session[_questionnaire] = questionnaire;
        }

        private string UpdateQuestionnaireInSession(HttpContext context)
        {
            string caption = context.Request.Form["caption"];
            string description = context.Request.Form["description"];
            string startDateStr = context.Request.Form["startDate"];
            string endDateStr = context.Request.Form["endDate"];
            string isEnableStr = context.Request.Form["isEnable"];
            Questionnaire questionnaire = context.Session[_questionnaire] as Questionnaire;

            if (DateTime.TryParse(startDateStr, out DateTime startDate) 
                && bool.TryParse(isEnableStr, out bool isEnable))
            {
                if (caption != questionnaire.Caption 
                    || description != questionnaire.Description 
                    || startDate != questionnaire.StartDate 
                    || isEnable != questionnaire.IsEnable)
                {
                    questionnaire.Caption = caption.Trim();
                    questionnaire.Description = description.Trim();
                    questionnaire.StartDate = startDate;
                    questionnaire.IsEnable = isEnable;
                    if (DateTime.TryParse(endDateStr, out DateTime endDate))
                        if (endDate != questionnaire.EndDate)
                            questionnaire.EndDate = endDate;

                    questionnaire.UpdateDate = DateTime.Now;
                    context.Session[_questionnaire] = questionnaire;
                    return _successedResponse;
                }

                return _nullResponse;
            }

            return _failedResponse;
        }

        private void CreateQuestionInSession(bool isUpdateMode, HttpContext context)
        {
            string questionName = context.Request.Form["questionName"];
            string questionAnswer = context.Request.Form["questionAnswer"];
            string questionCategory = context.Request.Form["questionCategory"];
            string questionTyping = context.Request.Form["questionTyping"];
            string questionRequiredStr = context.Request.Form["questionRequired"];
            Questionnaire questionnaire = context.Session[_questionnaire] as Questionnaire;

            if (isUpdateMode)
            {
                List<QuestionModel> questionModelList = context.Session[_questionList] as List<QuestionModel>;
                QuestionModel newQuestionModel = new QuestionModel()
                {
                    QuestionID = Guid.NewGuid(),
                    QuestionnaireID = questionnaire.QuestionnaireID,
                    QuestionCategory = questionCategory,
                    QuestionTyping = questionTyping,
                    QuestionName = questionName.Trim(),
                    QuestionAnswer = questionAnswer.Trim(),
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now,
                    IsCreated = true,
                    IsUpdated = false,
                    IsDeleted = false,
                };
                if (bool.TryParse(questionRequiredStr, out bool questionRequiredInUpdateMode))
                    newQuestionModel.QuestionRequired = questionRequiredInUpdateMode;
                else
                    newQuestionModel.QuestionRequired = false;

                questionModelList.Add(newQuestionModel);
                context.Session[_questionList] = questionModelList
                    .OrderByDescending(item => item.UpdateDate)
                    .ToList();
                return;
            }

            List<Question> questionList = context.Session[_questionList] as List<Question>;
            Question newQuestion = new Question() 
            {
                QuestionID = Guid.NewGuid(),
                QuestionnaireID = questionnaire.QuestionnaireID,
                QuestionCategory = questionCategory,
                QuestionTyping = questionTyping,
                QuestionName = questionName.Trim(),
                QuestionAnswer = questionAnswer.Trim(),
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now,
            };
            if (bool.TryParse(questionRequiredStr, out bool questionRequired))
                newQuestion.QuestionRequired = questionRequired;
            else
                newQuestion.QuestionRequired = false;

            questionList.Add(newQuestion);
            context.Session[_questionList] = questionList;
        }

        private void DeleteQuestionListInSession(
            bool isUpdateMode,
            Guid[] questionIDArr,
            HttpContext context
            )
        {
            if (isUpdateMode)
            {
                List<QuestionModel> questionModelList = context.Session[_questionList] as List<QuestionModel>;
                var toDeleteQuestionModelList = questionIDArr
                    .Select(questionID => questionModelList
                    .Where(questionModel => questionModel.QuestionID == questionID)
                    .FirstOrDefault());
                foreach (var questionItem in toDeleteQuestionModelList)
                    questionItem.IsDeleted = true;

                context.Session[_questionList] = questionModelList.ToList();
                return;
            }

            List<Question> questionList = context.Session[_questionList] as List<Question>;
            var toDeleteQuestionList = questionIDArr
                .Select(questionID => questionList
                .Where(question => question.QuestionID == questionID)
                .FirstOrDefault());
            foreach (var questionItem in toDeleteQuestionList)
                questionList.Remove(questionItem);

            context.Session[_questionList] = questionList;
        }

        private void UpdateQuestionInSession(Guid questionID, HttpContext context)
        {
            string questionName = context.Request.Form["questionName"];
            string questionAnswer = context.Request.Form["questionAnswer"];
            string questionCategory = context.Request.Form["questionCategory"];
            string questionTyping = context.Request.Form["questionTyping"];
            string questionRequiredStr = context.Request.Form["questionRequired"];
            List<QuestionModel> questionModelList = context.Session[_questionList] as List<QuestionModel>;

            var toEditQuestionModel = questionModelList
                .SingleOrDefault(questionModel => questionModel.QuestionID == questionID);
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

            context.Session[_questionList] = questionModelList
                .OrderByDescending(item => item.UpdateDate)
                .ToList();
        }

        private List<QuestionModel> CreateQuestionModelListForShowTheyInUpdateMode(List<Question> questionList)
        {
            List<QuestionModel> questionModelList = new List<QuestionModel>();

            foreach (var question in questionList)
            {
                QuestionModel questionModel = new QuestionModel()
                {
                    QuestionID = question.QuestionID,
                    QuestionnaireID = question.QuestionnaireID,
                    QuestionCategory = question.QuestionCategory,
                    QuestionTyping = question.QuestionTyping,
                    QuestionName = question.QuestionName,
                    QuestionRequired = question.QuestionRequired,
                    QuestionAnswer = question.QuestionAnswer,
                    CreateDate = question.CreateDate,
                    UpdateDate = question.UpdateDate,
                    IsCreated = false,
                    IsUpdated = false,
                    IsDeleted = false,
                };

                questionModelList.Add(questionModel);
            }

            return questionModelList;
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