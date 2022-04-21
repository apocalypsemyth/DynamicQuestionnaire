﻿using DynamicQuestionnaire.DynamicQuestionnaire.ORM;
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
        public List<UserAnswer> GetUserAnswerList(Guid questionnaireID, Guid userID)
        {
            try
            {
                using (ContextModel context = new ContextModel())
                {
                    return context.UserAnswers
                        .Where(userAnswer => userAnswer.QuestionnaireID == questionnaireID 
                        && userAnswer.UserID == userID)
                        .ToList();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("UserAnswerManager.GetUserAnswerList", ex);
                throw;
            }
        }

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
                            QuestionnaireID = userAnswerModel.QuestionnaireID,
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