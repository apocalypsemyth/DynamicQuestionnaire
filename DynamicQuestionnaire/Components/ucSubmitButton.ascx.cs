using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DynamicQuestionnaire.Components
{
    public partial class ucSubmitButton : System.Web.UI.UserControl
    {
        public event EventHandler OnSubmitClick = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.btnSubmit.Attributes.Add("onClick", "return SubmitQuestionnaire();");
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (this.OnSubmitClick != null)
                this.OnSubmitClick(this, e);
        }
    }
}