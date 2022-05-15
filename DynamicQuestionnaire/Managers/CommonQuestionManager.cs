using DynamicQuestionnaire.DynamicQuestionnaire.ORM;
using DynamicQuestionnaire.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynamicQuestionnaire.Managers
{
    public class CommonQuestionManager
    {
        public CommonQuestion GetCommonQuestion(Guid commonQuestionID)
        {
            try
            {
                using (ContextModel contextModel = new ContextModel())
                {
                    return contextModel.CommonQuestions
                        .SingleOrDefault(commonQuestion => commonQuestion.CommonQuestionID 
                        == commonQuestionID);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("CommonQuestionManager.GetCommonQuestion", ex);
                throw;
            }
        }

        public List<CommonQuestion> GetCommonQuestionList(
            string keyword,
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
                        var filteredCommonQuestionList = 
                            contextModel.CommonQuestions
                            .Where(commonQuestion => commonQuestion.CommonQuestionName
                            .Contains(keyword));

                        var commonQuestionList = filteredCommonQuestionList
                            .OrderByDescending(item2 => item2.UpdateDate)
                            .Skip(skip)
                            .Take(pageSize)
                            .ToList();

                        totalRows = filteredCommonQuestionList.Count();

                        return commonQuestionList;
                    }
                    else
                    {
                        var filteredCommonQuestionList = contextModel.CommonQuestions;

                        totalRows = filteredCommonQuestionList.Count();

                        return filteredCommonQuestionList
                            .OrderByDescending(item2 => item2.UpdateDate)
                            .Skip(skip)
                            .Take(pageSize)
                            .ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("CommonQuestionManager.GetCommonQuestionList", ex);
                throw;
            }
        }

        public void CreateCommonQuestion(CommonQuestion newCommonQuestion)
        {
            try
            {
                using (ContextModel contextModel = new ContextModel())
                {
                    contextModel.CommonQuestions.Add(newCommonQuestion);
                    contextModel.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("CommonQuestionManager.CreateCommonQuestion", ex);
                throw;
            }
        }

        public bool DeleteCommonQuestionListTransaction(
            List<Guid> commonQuestionIDList, 
            out List<string> errorMsgList
            )
        {
            try
            {
                using (ContextModel contextModel = new ContextModel())
                {
                    errorMsgList = new List<string>();

                    foreach (var commonQuestionID in commonQuestionIDList)
                    {
                        var toDeleteCategoryOfCommonQuestion = 
                            contextModel.Categories
                            .SingleOrDefault(category => category.CommonQuestionID 
                            == commonQuestionID);
                        if (toDeleteCategoryOfCommonQuestion == null)
                            errorMsgList.Add("發生錯誤，找不到應刪除的問題種類。");

                        var toDeleteQuestionListOfCommonQuestion = 
                            contextModel.Questions.Where(question => question.CommonQuestionID 
                            == commonQuestionID)
                            .ToList();
                        if (toDeleteQuestionListOfCommonQuestion == null
                            || toDeleteQuestionListOfCommonQuestion.Count == 0)
                            errorMsgList.Add("發生錯誤，找不到應刪除的問題。");

                        var toDeleteCommonQuestion = 
                            contextModel.CommonQuestions
                            .SingleOrDefault(commonQuestion => commonQuestion.CommonQuestionID 
                            == commonQuestionID);
                        if (toDeleteCommonQuestion == null)
                            errorMsgList.Add("發生錯誤，找不到應刪除的常用問題。");

                        if (errorMsgList.Count > 0)
                            return false;

                        contextModel.Categories.Remove(toDeleteCategoryOfCommonQuestion);
                        contextModel.Questions.RemoveRange(toDeleteQuestionListOfCommonQuestion);
                        contextModel.CommonQuestions.Remove(toDeleteCommonQuestion);
                    }

                    contextModel.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("CommonQuestionManager.DeleteCommonQuestionListTransaction", ex);
                throw;
            }
        }

        public void UpdateCommonQuestion(CommonQuestion commonQuestion)
        {
            try
            {
                using (ContextModel contextModel = new ContextModel())
                {
                    var toUpdateCommonQuestion = 
                        contextModel.CommonQuestions
                        .SingleOrDefault(item => item.CommonQuestionID
                        == commonQuestion.CommonQuestionID);

                    if (toUpdateCommonQuestion.UpdateDate != commonQuestion.UpdateDate)
                    {
                        toUpdateCommonQuestion.CommonQuestionName = commonQuestion.CommonQuestionName;
                        toUpdateCommonQuestion.CreateDate = commonQuestion.CreateDate;
                        toUpdateCommonQuestion.UpdateDate = commonQuestion.UpdateDate;

                        contextModel.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("CommonQuestionManager.UpdateCommonQuestion", ex);
                throw;
            }
        }
    }
}