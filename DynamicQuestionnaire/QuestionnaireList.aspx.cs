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
    public partial class QuestionnaireList : System.Web.UI.Page
    {
        private int _questionListAmount { get; set; }
        private int _questionListPageIndex { get; set; }
        private const int _pageSize = 10;

        // Session name of QuestionnaireList
        private string _toSetIsEnableOfQuestionnaireList = "ToSetIsEnableOfQuestionnaireList";

        // Session name of QuestionnaireDetail
        private string _isEnable = "IsEnable";

        // Session name of CheckingQuestionnaireDetail
        private string _user = "User";
        private string _userAnswer = "UserAnswer";

        private QuestionnaireManager _questionnaireMgr = new QuestionnaireManager();

        protected void Page_Load(object sender, EventArgs e)
        {
            // Session name of QuestionnaireDetail
            this.Session.Remove(_isEnable);

            // Session name of CheckingQuestionnaireDetail
            this.Session.Remove(_user);
            this.Session.Remove(_userAnswer);

            string pageIndexStr = this.Request.QueryString["Index"];
            int pageIndex =
                (string.IsNullOrWhiteSpace(pageIndexStr))
                    ? 1
                    : int.Parse(pageIndexStr);

            if (!this.IsPostBack)
            {
                string keyword = this.Request.QueryString["Keyword"];
                string date = this.Request.QueryString["Date"];

                if (!string.IsNullOrWhiteSpace(keyword))
                    this.txtKeyword.Text = keyword;

                string startDate = "";
                string endDate = "";
                if (!string.IsNullOrWhiteSpace(date))
                {
                    string[] dateArr = date.Split('~');
                    startDate = dateArr.FirstOrDefault();
                    endDate = dateArr.LastOrDefault();

                    this.txtStartDate.Text = startDate;
                    this.txtEndDate.Text = endDate;
                }

                var questionnaireList = this._questionnaireMgr
                    .GetQuestionnaireList(
                    keyword, 
                    startDate, 
                    endDate, 
                    _pageSize, 
                    pageIndex, 
                    out int totalRows
                    );

                bool hasAnyNotStartOrOverEndDate = this.CheckDateOfQuestionnaireList(questionnaireList);
                var setOrNotSetQuestionnaireList =
                    hasAnyNotStartOrOverEndDate
                    ? this.SetIsEnableOfQuestionnaireList(questionnaireList)
                    : questionnaireList;

                this.ucPager.TotalRows = totalRows;
                this.ucPager.PageIndex = pageIndex;
                this.ucPager.Bind("Keyword", keyword);
                if (string.IsNullOrWhiteSpace(startDate) || string.IsNullOrWhiteSpace(endDate))
                    this.ucPager.Bind("Date", null);
                else
                    this.ucPager.Bind("Date", startDate + "~" + endDate);

                if (setOrNotSetQuestionnaireList.Count == 0)
                {
                    this.gvQuestionnaireList.Visible = false;
                    this.plcEmpty.Visible = true;
                }
                else
                {
                    this.gvQuestionnaireList.Visible = true;
                    this.plcEmpty.Visible = false;

                    this._questionListAmount = totalRows;
                    this._questionListPageIndex = pageIndex;
                    this.gvQuestionnaireList.DataSource = setOrNotSetQuestionnaireList;
                    this.gvQuestionnaireList.DataBind();
                }

                if (hasAnyNotStartOrOverEndDate)
                {
                    this._questionnaireMgr.SetIsEnableOfQuestionnaireList(
                        (List<Questionnaire>)this.Session[_toSetIsEnableOfQuestionnaireList]
                        );
                    this.Session.Remove(_toSetIsEnableOfQuestionnaireList);
                }
            }
        }

        protected void gvQuestionnaireList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Literal ltlQuestionListNumber = e.Row.FindControl("ltlQuestionListNumber") as Literal;
                string reverseQuestionListNumber =
                    (_questionListAmount -
                    ((_questionListPageIndex - 1) * _pageSize) -
                    e.Row.RowIndex)
                    .ToString();
                ltlQuestionListNumber.Text = reverseQuestionListNumber;
            }
        }
        
        protected void btnSearchQuestionnaire_Click(object sender, EventArgs e)
        {
            string keyword = this.txtKeyword.Text.Trim();
            string startDateStr = this.txtStartDate.Text.Trim();
            string endDateStr = this.txtEndDate.Text.Trim();

            Regex dateRegex = new Regex(@"^[0-9]{4}\/(0[1-9]|1[0-9])\/(0[1-9]|[1-2][0-9]|3[0-1])$");

            if (!string.IsNullOrWhiteSpace(keyword) 
                && (!string.IsNullOrWhiteSpace(startDateStr) || !string.IsNullOrWhiteSpace(endDateStr)))
            {
                this.AlertMessage("一次只能搜尋關鍵字或始末日期。");
                return;
            }
            else if (!string.IsNullOrWhiteSpace(keyword))
                this.Response.Redirect("QuestionnaireList.aspx?Keyword=" + keyword);
            else if (!string.IsNullOrWhiteSpace(startDateStr)
                || !string.IsNullOrWhiteSpace(endDateStr))
            {
                if (string.IsNullOrWhiteSpace(startDateStr))
                {
                    this.AlertMessage("請輸入要搜尋的開始日期。");
                    return;
                }
                else if (!dateRegex.IsMatch(startDateStr))
                {
                    this.AlertMessage(@"請輸入 ""yyyy/MM/dd"" 格式的開始時間。");
                    return;
                }

                if (string.IsNullOrWhiteSpace(endDateStr))
                {
                    this.AlertMessage("請輸入要搜尋的結束日期。");
                    return;
                }
                else if (!dateRegex.IsMatch(endDateStr))
                {
                    this.AlertMessage(@"請輸入 ""yyyy/MM/dd"" 格式的結束時間。");
                    return;
                }

                if (!this.CheckSearchDateTime(
                    startDateStr,
                    endDateStr,
                    out DateTime startDate,
                    out DateTime endDate,
                    out List<string> errorMsgList)
                    )
                {
                    string errorMsg = string.Join("\\n", errorMsgList);

                    this.AlertMessage(errorMsg);
                    return;
                }

                this.Response.Redirect(
                    "QuestionnaireList.aspx?Date="
                    + startDate.ToShortDateString()
                    + "~"
                    + endDate.ToShortDateString()
                    );
            }
            else
                this.AlertMessage("請輸入要搜尋的關鍵字或始末日期。");
        }

        private bool CheckSearchDateTime(
            string startDateStr,
            string endDateStr,
            out DateTime startDate,
            out DateTime endDate,
            out List<string> errorMsgList
            )
        {
            errorMsgList = new List<string>();

            if (!DateTime.TryParse(startDateStr, out DateTime startDateMayContainHrMinSec))
                errorMsgList.Add(@"請輸入合法的開始時間。");

            if (!DateTime.TryParse(endDateStr, out DateTime endDateMayContainHrMinSec))
                errorMsgList.Add(@"請輸入合法的結束時間。");
            else
            {
                if (startDateMayContainHrMinSec.Date > endDateMayContainHrMinSec.Date)
                    errorMsgList.Add("請輸入一前一後時序的始末日期。");
            }

            if (errorMsgList.Count > 0)
            {
                startDate = new DateTime(0001, 01, 01);
                endDate = new DateTime(0001, 01, 01);
                return false;
            }
            else
            {
                startDate = startDateMayContainHrMinSec.Date;
                endDate = endDateMayContainHrMinSec.Date;
                return true;
            }
        }

        private bool CheckDateOfQuestionnaireList(List<Questionnaire> questionnaireList)
        {
            bool hasAnyNotStartOrOverEndDate = false;

            if (questionnaireList == null || questionnaireList.Count == 0)
                return hasAnyNotStartOrOverEndDate;

            foreach (var questionnaire in questionnaireList)
            {
                if (questionnaire.StartDate.Date == DateTime.Now.Date
                    && questionnaire.IsEnable == false)
                {
                    hasAnyNotStartOrOverEndDate = true;
                    return hasAnyNotStartOrOverEndDate;
                }

                else if (questionnaire.EndDate != null
                    && questionnaire.EndDate?.Date < DateTime.Now.Date
                    && questionnaire.IsEnable == true)
                {
                    hasAnyNotStartOrOverEndDate = true;
                    return hasAnyNotStartOrOverEndDate;
                }
            }

            return hasAnyNotStartOrOverEndDate;
        }

        private List<Questionnaire> SetIsEnableOfQuestionnaireList(List<Questionnaire> questionnaireList)
        {
            this.Session[_toSetIsEnableOfQuestionnaireList] = new List<Questionnaire>();
            List<Questionnaire> toSetIsEnableOfQuestionnaireList =
                this.Session[_toSetIsEnableOfQuestionnaireList] as List<Questionnaire>;

            foreach (var questionnaire in questionnaireList)
            {
                if (questionnaire.StartDate.Date == DateTime.Now.Date)
                {
                    questionnaire.IsEnable = true;
                    toSetIsEnableOfQuestionnaireList.Add(questionnaire);
                }

                if (questionnaire.EndDate != null
                    && questionnaire.EndDate?.Date < DateTime.Now.Date)
                {
                    questionnaire.IsEnable = false;
                    toSetIsEnableOfQuestionnaireList.Add(questionnaire);
                }
            }

            return questionnaireList;
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