using DynamicQuestionnaire.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DynamicQuestionnaire.BackAdmin
{
    public partial class CommonQuestionList : System.Web.UI.Page
    {
        private const int _pageSize = 10;

        // Session name of QuestionnaireDetail or CommonQuestionDetail
        private string _isUpdateMode = "IsUpdateMode";

        // Session name of QuestionnaireDetail
        private string _questionnaire = "Questionnaire";
        private string _questionList = "QuestionList";
        private string _currentPagerIndex = "CurrentPagerIndex";
        private string _isSetCommonQuestionOnQuestionnaire = "IsSetCommonQuestionOnQuestionnaire";

        // Session name of CommonQuestionDetail
        private string _commonQuestion = "CommonQuestion";
        private string _questionListOfCommonQuestion = "QuestionListOfCommonQuestion";

        private CommonQuestionManager _commonQuestionMgr = new CommonQuestionManager();
        private QuestionManager _questionMgr = new QuestionManager();
        private CategoryManager _categoryMgr = new CategoryManager();

        protected void Page_Load(object sender, EventArgs e)
        {
            // Session name of QuestionnaireDetail or CommonQuestionDetail
            this.Session.Remove(_isUpdateMode);

            // Session name of QuestionnaireDetail
            this.Session.Remove(_questionnaire);
            this.Session.Remove(_questionList);
            this.Session.Remove(_currentPagerIndex);
            this.Session.Remove(_isSetCommonQuestionOnQuestionnaire);

            // Session name of CommonQuestionDetail
            this.Session.Remove(_commonQuestion);
            this.Session.Remove(_questionListOfCommonQuestion);

            string pageIndexStr = this.Request.QueryString["Index"];
            int pageIndex =
                (string.IsNullOrWhiteSpace(pageIndexStr))
                    ? 1
                    : int.Parse(pageIndexStr);

            if (!this.IsPostBack)
            {
                string keyword = this.Request.QueryString["Keyword"];
                if (!string.IsNullOrWhiteSpace(keyword))
                    this.txtKeyword.Text = keyword;

                var commonQuestionList = this._commonQuestionMgr
                    .GetCommonQuestionList(keyword, _pageSize, pageIndex, out int totalRows);

                this.ucPager.TotalRows = totalRows;
                this.ucPager.PageIndex = pageIndex;
                this.ucPager.Bind("Keyword", keyword);

                if (commonQuestionList.Count == 0)
                {
                    this.gvCommonQuestionList.Visible = false;
                    this.ucPager.Visible = false;
                    this.plcEmptyCommonQuestion.Visible = true;
                }
                else
                {
                    this.gvCommonQuestionList.Visible = true;
                    this.ucPager.Visible = true;
                    this.plcEmptyCommonQuestion.Visible = false;

                    this.gvCommonQuestionList.DataSource = commonQuestionList;
                    this.gvCommonQuestionList.DataBind();
                }
            }
        }

        protected void btnSearchCommonQuestion_Click(object sender, EventArgs e)
        {
            string keyword = this.txtKeyword.Text.Trim();

            if (string.IsNullOrWhiteSpace(keyword))
                this.Response.Redirect("CommonQuestionList.aspx");
            else
                this.Response.Redirect("CommonQuestionList.aspx?Keyword=" + keyword);
        }

        protected void btnDeleteCommonQuestion_Click(object sender, EventArgs e)
        {
            List<Guid> commonQuestionIDList = new List<Guid>();

            foreach (GridViewRow gRow in this.gvCommonQuestionList.Rows)
            {
                CheckBox ckbDeleteCommonQuestion = gRow.FindControl("ckbDeleteCommonQuestion") as CheckBox;
                HiddenField hfCommonQuestionID = gRow.FindControl("hfCommonQuestionID") as HiddenField;

                if (ckbDeleteCommonQuestion != null && hfCommonQuestionID != null)
                {
                    if (ckbDeleteCommonQuestion.Checked)
                    {
                        if (Guid.TryParse(hfCommonQuestionID.Value, out Guid commonQuestionID))
                            commonQuestionIDList.Add(commonQuestionID);
                    }
                }
            }

            if (commonQuestionIDList.Count > 0)
            {
                this._questionMgr.DeleteQuestionListOfCommonQuestionList(commonQuestionIDList);
                this._categoryMgr.DeleteCategoryListOfCommonQuestionList(commonQuestionIDList);
                this._commonQuestionMgr.DeleteCommonQuestionList(commonQuestionIDList);
                this.Response.Redirect(this.Request.RawUrl);
            }
        }

        protected void btnCreateCommonQuestion_Click(object sender, EventArgs e)
        {
            this.Response.Redirect("CommonQuestionDetail.aspx");
        }
    }
}