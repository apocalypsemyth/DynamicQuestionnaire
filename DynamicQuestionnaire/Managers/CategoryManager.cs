using DynamicQuestionnaire.DynamicQuestionnaire.ORM;
using DynamicQuestionnaire.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynamicQuestionnaire.Managers
{
    public class CategoryManager
    {
        public List<Category> GetCategoryList()
        {
            try
            {
                using (ContextModel contextModel = new ContextModel())
                {
                    return contextModel.Categories.ToList();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("CategoryManager.GetCategoryList", ex);
                throw;
            }
        }

        public void CreateCategoryOfCommonQuestion(CommonQuestion commonQuestion)
        {
            try
            {
                using (ContextModel contextModel = new ContextModel())
                {
                    Category newCategory = new Category() 
                    {
                        CategoryID = Guid.NewGuid(),
                        CategoryName = commonQuestion.CommonQuestionName,
                        CommonQuestionID = commonQuestion.CommonQuestionID,
                    };

                    contextModel.Categories.Add(newCategory);
                    contextModel.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("CategoryManager.CreateCategoryOfCommonQuestion", ex);
                throw;
            }
        }

        public void DeleteCategoryListOfCommonQuestionList(List<Guid> commonQuestionIDList)
        {
            try
            {
                using (ContextModel contextModel = new ContextModel())
                {
                    var toDeleteCategoryListOfCommonQuestionList = commonQuestionIDList
                        .Select(commonQuestionID => contextModel.Categories
                        .Where(category => category.CommonQuestionID 
                        == commonQuestionID)
                        .FirstOrDefault());

                    contextModel.Categories.RemoveRange(toDeleteCategoryListOfCommonQuestionList);
                    contextModel.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("CategoryManager.DeleteCategoryListOfCommonQuestionList", ex);
                throw;
            }
        }

        public void UpdateCategoryByCommonQuestion(CommonQuestion commonQuestion)
        {
            try
            {
                using (ContextModel contextModel = new ContextModel())
                {
                    var toUpdateCommonQuestion =
                        contextModel.CommonQuestions
                        .SingleOrDefault(item => item.CommonQuestionID
                        == commonQuestion.CommonQuestionID);

                    var toUpdateCategory = 
                        contextModel.Categories
                        .SingleOrDefault(item2 => item2.CommonQuestionID 
                        == commonQuestion.CommonQuestionID);

                    if (toUpdateCommonQuestion.UpdateDate != commonQuestion.UpdateDate)
                    {
                        toUpdateCategory.CategoryName = commonQuestion.CommonQuestionName;
                        contextModel.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("CategoryManager.UpdateCategoryOfCommonQuestion", ex);
                throw;
            }
        }

    }
}