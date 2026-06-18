using System;
using System.Data;
using System.Collections;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace CSSFriendly
{
    public class LinkButtonAdapter : System.Web.UI.WebControls.Adapters.WebControlAdapter
    {
        private WebControlAdapterExtender _extender = null;
        private WebControlAdapterExtender Extender
        {
            get
            {
                if (((_extender == null) && (Control != null)) ||
                    ((_extender != null) && (Control != _extender.AdaptedControl)))
                {
                    _extender = new WebControlAdapterExtender(Control);
                }

                System.Diagnostics.Debug.Assert(_extender != null, "Adapters internal error", "Null extender instance");
                return _extender;
            }
        }

        protected override void RenderBeginTag(HtmlTextWriter writer)
        {
            if (Extender.AdapterEnabled)
            {
                //  The LinkButton Adapter is very simple INPUT or A tag so we don't wrap it with an begin/end tag (e.g., no DIV wraps it).
            }
            else
            {
                base.RenderBeginTag(writer);
            }
        }

        protected override void RenderEndTag(HtmlTextWriter writer)
        {
            if (Extender.AdapterEnabled)
            {
                //  The LinkButton Adapter is very simple INPUT or A tag so we don't wrap it with an begin/end tag (e.g., no DIV wraps it).
            }
            else
            {
                base.RenderEndTag(writer);
            }
        }

        protected override void RenderContents(HtmlTextWriter writer)
        {
            if (Extender.AdapterEnabled)
            {
                LinkButton linkButton = Control as LinkButton;
                if (linkButton != null)
                {
                    string className = (!String.IsNullOrEmpty(linkButton.CssClass)) ? (linkButton.CssClass) : "AspNet-LinkButton";

                    Control ctl = linkButton.FindControl("ctl00");
                    if (ctl != null)
                    {
                        writer.WriteBeginTag("a");
                        writer.WriteAttribute("id", linkButton.ClientID);
                        writer.WriteAttribute("title", linkButton.ToolTip);
                        writer.WriteAttribute("class", className);
                        writer.WriteAttribute("href", Page.ClientScript.GetPostBackClientHyperlink(linkButton, ""));
                        writer.Write(HtmlTextWriter.TagRightChar);
                        writer.WriteBeginTag("span");
						writer.WriteAttribute("class", "AspNet-LinkButton-Text");
                        writer.Write(HtmlTextWriter.TagRightChar);
                        writer.Write(linkButton.Text);
                        writer.WriteEndTag("span");
                        writer.WriteEndTag("a"); Page.ClientScript.RegisterForEventValidation(linkButton.UniqueID);
                    }
                    else
                    {
                        base.RenderContents(writer);
                    }
                }
            }
        }
    }
}
