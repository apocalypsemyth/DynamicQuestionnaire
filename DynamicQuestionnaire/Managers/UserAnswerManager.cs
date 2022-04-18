using DynamicQuestionnaire.DynamicQuestionnaire.ORM;
using DynamicQuestionnaire.Helpers;
using DynamicQuestionnaire.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynamicQuestionnaire.Managers
{
    public class UserAnswerManager
    {
        public void CreateUserAnswerList(List<UserAnswerModel> userAnswerModelList)
        {
            try
            {
                using (ContextModel context = new ContextModel())
                {
                    List<UserAnswer> newUserAnswerList = new List<UserAnswer>();

                    foreach (var userAnswerModel in userAnswerModelList)
                    {
                        UserAnswer newUserAnswer = new UserAnswer()
                        {
                            UserID = userAnswerModel.UserID,
                            QuestionID = userAnswerModel.QuestionID,
                            QuestionTyping = userAnswerModel.QuestionTyping,
                            AnswerNum = userAnswerModel.AnswerNum,
                            Answer = userAnswerModel.Answer,
                        };

                        newUserAnswerList.Add(newUserAnswer);
                    }

                    context.UserAnswers.AddRange(newUserAnswerList);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("UserAnswerManager.CreateUserAnswerList", ex);
                throw;
            }
        }
    }
}