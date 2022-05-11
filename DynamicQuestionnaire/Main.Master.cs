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
        protected void Page_Load(object sender, EventArgs e)
        {
            string currentRawUrl = this.Request.RawUrl;
            bool isQuestionnaireList = currentRawUrl.Contains("QuestionnaireList.aspx");

            if (isQuestionnaireList)
            {
                this.plcToggleFormSidebar.Visible = true;
                this.formMain.Attributes["class"] = "col-md-8 offset-md-3";
            }
            else
            {
                this.plcToggleFormSidebar.Visible = false;
                this.formMain.Attributes["class"] = "col-md-10";
            }
        }
    }
}