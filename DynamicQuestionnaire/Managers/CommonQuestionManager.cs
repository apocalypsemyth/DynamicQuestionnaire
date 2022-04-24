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
                        var filteredCommonQuestionList = contextModel.CommonQuestions
                            .Where(commonQuestion => commonQuestion.CommonQuestionName.Contains(keyword));

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

        public void DeleteCommonQuestionList(List<Guid> commonQuestionIDList)
        {
            try
            {
                using (ContextModel contextModel = new ContextModel())
                {
                    var toDeleteCommonQuestionList = commonQuestionIDList
                        .Select(commonQuestionID => contextModel.CommonQuestions
                        .Where(commonQuestion => commonQuestion.CommonQuestionID 
                        == commonQuestionID)
                        .FirstOrDefault())
                        .ToList();

                    contextModel.CommonQuestions.RemoveRange(toDeleteCommonQuestionList);
                    contextModel.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("CommonQuestionManager.DeleteCommonQuestionList", ex);
                throw;
            }
        }
    }
}