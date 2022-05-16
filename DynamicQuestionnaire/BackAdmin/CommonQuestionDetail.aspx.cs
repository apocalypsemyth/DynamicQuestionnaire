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
        private string _isPostBack = "IsPostBack";
        private string _isUpdateMode = "IsUpdateMode";
        private string _commonQuestion = "CommonQuestion";
        private string _questionListOfCommonQuestion = "QuestionListOfCommonQuestion";

        private CategoryManager _categoryMgr = new CategoryManager();
        private TypingManager _typingMgr = new TypingManager();
        private CommonQuestionManager _commonQuestionMgr = new CommonQuestionManager();
        private QuestionManager _questionMgr = new QuestionManager();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Session[_isPostBack] == null)
                this.Session[_isPostBack] = false;

            if (!(bool)this.Session[_isPostBack])
                Session["update"] = Server.UrlEncode(DateTime.Now.ToString());

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

                if (_isEditMode)
                {
                    Guid commonQuestionID = this.GetCommonQuestionIDOrBackToList();
                    this.InitEditMode(commonQuestionID);
                    this.btnSubmit.Attributes.Add(
                        "onClick", 
                        "return SubmitCommonQuestionForServer('UPDATE');"
                        );
                }
                else
                {
                    this.InitCreateMode();
                    ClientScript.RegisterStartupScript(
                        this.GetType(), 
                        "ResetCommonQuestionDetailInputsForServer", 
                        "ResetCommonQuestionDetailInputsForServer();",
                        true
                        );
                    this.btnSubmit.Attributes.Add(
                        "onClick",
                        "return SubmitCommonQuestionForServer('CREATE');"
                        );
                }
            }
            else if (this.Session[_isUpdateMode] != null)
            {
                bool isUpdateMode = (bool)this.Session[_isUpdateMode];
                _isEditMode = isUpdateMode;

                if (_isEditMode)
                {
                    Guid commonQuestionID = this.GetCommonQuestionIDOrBackToList();
                    this.InitEditMode(commonQuestionID);
                }
                else
                    this.InitCreateMode();
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            this.SameLogicOfBackToList();
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (Session["update"].ToString() == ViewState["update"].ToString())
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
                        this.Session[_isPostBack] = false;
                        return;
                    }

                    if (toUpdateQuestionModelListOfCommonQuestion.All(item => item.IsDeleted))
                    {
                        this.AlertMessage("問題不能全空，請填寫或留下至少一個問題。");

                        foreach (var toUpdateQuestionModel in toUpdateQuestionModelListOfCommonQuestion)
                            toUpdateQuestionModel.IsDeleted = false;

                        this.Session[_questionListOfCommonQuestion] = toUpdateQuestionModelListOfCommonQuestion;
                        this.Session[_isPostBack] = false;
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
                }
                else
                {
                    List<Question> newQuestionListOfCommonQuestion =
                        this.Session[_questionListOfCommonQuestion] as List<Question>;
                    if (newQuestionListOfCommonQuestion == null
                        || newQuestionListOfCommonQuestion.Count == 0)
                    {
                        this.AlertMessage("請填寫至少一個問題。");
                        this.Session[_isPostBack] = false;
                        return;
                    }

                    this._commonQuestionMgr.CreateCommonQuestion(newOrToUpdateCommonQuestion);
                    this._questionMgr.CreateQuestionList(newQuestionListOfCommonQuestion);
                    this._categoryMgr.CreateCategoryOfCommonQuestion(newOrToUpdateCommonQuestion);
                }

                this.SameLogicOfBackToList();

                Session["update"] = Server.UrlEncode(DateTime.Now.ToString());
            }
            else
                return;
        }

        protected override void OnPreRender(EventArgs e)
        {
            ViewState["update"] = Session["update"];
        }

        private void SameLogicOfBackToList()
        {
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
            var resultCategoryList =
                this.CheckCategoryExist(categoryList)
                ? categoryList
                : this._categoryMgr.GetCategoryList();
            var onlyCommonQuestionCategoryList =
                resultCategoryList
                .Where(category => category.CategoryName == "常用問題"
                && category.CommonQuestionID == null);
            var typingList = this._typingMgr.GetTypingList();
            var resultTypingList =
                this.CheckTypingExist(typingList)
                ? typingList
                : this._typingMgr.GetTypingList();

            // 問題控制項繫結
            this.ddlCategoryList.DataTextField = "CategoryName";
            this.ddlCategoryList.DataValueField = "CategoryName";
            this.ddlCategoryList.DataSource = onlyCommonQuestionCategoryList;
            this.ddlCategoryList.DataBind();
            this.ddlCategoryList.ClearSelection();
            this.ddlCategoryList.Items.FindByValue("常用問題").Selected = true;

            this.ddlTypingList.DataTextField = "TypingName";
            this.ddlTypingList.DataValueField = "TypingName";
            this.ddlTypingList.DataSource = resultTypingList;
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
                var resultCategoryList =
                    this.CheckCategoryExist(categoryList)
                    ? categoryList
                    : this._categoryMgr.GetCategoryList();
                var onlyCommonQuestionCategoryList = 
                    resultCategoryList
                    .Where(category => category.CategoryName == "常用問題" 
                    && category.CommonQuestionID == null);
                var currentCommonQuestionItsCategoryName =
                    onlyCommonQuestionCategoryList
                    .SingleOrDefault().CategoryName;
                var typingList = this._typingMgr.GetTypingList();
                var resultTypingList =
                    this.CheckTypingExist(typingList)
                    ? typingList
                    : this._typingMgr.GetTypingList();
                var questionListOfCommonQuestion = 
                    this._questionMgr.GetQuestionListOfCommonQuestion(commonQuestionID);

                this.txtCommonQuestionName.Text = commonQuestion.CommonQuestionName;

                // 問題控制項繫結
                this.ddlCategoryList.DataTextField = "CategoryName";
                this.ddlCategoryList.DataValueField = "CategoryName";
                this.ddlCategoryList.DataSource = onlyCommonQuestionCategoryList;
                this.ddlCategoryList.DataBind();
                this.ddlCategoryList.ClearSelection();
                this.ddlCategoryList.Items.FindByValue(currentCommonQuestionItsCategoryName).Selected = true;

                this.ddlTypingList.DataTextField = "TypingName";
                this.ddlTypingList.DataValueField = "TypingName";
                this.ddlTypingList.DataSource = resultTypingList;
                this.ddlTypingList.DataBind();
                this.ddlTypingList.ClearSelection();
                this.ddlTypingList.Items.FindByValue("單選方塊").Selected = true;
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