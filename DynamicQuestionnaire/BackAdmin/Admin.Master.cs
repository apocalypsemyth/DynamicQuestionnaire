using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DynamicQuestionnaire.BackAdmin
{
    public partial class Admin : System.Web.UI.MasterPage
    {
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

        protected void Page_Load(object sender, EventArgs e)
        {
            string currentRawUrl = this.Request.RawUrl;
            bool isQuestionnaireList = currentRawUrl.Contains("QuestionnaireList.aspx");
            bool isCommonQuestionList = currentRawUrl.Contains("CommonQuestionList.aspx");

            if (isQuestionnaireList || isCommonQuestionList)
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
            }
        }
    }
}