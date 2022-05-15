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
        private string _isSetCommonQuestionOnQuestionnaire = "IsSetCommonQuestionOnQuestionnaire";
        
        private QuestionnaireManager _questionnaireMgr = new QuestionnaireManager();
        private CategoryManager _categoryMgr = new CategoryManager();
        private TypingManager _typingMgr = new TypingManager();
        private QuestionManager _questionMgr = new QuestionManager();
        private UserManager _userMgr = new UserManager();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Session[_isUpdateMode] == null)
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
            }
            else if (this.Session[_isUpdateMode] != null)
            {
                bool isUpdateMode = (bool)this.Session[_isUpdateMode];
                _isEditMode = isUpdateMode;
            }

            if (_isEditMode)
            {
                Guid questionnaireID = this.GetQuestionnaireIDOrBackToList();
                this.InitEditMode(questionnaireID);
            }
            else
                this.InitCreateMode();

            if (this.Session[_isSetCommonQuestionOnQuestionnaire] == null)
                this.Session[_isSetCommonQuestionOnQuestionnaire] = false;

            this.ucCancelButtonInQuestionnaireTab.OnCancelClick += UcInQuestionnaireTab_OnCancelClick;
            this.ucCancelButtonInQuestionTab.OnCancelClick += UcInQuestionTab_OnCancelClick;

            this.ucSubmitButtonInQuestionnaireTab.OnSubmitClick += UcInQuestionnaireTab_OnSubmitClick;
            this.ucSubmitButtonInQuestionTab.OnSubmitClick += UcInQuestionTab_OnSubmitClick;
        }
        
        protected void UcInQuestionnaireTab_OnCancelClick(object sender, EventArgs e)
        {
            this.SameLogicOfBackToList(sender, e);
        }

        protected void UcInQuestionTab_OnCancelClick(object sender, EventArgs e)
        {
            this.SameLogicOfBackToList(sender, e);
        }

        protected void UcInQuestionnaireTab_OnSubmitClick(object sender, EventArgs e)
        {
            this.SameLogicOfBtnSubmit_Click(sender, e);
        }
        
        protected void UcInQuestionTab_OnSubmitClick(object sender, EventArgs e)
        {
            this.SameLogicOfBtnSubmit_Click(sender, e);
        }

        private void SameLogicOfBtnSubmit_Click(object sender, EventArgs e)
        {
            bool isSetCommonQuestionOnQuestionnaire = 
                (bool)(this.Session[_isSetCommonQuestionOnQuestionnaire]);
            Questionnaire newOrToUpdateQuestionnaire = this.Session[_questionnaire] as Questionnaire;

            if (_isEditMode || isSetCommonQuestionOnQuestionnaire)
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

                    if (isSetCommonQuestionOnQuestionnaire)
                    {
                        foreach (var toUpdateQuestionModel in toUpdateQuestionModelList)
                        {
                            if (toUpdateQuestionModel.IsCreated == false)
                                toUpdateQuestionModel.IsDeleted = false;
                        }

                        this.Session[_questionList] = toUpdateQuestionModelList;
                    }

                    return;
                }

                if (isSetCommonQuestionOnQuestionnaire)
                {
                    foreach (var questionModel in toUpdateQuestionModelList)
                    {
                        questionModel.QuestionnaireID = newOrToUpdateQuestionnaire.QuestionnaireID;
                    }
                }

                this._questionMgr.UpdateQuestionList(
                    toUpdateQuestionModelList, 
                    out bool hasAnyUpdated
                    );

                if (isSetCommonQuestionOnQuestionnaire 
                    && toUpdateQuestionModelList.Where(item => item.QuestionCategory != "常用問題").Any()
                    && hasAnyUpdated)
                    newOrToUpdateQuestionnaire.UpdateDate = DateTime.Now;
                else if (!isSetCommonQuestionOnQuestionnaire && hasAnyUpdated)
                    newOrToUpdateQuestionnaire.UpdateDate = DateTime.Now;

                this._questionnaireMgr.UpdateQuestionnaire(
                    _isEditMode, 
                    isSetCommonQuestionOnQuestionnaire, 
                    newOrToUpdateQuestionnaire
                    );
            }
            else if (!(_isEditMode && isSetCommonQuestionOnQuestionnaire))
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

            this.SameLogicOfBackToList(sender, e);
        }

        private void SameLogicOfBackToList(object sender, EventArgs e)
        {
            this.Response.Redirect("QuestionnaireList.aspx", true);
        }
        
        protected void btnExportAndDownloadDataToCSV_Click(object sender, EventArgs e)
        {
            Guid questionnaireID = this.GetQuestionnaireIDOrBackToList();
            var csv = this._userMgr.ExportDataToCSV(questionnaireID);

            this.Response.Clear();
            this.Response.Buffer = true;
            this.Response.AddHeader(
                "content-disposition", 
                $"attachment;filename=UserAnswerList_{DateTime.Now}.csv"
                );
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
            var resultCategoryList = 
                this.CheckCategoryExist(categoryList) 
                ? categoryList 
                : this._categoryMgr.GetCategoryList();
            var typingList = this._typingMgr.GetTypingList();
            var resultTypingList = 
                this.CheckTypingExist(typingList) 
                ? typingList 
                : this._typingMgr.GetTypingList();

            // 問卷控制項繫結
            this.txtStartDate.Text = DateTime.Now.ToShortDateString();
            this.ckbIsEnable.Checked = true;

            // 問題控制項繫結
            this.ddlCategoryList.DataTextField = "CategoryName";
            this.ddlCategoryList.DataValueField = "CategoryID";
            this.ddlCategoryList.DataSource = resultCategoryList;
            this.ddlCategoryList.DataBind();
            this.ddlCategoryList.ClearSelection();
            this.ddlCategoryList.Items.FindByText("自訂問題").Selected = true;

            this.ddlTypingList.DataTextField = "TypingName";
            this.ddlTypingList.DataValueField = "TypingName";
            this.ddlTypingList.DataSource = resultTypingList;
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
                var resultCategoryList =
                    this.CheckCategoryExist(categoryList)
                    ? categoryList
                    : this._categoryMgr.GetCategoryList();
                var typingList = this._typingMgr.GetTypingList();
                var resultTypingList =
                    this.CheckTypingExist(typingList)
                    ? typingList
                    : this._typingMgr.GetTypingList();
                var questionList = this._questionMgr.GetQuestionListOfQuestionnaire(questionnaireID);
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
                this.ddlCategoryList.DataSource = resultCategoryList;
                this.ddlCategoryList.DataBind();
                this.ddlCategoryList.ClearSelection();
                if (questionList.Where(item => item.QuestionCategory == "常用問題").Any())
                {
                    ClientScript.RegisterStartupScript(
                        this.GetType(), 
                        "SetCommonQuestionOnQuestionnaireStateSessionForServer", 
                        "SetCommonQuestionOnQuestionnaireStateSessionForServer('set')", 
                        true
                        );
                    this.ddlCategoryList.Items.FindByText("常用問題").Selected = true;
                }
                else
                {
                    ClientScript.RegisterStartupScript(
                        this.GetType(),
                        "SetCommonQuestionOnQuestionnaireStateSessionForServer",
                        "SetCommonQuestionOnQuestionnaireStateSessionForServer('notSet')",
                        true
                        );
                    this.ddlCategoryList.Items.FindByText("自訂問題").Selected = true;
                }

                this.ddlTypingList.DataTextField = "TypingName";
                this.ddlTypingList.DataValueField = "TypingName";
                this.ddlTypingList.DataSource = resultTypingList;
                this.ddlTypingList.DataBind();
                this.ddlTypingList.ClearSelection();
                this.ddlTypingList.Items.FindByValue("單選方塊").Selected = true;

                if (userList == null || userList.Count == 0)
                    this.btnExportAndDownloadDataToCSV.Visible = false;
                else
                    this.btnExportAndDownloadDataToCSV.Visible = true;
            }
        }
        
        private bool CheckCategoryExist(List<Category> categoryList)
        {
            bool areAllCategoryExist = true;

            if (categoryList == null || categoryList.Count == 0)
            {
                this._categoryMgr.CreateCategory("自訂問題");
                this._categoryMgr.CreateCategory("常用問題");
                areAllCategoryExist = false;

                return areAllCategoryExist;
            }
            
            if (!categoryList.Where(item => item.CategoryName == "自訂問題").Any())
            {
                this._categoryMgr.CreateCategory("自訂問題");
                areAllCategoryExist = false;
            }
            
            if (!categoryList.Where(item => item.CategoryName == "自訂問題").Any())
            {
                this._categoryMgr.CreateCategory("常用問題");
                areAllCategoryExist = false;
            }

            return areAllCategoryExist;
        }

        private bool CheckTypingExist(List<Typing> typingList)
        {
            bool areAllTypingExist = true;

            if (typingList == null || typingList.Count == 0)
            {
                this._typingMgr.CreateTyping("單選方塊");
                this._typingMgr.CreateTyping("複選方塊");
                this._typingMgr.CreateTyping("文字");
                areAllTypingExist = false;

                return areAllTypingExist;
            }
            
            if (!typingList.Where(item => item.TypingName == "單選方塊").Any())
            {
                this._typingMgr.CreateTyping("單選方塊");
                areAllTypingExist = false;
            }
            
            if (!typingList.Where(item => item.TypingName == "複選方塊").Any())
            {
                this._typingMgr.CreateTyping("複選方塊");
                areAllTypingExist = false;
            }
            
            if (!typingList.Where(item => item.TypingName == "文字").Any())
            {
                this._typingMgr.CreateTyping("文字");
                areAllTypingExist = false;
            }

            return areAllTypingExist;
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