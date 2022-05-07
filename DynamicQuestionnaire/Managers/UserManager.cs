using DynamicQuestionnaire.DynamicQuestionnaire.ORM;
using DynamicQuestionnaire.Helpers;
using DynamicQuestionnaire.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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

        public string ExportDataToCSV(Guid questionnaireID)
        {
            string connectStr = ConfigHelper.GetConnectionString();

            using (SqlConnection connect = new SqlConnection(connectStr))
            {
                string commandText =
                $@" 
                    SELECT	UserName, 
		                    Phone, 
		                    Email, 
		                    Age, 
		                    AnswerDate,
		                    QuestionCategory,
		                    Temp.QuestionTyping,
		                    QuestionName,
		                    QuestionRequired,
		                    QuestionAnswer,
		                    AnswerNum,
		                    Answer
                    FROM (
	                    SELECT  UserID,
			                    UserName, 
			                    Phone, 
			                    Email, 
			                    Age, 
			                    AnswerDate,
			                    QuestionID,
			                    Questions.QuestionnaireID,
			                    QuestionCategory,
			                    QuestionTyping,
			                    QuestionName,
			                    QuestionRequired,
			                    QuestionAnswer,
			                    UpdateDate
	                    FROM Users
	                    JOIN Questions
	                    ON Users.QuestionnaireID = Questions.QuestionnaireID
	                    WHERE Users.QuestionnaireID = @QuestionnaireID
                    ) AS Temp
                    JOIN UserAnswers
                    ON Temp.UserID = UserAnswers.UserID AND Temp.QuestionID = UserAnswers.QuestionID
                    ORDER BY Temp.AnswerDate DESC,
                    Temp.UpdateDate DESC
                ";

                using (SqlCommand cmd = new SqlCommand(commandText, connect))
                {
                    cmd.Parameters.AddWithValue("@QuestionnaireID", questionnaireID);

                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.Connection = connect;
                        sda.SelectCommand = cmd;

                        using (DataTable dt = new DataTable())
                        {
                            sda.Fill(dt);
                            string csv = "";

                            foreach (DataColumn column in dt.Columns)
                            {
                                csv += column.ColumnName + ',';
                            }

                            csv += "\r\n";

                            string userName = "";
                            foreach (DataRow row in dt.Rows)
                            {
                                if (userName != row[0].ToString())
                                {
                                    userName = row[0].ToString();

                                    foreach (DataColumn column in dt.Columns)
                                    {
                                        csv += row[column.ColumnName].ToString().Replace(",", ";") + ',';
                                    }

                                    csv += "\r\n";
                                }
                                else if (userName == row[0].ToString())
                                {
                                    foreach (DataColumn column in dt.Columns)
                                    {
                                        if (column.ColumnName == "UserName" 
                                            || column.ColumnName == "Phone" 
                                            || column.ColumnName == "Email" 
                                            || column.ColumnName == "Age" 
                                            || column.ColumnName == "AnswerDate")
                                        {
                                            csv += "" + ',';
                                        }
                                        else
                                            csv += row[column.ColumnName].ToString().Replace(",", ";") + ',';
                                    }

                                    csv += "\r\n";
                                }
                            }

                            return csv;
                        }
                    }
                }
            }
        }

        public UserModel BuildUserModel(User user)
        {
            return new UserModel()
            {
                UserID = user.UserID,
                QuestionnaireID = user.QuestionnaireID,
                UserName = user.UserName,
                Phone = user.Phone,
                Email = user.Email,
                Age = user.Age,
                AnswerDate = user.AnswerDate,
            };
        }
    }
}