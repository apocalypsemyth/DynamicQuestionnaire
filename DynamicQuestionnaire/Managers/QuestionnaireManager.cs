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

        public void DeleteQuestionnaireList(List<Guid> questionnaireIDList)
        {
            try
            {
                using (ContextModel contextModel = new ContextModel())
                {
                    foreach (var questionnaireID in questionnaireIDList)
                    {
                        var toDeleteQuestionListOfQuestionnaire = 
                            contextModel.Questions
                            .Where(question => question.QuestionnaireID == questionnaireID);
                        contextModel.Questions.RemoveRange(toDeleteQuestionListOfQuestionnaire);

                        var toDeleteQuestionnaire = 
                            contextModel.Questionnaires
                            .Where(questionnaire => questionnaire.QuestionnaireID == questionnaireID)
                            .FirstOrDefault();
                        contextModel.Questionnaires.Remove(toDeleteQuestionnaire);
                    }
                    
                    contextModel.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("QuestionnaireManager.DeleteQuestionnaireList", ex);
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
    }
}