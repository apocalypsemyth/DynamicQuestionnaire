using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DynamicQuestionnaire
{
    public partial class Main : System.Web.UI.MasterPage
    {
        // Session name of QuestionnaireDetail
        private string _isEnable = "IsEnable";

        // Session name of CheckingQuestionnaireDetail
        private string _user = "User";
        private string _userAnswer = "UserAnswer";

        protected void Page_Load(object sender, EventArgs e)
        {
            string currentRawUrl = this.Request.RawUrl;
            bool isQuestionnaireList = currentRawUrl.Contains("QuestionnaireList.aspx");

            if (isQuestionnaireList)
            {
                this.plcToggleFormSidebar.Visible = true;
                this.formMain.Attributes["class"] = "col-md-8 offset-md-3";

                // Session name of QuestionnaireDetail
                this.Session.Remove(_isEnable);

                // Session name of CheckingQuestionnaireDetail
                this.Session.Remove(_user);
                this.Session.Remove(_userAnswer);
            }
            else
            {
                this.plcToggleFormSidebar.Visible = false;
                this.formMain.Attributes["class"] = "col-md-10";
            }
        }
    }
}