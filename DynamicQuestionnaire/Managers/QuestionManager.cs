using DynamicQuestionnaire.DynamicQuestionnaire.ORM;
using DynamicQuestionnaire.Helpers;
using DynamicQuestionnaire.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynamicQuestionnaire.Managers
{
    public class QuestionManager
    {
        public List<Question> GetQuestionListOfQuestionnaire(Guid questionnaireID)
        {
            try
            {
                using (ContextModel contextModel = new ContextModel())
                {
                    var questionList = contextModel.Questions
                        .Where(question => question.QuestionnaireID == questionnaireID)
                        .OrderByDescending(item => item.UpdateDate)
                        .ToList();

                    return questionList;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("QuestionManager.GetQuestionListOfQuestionnaire", ex);
                throw;
            }
        }

        public List<Question> GetQuestionListOfCommonQuestion(Guid commonQuestionID)
        {
            try
            {
                using (ContextModel contextModel = new ContextModel())
                {
                    var questionList = contextModel.Questions
                        .Where(question => question.CommonQuestionID == commonQuestionID)
                        .OrderByDescending(item => item.UpdateDate)
                        .ToList();

                    return questionList;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("QuestionManager.GetQuestionListOfCommonQuestion", ex);
                throw;
            }
        }

        public void CreateQuestionList(List<Question> questionList)
        {
            try
            {
                using (ContextModel contextModel = new ContextModel())
                {
                    contextModel.Questions.AddRange(questionList);
                    contextModel.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("QuestionManager.CreateQuestionList", ex);
                throw;
            }
        }

        public void UpdateQuestionList(List<QuestionModel> questionModelList, out bool hasAnyUpdated)
        {
            try
            {
                using (ContextModel contextModel = new ContextModel())
                {
                    hasAnyUpdated = false;

                    foreach (var questionModel in questionModelList)
                    {
                        if (!questionModel.IsCreated)
                        {
                            if (questionModel.IsDeleted)
                            {
                                var toDeleteQuestion = contextModel.Questions
                                    .SingleOrDefault(question => question.QuestionID
                                    == questionModel.QuestionID);

                                if (toDeleteQuestion != null)
                                    contextModel.Questions.Remove(toDeleteQuestion);

                                hasAnyUpdated = true;
                            }
                            else if (questionModel.IsUpdated)
                            {
                                var toUpdateQuestion = contextModel.Questions
                                    .SingleOrDefault(question => question.QuestionID
                                    == questionModel.QuestionID);

                                if (toUpdateQuestion != null)
                                {
                                    toUpdateQuestion.QuestionCategory = questionModel.QuestionCategory;
                                    toUpdateQuestion.QuestionTyping = questionModel.QuestionTyping;
                                    toUpdateQuestion.QuestionName = questionModel.QuestionName;
                                    toUpdateQuestion.QuestionAnswer = questionModel.QuestionAnswer;
                                    toUpdateQuestion.QuestionRequired = questionModel.QuestionRequired;
                                    toUpdateQuestion.UpdateDate = questionModel.UpdateDate;
                                }

                                hasAnyUpdated = true;
                            }
                        }
                        else if (!questionModel.IsDeleted)
                        {
                            Question newQuestion = new Question()
                            {
                                QuestionID = questionModel.QuestionID,
                                QuestionnaireID = questionModel.QuestionnaireID,
                                QuestionCategory = questionModel.QuestionCategory,
                                QuestionTyping = questionModel.QuestionTyping,
                                QuestionName = questionModel.QuestionName,
                                QuestionAnswer = questionModel.QuestionAnswer,
                                QuestionRequired = questionModel.QuestionRequired,
                                CreateDate = questionModel.CreateDate,
                                UpdateDate = questionModel.UpdateDate,
                                CommonQuestionID = questionModel.CommonQuestionID,
                            };

                            contextModel.Questions.Add(newQuestion);
                            hasAnyUpdated = true;
                        }
                        else
                            continue;
                    }

                    contextModel.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("QuestionManager.UpdateQuestionList", ex);
                throw;
            }
        }

        public List<QuestionModel> BuildQuestionModelList(
            bool isUpdateMode,
            bool isSetCommonQuestionOnQuestionnaire,
            List<Question> questionList
            )
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
                    CommonQuestionID = question.CommonQuestionID,
                    IsCreated = false,
                    IsUpdated = false,
                    IsDeleted = false,
                };

                if (!isUpdateMode && isSetCommonQuestionOnQuestionnaire)
                {
                    questionModel.QuestionID = Guid.NewGuid();
                    questionModel.CreateDate = DateTime.Now;
                    questionModel.UpdateDate = DateTime.Now;
                    questionModel.CommonQuestionID = null;
                    questionModel.IsCreated = true;
                }

                questionModelList.Add(questionModel);
            }

            return questionModelList;
        }
    }
}