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

        public void CreateTyping(string typingName)
        {
            try
            {
                using (ContextModel contextModel = new ContextModel())
                {
                    Typing newTyping = new Typing()
                    {
                        TypingID = Guid.NewGuid(),
                        TypingName = typingName,
                    };

                    contextModel.Typings.Add(newTyping);
                    contextModel.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("TypingManager.CreateTyping", ex);
                throw;
            }
        }
    }
}