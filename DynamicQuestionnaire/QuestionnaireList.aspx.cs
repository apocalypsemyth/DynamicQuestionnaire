using DynamicQuestionnaire.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DynamicQuestionnaire
{
    public partial class QuestionnaireList : System.Web.UI.Page
    {
        private const int _pageSize = 10;
        private QuestionnaireManager _questionnaireMgr = new QuestionnaireManager();

        protected void Page_Load(object sender, EventArgs e)
        {
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

                var questionnaireList = this._questionnaireMgr
                    .GetQuestionnaireList(keyword, _pageSize, pageIndex, out int totalRows);

                this.ucPager.TotalRows = totalRows;
                this.ucPager.PageIndex = pageIndex;
                this.ucPager.Bind("Keyword", keyword);

                if (questionnaireList.Count == 0)
                {
                    this.gvQuestionnaireList.Visible = false;
                    this.plcEmpty.Visible = true;
                }
                else
                {
                    this.gvQuestionnaireList.Visible = true;
                    this.plcEmpty.Visible = false;

                    this.gvQuestionnaireList.DataSource = questionnaireList;
                    this.gvQuestionnaireList.DataBind();
                }
            }
        }

        protected void btnSearchQuestionnaire_Click(object sender, EventArgs e)
        {
            string keyword = this.txtKeyword.Text.Trim();

            if (string.IsNullOrWhiteSpace(keyword))
                this.Response.Redirect("QuestionnaireList.aspx");
            else
                this.Response.Redirect("QuestionnaireList.aspx?Keyword=" + keyword);
        }
    }
}