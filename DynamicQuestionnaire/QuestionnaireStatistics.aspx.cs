using DynamicQuestionnaire.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DynamicQuestionnaire
{
    public partial class QuestionnaireStatistics : System.Web.UI.Page
    {
        private QuestionnaireManager _questionnaireMgr = new QuestionnaireManager();

        protected void Page_Load(object sender, EventArgs e)
        {
            Guid questionnaireID = this.GetQuestionnaireIDOrBackToList();
            ClientScript.RegisterStartupScript(
                this.GetType(), 
                "GetQuestionnaireStatistics", 
                "GetQuestionnaireStatistics('" + questionnaireID + "')", 
                true
                );
            
            if (!this.IsPostBack)
            {
                var questionnaire = this._questionnaireMgr.GetQuestionnaire(questionnaireID);

                this.ltlCaption.Text = questionnaire.Caption;
            }
        }

        protected Guid GetQuestionnaireIDOrBackToList()
        {
            string questionnaireIDStr = this.Request.QueryString["ID"];

            bool isValidQuestionnaireID = Guid.TryParse(questionnaireIDStr, out Guid questionnaireID);
            if (!isValidQuestionnaireID)
                this.Response.Redirect("QuestionnaireList.aspx", true);

            return questionnaireID;
        }
    }
}