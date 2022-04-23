using DynamicQuestionnaire.DynamicQuestionnaire.ORM;
using DynamicQuestionnaire.Managers;
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
            }
            else
                this.InitCreateMode();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            this.Response.Redirect("CommonQuestionList.aspx", true);
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (!this.CheckCommonQuestionInputs(out string errorMsg))
            {
                this.AlertMessage(errorMsg);
                return;
            }

            CommonQuestion commonQuestion = this.Session[_commonQuestion] as CommonQuestion;
            if (commonQuestion == null)
            {
                this.AlertMessage("請填寫常用問題名稱與問題後，先按下加入按鈕。");
                return;
            }

            this.CreateCommonQuestion(commonQuestion);

            List<Question> questionListOfCommonQuestion =
                this.Session[_questionListOfCommonQuestion] as List<Question>;
            if (questionListOfCommonQuestion == null || questionListOfCommonQuestion.Count == 0)
            {
                this.AlertMessage("請至少加入一個問題。");
                return;
            }

            this._commonQuestionMgr.CreateCommonQuestion(commonQuestion);
            this._questionMgr.CreateQuestionList(questionListOfCommonQuestion);

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
            var typingList = this._typingMgr.GetTypingList();

            // 問題控制項繫結
            this.ddlCategoryList.DataTextField = "CategoryName";
            this.ddlCategoryList.DataValueField = "CategoryName";
            this.ddlCategoryList.DataSource = categoryList;
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
            var commonQuestion = this._questionMgr.GetQuestionOfCommonQuestion(commonQuestionID);

            if (commonQuestion == null)
            {
                this.AlertMessage("查無此常用問題");
                this.Response.Redirect("CommonQuestionList.aspx", true);
            }
            else
            {
                var categoryList = this._categoryMgr.GetCategoryList();
                var typingList = this._typingMgr.GetTypingList();
                var questionList = this._questionMgr.GetQuestionListOfCommonQuestion(commonQuestionID);
                var firstQuestion = questionList.FirstOrDefault();

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

        private bool CheckCommonQuestionInputs(out string errorMsg)
        {
            errorMsg = "";

            if (string.IsNullOrWhiteSpace(this.txtCommonQuestionName.Text))
                errorMsg = "請填入常用問題名稱。";

            if (!string.IsNullOrWhiteSpace(errorMsg))
                return false;
            else
                return true;
        }

        private void CreateCommonQuestion(CommonQuestion commonQuestion)
        {
            CommonQuestion newCommonQuestion = new CommonQuestion() 
            {
                CommonQuestionID = commonQuestion.CommonQuestionID,
                CommonQuestionName = commonQuestion.CommonQuestionName,
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now,
            };

            this.Session[_commonQuestion] = newCommonQuestion;
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