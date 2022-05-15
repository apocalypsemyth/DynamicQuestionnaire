using DynamicQuestionnaire.DynamicQuestionnaire.ORM;
using DynamicQuestionnaire.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DynamicQuestionnaire
{
    public partial class QuestionnaireDetail : System.Web.UI.Page
    {
        private bool _isPostBack = false;

        // Session name
        private string _isEnable = "IsEnable";

        private QuestionnaireManager _questionnaireMgr = new QuestionnaireManager();
        private QuestionManager _questionMgr = new QuestionManager();

        protected void Page_Load(object sender, EventArgs e)
        {
            this.btnCancel.Attributes.Add("onClick", "return BackToListForServer();");

            this._isPostBack = this.IsPostBack;

            if (this.Session[_isEnable] == null)
            {
                ClientScript.RegisterStartupScript(
                    this.GetType(), 
                    "ResetQuestionnaireDetailInputsForServer", 
                    "ResetQuestionnaireDetailInputsForServer();", 
                    true
                    );
            }

            if (!this.IsPostBack)
            {
                Guid questionnaireID = this.GetQuestionnaireIDOrBackToList();
                var questionnaire = this._questionnaireMgr.GetQuestionnaire(questionnaireID);

                if (questionnaire == null)
                {
                    this.AlertMessage("查無此問卷");
                    this.Response.Redirect("QuestionnaireList.aspx", true);
                }

                this.Session[_isEnable] = questionnaire.IsEnable;

                if (questionnaire.IsEnable)
                {
                    this.questionnaireUserForm.Visible = true;
                    this.btnCancel.Visible = true;
                    this.aLinkCheckingQuestionnaireDetail.Visible = true;
                }
                else
                {
                    this.questionnaireUserForm.Visible = false;
                    this.btnCancel.Visible = false;
                    this.aLinkCheckingQuestionnaireDetail.Visible = false;
                }

                // 為使用Repeater創建的List
                List<Questionnaire> questionnaireList = new List<Questionnaire>();
                questionnaireList.Add(questionnaire);
                var questionList = this._questionMgr.GetQuestionListOfQuestionnaire(questionnaireID);
                    
                this.rptQuestionnaireDetail.DataSource = questionnaireList;
                this.rptQuestionnaireDetail.DataBind();

                this.rptQuestionList.DataSource = questionList;
                this.rptQuestionList.DataBind();

                this.aLinkCheckingQuestionnaireDetail.HRef = 
                    "CheckingQuestionnaireDetail.aspx?ID=" + questionnaireID;
            }
        }

        protected void rptQuestionList_PreRender(object sender, EventArgs e)
        {
            if (_isPostBack) return;
            bool isEnable = (bool)this.Session[_isEnable];
            string isEnableAttribute = isEnable ? "" : "disabled";

            foreach (RepeaterItem rptItem in this.rptQuestionList.Items)
            {
                if (rptItem.ItemType == ListItemType.Item || rptItem.ItemType == ListItemType.AlternatingItem)
                {
                    HiddenField hfQuestionID = rptItem.FindControl("hfQuestionID") as HiddenField;
                    string questionID = hfQuestionID.Value;
                    HiddenField hfQuestionTyping = rptItem.FindControl("hfQuestionTyping") as HiddenField;
                    string questionTyping = hfQuestionTyping.Value;
                    Literal ltlQuestionName = rptItem.FindControl("ltlQuestionName") as Literal;
                    bool isRequired = ltlQuestionName.Text.Contains("必填") ? true : false;

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
                                        <input id='rdoQuestionAnswer_{questionID}_{iPlus1}' class='form-check-input' type='radio' name='rdoQuestionAnswer_{questionID}' required='{isRequired}' {isEnableAttribute} />
                                        <label class='form-check-label' for='rdoQuestionAnswer_{questionID}_{iPlus1}'>
                                            {qaArr[i]}
                                        </label>
                                    </div>
                                ";

                        if (questionTyping == "複選方塊")
                            ltlQuestionAnswer.Text +=
                                $@"
                                    <div class='form-check'>
                                        <input id='ckbQuestionAnswer_{questionID}_{iPlus1}' class='form-check-input' type='checkbox' required='{isRequired}' {isEnableAttribute} />
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
                                            <input id='txtQuestionAnswer_{questionID}_{iPlus1}' class='form-control' type='text' required='{isRequired}' {isEnableAttribute} />
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

        }

        protected Guid GetQuestionnaireIDOrBackToList()
        {
            string questionnaireIDStr = this.Request.QueryString["ID"];

            bool isValidQuestionnaireID = Guid.TryParse(questionnaireIDStr, out Guid questionnaireID);
            if (!isValidQuestionnaireID)
                this.Response.Redirect("QuestionnaireList.aspx", true);

            return questionnaireID;
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