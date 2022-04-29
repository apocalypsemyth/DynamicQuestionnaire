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
        private string _isUpdateModeOfCommonQuestion = "IsUpdateModeOfCommonQuestion";

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
                this.btnSubmitInQuestionnaireTab.Attributes.Add(
                    "onClick",
                    "return SubmitQuestionnaireAndItsQuestionList('UPDATE');"
                    );
                this.btnSubmitInQuestionTab.Attributes.Add(
                    "onClick",
                    "return SubmitQuestionnaireAndItsQuestionList('UPDATE');"
                    );
            }
            else
            {
                this.InitCreateMode();
                this.btnSubmitInQuestionnaireTab.Attributes.Add(
                    "onClick", 
                    "return SubmitQuestionnaireAndItsQuestionList('CREATE');"
                    );
                this.btnSubmitInQuestionTab.Attributes.Add(
                    "onClick", 
                    "return SubmitQuestionnaireAndItsQuestionList('CREATE');"
                    );
            }

            if (!this.IsPostBack)
                this.Session[_isUpdateModeOfCommonQuestion] = false;

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

        protected void btnSubmitInQuestionnaireTab_Click(object sender, EventArgs e)
        {
            this.SameLogicOfBtnSubmit_Click(sender, e);
        }

        protected void btnSubmitInQuestionTab_Click(object sender, EventArgs e)
        {
            this.SameLogicOfBtnSubmit_Click(sender, e);
        }

        private void SameLogicOfBtnSubmit_Click(
            object sender,
            EventArgs e
            )
        {
            bool isUpdateModeOfCommonQuestion = (bool)(this.Session[_isUpdateModeOfCommonQuestion]);

            if (isUpdateModeOfCommonQuestion)
            {
                this.CreateQuestionnaireInSessionInIsUpdateModeOfCommonQuestion();
            }

            Questionnaire newOrToUpdateQuestionnaire = this.Session[_questionnaire] as Questionnaire;

            if (_isEditMode || isUpdateModeOfCommonQuestion)
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

                if (isUpdateModeOfCommonQuestion)
                {
                    Guid questionnaireID = newOrToUpdateQuestionnaire.QuestionnaireID;
                    
                    this._questionMgr
                        .UpdateQuestionListInIsUpdateModeOfCommonQuestion(
                        questionnaireID, 
                        toUpdateQuestionModelList
                        );

                    this._questionnaireMgr
                        .UpdateQuestionnaireInIsUpdateModeOfCommonQuestion(newOrToUpdateQuestionnaire);
                }
                else
                {
                    this._questionMgr.UpdateQuestionList(
                        toUpdateQuestionModelList, 
                        out bool hasAnyUpdated
                        );

                    if (hasAnyUpdated)
                        newOrToUpdateQuestionnaire.UpdateDate = DateTime.Now;

                    this._questionnaireMgr.UpdateQuestionnaire(newOrToUpdateQuestionnaire);
                }
            }
            else if (!(_isEditMode && isUpdateModeOfCommonQuestion))
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
            this.Session.Remove(_isUpdateModeOfCommonQuestion);

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
            var excludeCommonQuestionCategoryList = 
                categoryList
                .Where(item => item.CategoryName != "常用問題");
            var typingList = this._typingMgr.GetTypingList();

            // 問卷控制項繫結
            this.txtStartDate.Text = DateTime.Now.ToShortDateString();
            this.ckbIsEnable.Checked = true;

            // 問題控制項繫結
            this.ddlCategoryList.DataTextField = "CategoryName";
            this.ddlCategoryList.DataValueField = "CategoryID";
            this.ddlCategoryList.DataSource = excludeCommonQuestionCategoryList;
            this.ddlCategoryList.DataBind();
            this.ddlCategoryList.ClearSelection();
            this.ddlCategoryList.Items.FindByText("自訂問題").Selected = true;

            this.ddlTypingList.DataTextField = "TypingName";
            this.ddlTypingList.DataValueField = "TypingName";
            this.ddlTypingList.DataSource = typingList;
            this.ddlTypingList.DataBind();
            this.ddlTypingList.ClearSelection();
            this.ddlTypingList.Items.FindByValue("單選方塊").Selected = true;

            this.ckbQuestionRequired.Checked = false;
            this.btnExportAndDownloadDataToCSV.Visible = false;
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
                var excludeCommonQuestionCategoryList =
                    categoryList
                    .Where(item => item.CategoryName != "常用問題");
                var typingList = this._typingMgr.GetTypingList();
                var userList = this._userMgr.GetUserList(questionnaireID);

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
                this.ddlCategoryList.DataValueField = "CategoryID";
                this.ddlCategoryList.DataSource = excludeCommonQuestionCategoryList;
                this.ddlCategoryList.DataBind();
                this.ddlCategoryList.ClearSelection();
                this.ddlCategoryList.Items.FindByText("自訂問題").Selected = true;

                this.ddlTypingList.DataTextField = "TypingName";
                this.ddlTypingList.DataValueField = "TypingName";
                this.ddlTypingList.DataSource = typingList;
                this.ddlTypingList.DataBind();
                this.ddlTypingList.ClearSelection();
                this.ddlTypingList.Items.FindByValue("單選方塊").Selected = true;

                if (userList == null || userList.Count == 0)
                    this.btnExportAndDownloadDataToCSV.Visible = false;
                else
                    this.btnExportAndDownloadDataToCSV.Visible = true;
            }
        }

        private void CreateQuestionnaireInSessionInIsUpdateModeOfCommonQuestion()
        {
            Questionnaire newQuestionnaire = new Questionnaire()
            {
                QuestionnaireID = Guid.NewGuid(),
                Caption = this.txtCaption.Text.Trim(),
                Description = this.txtDescription.Text.Trim(),
                IsEnable = this.ckbIsEnable.Checked,
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now,
            };

            if (DateTime.TryParse(this.txtStartDate.Text, out DateTime startDate))
                newQuestionnaire.StartDate = startDate;

            if (!string.IsNullOrWhiteSpace(this.txtEndDate.Text))
            {
                if (DateTime.TryParse(this.txtEndDate.Text, out DateTime endDate))
                    newQuestionnaire.EndDate = endDate;
            }

            this.Session[_questionnaire] = newQuestionnaire;
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