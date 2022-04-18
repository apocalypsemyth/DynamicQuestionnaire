using DynamicQuestionnaire.DynamicQuestionnaire.ORM;
using DynamicQuestionnaire.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DynamicQuestionnaire
{
    public partial class QuestionnaireDetail : System.Web.UI.Page
    {
        private bool _isPostBack = false;

        // Session name
        //private string _user = "User";

        private QuestionnaireManager _questionnaireMgr = new QuestionnaireManager();
        private QuestionManager _questionMgr = new QuestionManager();

        protected void Page_Load(object sender, EventArgs e)
        {
            _isPostBack = this.IsPostBack;

            if (!this.IsPostBack)
            {
                Guid questionnaireID = this.GetQuestionnaireIDOrBackToList();
                var questionnaire = this._questionnaireMgr.GetQuestionnaire(questionnaireID);

                if (questionnaire == null)
                {
                    this.AlertMessage("查無此問卷");
                    this.Response.Redirect("QuestionnaireList.aspx", true);
                }

                // 為使用Repeater創建的List
                List<Questionnaire> questionnaireList = new List<Questionnaire>();
                questionnaireList.Add(questionnaire);
                var questionList = this._questionMgr.GetQuestionListOfQuestionnaire(questionnaireID);
                    
                this.rptQuestionnaireDetail.DataSource = questionnaireList;
                this.rptQuestionnaireDetail.DataBind();

                this.rptQuestionList.DataSource = questionList;
                this.rptQuestionList.DataBind();

                this.aLinkCheckingQuestionnaireDetail.HRef = "CheckingQuestionnaireDetail.aspx?ID=" + questionnaireID;
            }
        }

        protected void rptQuestionList_PreRender(object sender, EventArgs e)
        {
            if (_isPostBack) return;

            foreach (RepeaterItem rptItem in this.rptQuestionList.Items)
            {
                if (rptItem.ItemType == ListItemType.Item || rptItem.ItemType == ListItemType.AlternatingItem)
                {
                    HiddenField hfQuestionID = rptItem.FindControl("hfQuestionID") as HiddenField;
                    string questionID = hfQuestionID.Value;
                    HiddenField hfQuestionTyping = rptItem.FindControl("hfQuestionTyping") as HiddenField;
                    string questionTyping = hfQuestionTyping.Value;

                    Literal ltlQuestionAnswer = rptItem.FindControl("ltlQuestionAnswer") as Literal;
                    string[] qaArr = ltlQuestionAnswer.Text.Split(';');
                    ltlQuestionAnswer.Text = "<div class='d-flex flex-column gap-3'>";

                    for (int i = 0; i < qaArr.Length; i++)
                    {
                        int anthorI = i;
                        string iPlus1 = (anthorI + 1).ToString();

                        if (questionTyping == "單選方塊")
                            ltlQuestionAnswer.Text += 
                                $@"
                                    <div class='form-check'>
                                        <input id='rdoQuestionAnswer_{questionID}_{iPlus1}' class='form-check-input' type='radio' name='rdoQuestionAnswer_{questionID}' />
                                        <label class='form-check-label' for='rdoQuestionAnswer_{questionID}_{iPlus1}'>
                                            {qaArr[i]}
                                        </label>
                                    </div>
                                ";

                        if (questionTyping == "複選方塊")
                            ltlQuestionAnswer.Text +=
                                $@"
                                    <div class='form-check'>
                                        <input id='ckbQuestionAnswer_{questionID}_{iPlus1}' class='form-check-input' type='checkbox' />
                                        <label class='form-check-label' for='ckbQuestionAnswer_{questionID}_{iPlus1}'>
                                            {qaArr[i]}
                                        </label>
                                    </div>
                                ";
                        
                        if (questionTyping == "文字")
                            ltlQuestionAnswer.Text +=
                                $@"
                                    <div class='row'>
                                        <label class='col-sm-2 col-form-label' for='txtQuestionAnswer_{questionID}_{iPlus1}'>
                                            {qaArr[i]}
                                        </label>
                                        <div class='col-sm-10'>
                                            <input id='txtQuestionAnswer_{questionID}_{iPlus1}' class='form-control' type='text' />
                                        </div>
                                    </div>
                                ";
                    }

                    ltlQuestionAnswer.Text += "</div>";
                }
            }
        }
        
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            this.Response.Redirect("QuestionnaireList.aspx", true);
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {

            //bool isValidUser = this.CheckUserInputs();
            //if (!isValidUser) return;
            //this.Response.Write("after testFn");
            //this.CreateUserInSession();
            //Guid questionnaireID = this.GetQuestionnaireIDOrBackToList();
            //this.Response.Redirect("CheckingQuestionnaireDetail.aspx?ID=" + questionnaireID, true);
        }

        protected Guid GetQuestionnaireIDOrBackToList()
        {
            string questionnaireIDStr = this.Request.QueryString["ID"];

            bool isValidQuestionnaireID = Guid.TryParse(questionnaireIDStr, out Guid questionnaireID);
            if (!isValidQuestionnaireID)
                this.Response.Redirect("QuestionnaireList.aspx", true);

            return questionnaireID;
        }

        //private bool CheckUserInputs()
        //{
        //    this.lblUserName.Text = "";
        //    this.lblUserPhone.Text = "";
        //    this.lblUserEmail.Text = "";
        //    this.lblUserAge.Text = "";

        //    bool resultChecked = true;
        //    Regex phoneRx = new Regex(@"^[0]{1}\d{9}");
        //    Regex emailRx = new Regex(@"^(([^<>()[\]\\.,;:\s@\""]+(\.[^<>()[\]\\.,;:\s@\""]+)*)|(\"".+\""))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$");

        //    if (string.IsNullOrWhiteSpace(this.txtUserName.Text))
        //    {
        //        resultChecked = false;
        //        this.lblUserName.Text = "請填入您的姓名。";
        //    }

        //    if (string.IsNullOrWhiteSpace(this.txtUserPhone.Text))
        //    {
        //        resultChecked = false;
        //        this.lblUserPhone.Text = "請填入您的手機。";
        //    }
        //    else
        //    {
        //        if (!phoneRx.IsMatch(this.txtUserPhone.Text.Trim()))
        //        {
        //            resultChecked = false;
        //            this.lblUserPhone.Text = @"請以 ""0123456789"" 開頭零後九碼的格式填寫。";
        //        }
        //    }
            
        //    if (string.IsNullOrWhiteSpace(this.txtUserEmail.Text))
        //    {
        //        resultChecked = false;
        //        this.lblUserEmail.Text = "請填入您的信箱。";
        //    }
        //    else
        //    {
        //        if (!emailRx.IsMatch(this.txtUserEmail.Text.Trim()))
        //        {
        //            resultChecked = false;
        //            this.lblUserEmail.Text = "請填入合法的信箱格式。";
        //        }
        //    }
            
        //    if (string.IsNullOrWhiteSpace(this.txtUserAge.Text))
        //    {
        //        resultChecked = false;
        //        this.lblUserAge.Text = "請填入您的年齡。";
        //    }
        //    else
        //    {
        //        if (!int.TryParse(this.txtUserAge.Text.Trim(), out int age))
        //        {
        //            resultChecked = false;
        //            this.lblUserAge.Text = "請填數數字。";
        //        }
        //    }

        //    return resultChecked;
        //}

        //private void CreateUserInSession()
        //{
        //    User newUser = new User()
        //    {
        //        UserID = Guid.NewGuid(),
        //        UserName = this.txtUserName.Text.Trim(),
        //        Phone = this.txtUserPhone.Text.Trim(),
        //        Email = this.txtUserEmail.Text.Trim(),
        //        AnswerDate = DateTime.Now,
        //    };

        //    if (int.TryParse(this.txtUserAge.Text.Trim(), out int age))
        //        newUser.Age = age;

        //    this.Session[_user] = newUser;
        //}

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