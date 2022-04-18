using DynamicQuestionnaire.DynamicQuestionnaire.ORM;
using DynamicQuestionnaire.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynamicQuestionnaire.Managers
{
    public class TypingManager
    {
        public List<Typing> GetTypingList()
        {
            try
            {
                using (ContextModel contextModel = new ContextModel())
                {
                    return contextModel.Typings.ToList();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("TypingManager.GetTypingList", ex);
                throw;
            }
        }
    }
}