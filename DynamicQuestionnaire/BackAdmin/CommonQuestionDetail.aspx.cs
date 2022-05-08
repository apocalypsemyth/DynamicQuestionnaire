using DynamicQuestionnaire.DynamicQuestionnaire.ORM;
using DynamicQuestionnaire.Managers;
using DynamicQuestionnaire.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DynamicQuestionnaire.BackAdmin
{
    public partial class CommonQuestionDetail : System.Web.UI.Page
    {
        private bool _isEditMode = false;

        // Session name
        private string _isUpdateMode = "IsUpdateMode";
        private string _commonQuestion = "CommonQuestion";
        private string _questionListOfCommonQuestion = "QuestionListOfCommonQuestion";

        private CategoryManager _categoryMgr = new CategoryManager();
        private TypingManager _typingMgr = new TypingManager();
        private CommonQuestionManager _commonQuestionMgr = new CommonQuestionManager();
        private QuestionManager _questionMgr = new QuestionManager();

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
                Guid commonQuestionID = this.GetCommonQuestionIDOrBackToList();
                this.InitEditMode(commonQuestionID);
                this.btnSubmit.Attributes.Add(
                    "onClick", 
                    "return SubmitCommonQuestionAndItsQuestionList('UPDATE');"
                    );
            }
            else
            {
                this.InitCreateMode();
                this.btnSubmit.Attributes.Add(
                    "onClick",
                    "return SubmitCommonQuestionAndItsQuestionList('CREATE');"
                    );
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            this.SameLogicOfRemoveSessionAndBackToList();
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            CommonQuestion newOrToUpdateCommonQuestion = 
                this.Session[_commonQuestion] as CommonQuestion;

            if (_isEditMode)
            {
                List<QuestionModel> toUpdateQuestionModelListOfCommonQuestion = 
                    this.Session[_questionListOfCommonQuestion] as List<QuestionModel>;

                if (toUpdateQuestionModelListOfCommonQuestion == null 
                    || toUpdateQuestionModelListOfCommonQuestion.Count == 0)
                {
                    this.AlertMessage("請填寫至少一個問題。");
                    return;
                }

                if (toUpdateQuestionModelListOfCommonQuestion.All(item => item.IsDeleted))
                {
                    this.AlertMessage("問題不能全空，請填寫或留下至少一個問題。");

                    foreach (var toUpdateQuestionModel in toUpdateQuestionModelListOfCommonQuestion)
                        toUpdateQuestionModel.IsDeleted = false;

                    this.Session[_questionListOfCommonQuestion] = toUpdateQuestionModelListOfCommonQuestion;
                    return;
                }

                this._questionMgr.UpdateQuestionList(
                    toUpdateQuestionModelListOfCommonQuestion, 
                    out bool hasAnyUpdated
                    );

                if (hasAnyUpdated)
                    newOrToUpdateCommonQuestion.UpdateDate = DateTime.Now;

                this._commonQuestionMgr.UpdateCommonQuestion(newOrToUpdateCommonQuestion);
                this._categoryMgr.UpdateCategoryByCommonQuestion(newOrToUpdateCommonQuestion);

                this.SameLogicOfRemoveSessionAndBackToList();
            }
            else
            {
                List<Question> newQuestionListOfCommonQuestion = 
                    this.Session[_questionListOfCommonQuestion] as List<Question>;
                if (newQuestionListOfCommonQuestion == null 
                    || newQuestionListOfCommonQuestion.Count == 0)
                {
                    this.AlertMessage("請填寫至少一個問題。");
                    return;
                }

                this._commonQuestionMgr.CreateCommonQuestion(newOrToUpdateCommonQuestion);
                this._questionMgr.CreateQuestionList(newQuestionListOfCommonQuestion);
                this._categoryMgr.CreateCategoryOfCommonQuestion(newOrToUpdateCommonQuestion);

                this.SameLogicOfRemoveSessionAndBackToList();
            }
        }

        private void SameLogicOfRemoveSessionAndBackToList()
        {
            this.Session.Remove(_isUpdateMode);
            this.Session.Remove(_commonQuestion);
            this.Session.Remove(_questionListOfCommonQuestion);

            this.Response.Redirect("CommonQuestionList.aspx", true);
        }

        protected Guid GetCommonQuestionIDOrBackToList()
        {
            string commonQuestionIDStr = this.Request.QueryString["ID"];

            bool isValidCommonQuestionID = Guid.TryParse(commonQuestionIDStr, out Guid commonQuestionID);
            if (!isValidCommonQuestionID)
                this.Response.Redirect("CommonQuestionList.aspx", true);

            return commonQuestionID;
        }

        private void InitCreateMode()
        {
            var categoryList = this._categoryMgr.GetCategoryList();
            var onlyCommonQuestionCategoryList = categoryList
                    .Where(category => category.CategoryName == "常用問題" 
                    && category.CommonQuestionID == null);
            var typingList = this._typingMgr.GetTypingList();

            // 問題控制項繫結
            this.ddlCategoryList.DataTextField = "CategoryName";
            this.ddlCategoryList.DataValueField = "CategoryName";
            this.ddlCategoryList.DataSource = onlyCommonQuestionCategoryList;
            this.ddlCategoryList.DataBind();
            this.ddlCategoryList.ClearSelection();
            this.ddlCategoryList.Items.FindByValue("常用問題").Selected = true;

            this.ddlTypingList.DataTextField = "TypingName";
            this.ddlTypingList.DataValueField = "TypingName";
            this.ddlTypingList.DataSource = typingList;
            this.ddlTypingList.DataBind();
            this.ddlTypingList.ClearSelection();
            this.ddlTypingList.Items.FindByValue("單選方塊").Selected = true;

            this.ckbQuestionRequiredOfCommonQuestion.Checked = false;
        }

        private void InitEditMode(Guid commonQuestionID)
        {
            var commonQuestion = this._commonQuestionMgr.GetCommonQuestion(commonQuestionID);

            if (commonQuestion == null)
            {
                this.AlertMessage("查無此常用問題");
                this.Response.Redirect("CommonQuestionList.aspx", true);
            }
            else
            {
                var categoryList = this._categoryMgr.GetCategoryList();
                var excludeCustomizedQuestionCategoryList = categoryList
                    .Where(category => category.CategoryName == "常用問題" 
                    && category.CommonQuestionID == null);
                var currentCommonQuestionItsCategoryName =
                    excludeCustomizedQuestionCategoryList
                    .SingleOrDefault().CategoryName;

                var typingList = this._typingMgr.GetTypingList();
                var questionListOfCommonQuestion = 
                    this._questionMgr.GetQuestionListOfCommonQuestion(commonQuestionID);

                this.txtCommonQuestionName.Text = commonQuestion.CommonQuestionName;

                // 問題控制項繫結
                this.ddlCategoryList.DataTextField = "CategoryName";
                this.ddlCategoryList.DataValueField = "CategoryName";
                this.ddlCategoryList.DataSource = excludeCustomizedQuestionCategoryList;
                this.ddlCategoryList.DataBind();
                this.ddlCategoryList.ClearSelection();
                this.ddlCategoryList.Items.FindByValue(currentCommonQuestionItsCategoryName).Selected = true;

                this.ddlTypingList.DataTextField = "TypingName";
                this.ddlTypingList.DataValueField = "TypingName";
                this.ddlTypingList.DataSource = typingList;
                this.ddlTypingList.DataBind();
                this.ddlTypingList.ClearSelection();
                this.ddlTypingList.Items.FindByValue("單選方塊").Selected = true;
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