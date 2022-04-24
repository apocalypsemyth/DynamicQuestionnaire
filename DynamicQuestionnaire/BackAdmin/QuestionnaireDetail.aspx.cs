using DynamicQuestionnaire.DynamicQuestionnaire.ORM;
using DynamicQuestionnaire.Managers;
using DynamicQuestionnaire.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DynamicQuestionnaire.BackAdmin
{
    public partial class QuestionnaireDetail1 : System.Web.UI.Page
    {
        private bool _isEditMode = false;

        // Session name
        private string _isUpdateMode = "IsUpdateMode";
        private string _questionnaire = "Questionnaire";
        private string _questionList = "QuestionList";
        private string _currentPagerIndex = "CurrentPagerIndex";

        private QuestionnaireManager _questionnaireMgr = new QuestionnaireManager();
        private CategoryManager _categoryMgr = new CategoryManager();
        private TypingManager _typingMgr = new TypingManager();
        private QuestionManager _questionMgr = new QuestionManager();
        private UserManager _userMgr = new UserManager();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(this.Request.QueryString["ID"]))
            {
                _isEditMode = true;
                this.Session[_isUpdateMode] = _isEditMode;
            }
            else
            {
                _isEditMode = false;
                this.Session[_isUpdateMode] = _isEditMode;
            }

            if (_isEditMode)
            {
                Guid questionnaireID = this.GetQuestionnaireIDOrBackToList();
                this.InitEditMode(questionnaireID);
                this.btnExportAndDownloadDataToCSV.Visible = true;
            }
            else
            {
                this.InitCreateMode();
                this.btnExportAndDownloadDataToCSV.Visible = false;
            }

            this.ucSubmitButtonInQuestionnaireTab.OnSubmitClick += UcInQuestionnaireTab_OnSubmitClick;
            this.ucSubmitButtonInQuestionTab.OnSubmitClick += UcInQuestionTab_OnSubmitClick;
            this.ucCancelButtonInQuestionnaireTab.OnCancelClick += UcInQuestionnaireTab_OnCancelClick;
            this.ucCancelButtonInQuestionTab.OnCancelClick += UcInQuestionTab_OnCancelClick;
        }

        protected void UcInQuestionnaireTab_OnCancelClick(
            object sender,
            EventArgs e
            )
        {
            this.SameLogicOfRemoveSessionAndBackToList(sender, e);
        }

        protected void UcInQuestionTab_OnCancelClick(object sender, EventArgs e)
        {
            this.SameLogicOfRemoveSessionAndBackToList(sender, e);
        }

        protected void UcInQuestionnaireTab_OnSubmitClick(
            object sender,
            EventArgs e
            )
        {
            this.SameLogicOfBtnSubmit_Click(sender, e);
        }

        protected void UcInQuestionTab_OnSubmitClick(
            object sender,
            EventArgs e
            )
        {
            this.SameLogicOfBtnSubmit_Click(sender, e);
        }

        private void SameLogicOfBtnSubmit_Click(
            object sender,
            EventArgs e
            )
        {
            if (!this.CheckQuestionnaireInputs(out List<string> errorMsgList))
            {
                string errorMsg = string.Join("\\n", errorMsgList);
                this.AlertMessage(errorMsg);

                return;
            }

            Questionnaire newOrToUpdateQuestionnaire = 
                this.Session[_questionList] as Questionnaire;
            if (newOrToUpdateQuestionnaire == null)
            {
                this.AlertMessage("請填寫問題後，先按下加入按鈕。");
                return;
            }

            var newOrUpdatedQuestionnaire = 
                this.FinalUpdateQuestionnaire(newOrToUpdateQuestionnaire);

            if (_isEditMode)
            {
                List<QuestionModel> toUpdateQuestionModelList = 
                    this.Session[_questionList] as List<QuestionModel>;
                if (toUpdateQuestionModelList == null 
                    || toUpdateQuestionModelList.Count == 0)
                {
                    this.AlertMessage("請填寫至少一個問題。");
                    return;
                }

                if (toUpdateQuestionModelList.All(item => item.IsDeleted))
                {
                    this.AlertMessage("問題不能全空，請填寫或留下至少一個問題。");
                    return;
                }

                this._questionMgr.UpdateQuestionList(
                    toUpdateQuestionModelList, 
                    out bool hasAnyUpdated
                    );

                if (hasAnyUpdated)
                    newOrUpdatedQuestionnaire.UpdateDate = DateTime.Now;

                this._questionnaireMgr.UpdateQuestionnaire(newOrUpdatedQuestionnaire);
            }
            else
            {
                List<Question> newQuestionList = 
                    this.Session[_questionList] as List<Question>;
                if (newQuestionList == null || newQuestionList.Count == 0)
                {
                    this.AlertMessage("請填寫至少一個問題。");
                    return;
                }

                this._questionnaireMgr.CreateQuestionnaire(newOrToUpdateQuestionnaire);
                this._questionMgr.CreateQuestionList(newQuestionList);
            }

            this.SameLogicOfRemoveSessionAndBackToList(sender, e);
        }

        private void SameLogicOfRemoveSessionAndBackToList(
            object sender,
            EventArgs e
            )
        {
            this.Session.Remove(_isUpdateMode);
            this.Session.Remove(_questionnaire);
            this.Session.Remove(_questionList);
            this.Session.Remove(_currentPagerIndex);

            this.Response.Redirect("QuestionnaireList.aspx", true);
        }

        protected void btnExportAndDownloadDataToCSV_Click(object sender, EventArgs e)
        {
            Guid questionnaireID = this.GetQuestionnaireIDOrBackToList();
            var csv = this._userMgr.ExportDataToCSV(questionnaireID);

            this.Response.Clear();
            this.Response.Buffer = true;
            this.Response.AddHeader("content-disposition", "attachment;filename=UserList.csv");
            this.Response.Charset = "";
            this.Response.ContentType = "application/csv";
            this.Response.ContentEncoding = Encoding.UTF8;
            this.Response.BinaryWrite(Encoding.UTF8.GetPreamble());
            this.Response.Output.Write(csv);
            this.Response.Flush();
            this.Response.End();
        }

        protected Guid GetQuestionnaireIDOrBackToList()
        {
            string questionnaireIDStr = this.Request.QueryString["ID"];

            bool isValidQuestionnaireID = Guid.TryParse(questionnaireIDStr, out Guid questionnaireID);
            if (!isValidQuestionnaireID)
                this.Response.Redirect("QuestionnaireList.aspx", true);

            return questionnaireID;
        }

        private void InitCreateMode()
        {
            var categoryList = this._categoryMgr.GetCategoryList();
            var typingList = this._typingMgr.GetTypingList();

            // 問卷控制項繫結
            this.txtStartDate.Text = DateTime.Now.ToShortDateString();
            this.ckbIsEnable.Checked = true;

            // 問題控制項繫結
            this.ddlCategoryList.DataTextField = "CategoryName";
            this.ddlCategoryList.DataValueField = "CategoryName";
            this.ddlCategoryList.DataSource = categoryList;
            this.ddlCategoryList.DataBind();
            this.ddlCategoryList.ClearSelection();
            this.ddlCategoryList.Items.FindByValue("自訂問題").Selected = true;

            this.ddlTypingList.DataTextField = "TypingName";
            this.ddlTypingList.DataValueField = "TypingName";
            this.ddlTypingList.DataSource = typingList;
            this.ddlTypingList.DataBind();
            this.ddlTypingList.ClearSelection();
            this.ddlTypingList.Items.FindByValue("單選方塊").Selected = true;

            this.ckbQuestionRequired.Checked = false;
        }

        private void InitEditMode(Guid questionnaireID)
        {
            var questionnaire = this._questionnaireMgr.GetQuestionnaire(questionnaireID);

            if (questionnaire == null)
            {
                this.AlertMessage("查無此問卷");
                this.Response.Redirect("QuestionnaireList.aspx", true);
            }
            else
            {
                var categoryList = this._categoryMgr.GetCategoryList();
                var typingList = this._typingMgr.GetTypingList();
                var questionList = this._questionMgr.GetQuestionListOfQuestionnaire(questionnaireID);
                var firstQuestion = questionList.FirstOrDefault();

                // 問卷控制項繫結
                this.txtCaption.Text = questionnaire.Caption;
                this.txtDescription.Text = questionnaire.Description;
                this.ckbIsEnable.Checked = questionnaire.IsEnable;
                this.txtStartDate.Text = questionnaire.StartDate.ToShortDateString();

                if (questionnaire.EndDate == null)
                    this.txtEndDate.Text = "";
                else
                    this.txtEndDate.Text = questionnaire.EndDate?.ToShortDateString();

                // 問題控制項繫結
                this.ddlCategoryList.DataTextField = "CategoryName";
                this.ddlCategoryList.DataValueField = "CategoryName";
                this.ddlCategoryList.DataSource = categoryList;
                this.ddlCategoryList.DataBind();
                this.ddlCategoryList.ClearSelection();
                this.ddlCategoryList.Items.FindByValue(firstQuestion.QuestionCategory).Selected = true;

                this.ddlTypingList.DataTextField = "TypingName";
                this.ddlTypingList.DataValueField = "TypingName";
                this.ddlTypingList.DataSource = typingList;
                this.ddlTypingList.DataBind();
                this.ddlTypingList.ClearSelection();
                this.ddlTypingList.Items.FindByValue(firstQuestion.QuestionTyping).Selected = true;
            }
        }

        private bool CheckQuestionnaireInputs(out List<string> errorMsgList)
        {
            errorMsgList = new List<string>();

            if (string.IsNullOrWhiteSpace(this.txtCaption.Text))
                errorMsgList.Add("請填入問卷名稱。");

            if (string.IsNullOrWhiteSpace(this.txtDescription.Text))
                errorMsgList.Add("請填入描述內容。");

            if (string.IsNullOrWhiteSpace(this.txtStartDate.Text))
                errorMsgList.Add("請填入開始時間。");

            if (errorMsgList.Count > 0)
                return false;
            else
                return true;
        }

        private Questionnaire FinalUpdateQuestionnaire(Questionnaire questionnaire)
        {
            Questionnaire newOrUpdatedQuestionnaire = new Questionnaire()
            {
                QuestionnaireID = questionnaire.QuestionnaireID,
                Caption = questionnaire.Caption,
                Description = questionnaire.Description,
                IsEnable = questionnaire.IsEnable,
                StartDate = questionnaire.StartDate,
                EndDate = questionnaire.EndDate,
                CreateDate = questionnaire.CreateDate,
                UpdateDate = questionnaire.UpdateDate,
            };

            return newOrUpdatedQuestionnaire;
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