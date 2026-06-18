using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.CodeDom.Compiler;
using System.Text;
using RulesEngine.Website;


    public partial class WebPages_RuleSets_CompileError : BasePage
    {
        private SessionData sessionData = SessionData.Get();

        protected global::System.Web.UI.WebControls.TextBox errorText;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.errorText.Text = ErrorString();
            this.generatedCodeText.Text = HttpUtility.HtmlEncode(sessionData.GeneratedCode);
        }
        protected string ErrorString()
        {
            StringBuilder sb = new StringBuilder();
            CompilerErrorCollection errors = (CompilerErrorCollection)sessionData.CompilerErrors;
            for (int i = 0; i < errors.Count; i++)
            {
                sb.AppendLine(errors[i].ErrorText);
            }
            return sb.ToString();
        }
    }

