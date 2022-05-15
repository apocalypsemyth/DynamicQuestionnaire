using DynamicQuestionnaire.DynamicQuestionnaire.ORM;
using DynamicQuestionnaire.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynamicQuestionnaire.Managers
{
    public class QuestionnaireManager
    {
        public Questionnaire GetQuestionnaire(Guid questionnaireID)
        {
            try
            {
                using (ContextModel contextModel = new ContextModel())
                {
                    var questionnaire = contextModel.Questionnaires
                        .Where(item => item.QuestionnaireID == questionnaireID)
                        .FirstOrDefault();

                    return questionnaire;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("QuestionnaireManager.GetQuestionnaire", ex);
                throw;
            }
        }

        public List<Questionnaire> GetQuestionnaireList(
            string keyword,
            string startDateStr,
            string endDateStr,
            int pageSize,
            int pageIndex,
            out int totalRows
            )
        {
            try
            {
                int skip = pageSize * (pageIndex - 1);
                if (skip < 0) skip = 0;

                using (ContextModel contextModel = new ContextModel())
                {
                    if (!string.IsNullOrWhiteSpace(keyword))
                    {
                        var filteredQuestionnaireList = contextModel.Questionnaires
                            .Where(questionnaire => questionnaire.Caption.Contains(keyword));

                        var questionnaireList = filteredQuestionnaireList
                            .OrderByDescending(item => item.StartDate)
                            .ThenByDescending(item2 => item2.UpdateDate)
                            .Skip(skip)
                            .Take(pageSize)
                            .ToList();

                        totalRows = filteredQuestionnaireList.Count();

                        return questionnaireList;
                    }
                    else if (!string.IsNullOrWhiteSpace(startDateStr)
                        && !string.IsNullOrWhiteSpace(endDateStr))
                    {
                        DateTime startDate = DateTime.Parse(startDateStr);
                        DateTime endDate = DateTime.Parse(endDateStr);
                        DateTime endDatePlus1 = endDate.AddDays(1);

                        var filteredQuestionnaireList = contextModel.Questionnaires
                            .Where(questionnaire => questionnaire.EndDate == null
                            ?
                            questionnaire.StartDate >= startDate &&
                            questionnaire.StartDate < endDatePlus1
                            :
                            questionnaire.StartDate >= startDate
                            && questionnaire.EndDate < endDatePlus1);

                        var questionnaireList = filteredQuestionnaireList
                            .OrderByDescending(item => item.StartDate)
                            .ThenByDescending(item2 => item2.UpdateDate)
                            .Skip(skip)
                            .Take(pageSize)
                            .ToList();

                        totalRows = filteredQuestionnaireList.Count();

                        return questionnaireList;
                    }
                    else
                    {
                        totalRows = contextModel.Questionnaires.Count();

                        return contextModel.Questionnaires
                            .OrderByDescending(item => item.StartDate)
                            .ThenByDescending(item2 => item2.UpdateDate)
                            .Skip(skip)
                            .Take(pageSize)
                            .ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("QuestionnaireManager.GetQuestionnaireList", ex);
                throw;
            }
        }

        public void CreateQuestionnaire(Questionnaire questionnaire)
        {
            try
            {
                using (ContextModel contextModel = new ContextModel())
                {
                    Questionnaire newQuestionnaire = new Questionnaire()
                    {
                        QuestionnaireID = questionnaire.QuestionnaireID,
                        Caption = questionnaire.Caption,
                        Description = questionnaire.Description,
                        StartDate = questionnaire.StartDate,
                        IsEnable = questionnaire.IsEnable,
                        CreateDate = questionnaire.CreateDate,
                        UpdateDate = questionnaire.UpdateDate,
                    };

                    if (questionnaire.EndDate != null)
                        newQuestionnaire.EndDate = questionnaire.EndDate;

                    contextModel.Questionnaires.Add(newQuestionnaire);
                    contextModel.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("QuestionnaireManager.CreateQuestionnaire", ex);
                throw;
            }
        }

        public bool DeleteQuestionnaireListTransaction(
            List<Guid> questionnaireIDList, 
            out List<string> errorMsgList
            )
        {
            try
            {
                using (ContextModel contextModel = new ContextModel())
                {
                    errorMsgList = new List<string>();

                    foreach (var questionnaireID in questionnaireIDList)
                    {
                        var toDeleteQuestionListOfQuestionnaire = 
                            contextModel.Questions
                            .Where(question => question.QuestionnaireID 
                            == questionnaireID)
                            .ToList();
                        if (toDeleteQuestionListOfQuestionnaire == null
                            || toDeleteQuestionListOfQuestionnaire.Count == 0)
                            errorMsgList.Add("發生錯誤，找不到應刪除的問題。");

                        var toDeleteQuestionnaire = 
                            contextModel.Questionnaires
                            .SingleOrDefault(questionnaire => questionnaire.QuestionnaireID 
                            == questionnaireID);
                        if (toDeleteQuestionnaire == null)
                            errorMsgList.Add("發生錯誤，找不到應刪除的問卷。");

                        if (errorMsgList.Count > 0)
                            return false;

                        var toDeleteUserListOfQuestionnaire =
                            contextModel.Users
                            .Where(user => user.QuestionnaireID 
                            == questionnaireID)
                            .ToList();

                        var toDeleteUserAnswerListOfQuestionnaire =
                            contextModel.UserAnswers
                            .Where(userAnswer => userAnswer.QuestionnaireID 
                            == questionnaireID)
                            .ToList();

                        if (toDeleteUserListOfQuestionnaire != null
                            && toDeleteUserListOfQuestionnaire.Count > 0)
                            contextModel.Users.RemoveRange(toDeleteUserListOfQuestionnaire);
                        if (toDeleteUserAnswerListOfQuestionnaire != null
                            && toDeleteUserAnswerListOfQuestionnaire.Count > 0)
                            contextModel.UserAnswers.RemoveRange(toDeleteUserAnswerListOfQuestionnaire);

                        contextModel.Questions.RemoveRange(toDeleteQuestionListOfQuestionnaire);
                        contextModel.Questionnaires.Remove(toDeleteQuestionnaire);
                    }
                    
                    contextModel.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("QuestionnaireManager.DeleteQuestionnaireListTransaction", ex);
                throw;
            }
        }

        public void UpdateQuestionnaire(
            bool isUpdateMode, 
            bool isSetCommonQuestionOnQuestionnaire, 
            Questionnaire questionnaire
            )
        {
            try
            {
                using (ContextModel contextModel = new ContextModel())
                {
                    if (isUpdateMode && !isSetCommonQuestionOnQuestionnaire 
                        || isUpdateMode && isSetCommonQuestionOnQuestionnaire)
                    {
                        var toUpdateQuestionnaire = contextModel.Questionnaires
                            .SingleOrDefault(item => item.QuestionnaireID
                            == questionnaire.QuestionnaireID);

                        if (toUpdateQuestionnaire.UpdateDate != questionnaire.UpdateDate)
                        {
                            toUpdateQuestionnaire.Caption = questionnaire.Caption;
                            toUpdateQuestionnaire.Description = questionnaire.Description;
                            toUpdateQuestionnaire.StartDate = questionnaire.StartDate;
                            toUpdateQuestionnaire.EndDate = questionnaire.EndDate;
                            toUpdateQuestionnaire.IsEnable = questionnaire.IsEnable;
                            toUpdateQuestionnaire.CreateDate = questionnaire.CreateDate;
                            toUpdateQuestionnaire.UpdateDate = questionnaire.UpdateDate;
                        }
                    }
                    else
                        contextModel.Questionnaires.Add(questionnaire);
                    
                    contextModel.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("QuestionnaireManager.UpdateQuestionnaire", ex);
                throw;
            }
        }
        
        public void SetIsEnableOfQuestionnaireList(List<Questionnaire> toSetQuestionnaireList)
        {
            try
            {
                using (ContextModel contextModel = new ContextModel())
                {
                    foreach (var questionnaire in toSetQuestionnaireList)
                    {
                        var toSetQuestionnaire = contextModel.Questionnaires
                            .SingleOrDefault(item => item.QuestionnaireID 
                            == questionnaire.QuestionnaireID);

                        toSetQuestionnaire.IsEnable = questionnaire.IsEnable;
                    }

                    contextModel.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("QuestionnaireManager.SetIsEnableOfQuestionnaireList", ex);
                throw;
            }
        }
    }
}