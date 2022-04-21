using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DynamicQuestionnaire.Components
{
    public partial class ucCancelButton : System.Web.UI.UserControl
    {
        public event EventHandler OnCancelClick = null;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            if (this.OnCancelClick != null)
                this.OnCancelClick(this, e);
        }
    }
}