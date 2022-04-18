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
        public Question GetQuestion(Guid questionID)
        {
            try
            {
                using (ContextModel contextModel = new ContextModel())
                {
                    return contextModel.Questions
                        .Where(question => question.QuestionID == questionID)
                        .FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("QuestionManager.GetQuestion", ex);
                throw;
            }
        }

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

        public void DeleteQuestionList(List<Guid> questionIDList)
        {
            try
            {
                using (ContextModel contextModel = new ContextModel())
                {
                    var toDeleteQuestionList = questionIDList
                        .Select(questionID => contextModel.Questions
                        .Where(question => question.QuestionID == questionID)
                        .FirstOrDefault())
                        .ToList();

                    contextModel.Questions.RemoveRange(toDeleteQuestionList);
                    contextModel.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("QuestionManager.DeleteQuestionList", ex);
                throw;
            }
        }

        public void UpdateQuestionList(List<QuestionModel> questionModelList)
        {
            try
            {
                using (ContextModel contextModel = new ContextModel())
                {
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
                            };

                            contextModel.Questions.Add(newQuestion);
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
    }
}