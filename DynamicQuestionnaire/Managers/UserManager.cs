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
        public User GetUser(Guid questionnaireID, Guid userID)
        {
            try
            {
                using (ContextModel contextModel = new ContextModel())
                {
                    return contextModel.Users
                        .Where(user => user.QuestionnaireID == questionnaireID 
                        && user.UserID == userID)
                        .OrderByDescending(user2 => user2.AnswerDate)
                        .FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("UserManager.GetUser", ex);
                throw;
            }
        }

        public List<User> GetUserList(Guid questionnaireID)
        {
            try
            {
                using (ContextModel contextModel = new ContextModel())
                {
                    return contextModel.Users
                        .Where(user => user.QuestionnaireID == questionnaireID)
                        .OrderByDescending(item => item.AnswerDate)
                        .ToList();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("UserManager.GetUserList", ex);
                throw;
            }
        }

        public List<User> GetUserList(
            Guid questionnaireID, 
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
                    var filteredUserList = contextModel.Users
                        .Where(user => user.QuestionnaireID == questionnaireID);

                    totalRows = filteredUserList.Count();

                    return filteredUserList
                        .OrderByDescending(item => item.AnswerDate)
                        .Skip(skip)
                        .Take(pageSize)
                        .ToList();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("UserManager.GetUserList", ex);
                throw;
            }
        }

        public void CreateUser(User user)
        {
            try
            {
                using (ContextModel contextModel = new ContextModel())
                {
                    contextModel.Users.Add(user);
                    contextModel.SaveChanges();
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