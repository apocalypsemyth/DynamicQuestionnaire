using DynamicQuestionnaire.DynamicQuestionnaire.ORM;
using DynamicQuestionnaire.Managers;
using DynamicQuestionnaire.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DynamicQuestionnaire
{
    public partial class CheckingQuestionnaireDetail : System.Web.UI.Page
    {
        private bool _isPostBack = false;

        // Session name
        private string _user = "User";
        private string _userAnswer = "UserAnswer";

        private QuestionnaireManager _questionnaireMgr = new QuestionnaireManager();
        private QuestionManager _questionMgr = new QuestionManager();
        private UserManager _userMgr = new UserManager();
        private UserAnswerManager _userAnswerMgr = new UserAnswerManager();

        protected void Page_Load(object sender, EventArgs e)
        {
            this.btnEdit.Attributes.Add("onClick", "javascript:history.back(); return false;");
            _isPostBack = this.IsPostBack;

            if (!this.IsPostBack)
            {
                Guid questionnaireID = this.GetQuestionnaireIDOrBackToList();
                var questionnaire = this._questionnaireMgr.GetQuestionnaire(questionnaireID);
                var user = this.Session[_user] as User;
                var userAnswerModelList = this.Session[_userAnswer] as List<UserAnswerModel>;

                if (questionnaire == null)
                {
                    this.AlertMessage("查無此問卷");
                    this.Response.Redirect("QuestionnaireList.aspx", true);
                }
                this.CheckUserInfoInSession(user, userAnswerModelList, questionnaireID);

                // 為使用Repeater創建的List
                List<Questionnaire> questionnaireList = new List<Questionnaire>();
                questionnaireList.Add(questionnaire);
                var questionList = this._questionMgr.GetQuestionListOfQuestionnaire(questionnaireID);

                this.rptCheckingQuestionnaireDetail.DataSource = questionnaireList;
                this.rptCheckingQuestionnaireDetail.DataBind();

                this.SetUserInfo(user);

                var filteredQuestionList = userAnswerModelList
                    .GroupBy(item => item.QuestionID)
                    .Select(item2 => item2.FirstOrDefault())
                    .Select(userAnswerModel => questionList
                    .Where(question => question.QuestionID 
                    == userAnswerModel.QuestionID)
                    .FirstOrDefault())
                    .ToList();

                this.rptCheckingQuestionList.DataSource = filteredQuestionList;
                this.rptCheckingQuestionList.DataBind();
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            User user = this.Session[_user] as User;
            List<UserAnswerModel> userAnswerModelList = this.Session[_userAnswer] as List<UserAnswerModel>;
            Guid questionnaireID = this.GetQuestionnaireIDOrBackToList();

            this.CheckUserInfoInSession(user, userAnswerModelList, questionnaireID);

            this._userMgr.CreateUser(user);
            this._userAnswerMgr.CreateUserAnswerList(userAnswerModelList);
            this.Session.Remove(_user);
            this.Session.Remove(_userAnswer);

            this.Response.Redirect("QuestionnaireList.aspx", true);
        }
        
        protected void rptCheckingQuestionList_PreRender(object sender, EventArgs e)
        {
            if (_isPostBack) return;

            List<UserAnswerModel> userAnswerModelList = this.Session[_userAnswer] as List<UserAnswerModel>;

            foreach (RepeaterItem rptItem in this.rptCheckingQuestionList.Items)
            {
                if (rptItem.ItemType == ListItemType.Item || rptItem.ItemType == ListItemType.AlternatingItem)
                {
                    HiddenField hfQuestionID = rptItem.FindControl("hfQuestionID") as HiddenField;
                    string questionIDStr = hfQuestionID.Value;
                    Literal ltlQuestionAnswer = rptItem.FindControl("ltlQuestionAnswer") as Literal;
                    string[] qaArr = ltlQuestionAnswer.Text.Split(';');
                    ltlQuestionAnswer.Text = "<div class='d-flex flex-column gap-3'>";

                    if (Guid.TryParse(questionIDStr, out Guid questionID))
                    {
                        var userAnswerByQuestionIDList = userAnswerModelList
                            .Where(userAnswer => userAnswer.QuestionID == questionID)
                            .OrderBy(item => item.AnswerNum)
                            .ToList();

                        ltlQuestionAnswer.Text += "<div class='d-flex flex-column gap-3'>";

                        foreach (var userAnswer in userAnswerByQuestionIDList)
                        {
                            string userAnswerQuestionTyping = userAnswer.QuestionTyping;
                            int userAnswerNum = userAnswer.AnswerNum;
                            int userAnswerNumMinus1 = userAnswerNum - 1;

                            if (userAnswerQuestionTyping == "文字") 
                            {
                                ltlQuestionAnswer.Text +=
                                $@"
                                    <h5 id='questionAnswer_{questionID}_{userAnswerNum}'>
                                        {qaArr[userAnswerNumMinus1]}：
                                        {userAnswer.Answer}
                                    </h5>
                                ";
                            }
                            else
                            {
                                ltlQuestionAnswer.Text +=
                                $@"
                                    <h5 id='questionAnswer_{questionID}_{userAnswerNum}'>
                                        {qaArr[userAnswerNumMinus1]}
                                    </h5>
                                ";
                            }
                        }

                        ltlQuestionAnswer.Text += "</div>";
                    }

                    ltlQuestionAnswer.Text += "</div>";
                }
            }
        }

        protected Guid GetQuestionnaireIDOrBackToList()
        {
            string questionnaireIDStr = this.Request.QueryString["ID"];

            bool isValidQuestionnaireID = Guid.TryParse(questionnaireIDStr, out Guid questionnaireID);
            if (!isValidQuestionnaireID)
                this.Response.Redirect("QuestionnaireList.aspx", true);

            return questionnaireID;
        }

        private void SetUserInfo(User user)
        {
            this.ltlUserName.Text = user.UserName;
            this.ltlUserPhone.Text = user.Phone;
            this.ltlUserEmail.Text = user.Email;
            this.ltlUserAge.Text = user.Age.ToString();
        }

        private void CheckUserInfoInSession(
            User user, 
            List<UserAnswerModel> userAnswerModelList, 
            Guid questionnaireID
            )
        {
            if (user == null)
            {
                this.AlertMessage("沒有您的填寫資料");
                this.Response.Redirect("QuestionnaireDetail.aspx?ID=" + questionnaireID, true);
            }

            if (userAnswerModelList == null || userAnswerModelList.Count == 0)
            {
                this.AlertMessage("沒有您的回答資料");
                this.Response.Redirect("QuestionnaireDetail.aspx?ID=" + questionnaireID, true);
            }
        }

        private void AlertMessage(string errorMsg)
        {
            ClientScript.RegisterStartupScript(
                this.GetType(),
                "alert",
                "alert('" + errorMsg + "');",
                true
            );
        }
    }
}