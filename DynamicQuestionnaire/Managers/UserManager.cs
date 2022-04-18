using DynamicQuestionnaire.DynamicQuestionnaire.ORM;
using DynamicQuestionnaire.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynamicQuestionnaire.Managers
{
    public class UserManager
    {
        public void CreateUser(User user)
        {
            try
            {
                using (ContextModel context = new ContextModel())
                {
                    context.Users.Add(user);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("UserManager.CreateUser", ex);
                throw;
            }
        }
    }
}