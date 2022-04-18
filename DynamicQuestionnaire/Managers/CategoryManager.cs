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
    }
}