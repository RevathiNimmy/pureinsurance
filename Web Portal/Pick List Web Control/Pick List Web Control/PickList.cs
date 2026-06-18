using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Reflection;
using System.Security.Permissions;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.Design.WebControls;

namespace SCS.Web.UI.WebControls
{
    #region Enums
    public enum SelectionModeType { Multiple = 0, Single }
    public enum ButtonActionType { Add = 0, AddAll, Remove, RemoveAll }
    public enum SideType { Left = 0, Right }
    #endregion

    [AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [DefaultProperty("Items"), DefaultEvent("SelectionChanged")]
    [ToolboxData("<{0}:PickList runat=server></{0}:PickList>")]
    [ParseChildren(true, "Items")]
    public class PickList : WebControl, INamingContainer, IPostBackDataHandler
    {
        #region Fields

        private static string _clientCodePrefix = "_SCS_PickList_";
        private static string _leftListName = "AvailList";
        private static string _rightListName = "CurrentList";
        private static string _selectedValuesName = "SelectedValues";

        private object _dataSource;
        private ListItemCollection _items = new ListItemCollection();
        private ArrayList _originalStates = new ArrayList();

        private string _dataValueField = string.Empty;
        private string _dataTextField = string.Empty;
        private string _selectedValuesList = string.Empty;

        bool _isInDesignMode = false;

        private Style _addButtonStyle;
        private Style _addAllButtonStyle;
        private Style _removeButtonStyle;
        private Style _removeAllButtonStyle;

        private Style _availableLabelStyle;
        private Style _selectedLabelStyle;

        private Style _availableListBoxStyle;
        private Style _selectedListBoxStyle;

        #endregion

        private static readonly object EventSelectionChanged = new object();

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            string scriptUrl = Page.ClientScript.GetWebResourceUrl(this.GetType(),
                "SCS.Web.UI.WebControls.Resources.PickListScriptLibrary.js");

            this.Page.ClientScript.RegisterClientScriptInclude(this.GetType(), "_SCS_PickList", scriptUrl);
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (!Page.IsPostBack)
            {
                this.BaselineValues();
            }
        }

        public override void RenderBeginTag(HtmlTextWriter writer)
        {
            // disable outer span tag
            //base.RenderBeginTag(writer);  

            // write table

            if (!this.Width.IsEmpty)
                writer.AddAttribute(HtmlTextWriterAttribute.Width, this.Width.ToString(), false);

            if (!this.BorderColor.IsEmpty)
                writer.AddStyleAttribute(HtmlTextWriterStyle.BorderColor, ColorTranslator.ToHtml(this.BorderColor));

            if (!string.IsNullOrEmpty(this.BorderStyle.ToString()))
                writer.AddStyleAttribute(HtmlTextWriterStyle.BorderStyle, this.BorderStyle.ToString());

            if (!this.BorderWidth.IsEmpty)
                writer.AddStyleAttribute(HtmlTextWriterStyle.BorderWidth, this.BorderWidth.ToString());

            if (!this.BackColor.IsEmpty)
                writer.AddStyleAttribute(HtmlTextWriterStyle.BackgroundColor, ColorTranslator.ToHtml(this.BackColor));

            if (!this.Height.IsEmpty)
                writer.AddAttribute(HtmlTextWriterAttribute.Height, this.Height.ToString(), false);

            if (!string.IsNullOrEmpty(this.AccessKey))
                writer.AddAttribute(HtmlTextWriterAttribute.Accesskey, this.AccessKey, false);

            if (this.TabIndex > 0)
                writer.AddAttribute(HtmlTextWriterAttribute.Tabindex, this.TabIndex.ToString(), false);

            if (!string.IsNullOrEmpty(this.CssClass))
                writer.AddAttribute(HtmlTextWriterAttribute.Class, this.CssClass, false);

            writer.RenderBeginTag(HtmlTextWriterTag.Table);
        }
        public override void RenderEndTag(HtmlTextWriter writer)
        {
            // disable outer span tag close
            //base.RenderEndTag(writer); 

            // close table
            writer.RenderEndTag();
        }

        protected override void RenderContents(HtmlTextWriter writer)
        {
            base.RenderContents(writer);

            if (HttpContext.Current == null)
                _isInDesignMode = true;

            // write one and only row
            writer.RenderBeginTag(HtmlTextWriterTag.Tr);

            // write left cell
            writer.RenderBeginTag(HtmlTextWriterTag.Td);
            writer.AddAttribute(HtmlTextWriterAttribute.Valign, "top", false);
            writer.AddAttribute(HtmlTextWriterAttribute.Align, "left", false);

            // write left list box's label
            this.WriteLabel(writer, SideType.Left, "AvailLabel", this.AvailableLabelText);

            // write left list box
            this.WriteListBox(writer, SideType.Left, _leftListName, this.Rows.ToString(), this.ListBoxWidth);

            // write left list box options
            this.WriteListItems(writer, false);

            // close left list box
            writer.RenderEndTag();

            // close left cell
            writer.RenderEndTag();

            writer.AddAttribute(HtmlTextWriterAttribute.Align, "center", false);
            writer.AddAttribute(HtmlTextWriterAttribute.Valign, "middle", false);
            writer.RenderBeginTag(HtmlTextWriterTag.Td);

            // write buttons
            //

            // write add button
            if (this.DisplayAddButton)
            this.WriteButton(writer, "AddCmd", this.AddButtonText, ButtonActionType.Add);

            // write remove button
            if (this.DisplayRemoveButton)
            this.WriteButton(writer, "RemoveCmd", this.RemoveButtonText, ButtonActionType.Remove);

            // write add all button
            if (this.DisplayAddAllButton)
                this.WriteButton(writer, "RemoveAllCmd", this.AddAllButtonText, ButtonActionType.AddAll);

            // write remove all button
            if (this.DisplayRemoveAllButton)
                this.WriteButton(writer, "RemoveAllCmd", this.RemoveAllButtonText, ButtonActionType.RemoveAll);

            if (!_isInDesignMode)
            {
                // write textbox to track selection values            
                writer.AddAttribute(HtmlTextWriterAttribute.Name, this.UniqueID, false);
                writer.AddAttribute(HtmlTextWriterAttribute.Id, this.ClientID + "_" + _selectedValuesName, false);
                writer.AddAttribute(HtmlTextWriterAttribute.Type, "hidden", false);
                writer.AddAttribute(HtmlTextWriterAttribute.Value, _selectedValuesList, false);
                writer.RenderBeginTag(HtmlTextWriterTag.Input);
                writer.RenderEndTag();
            }

            // close center cell
            writer.RenderEndTag();

            writer.RenderBeginTag(HtmlTextWriterTag.Td);
            writer.AddAttribute(HtmlTextWriterAttribute.Valign, "top", false);
            writer.AddAttribute(HtmlTextWriterAttribute.Align, "left", false);

            // write left right box's label
            this.WriteLabel(writer, SideType.Right, "CurrentLabel", this.CurrentLabelText);

            // write right list box    
            this.WriteListBox(writer, SideType.Right, _rightListName, Rows.ToString(), this.ListBoxWidth);
                    
            // write right list box options
            this.WriteListItems(writer, true);

            // close lift list box
            writer.RenderEndTag();

            // close left cell and the row
            writer.RenderEndTag();
            writer.RenderEndTag();
        }

        private void WriteBreak(HtmlTextWriter writer)
        {
            writer.WriteBeginTag("br");
            writer.Write(HtmlTextWriter.SelfClosingTagEnd);
        }
        private void WriteLabel(HtmlTextWriter writer, SideType side, string id, string text)
        {
            string style = "";
            string className = "";
            
            if (side.Equals(SideType.Left))
            {
               style = this.GetCssStyle(_availableLabelStyle, out className);               
            }
            else
            {
                style = this.GetCssStyle(_selectedLabelStyle, out className);                           
            }

            writer.AddAttribute(HtmlTextWriterAttribute.Id, this.ClientID + "_" + id, false);

            if (!string.IsNullOrEmpty(style))
                writer.AddAttribute(HtmlTextWriterAttribute.Style, style);

            if (className.Length > 0)
                writer.AddAttribute(HtmlTextWriterAttribute.Class, className, false);

            writer.RenderBeginTag(HtmlTextWriterTag.Label);
            writer.WriteEncodedText(text);

            writer.RenderEndTag();

            this.WriteBreak(writer);
        }
        private void WriteButton(HtmlTextWriter writer, string id, string buttonText, ButtonActionType actionType)
        {
            string sourceListId = string.Empty;
            string targetListId = string.Empty;
            string style = string.Empty;
            string className = string.Empty;

            bool isTargetListSelected = false;

            switch (actionType)
            {
                case ButtonActionType.Add:

                    style = this.GetCssStyle(_addButtonStyle, out className);
                    sourceListId = _leftListName;
                    targetListId = _rightListName;
                    isTargetListSelected = true;
                    break;

                case ButtonActionType.AddAll:

                    style = this.GetCssStyle(_addAllButtonStyle, out className);
                    sourceListId = _leftListName;
                    targetListId = _rightListName;
                    isTargetListSelected = true;
                    break;

                case ButtonActionType.Remove:

                    style = this.GetCssStyle(_removeButtonStyle, out className);
                    sourceListId = _rightListName;
                    targetListId = _leftListName;
                    break;

                case ButtonActionType.RemoveAll:

                    style = this.GetCssStyle(_removeAllButtonStyle, out className);
                    sourceListId = _rightListName;
                    targetListId = _leftListName;
                    break;
            }

            bool allFlag = (actionType == ButtonActionType.Add || actionType == ButtonActionType.Remove) ? false : true;

            writer.AddAttribute(HtmlTextWriterAttribute.Type, "button", false);
            writer.AddAttribute(HtmlTextWriterAttribute.Id, this.ClientID + "_" + id, false);
            writer.AddAttribute(HtmlTextWriterAttribute.Value, buttonText, true);
            writer.AddAttribute(HtmlTextWriterAttribute.Onclick,
                string.Format("{0}MoveOption('{1}_{2}', '{3}_{4}', {5}, '{6}_{7}', {8})",
                       _clientCodePrefix,
                       this.ClientID, sourceListId,
                       this.ClientID, targetListId,
                       isTargetListSelected.ToString().ToLower(),
                       this.ClientID, _selectedValuesName,
                       allFlag.ToString().ToLower()), false);
       
            if (!string.IsNullOrEmpty(style))
                writer.AddAttribute(HtmlTextWriterAttribute.Style, style);

            if (!string.IsNullOrEmpty(className))
                writer.AddAttribute(HtmlTextWriterAttribute.Class, className);

            writer.RenderBeginTag(HtmlTextWriterTag.Input);
            writer.RenderEndTag();

            this.WriteBreak(writer);
        }
        private void WriteListBox(HtmlTextWriter writer, SideType side, string id, string rowCount, string listBoxWidth)
        {
            string className = string.Empty;

            string style = (side.Equals(SideType.Left))
                ? this.GetCssStyle(_availableListBoxStyle, out className) : this.GetCssStyle(_selectedListBoxStyle, out className);

            writer.AddAttribute(HtmlTextWriterAttribute.Id, this.ClientID + "_" + id, false);
            writer.AddAttribute(HtmlTextWriterAttribute.Name, this.UniqueID + "_" + id, false);

            if (this.SelectionMode.Equals(SelectionModeType.Multiple))
                writer.AddAttribute(HtmlTextWriterAttribute.Multiple, "true");

            writer.AddAttribute(HtmlTextWriterAttribute.Size, rowCount, false);

            if (!string.IsNullOrEmpty(style))
                writer.AddAttribute(HtmlTextWriterAttribute.Style, style);

            if (!string.IsNullOrEmpty(className))
                writer.AddAttribute(HtmlTextWriterAttribute.Class, className);

            writer.RenderBeginTag(HtmlTextWriterTag.Select);
        }
        private void WriteListItems(HtmlTextWriter writer, bool writeSelected)
        {
            if (_items == null)
                return;

            foreach (ListItem item in _items)
            {
                if (item.Selected == writeSelected)
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Value, item.Value, true);
                    writer.RenderBeginTag(HtmlTextWriterTag.Option);
                    writer.WriteEncodedText(item.Text);
                    writer.RenderEndTag();
                }
            }
        }

        private ArrayList ResizeStatesArray(ListItemCollection items, ArrayList originalStates)
        {
            ArrayList newOriginalStates = new ArrayList(items.Count);

            // initialize array
            foreach (ListItem item in items)
            {
                newOriginalStates.Add(0);
            }

            // copy states to new larger array matching count of the list items collection
            for (int i=0; i < originalStates.Count; i++)
            {
                newOriginalStates[i] = originalStates[i];
            }

            return newOriginalStates;
        }

        private string GetCssStyle(Style style, out string className)
        {
            if (style == null)
            {
                className = "";
                return string.Empty;
            }

            className = style.CssClass;

            StringBuilder sb = new StringBuilder(256);
            Color c;

            c = style.ForeColor;
            if (!c.IsEmpty)
            {
                sb.Append("color:");
                sb.Append(ColorTranslator.ToHtml(c));
                sb.Append(";");
            }
            c = style.BackColor;
            if (!c.IsEmpty)
            {
                sb.Append("background-color:");
                sb.Append(ColorTranslator.ToHtml(c));
                sb.Append(";");
            }

            FontInfo fi = style.Font;
            string s;

            s = fi.Name;
            if (s.Length != 0)
            {
                sb.Append("font-family:'");
                sb.Append(s);
                sb.Append("';");
            }
            if (fi.Bold)
            {
                sb.Append("font-weight:bold;");
            }
            if (fi.Italic)
            {
                sb.Append("font-style:italic;");
            }

            s = String.Empty;
            if (fi.Underline)
                s += "underline";
            if (fi.Strikeout)
                s += " line-through";
            if (fi.Overline)
                s += " overline";
            if (s.Length != 0)
            {
                sb.Append("text-decoration:");
                sb.Append(s);
                sb.Append(';');
            }

            FontUnit fu = fi.Size;
            if (fu.IsEmpty == false)
            {
                sb.Append("font-size:");
                sb.Append(fu.ToString(CultureInfo.InvariantCulture));
                sb.Append(';');
            }

            s = String.Empty;
            Unit u = style.BorderWidth;
            BorderStyle bs = style.BorderStyle;
            if (u.IsEmpty == false)
            {
                s = u.ToString(CultureInfo.InvariantCulture);
                if (bs == BorderStyle.NotSet)
                {
                    s += " solid";
                }
            }
            c = style.BorderColor;
            if (!c.IsEmpty)
            {
                s += " " + ColorTranslator.ToHtml(c);
            }
            if (bs != BorderStyle.NotSet)
            {
                s += " " + Enum.Format(typeof(BorderStyle), bs, "G");
            }
            if (s.Length != 0)
            {
                sb.Append("border:");
                sb.Append(s);
                sb.Append(';');
            }

            u = style.Width;
            if (!u.IsEmpty)
            {
                sb.Append("width:");
                sb.Append(u.ToString());
                sb.Append(';');
            }

            u = style.Height;
            if (!u.IsEmpty)
            {
                sb.Append("height:");
                sb.Append(u.ToString());
                sb.Append(';');
            }

            return sb.ToString();
        }

        #region Public Methods

        public override void DataBind()
        {
            if (string.IsNullOrEmpty(_dataTextField) || string.IsNullOrEmpty(_dataValueField))
            {
                throw new ApplicationException("Data value or text field has not been set.");
            }

            IEnumerable list = null;

            if (_dataSource is IEnumerable)
            {
                list = (IEnumerable)_dataSource;
            }
            else if (_dataSource is IListSource)
            {
                IListSource listSource = (IListSource)_dataSource;
                IList memberList = listSource.GetList();
                list = (IEnumerable)memberList;
            }

            foreach (object obj in list)
            {
                string itemText = DataBinder.GetPropertyValue(obj, _dataTextField, String.Empty);
                string itemValue = DataBinder.GetPropertyValue(obj, _dataValueField, String.Empty);

                ListItem item = new ListItem(itemText, itemValue);
                this.Items.Add(item);
            }

            if (!IsTrackingViewState)
                TrackViewState();
        }
        
        public void SetSelectedValues(params object[] values)
        {
            this.SetSelectedValues(false, values);
        }
        public void BaselineValues()
        {
            if (_originalStates.Count != Items.Count)
            {
                _originalStates = this.ResizeStatesArray(_items, _originalStates);
            }

            for (int i = 0; i < Items.Count; i++ )
            {
                _originalStates[i] = (Items[i].Selected) ? 1 : 0;
            }
        }
        
        public ListItemCollection GetSelectedItems()
        {
            ListItemCollection selectedItems = new ListItemCollection();

            foreach (ListItem item in _items)
            {
                if (item.Selected)
                    selectedItems.Add(item);
            }
            return selectedItems;
        }
        public ListItemCollection GetAddedItems()
        {
            ListItemCollection addedItems = new ListItemCollection();

            int idx = 0;
            foreach (ListItem item in _items)
            {
                if (item.Selected && ((int)_originalStates[idx]) == 0)
                {
                    // need to return a copy and not a reference
                    ListItem newItem = new ListItem(item.Text, item.Value, true);
                    addedItems.Add(newItem);
                }
                idx++;
            }

            return addedItems;
        }
        public ListItemCollection GetRemovedItems()
        {
            ListItemCollection removedItems = new ListItemCollection();

            int idx = 0;
            foreach (ListItem item in _items)
            {
                if (!item.Selected && ((int)_originalStates[idx]) == 1)
                {
                    // need to return a copy and not a reference
                    ListItem newItem = new ListItem(item.Text, item.Value, false);
                    removedItems.Add(newItem);
                }
                idx++;
            }

            return removedItems;
        }

        public int[] ListItemToIntArray(ListItemCollection listItems)
        {
            int[] ids = new int[listItems.Count];

            for (int i = 0; i < listItems.Count; i++)
            {
                int value;

                if (!int.TryParse(listItems[i].Value, out value))
                {
                    throw new InvalidCastException(string.Format("Unable to convert '{0}' to type int.", listItems[i].Value));
                }
                ids[i] = value;
            }

            return ids;
        }
        public object[] ListItemToObjectArray(ListItemCollection listItems)
        {
            object[] ids = new object[listItems.Count];

            for (int i = 0; i < listItems.Count; i++)
            {
                ids[i] = listItems[i].Value;
            }
            return ids;
        }

        #endregion

        private bool IsListValuesSame(string list1, string list2)
        {
            string[] array1 = list1.Split(',');
            string[] array2 = list2.Split(',');

            if (array1.Length != array2.Length)
                return false;

            foreach (string value1 in array1)
            {
                bool matchFound = false;
                foreach (string value2 in array2)
                {
                    if (value1.Equals(value2))
                    {
                        matchFound = true;
                        break;
                    }
                }
                if (!matchFound)
                    return false;
            }
            return true;
        }
        private void SetSelectedValues(bool fromPostback, params object[] values)
        {
            if (_originalStates.Count != Items.Count)
            {
                _originalStates = this.ResizeStatesArray(_items, _originalStates);
            }

            foreach (object value in values)
            {
                int idx = 0;
                foreach (ListItem item in Items)
                {
                    if (item.Value.Equals(value.ToString(), StringComparison.OrdinalIgnoreCase))
                    {
                        item.Selected = true;
                        _selectedValuesList += "," + item.Value;

                        if (!fromPostback)
                            _originalStates[idx] = 1;
                    }
                    idx++;
                }
            }

            if (_selectedValuesList.StartsWith(","))
            {
                _selectedValuesList = _selectedValuesList.Substring(1);
                this.SelectedValues = _selectedValuesList;
            }
        }

        #region INamingContainter Implementation (View State)

        protected override object SaveViewState()
        {
            object[] state = new object[11];

            state[0] = base.SaveViewState();
            state[1] = (_items != null) ? ((IStateManager)_items).SaveViewState() : null;
            state[2] = (_addButtonStyle != null) ? ((IStateManager)_addButtonStyle).SaveViewState() : null;
            state[3] = (_addAllButtonStyle != null) ? ((IStateManager)_addAllButtonStyle).SaveViewState() : null;
            state[4] = (_removeButtonStyle != null) ? ((IStateManager)_removeButtonStyle).SaveViewState() : null;
            state[5] = (_removeAllButtonStyle != null) ? ((IStateManager)_removeAllButtonStyle).SaveViewState() : null;
            state[6] = (_availableLabelStyle != null) ? ((IStateManager)_availableLabelStyle).SaveViewState() : null;
            state[7] = (_selectedLabelStyle != null) ? ((IStateManager)_selectedLabelStyle).SaveViewState() : null;
            state[8] = (_availableListBoxStyle != null) ? ((IStateManager)_availableListBoxStyle).SaveViewState() : null;
            state[9] = (_selectedListBoxStyle != null) ? ((IStateManager)_selectedListBoxStyle).SaveViewState() : null;
            state[10] = _originalStates; 

            for (int i = 0; i < 10; i++)
                if (state[i] != null)
                    return state;

            // Another perfomance optimization. If no modifications were made to any
            // properties from their persisted state, the view state for this control
            // is null. Returning null, rather than an array of null values helps
            // minimize the view state significantly.
            return null;
        }
        protected override void LoadViewState(object savedState)
        {
            object baseState = null;
            object[] state = null;

            if (savedState != null)
            {
                state = (object[])savedState;
                baseState = state[0];
            }

            // Always call the base class, even if the state is null, so
            // the base class gets a chance to fully implement its LoadViewState
            // functionality.
            base.LoadViewState(baseState);

            if (state == null)
                return;

            if (state[1] != null)
                ((IStateManager)Items).LoadViewState(state[1]);

            if (state[2] != null)
                ((IStateManager)AddButtonStyle).LoadViewState(state[2]);

            if (state[3] != null)
                ((IStateManager)AddAllButtonStyle).LoadViewState(state[3]);

            if (state[4] != null)
                ((IStateManager)RemoveButtonStyle).LoadViewState(state[4]);

            if (state[5] != null)
                ((IStateManager)RemoveAllButtonStyle).LoadViewState(state[5]);

            if (state[6] != null)
                ((IStateManager)AvailableLabelStyle).LoadViewState(state[6]);

            if (state[7] != null)
                ((IStateManager)SelectedLabelStyle).LoadViewState(state[7]);

            if (state[8] != null)
                ((IStateManager)AvailableListBoxStyle).LoadViewState(state[8]);

            if (state[9] != null)
                ((IStateManager)SelectedListBoxStyle).LoadViewState(state[9]);

            if (state[10] != null)
            {
                _originalStates = (ArrayList)state[10];
            }
        }
        protected override void TrackViewState()
        {
            //if (_items != null)
            ((IStateManager)_items).TrackViewState();

            if (_addButtonStyle != null)
                ((IStateManager)_addButtonStyle).TrackViewState();

            if (_addAllButtonStyle != null)
                ((IStateManager)_addAllButtonStyle).TrackViewState();

            if (_removeAllButtonStyle != null)
                ((IStateManager)_removeAllButtonStyle).TrackViewState();

            if (_removeButtonStyle != null)
                ((IStateManager)_removeButtonStyle).TrackViewState();

            if (_availableLabelStyle != null)
                ((IStateManager)_availableLabelStyle).TrackViewState();

            if (_selectedLabelStyle != null)
                ((IStateManager)_selectedLabelStyle).TrackViewState();

            if (_availableListBoxStyle != null)
                ((IStateManager)_availableListBoxStyle).TrackViewState();

            if (_selectedListBoxStyle != null)
                ((IStateManager)_selectedListBoxStyle).TrackViewState();
        }

        #endregion

        #region IPostBackDataHandler Implementation

        public virtual bool LoadPostData(string postDataKey, NameValueCollection values)
        {
            string presentValue = this.SelectedValues;
            string postedValue = values[postDataKey];

            this.SetSelectedValues(true, postedValue.Split(','));

            if (this.IsListValuesSame(presentValue, postedValue))
            {
                this.SelectedValues = postedValue;
                return true;
            }
            return false;
        }
        public virtual void RaisePostDataChangedEvent()
        {
            OnSelectionChanged(EventArgs.Empty);
        }

        #endregion

        #region Event Implementation
        [Category("Action"), Description("Raised when the selection changes.")]
        public event EventHandler SelectionChanged
        {
            add
            {
                Events.AddHandler(EventSelectionChanged, value);
            }
            remove
            {
                Events.RemoveHandler(EventSelectionChanged, value);
            }
        }

        protected virtual void OnSelectionChanged(EventArgs e)
        {
            EventHandler selectionChangedHandler = (EventHandler)Events[EventSelectionChanged];

            if (selectionChangedHandler != null)
            {
                selectionChangedHandler(this, e);
            }
        }
        #endregion

        #region Properties

        #region Appearance

        [Category("Appearance"), DefaultValue("100%"), Description("Width of list boxes."), Bindable(true),]
        public string ListBoxWidth
        {
            get
            {
                object o = ViewState["ListBoxWidth"];
                return ((o == null) ? "100%" : (string)o);
            }
            set
            {
                ViewState["ListBoxWidth"] = value;
            }
        }

        [Category("Appearance"), DefaultValue("Available Options:"), Description("Text used to represent the available list box."), Bindable(true),]
        public string AvailableLabelText
        {
            get
            {
                object o = ViewState["AvailableLabelText"];
                return ((o == null) ? "Available Options:" : (string)o);
            }
            set
            {
                ViewState["AvailableLabelText"] = value;
            }
        }

        [Category("Appearance"), DefaultValue("Selected Options:"), Description("Text used to represent the current list box."), Bindable(true),]
        public string CurrentLabelText
        {
            get
            {
                object o = ViewState["CurrentLabelText"];
                return ((o == null) ? "Selected Options:" : (string)o);
            }
            set
            {
                ViewState["CurrentLabelText"] = value;
            }
        }

        [Category("Appearance"), DefaultValue(""), Description("The number of visible rows to display."), Bindable(true),]
        public int Rows
        {
            get
            {
                object o = ViewState["Rows"];
                return ((o == null) ? 8 : (int)o);
            }
            set
            {
                ViewState["Rows"] = value;
            }
        }

        [Category("Appearance"), DefaultValue("Add >>"), Description("Text used to for the add button."), Bindable(true),]
        public string AddButtonText
        {
            get
            {
                object o = ViewState["AddButtonText"];
                return ((o == null) ? "Add >>" : (string)o);
            }
            set
            {
                ViewState["AddButtonText"] = value;
            }
        }

        [Category("Appearance"), DefaultValue("Add All >>"), Description("Text used to for the add all button."), Bindable(true),]
        public string AddAllButtonText
        {
            get
            {
                object o = ViewState["AddAllButtonText"];
                return ((o == null) ? "Add All >>" : (string)o);
            }
            set
            {
                ViewState["AddAllButtonText"] = value;
            }
        }

        [Category("Appearance"), DefaultValue("<< Remove"), Description("Text used to for the remove button."), Bindable(true),]
        public string RemoveButtonText
        {
            get
            {
                object o = ViewState["RemoveButtonText"];
                return ((o == null) ? "<< Remove" : (string)o);
            }
            set
            {
                ViewState["RemoveButtonText"] = value;
            }
        }

        [Category("Appearance"), DefaultValue("<< Remove All"), Description("Text used to for the remove all button."), Bindable(true),]
        public string RemoveAllButtonText
        {
            get
            {
                object o = ViewState["RemoveAllButtonText"];
                return ((o == null) ? "<< Remove All" : (string)o);
            }
            set
            {
                ViewState["RemoveAllButtonText"] = value;
            }
        }

        [Category("Appearance"), DefaultValue("90px"), Description("Width used for each button."), Bindable(true),]
        public string ButtonWidth
        {
            get
            {
                object o = ViewState["ButtonWidth"];
                return ((o == null) ? "90px" : (string)o);
            }
            set
            {
                ViewState["ButtonWidth"] = value;
            }
        }
        #endregion

        #region Behavior

        [Category("Behavior"), DefaultValue(SelectionModeType.Multiple), Description("The selection mode."), Bindable(true),]
        public SelectionModeType SelectionMode
        {
            get
            {
                object o = ViewState["SelectionMode"];
                return ((o == null) ? SelectionModeType.Multiple : (SelectionModeType)o);
            }
            set
            {
                ViewState["SelectionMode"] = value;
            }
        }

        [Category("Behavior"), DefaultValue(false), Description("Determinds visibility of the remove all button."), Bindable(true),]
        public bool DisplayRemoveAllButton
        {
            get
            {
                object o = ViewState["DisplayRemoveAllButton"];
                return ((o == null) ? false : (bool)o);
            }
            set
            {
                ViewState["DisplayRemoveAllButton"] = value;
            }
        }

        [Category("Behavior"), DefaultValue(false), Description("Determinds visibility of the add all button."), Bindable(true),]
        public bool DisplayAddAllButton
        {
            get
            {
                object o = ViewState["DisplayAddAllButton"];
                return ((o == null) ? false : (bool)o);
            }
            set
            {
                ViewState["DisplayAddAllButton"] = value;
            }
        }

        [Category("Behavior"), DefaultValue(false), Description("Determinds visibility of the add button."), Bindable(true),]
        public bool DisplayAddButton
        {
            get
            {
                object o = ViewState["DisplayAddButton"];
                return ((o == null) ? false : (bool)o);
            }
            set
            {
                ViewState["DisplayAddButton"] = value;
            }
        }

        [Category("Behavior"), DefaultValue(false), Description("Determinds visibility of the remove button."), Bindable(true),]
        public bool DisplayRemoveButton
        {
            get
            {
                object o = ViewState["DisplayRemoveButton"];
                return ((o == null) ? false : (bool)o);
            }
            set
            {
                ViewState["DisplayRemoveButton"] = value;
            }
        }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override string ToolTip
        {
            get
            {
                return base.ToolTip;
            }
            set
            {
                base.ToolTip = value;
            }
        }
        #endregion

        #region Data

        [Category("Data"), DefaultValue(""), Description("Data table, view, or collection for the current list box to bind to."), Bindable(true),]
        public object DataSource
        {
            set { _dataSource = value; }
        }

        [Category("Data"), DefaultValue(""), Description("The field in the data source that provides the item text."), Bindable(true),]
        public string DataTextField
        {
            set { _dataTextField = value; }
        }

        [Category("Data"), DefaultValue(""), Description("The field in the data source that provides the item value."), Bindable(true),]
        public string DataValueField
        {
            set { _dataValueField = value; }
        }

        #endregion

        #region Style

        [
         Category("Style"), Description("The style applied to the add button."),
         DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
         NotifyParentProperty(true), PersistenceMode(PersistenceMode.InnerProperty)
        ]
        public virtual Style AddButtonStyle
        {
            get
            {
                if (_addButtonStyle == null)
                {
                    _addButtonStyle = new Style();
                    if (IsTrackingViewState)
                    {
                        ((IStateManager)_addButtonStyle).TrackViewState();
                    }
                }
                return _addButtonStyle;
            }
        }

        [
         Category("Style"), Description("The style applied to the add all button."),
         DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
         NotifyParentProperty(true), PersistenceMode(PersistenceMode.InnerProperty)
        ]
        public virtual Style AddAllButtonStyle
        {
            get
            {
                if (_addAllButtonStyle == null)
                {
                    _addAllButtonStyle = new Style();
                    if (IsTrackingViewState)
                    {
                        ((IStateManager)_addAllButtonStyle).TrackViewState();
                    }
                }
                return _addAllButtonStyle;
            }
        }

        [
         Category("Style"), Description("The style applied to the remove button."),
         DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
         NotifyParentProperty(true), PersistenceMode(PersistenceMode.InnerProperty)
        ]
        public virtual Style RemoveButtonStyle
        {
            get
            {
                if (_removeButtonStyle == null)
                {
                    _removeButtonStyle = new Style();
                    if (IsTrackingViewState)
                    {
                        ((IStateManager)_removeButtonStyle).TrackViewState();
                    }
                }
                return _removeButtonStyle;
            }
        }

        [
        Category("Style"), Description("The style applied to the remove all button."),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
        NotifyParentProperty(true), PersistenceMode(PersistenceMode.InnerProperty)
        ]
        public virtual Style RemoveAllButtonStyle
        {
            get
            {
                if (_removeAllButtonStyle == null)
                {
                    _removeAllButtonStyle = new Style();
                    if (IsTrackingViewState)
                    {
                        ((IStateManager)_removeAllButtonStyle).TrackViewState();
                    }
                }
                return _removeAllButtonStyle;
            }
        }

        [
        Category("Style"), Description("The style applied to the available listbox's label."),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
        NotifyParentProperty(true), PersistenceMode(PersistenceMode.InnerProperty)
        ]
        public virtual Style AvailableLabelStyle
        {
            get
            {
                if (_availableLabelStyle == null)
                {
                    _availableLabelStyle = new Style();
                    if (IsTrackingViewState)
                    {
                        ((IStateManager)_availableLabelStyle).TrackViewState();
                    }
                }
                return _availableLabelStyle;
            }
        }

        [
         Category("Style"), Description("The style applied to the selected listbox's label."),
         DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
         NotifyParentProperty(true), PersistenceMode(PersistenceMode.InnerProperty)
        ]
        public virtual Style SelectedLabelStyle
        {
            get
            {
                if (_selectedLabelStyle == null)
                {
                    _selectedLabelStyle = new Style();
                    if (IsTrackingViewState)
                    {
                        ((IStateManager)_selectedLabelStyle).TrackViewState();
                    }
                }
                return _selectedLabelStyle;
            }
        }

        [
         Category("Style"), Description("The style applied to the available listbox."),
         DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
         NotifyParentProperty(true), PersistenceMode(PersistenceMode.InnerProperty)
        ]
        public virtual Style AvailableListBoxStyle
        {
            get
            {
                if (_availableListBoxStyle == null)
                {
                    _availableListBoxStyle = new Style();
                    if (IsTrackingViewState)
                    {
                        ((IStateManager)_availableListBoxStyle).TrackViewState();
                    }
                }
                return _availableListBoxStyle;
            }
        }

        [
         Category("Style"), Description("The style applied to the selected listbox."),
         DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
         NotifyParentProperty(true), PersistenceMode(PersistenceMode.InnerProperty)
        ]
        public virtual Style SelectedListBoxStyle
        {
            get
            {
                if (_selectedListBoxStyle == null)
                {
                    _selectedListBoxStyle = new Style();
                    if (IsTrackingViewState)
                    {
                        ((IStateManager)_selectedListBoxStyle).TrackViewState();
                    }
                }
                return _selectedListBoxStyle;
            }
        }
        #endregion

        #region Misc

        [Category("Misc"), Description("The collection of items between the two list boxes."), Bindable(false),]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), NotifyParentProperty(true)]
        [PersistenceMode(PersistenceMode.InnerDefaultProperty)]
        [Editor(typeof(ListItemsCollectionEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public ListItemCollection Items
        {
            get
            {
                if (_items == null)
                {
                    _items = new ListItemCollection();

                    if (IsTrackingViewState)
                    {
                        ((IStateManager)this._items).TrackViewState();
                    }
                }
                return this._items;
            }
        }

        #endregion

        private string SelectedValues
        {
            get
            {
                object o = ViewState["SelectedValues"];
                return ((o == null) ? string.Empty : (string)o);
            }
            set
            {
                ViewState["SelectedValues"] = value;
                ViewState.SetItemDirty("SelectedValues", true);
            }
        }

        #endregion
    }
}
