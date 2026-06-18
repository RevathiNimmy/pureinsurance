using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CSSFriendly
{
	/// <summary>
	/// Overrides the default table layout of the <see cref="System.Web.UI.WebControls.RadioButtonList"/>
	/// control, to a XHTML unordered list layout (structural markup).
	/// </summary>
	public class RadioButtonListAdapter : System.Web.UI.WebControls.Adapters.WebControlAdapter
	{
		private const string WRAPPER_CSS_CLASS = "AspNet-RadioButtonList";
		private const string ITEM_CSS_CLASS = "AspNet-RadioButtonList-Item";
		private const string SELECTED_CSS_CLASS = "AspNet-RadioButtonList-Selected";
		private const string DISABLED_CSS_CLASS = "AspNet-RadioButtonList-Disabled";
		private const string REPEATDIRECTION_CSS_CLASS = "AspNet-RadioButtonList-RepeatDirection-";

		protected override void RenderBeginTag(HtmlTextWriter writer)
		{
			// Div
			writer.AddAttribute(HtmlTextWriterAttribute.Class, WRAPPER_CSS_CLASS);
			writer.AddAttribute(HtmlTextWriterAttribute.Id, this.Control.ClientID);
			writer.RenderBeginTag(HtmlTextWriterTag.Div);
			writer.Indent++;

			// Ul
			string cssClass = String.Empty;
			if (!string.IsNullOrEmpty(this.Control.CssClass))
			{
				cssClass = this.Control.CssClass;
			}

			RadioButtonList radioList = this.Control as RadioButtonList;
			if (radioList != null)
			{
				cssClass += " " + REPEATDIRECTION_CSS_CLASS + radioList.RepeatDirection.ToString();
			}

			writer.AddAttribute(HtmlTextWriterAttribute.Class, cssClass.Trim());
			writer.RenderBeginTag(HtmlTextWriterTag.Ul);
		}

		protected override void RenderEndTag(HtmlTextWriter writer)
		{
			writer.RenderEndTag(); // Ul
			writer.Indent--;
			writer.RenderEndTag(); // Div
		}

		protected override void RenderContents(HtmlTextWriter writer)
		{
			RadioButtonList buttonList = this.Control as RadioButtonList;
			if (buttonList != null)
			{
				foreach (ListItem li in buttonList.Items)
				{
					string itemClientID = Helpers.GetListItemClientID(buttonList, li);

					// Li
					string cssClass = ITEM_CSS_CLASS;

					if (li.Selected)
					{
						cssClass += " " + SELECTED_CSS_CLASS;
					}
					if (li.Enabled == false || buttonList.Enabled == false)
					{
						cssClass += " " + DISABLED_CSS_CLASS;
					}
					writer.AddAttribute(HtmlTextWriterAttribute.Class, cssClass);
					writer.RenderBeginTag(HtmlTextWriterTag.Li);

					if (buttonList.TextAlign == TextAlign.Right)
					{
						RenderRadioButtonListInput(writer, buttonList, li);
						RenderRadioButtonListLabel(writer, buttonList, li);
					}
					else // TextAlign.Left
					{
						RenderRadioButtonListLabel(writer, buttonList, li);
						RenderRadioButtonListInput(writer, buttonList, li);
					}

					writer.RenderEndTag(); // </li>
					if (this.Page != null)
						Page.ClientScript.RegisterForEventValidation(buttonList.UniqueID, li.Value);
				}

				if (this.Page != null)
					Page.ClientScript.RegisterForEventValidation(buttonList.UniqueID);
			}
		}

		private void RenderRadioButtonListInput(HtmlTextWriter writer, RadioButtonList buttonList, ListItem li)
		{
			// Input
			writer.AddAttribute(HtmlTextWriterAttribute.Id, Helpers.GetListItemClientID(buttonList, li));
			writer.AddAttribute(HtmlTextWriterAttribute.Type, "radio");
			writer.AddAttribute(HtmlTextWriterAttribute.Name, buttonList.UniqueID);
			writer.AddAttribute(HtmlTextWriterAttribute.Value, li.Value);
			if (li.Selected)
			{
				writer.AddAttribute(HtmlTextWriterAttribute.Checked, "checked");
			}
			if (li.Enabled == false || buttonList.Enabled == false)
			{
				writer.AddAttribute(HtmlTextWriterAttribute.Disabled, "disabled");
			}
			if (li.Enabled == true && buttonList.Enabled == true && buttonList.AutoPostBack)
			{
				writer.AddAttribute(HtmlTextWriterAttribute.Onclick,
					String.Format(@"setTimeout('__doPostBack(\'{0}\',\'\')', 0)",
						Helpers.GetListItemUniqueID(buttonList, li)));
			}
			writer.RenderBeginTag(HtmlTextWriterTag.Input);
			writer.RenderEndTag(); // </input>
		}

		private void RenderRadioButtonListLabel(HtmlTextWriter writer, RadioButtonList buttonList, ListItem li)
		{
			// Label
			writer.AddAttribute("for", Helpers.GetListItemClientID(buttonList, li));
			writer.RenderBeginTag(HtmlTextWriterTag.Label);
			writer.Write(li.Text);
			writer.RenderEndTag(); // </label>
		}
	}
}