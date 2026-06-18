Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports System.Collections.Specialized
Imports System.ComponentModel
Imports System.Drawing
Imports System.Globalization
Imports System.Reflection
Imports System.Security.Permissions
Imports System.Text
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.Design.WebControls
Imports System.Enum
Imports System.Net


#Region "Enums"
Public Enum SelectionModeType

    Multiple = 0
    Single1 = 1
End Enum

Public Enum ButtonActionType

    Add = 0

    AddAll

    Remove

    RemoveAll

    Find

    Clear
End Enum

Public Enum SideType

    Left = 0

    Right
End Enum
#End Region

<AspNetHostingPermission(SecurityAction.LinkDemand, Level:=AspNetHostingPermissionLevel.Minimal)> _
<AspNetHostingPermission(SecurityAction.InheritanceDemand, Level:=AspNetHostingPermissionLevel.Minimal), _
 DefaultProperty("Items"), _
 DefaultEvent("SelectionChanged"), _
 ToolboxData("<{0}:PickList runat=server></{0}:PickList>"), _
 ParseChildren(True, "Items")> _
Public Class PickList
    Inherits WebControl
    Implements INamingContainer, IPostBackDataHandler

    Private Shared _clientCodePrefix As String = "NexusPickList_"

    Private Shared _leftListName As String = "AvailList"

    Private Shared _rightListName As String = "CurrentList"

    Private Shared _selectedValuesName As String = "SelectedValues"

    Private _dataSource As Object

    Private _items As ListItemCollection = New ListItemCollection

    Private _originalStates As ArrayList = New ArrayList

    Private _dataValueField As String = String.Empty

    Private _dataTextField As String = String.Empty

    Public _selectedValuesList As String = String.Empty

    Private _isInDesignMode As Boolean = False

    Private _addButtonStyle As Style

    Private _addAllButtonStyle As Style

    Private _removeButtonStyle As Style

    Private _removeAllButtonStyle As Style

    Private _availableLabelStyle As Style

    Private _selectedLabelStyle As Style

    Private _availableListBoxStyle As Style

    Private _selectedListBoxStyle As Style

    Private Shared EventSelectionChanged As Object = New Object

    Private _dataCodeField As String = String.Empty

    Protected Overrides Sub OnInit(ByVal e As EventArgs)
        MyBase.OnInit(e)
        Me.Page.ClientScript.RegisterClientScriptInclude(Me.GetType(), "NexusPickList", Page.ClientScript.GetWebResourceUrl(Me.GetType(), "Nexus.Web.UI.WebControls.PickListScriptLibrary.js"))
    End Sub

    Protected Overrides Sub OnLoad(ByVal e As EventArgs)
        MyBase.OnLoad(e)
    End Sub

    Protected Overrides Sub OnPreRender(ByVal e As EventArgs)
        MyBase.OnPreRender(e)
        If Not Page.IsPostBack Then
            Me.BaselineValues()
        End If
    End Sub

    Public Overrides Sub RenderBeginTag(ByVal writer As HtmlTextWriter)
        ' disable outer span tag
        'base.RenderBeginTag(writer);  
        ' write table
        If Not Me.Width.IsEmpty Then
            writer.AddAttribute(HtmlTextWriterAttribute.Width, Me.Width.ToString, False)
        End If
        If Not Me.BorderColor.IsEmpty Then
            writer.AddStyleAttribute(HtmlTextWriterStyle.BorderColor, ColorTranslator.ToHtml(Me.BorderColor))
        End If
        If Not String.IsNullOrEmpty(Me.BorderStyle.ToString) Then
            writer.AddStyleAttribute(HtmlTextWriterStyle.BorderStyle, Me.BorderStyle.ToString)
        End If
        If Not Me.BorderWidth.IsEmpty Then
            writer.AddStyleAttribute(HtmlTextWriterStyle.BorderWidth, Me.BorderWidth.ToString)
        End If
        If Not Me.BackColor.IsEmpty Then
            writer.AddStyleAttribute(HtmlTextWriterStyle.BackgroundColor, ColorTranslator.ToHtml(Me.BackColor))
        End If
        If Not Me.Height.IsEmpty Then
            writer.AddAttribute(HtmlTextWriterAttribute.Height, Me.Height.ToString, False)
        End If
        If Not String.IsNullOrEmpty(Me.AccessKey) Then
            writer.AddAttribute(HtmlTextWriterAttribute.Accesskey, Me.AccessKey, False)
        End If
        If (Me.TabIndex > 0) Then
            writer.AddAttribute(HtmlTextWriterAttribute.Tabindex, Me.TabIndex.ToString, False)
        End If
        If Not String.IsNullOrEmpty(Me.CssClass) Then
            writer.AddAttribute(HtmlTextWriterAttribute.Class, Me.CssClass, False)
        End If
        writer.RenderBeginTag(HtmlTextWriterTag.Table)
    End Sub

    Public Overrides Sub RenderEndTag(ByVal writer As HtmlTextWriter)
        ' disable outer span tag close
        'base.RenderEndTag(writer); 
        ' close table
        writer.RenderEndTag()
    End Sub

    Protected Overrides Sub RenderContents(ByVal writer As HtmlTextWriter)
        MyBase.RenderContents(writer)
        If (HttpContext.Current Is Nothing) Then
            _isInDesignMode = True
        End If

        ' write one and only row
        writer.RenderBeginTag(HtmlTextWriterTag.Tr)
        ' write left cell
        writer.RenderBeginTag(HtmlTextWriterTag.Td)
        writer.AddAttribute(HtmlTextWriterAttribute.Valign, "top", False)
        writer.AddAttribute(HtmlTextWriterAttribute.Align, "left", False)
        ' write left list box's label
        Me.WriteLabel(writer, SideType.Left, "AvailLabel", Me.AvailableLabelText)
        ' write left list box
        Me.WriteListBox(writer, SideType.Left, _leftListName, Me.Rows.ToString, Me.ListBoxWidth)
        ' write left list box options
        Me.WriteListItems(writer, False)

        ' close left list box
        writer.RenderEndTag()

        ' close left cell
        writer.RenderEndTag()

        writer.AddAttribute(HtmlTextWriterAttribute.Align, "center", False)
        writer.AddAttribute(HtmlTextWriterAttribute.Valign, "middle", False)
        writer.RenderBeginTag(HtmlTextWriterTag.Td)
        ' write buttons
        '
        ' write add button
        If Me.DisplayAddButton Then
            Me.WriteButton(writer, "AddCmd", Me.AddButtonText, ButtonActionType.Add)
        End If
        ' write remove button
        If Me.DisplayRemoveButton Then
            Me.WriteButton(writer, "RemoveCmd", Me.RemoveButtonText, ButtonActionType.Remove)
        End If
        ' write add all button
        If Me.DisplayAddAllButton Then
            Me.WriteButton(writer, "RemoveAllCmd", Me.AddAllButtonText, ButtonActionType.AddAll)
        End If
        ' write remove all button
        If Me.DisplayRemoveAllButton Then
            Me.WriteButton(writer, "RemoveAllCmd", Me.RemoveAllButtonText, ButtonActionType.RemoveAll)
        End If
        If Not _isInDesignMode Then
            ' write textbox to track selection values            
            writer.AddAttribute(HtmlTextWriterAttribute.Name, Me.UniqueID, False)
            writer.AddAttribute(HtmlTextWriterAttribute.Id, (Me.ClientID + ("_" + _selectedValuesName)), False)
            writer.AddAttribute(HtmlTextWriterAttribute.Type, "hidden", False)
            writer.AddAttribute(HtmlTextWriterAttribute.Value, _selectedValuesList, False)
            writer.RenderBeginTag(HtmlTextWriterTag.Input)
            writer.RenderEndTag()
        End If
        ' close center cell
        writer.RenderEndTag()
        writer.RenderBeginTag(HtmlTextWriterTag.Td)
        writer.AddAttribute(HtmlTextWriterAttribute.Valign, "top", False)
        writer.AddAttribute(HtmlTextWriterAttribute.Align, "left", False)
        ' write left right box's label
        Me.WriteLabel(writer, SideType.Right, "CurrentLabel", Me.CurrentLabelText)
        ' write right list box    
        Me.WriteListBox(writer, SideType.Right, _rightListName, Rows.ToString, Me.ListBoxWidth)
        ' write right list box options
        Me.WriteListItems(writer, True)
        ' close lift list box
        writer.RenderEndTag()
        ' close left cell and the row
        writer.RenderEndTag()
        writer.RenderEndTag()

        'write filter if filter enabled
        If Me.EnableFilter Then
            ' write one and only row
            writer.RenderBeginTag(HtmlTextWriterTag.Tr)
            ' write left cell
            writer.RenderBeginTag(HtmlTextWriterTag.Td)
            Me.WriteFilter(writer)
            ' close left cell and the row
            writer.RenderEndTag()
            writer.RenderEndTag()
        End If
    End Sub

    Private Sub WriteBreak(ByVal writer As HtmlTextWriter)
        writer.WriteBeginTag("br")
        writer.Write(HtmlTextWriter.SelfClosingTagEnd)
    End Sub

    Private Sub WriteLabel(ByVal writer As HtmlTextWriter, ByVal side As SideType, ByVal id As String, ByVal text As String)
        Dim style As String = ""
        Dim className As String = ""
        If side.Equals(SideType.Left) Then
            style = Me.GetCssStyle(_availableLabelStyle, className)
        Else
            style = Me.GetCssStyle(_selectedLabelStyle, className)
        End If
        writer.AddAttribute(HtmlTextWriterAttribute.Id, (Me.ClientID + ("_" + id)), False)
        If Not String.IsNullOrEmpty(style) Then
            writer.AddAttribute(HtmlTextWriterAttribute.Style, style)
        End If
        If (className.Length > 0) Then
            writer.AddAttribute(HtmlTextWriterAttribute.Class, className, False)
        End If
        writer.RenderBeginTag(HtmlTextWriterTag.Label)
        writer.WriteEncodedText(text)
        writer.RenderEndTag()
        Me.WriteBreak(writer)
    End Sub

    Private Sub WriteButton(ByVal writer As HtmlTextWriter, ByVal id As String, ByVal buttonText As String, ByVal actionType As ButtonActionType)
        Dim sourceListId As String = String.Empty
        Dim targetListId As String = String.Empty
        Dim style As String = String.Empty
        Dim className As String = String.Empty
        Dim isTargetListSelected As Boolean = False
        Select Case (actionType)
            Case ButtonActionType.Add
                style = Me.GetCssStyle(_addButtonStyle, className)
                sourceListId = _leftListName
                targetListId = _rightListName
                isTargetListSelected = True
            Case ButtonActionType.AddAll
                style = Me.GetCssStyle(_addAllButtonStyle, className)
                sourceListId = _leftListName
                targetListId = _rightListName
                isTargetListSelected = True
            Case ButtonActionType.Remove
                style = Me.GetCssStyle(_removeButtonStyle, className)
                sourceListId = _rightListName
                targetListId = _leftListName
            Case ButtonActionType.RemoveAll
                style = Me.GetCssStyle(_removeAllButtonStyle, className)
                sourceListId = _rightListName
                targetListId = _leftListName
        End Select
        'Dim allFlag As Boolean = ((actionType = ButtonActionType.Add) _
        '            OrElse (actionType = ButtonActionType.Remove))
        Dim allFlag As Boolean = IIf((actionType = ButtonActionType.Add) OrElse (actionType = ButtonActionType.Remove), False, True)
        writer.AddAttribute(HtmlTextWriterAttribute.Type, "button", False)
        writer.AddAttribute(HtmlTextWriterAttribute.Id, (Me.ClientID + ("_" + id)), False)
        writer.AddAttribute(HtmlTextWriterAttribute.Value, buttonText, True)
        writer.AddAttribute(HtmlTextWriterAttribute.Onclick, String.Format("{0}MoveOption('{1}_{2}', '{3}_{4}', {5}, '{6}_{7}', {8})", _clientCodePrefix, Me.ClientID, sourceListId, Me.ClientID, targetListId, isTargetListSelected.ToString.ToLower, Me.ClientID, _selectedValuesName, allFlag.ToString.ToLower), False)
        If Not String.IsNullOrEmpty(style) Then
            writer.AddAttribute(HtmlTextWriterAttribute.Style, style)
        End If
        If Not String.IsNullOrEmpty(className) Then
            writer.AddAttribute(HtmlTextWriterAttribute.Class, className)
        End If
        writer.RenderBeginTag(HtmlTextWriterTag.Input)
        writer.RenderEndTag()
        Me.WriteBreak(writer)
    End Sub

    Private Sub WriteListBox(ByVal writer As HtmlTextWriter, ByVal side As SideType, ByVal id As String, ByVal rowCount As String, ByVal listBoxWidth As String)
        Dim className As String = String.Empty
        Dim style As String = IIf(side.Equals(SideType.Left), Me.GetCssStyle(_availableListBoxStyle, className).ToString, Me.GetCssStyle(_selectedListBoxStyle, className).ToString)

        writer.AddAttribute(HtmlTextWriterAttribute.Id, (Me.ClientID + ("_" + id)), False)
        writer.AddAttribute(HtmlTextWriterAttribute.Name, (Me.UniqueID + ("_" + id)), False)
        If Me.SelectionMode.Equals(SelectionModeType.Multiple) Then
            writer.AddAttribute(HtmlTextWriterAttribute.Multiple, "true")
        End If
        writer.AddAttribute(HtmlTextWriterAttribute.Size, rowCount, False)
        If Not String.IsNullOrEmpty(style) Then
            writer.AddAttribute(HtmlTextWriterAttribute.Style, style)
        End If
        If Not String.IsNullOrEmpty(className) Then
            writer.AddAttribute(HtmlTextWriterAttribute.Class, className)
        End If
        writer.RenderBeginTag(HtmlTextWriterTag.Select)



    End Sub

    Private Sub WriteListItems(ByVal writer As HtmlTextWriter, ByVal writeSelected As Boolean)
        If (_items Is Nothing) Then
            Return
        End If
        For Each item As ListItem In _items
            If (item.Selected = writeSelected) Then
                writer.AddAttribute(HtmlTextWriterAttribute.Value, item.Value, True)
                writer.AddAttribute("title", item.Attributes("title").ToString, True)
                writer.AddAttribute("Code", item.Attributes("Code").ToString, True)
                writer.RenderBeginTag(HtmlTextWriterTag.Option)
                If Me.AddCode Then
                    writer.WriteEncodedText(item.Attributes("Code").ToString & " - " & item.Text)
                Else
                    writer.WriteEncodedText(item.Text)
                End If
                writer.RenderEndTag()
            End If
        Next
    End Sub

    Private Function ResizeStatesArray(ByVal items As ListItemCollection, ByVal originalStates As ArrayList) As ArrayList
        Dim newOriginalStates As ArrayList = New ArrayList(items.Count)
        ' initialize array
        For Each item As ListItem In items
            newOriginalStates.Add(0)
        Next
        ' copy states to new larger array matching count of the list items collection
        Dim i As Integer = 0
        Do While (i < originalStates.Count)
            newOriginalStates(i) = originalStates(i)
            i = (i + 1)
        Loop
        Return newOriginalStates
    End Function

    Private Function GetCssStyle(ByVal style As Style, ByRef className As String) As String
        If (style Is Nothing) Then
            className = ""
            Return String.Empty
        End If
        className = style.CssClass
        Dim sb As StringBuilder = New StringBuilder(256)
        Dim c As Color
        c = style.ForeColor
        If Not c.IsEmpty Then
            sb.Append("color:")
            sb.Append(ColorTranslator.ToHtml(c))
            sb.Append(";")
        End If
        c = style.BackColor
        If Not c.IsEmpty Then
            sb.Append("background-color:")
            sb.Append(ColorTranslator.ToHtml(c))
            sb.Append(";")
        End If
        Dim fi As FontInfo = style.Font
        Dim s As String
        s = fi.Name
        If (s.Length <> 0) Then
            sb.Append("font-family:'")
            sb.Append(s)
            sb.Append("';")
        End If
        If fi.Bold Then
            sb.Append("font-weight:bold;")
        End If
        If fi.Italic Then
            sb.Append("font-style:italic;")
        End If
        s = String.Empty
        If fi.Underline Then
            s = (s + "underline")
        End If
        If fi.Strikeout Then
            s = (s + " line-through")
        End If
        If fi.Overline Then
            s = (s + " overline")
        End If
        If (s.Length <> 0) Then
            sb.Append("text-decoration:")
            sb.Append(s)
            sb.Append(Microsoft.VisualBasic.ChrW(59))
        End If
        Dim fu As FontUnit = fi.Size
        If (fu.IsEmpty = False) Then
            sb.Append("font-size:")
            sb.Append(fu.ToString(CultureInfo.InvariantCulture))
            sb.Append(Microsoft.VisualBasic.ChrW(59))
        End If
        s = String.Empty
        Dim u As Unit = style.BorderWidth
        Dim bs As BorderStyle = style.BorderStyle
        If (u.IsEmpty = False) Then
            s = u.ToString(CultureInfo.InvariantCulture)
            If (bs = BorderStyle.NotSet) Then
                s = (s + " solid")
            End If
        End If
        c = style.BorderColor
        If Not c.IsEmpty Then
            s = (s + (" " + ColorTranslator.ToHtml(c)))
        End If
        If (bs <> BorderStyle.NotSet) Then
            s = (s + (" " + System.Enum.Format(GetType(BorderStyle), bs, "G")))
        End If
        If (s.Length <> 0) Then
            sb.Append("border:")
            sb.Append(s)
            sb.Append(Microsoft.VisualBasic.ChrW(59))
        End If
        u = style.Width
        If Not u.IsEmpty Then
            sb.Append("width:")
            sb.Append(u.ToString)
            sb.Append(Microsoft.VisualBasic.ChrW(59))
        End If
        u = style.Height
        If Not u.IsEmpty Then
            sb.Append("height:")
            sb.Append(u.ToString)
            sb.Append(Microsoft.VisualBasic.ChrW(59))
        End If
        Return sb.ToString
    End Function
#Region "Public Methods"

    Public Overrides Sub DataBind()
        If (String.IsNullOrEmpty(_dataTextField) OrElse String.IsNullOrEmpty(_dataValueField)) Then
            Throw New ApplicationException("Data value or text field has not been set.")
        End If
        Dim list As IEnumerable = Nothing
        If (TypeOf _dataSource Is IEnumerable) Then
            list = CType(_dataSource, IEnumerable)
        ElseIf (TypeOf _dataSource Is IListSource) Then
            Dim listSource As IListSource = CType(_dataSource, IListSource)
            Dim memberList As IList = listSource.GetList
            list = CType(memberList, IEnumerable)
        End If
        For Each obj As Object In list
            Dim itemText As String = DataBinder.GetPropertyValue(obj, _dataTextField, String.Empty)
            Dim itemValue As String = DataBinder.GetPropertyValue(obj, _dataValueField, String.Empty)
            Dim itemCode As String = DataBinder.GetPropertyValue(obj, _dataCodeField, String.Empty)
            Dim itemToolTip As String = DataBinder.GetPropertyValue(obj, MyBase.ToolTip, String.Empty)
            Dim item As ListItem = New ListItem(WebUtility.HtmlDecode(itemText), itemValue)

            item.Attributes.Add("title", itemToolTip)
            item.Attributes.Add("Code", itemCode)

            Me.Items.Add(item)
        Next
        If Not IsTrackingViewState Then
            TrackViewState()
        End If
    End Sub

    Public Overloads Sub SetSelectedValues(ByVal values() As Object)
        Me.SetSelectedValues(False, values)
    End Sub

    Public Sub BaselineValues()
        If (_originalStates.Count <> Items.Count) Then
            _originalStates = Me.ResizeStatesArray(_items, _originalStates)
        End If
        Dim i As Integer = 0
        Do While (i < Items.Count)
            _originalStates(i) = IIf(Items(i).Selected, 1, 0)
            i = (i + 1)
        Loop
    End Sub

    Public Function GetSelectedItems() As ListItemCollection
        Dim selectedItems As ListItemCollection = New ListItemCollection
        For Each item As ListItem In _items
            If item.Selected Then
                selectedItems.Add(item)
            End If
        Next
        Return selectedItems
    End Function

    Public Function GetAddedItems() As ListItemCollection
        Dim addedItems As ListItemCollection = New ListItemCollection
        Dim idx As Integer = 0
        For Each item As ListItem In _items
            If (item.Selected _
                        AndAlso (CType(_originalStates(idx), Integer) = 0)) Then
                ' need to return a copy and not a reference
                Dim newItem As ListItem = New ListItem(item.Text, item.Value, True)
                newItem.Attributes.Add("Code", item.Attributes("Code").ToString)
                addedItems.Add(newItem)
            End If
            idx = (idx + 1)
        Next
        Return addedItems
    End Function

    Public Function GetRemovedItems() As ListItemCollection
        Dim removedItems As ListItemCollection = New ListItemCollection
        Dim idx As Integer = 0
        For Each item As ListItem In _items
            If (Not item.Selected _
                        AndAlso (CType(_originalStates(idx), Integer) = 1)) Then
                ' need to return a copy and not a reference
                Dim newItem As ListItem = New ListItem(item.Text, item.Value, False)
                newItem.Attributes.Add("Code", item.Attributes("Code").ToString)
                removedItems.Add(newItem)
            End If
            idx = (idx + 1)
        Next
        Return removedItems
    End Function

    Public Function ListItemToIntArray(ByVal listItems As ListItemCollection) As Integer()
        Dim ids() As Integer = New Integer((listItems.Count) - 1) {}
        Dim i As Integer = 0
        Do While (i < listItems.Count)
            Dim value As Integer
            If Not Integer.TryParse(listItems(i).Value, value) Then
                Throw New InvalidCastException(String.Format("Unable to convert '{0}' to type int.", listItems(i).Value))
            End If
            ids(i) = value
            i = (i + 1)
        Loop
        Return ids
    End Function

    Public Function ListItemToObjectArray(ByVal listItems As ListItemCollection) As Object()
        Dim ids() As Object = New Object((listItems.Count) - 1) {}
        Dim i As Integer = 0
        Do While (i < listItems.Count)
            ids(i) = listItems(i).Value
            i = (i + 1)
        Loop
        Return ids
    End Function
#End Region

    Private Function IsListValuesSame(ByVal list1 As String, ByVal list2 As String) As Boolean
        Dim array1() As String = list1.Split(Microsoft.VisualBasic.ChrW(44))
        Dim array2() As String = list2.Split(Microsoft.VisualBasic.ChrW(44))
        If (array1.Length <> array2.Length) Then
            Return False
        End If
        For Each value1 As String In array1
            Dim matchFound As Boolean = False
            For Each value2 As String In array2
                If value1.Equals(value2) Then
                    matchFound = True
                    Exit For
                End If
            Next
            If Not matchFound Then
                Return False
            End If
        Next
        Return True
    End Function

    Private Overloads Sub SetSelectedValues(ByVal fromPostback As Boolean, ByVal values() As Object)
        If (_originalStates.Count <> Items.Count) Then
            _originalStates = Me.ResizeStatesArray(_items, _originalStates)
        End If
        _selectedValuesList = ""
        Me.SelectedValues = ""
        For Each item As ListItem In Items
            item.Selected = False
        Next
        For Each value As Object In values
            Dim idx As Integer = 0
            For Each item As ListItem In Items
                If item.Value.Equals(value.ToString, StringComparison.OrdinalIgnoreCase) Then
                    item.Selected = True
                    _selectedValuesList = (_selectedValuesList + ("," + item.Value))
                    If Not fromPostback Then
                        _originalStates(idx) = 1
                    End If
                End If
                idx = (idx + 1)
            Next
        Next
        If _selectedValuesList.StartsWith(",") Then
            _selectedValuesList = _selectedValuesList.Substring(1)
            Me.SelectedValues = _selectedValuesList
        End If
    End Sub
#Region "INamingContainter Implementation (View State)"

    Protected Overrides Function SaveViewState() As Object
        Dim state() As Object = New Object((12) - 1) {}
        state(0) = MyBase.SaveViewState
        If _items IsNot Nothing Then
            state(1) = TryCast(_items, IStateManager).SaveViewState()
            If _items.Count > 0 Then
                Dim oAttArray As Object()
                ReDim oAttArray(_items.Count - 1)
                For iCount As Integer = 0 To _items.Count - 1
                    oAttArray(iCount) = _items(iCount).Attributes("Code") + "-" + _items(iCount).Attributes("title")
                Next
                state(11) = oAttArray
            End If
        End If
        If _addButtonStyle IsNot Nothing Then
            state(2) = TryCast(_addButtonStyle, IStateManager).SaveViewState()
        End If
        If _addAllButtonStyle IsNot Nothing Then
            state(3) = TryCast(_addAllButtonStyle, IStateManager).SaveViewState()
        End If
        If _removeButtonStyle IsNot Nothing Then
            state(4) = TryCast(_removeButtonStyle, IStateManager).SaveViewState()
        End If
        If _removeAllButtonStyle IsNot Nothing Then
            state(5) = TryCast(_removeAllButtonStyle, IStateManager).SaveViewState()
        End If
        If _availableLabelStyle IsNot Nothing Then
            state(6) = TryCast(_availableLabelStyle, IStateManager).SaveViewState()
        End If
        If _selectedLabelStyle IsNot Nothing Then
            state(7) = TryCast(_selectedLabelStyle, IStateManager).SaveViewState()
        End If
        If _availableListBoxStyle IsNot Nothing Then
            state(8) = TryCast(_availableListBoxStyle, IStateManager).SaveViewState()
        End If
        If _selectedListBoxStyle IsNot Nothing Then
            state(9) = TryCast(_selectedListBoxStyle, IStateManager).SaveViewState()
        End If
        state(10) = _originalStates
        Dim i As Integer = 0
        Do While (i < 10)
            If (Not (state(i)) Is Nothing) Then
                Return state
            End If
            i = (i + 1)
        Loop
        ' Another perfomance optimization. If no modifications were made to any
        ' properties from their persisted state, the view state for this control
        ' is null. Returning null, rather than an array of null values helps
        ' minimize the view state significantly.
        Return Nothing
    End Function

    Protected Overrides Sub LoadViewState(ByVal savedState As Object)
        Dim baseState As Object = Nothing
        Dim state() As Object = Nothing
        If (Not (savedState) Is Nothing) Then
            state = CType(savedState, Object())
            baseState = state(0)
        End If
        ' Always call the base class, even if the state is null, so
        ' the base class gets a chance to fully implement its LoadViewState
        ' functionality.
        MyBase.LoadViewState(baseState)
        If (state Is Nothing) Then
            Return
        End If
        If (Not (state(1)) Is Nothing) Then
            CType(Items, IStateManager).LoadViewState(state(1))
            Dim title As String
            Dim oAttArray As Object()
            ReDim oAttArray(_items.Count - 1)
            oAttArray = state(11)
            'CType(oAttArray, Object()).LoadViewState(state(11))
            For iCount As Integer = 0 To _items.Count - 1
                title = oAttArray(iCount).ToString().Substring(oAttArray(iCount).ToString().IndexOf("-") + 1).Trim
                _items(iCount).Attributes.Add("title", title)
                _items(iCount).Attributes.Add("Code", oAttArray(iCount).ToString().Split("-")(0))
            Next
        End If
        If (Not (state(2)) Is Nothing) Then
            CType(AddButtonStyle, IStateManager).LoadViewState(state(2))
        End If
        If (Not (state(3)) Is Nothing) Then
            CType(AddAllButtonStyle, IStateManager).LoadViewState(state(3))
        End If
        If (Not (state(4)) Is Nothing) Then
            CType(RemoveButtonStyle, IStateManager).LoadViewState(state(4))
        End If
        If (Not (state(5)) Is Nothing) Then
            CType(RemoveAllButtonStyle, IStateManager).LoadViewState(state(5))
        End If
        If (Not (state(6)) Is Nothing) Then
            CType(AvailableLabelStyle, IStateManager).LoadViewState(state(6))
        End If
        If (Not (state(7)) Is Nothing) Then
            CType(SelectedLabelStyle, IStateManager).LoadViewState(state(7))
        End If
        If (Not (state(8)) Is Nothing) Then
            CType(AvailableListBoxStyle, IStateManager).LoadViewState(state(8))
        End If
        If (Not (state(9)) Is Nothing) Then
            CType(SelectedListBoxStyle, IStateManager).LoadViewState(state(9))
        End If
        If (Not (state(10)) Is Nothing) Then
            _originalStates = CType(state(10), ArrayList)
        End If
    End Sub

    Protected Overrides Sub TrackViewState()
        'if (_items != null)
        CType(_items, IStateManager).TrackViewState()
        If (Not (_addButtonStyle) Is Nothing) Then
            CType(_addButtonStyle, IStateManager).TrackViewState()
        End If
        If (Not (_addAllButtonStyle) Is Nothing) Then
            CType(_addAllButtonStyle, IStateManager).TrackViewState()
        End If
        If (Not (_removeAllButtonStyle) Is Nothing) Then
            CType(_removeAllButtonStyle, IStateManager).TrackViewState()
        End If
        If (Not (_removeButtonStyle) Is Nothing) Then
            CType(_removeButtonStyle, IStateManager).TrackViewState()
        End If
        If (Not (_availableLabelStyle) Is Nothing) Then
            CType(_availableLabelStyle, IStateManager).TrackViewState()
        End If
        If (Not (_selectedLabelStyle) Is Nothing) Then
            CType(_selectedLabelStyle, IStateManager).TrackViewState()
        End If
        If (Not (_availableListBoxStyle) Is Nothing) Then
            CType(_availableListBoxStyle, IStateManager).TrackViewState()
        End If
        If (Not (_selectedListBoxStyle) Is Nothing) Then
            CType(_selectedListBoxStyle, IStateManager).TrackViewState()
        End If
    End Sub
#End Region
#Region "IPostBackDataHandler Implementation"

    Public Overridable Function LoadPostData(ByVal postDataKey As String, ByVal values As System.Collections.Specialized.NameValueCollection) As Boolean Implements System.Web.UI.IPostBackDataHandler.LoadPostData
        Dim presentValue As String = Me.SelectedValues
        Dim postedValue As String = values(postDataKey)
        Me.SetSelectedValues(True, postedValue.Split(","))
        If Me.IsListValuesSame(presentValue, postedValue) Then
            Me.SelectedValues = postedValue
            Return True
        End If
        Return False
    End Function

    Public Overridable Sub RaisePostDataChangedEvent() Implements System.Web.UI.IPostBackDataHandler.RaisePostDataChangedEvent
        OnSelectionChanged(EventArgs.Empty)
    End Sub


#End Region
#Region "Event Implementation"

    <Category("Action"), _
     Description("Raised when the selection changes.")> _
    Public Event SelectionChanged As EventHandler

    Protected Overridable Sub OnSelectionChanged(ByVal e As EventArgs)
        Dim selectionChangedHandler As EventHandler = CType(Events(EventSelectionChanged), EventHandler)
        If (Not (selectionChangedHandler) Is Nothing) Then
            selectionChangedHandler(Me, e)
        End If
    End Sub
#End Region
#Region "Properties"
#Region "Appearance"

    <Category("Appearance"), _
     DefaultValue("100%"), _
     Description("Width of list boxes."), _
     Bindable(True)> _
    Public Property ListBoxWidth() As String
        Get
            Return DirectCast(IIf(String.IsNullOrEmpty(Convert.ToString(ViewState("ListBoxWidth"))), String.Empty, Convert.ToString(ViewState("ListBoxWidth"))), String)
        End Get
        Set(ByVal value As String)
            ViewState("ListBoxWidth") = value
        End Set
    End Property

    <Category("Appearance"), _
     DefaultValue("Available Options:"), _
     Description("Text used to represent the available list box."), _
     Bindable(True)> _
    Public Property AvailableLabelText() As String
        Get
            Return DirectCast(IIf(String.IsNullOrEmpty(Convert.ToString(ViewState("AvailableLabelText"))), String.Empty, Convert.ToString(ViewState("AvailableLabelText"))), String)
        End Get
        Set(ByVal value As String)
            ViewState("AvailableLabelText") = value
        End Set
    End Property

    <Category("Appearance"), _
     DefaultValue("Selected Options:"), _
     Description("Text used to represent the current list box."), _
     Bindable(True)> _
    Public Property CurrentLabelText() As String
        Get
            Return DirectCast(IIf(String.IsNullOrEmpty(Convert.ToString(ViewState("CurrentLabelText"))), String.Empty, Convert.ToString(ViewState("CurrentLabelText"))), String)
        End Get
        Set(ByVal value As String)
            ViewState("CurrentLabelText") = value
        End Set
    End Property

    <Category("Appearance"), _
     DefaultValue(""), _
     Description("The number of visible rows to display."), _
     Bindable(True)> _
    Public Property Rows() As Integer
        Get
            Return ViewState("Rows") 'DirectCast(IIf(String.IsNullOrEmpty(Convert.ToInt32(ViewState("AddButtonText"))), 0, Convert.ToInt32(ViewState("AddButtonText"))), Integer)            
        End Get
        Set(ByVal value As Integer)
            ViewState("Rows") = value
        End Set
    End Property

    <Category("Appearance"), _
     DefaultValue("Add >>"), _
     Description("Text used to for the add button."), _
     Bindable(True)> _
    Public Property AddButtonText() As String
        Get
            Return DirectCast(IIf(String.IsNullOrEmpty(Convert.ToString(ViewState("AddButtonText"))), String.Empty, Convert.ToString(ViewState("AddButtonText"))), String)
        End Get
        Set(ByVal value As String)
            ViewState("AddButtonText") = value
        End Set
    End Property

    <Category("Appearance"), _
     DefaultValue("Add All >>"), _
     Description("Text used to for the add all button."), _
     Bindable(True)> _
    Public Property AddAllButtonText() As String
        Get
            Return DirectCast(IIf(String.IsNullOrEmpty(Convert.ToString(ViewState("AddAllButtonText"))), String.Empty, Convert.ToString(ViewState("AddAllButtonText"))), String)
        End Get
        Set(ByVal value As String)
            ViewState("AddAllButtonText") = value
        End Set
    End Property

    <Category("Appearance"), _
     DefaultValue("<< Remove"), _
     Description("Text used to for the remove button."), _
     Bindable(True)> _
    Public Property RemoveButtonText() As String
        Get
            Return DirectCast(IIf(String.IsNullOrEmpty(Convert.ToString(ViewState("RemoveButtonText"))), String.Empty, Convert.ToString(ViewState("RemoveButtonText"))), String)
        End Get
        Set(ByVal value As String)
            ViewState("RemoveButtonText") = value
        End Set
    End Property

    <Category("Appearance"), _
     DefaultValue("<< Remove All"), _
     Description("Text used to for the remove all button."), _
     Bindable(True)> _
    Public Property RemoveAllButtonText() As String
        Get
            Return DirectCast(IIf(String.IsNullOrEmpty(Convert.ToString(ViewState("RemoveAllButtonText"))), String.Empty, Convert.ToString(ViewState("RemoveAllButtonText"))), String)
        End Get
        Set(ByVal value As String)
            ViewState("RemoveAllButtonText") = value
        End Set
    End Property

    <Category("Appearance"), _
     DefaultValue("90px"), _
     Description("Width used for each button."), _
     Bindable(True)> _
    Public Property ButtonWidth() As String
        Get
            Return DirectCast(IIf(String.IsNullOrEmpty(Convert.ToString(ViewState("ButtonWidth"))), String.Empty, Convert.ToString(ViewState("ButtonWidth"))), String)
        End Get
        Set(ByVal value As String)
            ViewState("ButtonWidth") = value
        End Set
    End Property

    <Category("Appearance"), _
     DefaultValue("Find Code:"), _
     Description("Text used to filter the list"), _
     Bindable(True)> _
    Public Property FilterLabelText() As String
        Get
            Return DirectCast(IIf(String.IsNullOrEmpty(Convert.ToString(ViewState("FilterLabelText"))), String.Empty, Convert.ToString(ViewState("FilterLabelText"))), String)
        End Get
        Set(ByVal value As String)
            ViewState("FilterLabelText") = value
        End Set
    End Property
#End Region
#Region "Behavior"

    <Category("Behavior"), _
     DefaultValue(SelectionModeType.Multiple), _
     Description("The selection mode."), _
     Bindable(True)> _
    Public Property SelectionMode() As SelectionModeType
        Get
            Return ViewState("SelectionMode") 'DirectCast(IIf(String.IsNullOrEmpty(Convert.ToInt32(ViewState("FilterLabelText"))), 0, Convert.ToInt32(ViewState("FilterLabelText"))), Integer)            
        End Get
        Set(ByVal value As SelectionModeType)
            ViewState("SelectionMode") = value
        End Set
    End Property

    <Category("Behavior"), _
     DefaultValue(False), _
     Description("Determinds visibility of the remove all button."), _
     Bindable(True)> _
    Public Property DisplayRemoveAllButton() As Boolean
        Get
            Return DirectCast(IIf(String.IsNullOrEmpty(Convert.ToBoolean(ViewState("DisplayRemoveAllButton"))), False, Convert.ToBoolean(ViewState("DisplayRemoveAllButton"))), Boolean)
        End Get
        Set(ByVal value As Boolean)
            ViewState("DisplayRemoveAllButton") = value
        End Set
    End Property

    <Category("Behavior"), _
     DefaultValue(False), _
     Description("Determinds visibility of the add all button."), _
     Bindable(True)> _
    Public Property DisplayAddAllButton() As Boolean
        Get
            Return DirectCast(IIf(String.IsNullOrEmpty(Convert.ToBoolean(ViewState("DisplayAddAllButton"))), False, Convert.ToBoolean(ViewState("DisplayAddAllButton"))), Boolean)
        End Get
        Set(ByVal value As Boolean)
            ViewState("DisplayAddAllButton") = value
        End Set
    End Property

    <Category("Behavior"), _
     DefaultValue(False), _
     Description("Determinds visibility of the add button."), _
     Bindable(True)> _
    Public Property DisplayAddButton() As Boolean
        Get
            Return DirectCast(IIf(String.IsNullOrEmpty(Convert.ToBoolean(ViewState("DisplayAddButton"))), False, Convert.ToBoolean(ViewState("DisplayAddButton"))), Boolean)
        End Get
        Set(ByVal value As Boolean)
            ViewState("DisplayAddButton") = value
        End Set
    End Property

    <Category("Behavior"), _
     DefaultValue(False), _
     Description("Determinds visibility of the remove button."), _
     Bindable(True)> _
    Public Property DisplayRemoveButton() As Boolean
        Get
            Return DirectCast(IIf(String.IsNullOrEmpty(Convert.ToBoolean(ViewState("DisplayRemoveButton"))), False, Convert.ToBoolean(ViewState("DisplayRemoveButton"))), Boolean)
        End Get
        Set(ByVal value As Boolean)
            ViewState("DisplayRemoveButton") = value
        End Set
    End Property

    <Category("Behavior"), _
     DefaultValue(False), _
     Description("Determinds visibility of the filter."), _
     Bindable(True)> _
    Public Property EnableFilter() As Boolean
        Get
            Return DirectCast(IIf(String.IsNullOrEmpty(Convert.ToBoolean(ViewState("EnableFilter"))), False, Convert.ToBoolean(ViewState("EnableFilter"))), Boolean)
        End Get
        Set(ByVal value As Boolean)
            ViewState("EnableFilter") = value
        End Set
    End Property

    <Category("Behavior"), _
     DefaultValue(False), _
     Description("Determinds visibility of the Code with Description."), _
     Bindable(True)> _
    Public Property AddCode() As Boolean
        Get
            Return DirectCast(IIf(String.IsNullOrEmpty(Convert.ToBoolean(ViewState("AddCode"))), False, Convert.ToBoolean(ViewState("AddCode"))), Boolean)
        End Get
        Set(ByVal value As Boolean)
            ViewState("AddCode") = value
        End Set
    End Property

    <Category("Data"),
     DefaultValue(""),
     Description("The field in the data source that provides the item tooltip."),
     Bindable(True)>
    Public Overrides Property ToolTip() As String
        Get
            Return MyBase.ToolTip
        End Get
        Set(ByVal value As String)
            MyBase.ToolTip = value
        End Set
    End Property
#End Region
#Region "Data"

    <Category("Data"), _
     DefaultValue(""), _
     Description("Data table, view, or collection for the current list box to bind to."), _
     Bindable(True)> _
    Public WriteOnly Property DataSource() As Object
        Set(ByVal value As Object)
            _dataSource = value
        End Set
    End Property

    <Category("Data"), _
     DefaultValue(""), _
     Description("The field in the data source that provides the item text."), _
     Bindable(True)> _
    Public WriteOnly Property DataTextField() As String
        Set(ByVal value As String)
            _dataTextField = value
        End Set
    End Property

    <Category("Data"), _
     DefaultValue(""), _
     Description("The field in the data source that provides the item value."), _
     Bindable(True)> _
    Public WriteOnly Property DataValueField() As String
        Set(ByVal value As String)
            _dataValueField = value
        End Set
    End Property

    <Category("Data"), _
     DefaultValue(""), _
     Description("The field in the data source that provides the item key."), _
     Bindable(True)> _
    Public WriteOnly Property DataCodeField() As String
        Set(ByVal value As String)
            _DataCodeField = value
        End Set
    End Property
#End Region
#Region "Style"

    <Category("Style"), _
     Description("The style applied to the add button."), _
     DesignerSerializationVisibility(DesignerSerializationVisibility.Content), _
     NotifyParentProperty(True), _
     PersistenceMode(PersistenceMode.InnerProperty)> _
    Public Overridable ReadOnly Property AddButtonStyle() As Style
        Get
            If (_addButtonStyle Is Nothing) Then
                _addButtonStyle = New Style
                If IsTrackingViewState Then
                    CType(_addButtonStyle, IStateManager).TrackViewState()
                End If
            End If
            Return _addButtonStyle
        End Get
    End Property

    <Category("Style"), _
     Description("The style applied to the add all button."), _
     DesignerSerializationVisibility(DesignerSerializationVisibility.Content), _
     NotifyParentProperty(True), _
     PersistenceMode(PersistenceMode.InnerProperty)> _
    Public Overridable ReadOnly Property AddAllButtonStyle() As Style
        Get
            If (_addAllButtonStyle Is Nothing) Then
                _addAllButtonStyle = New Style
                If IsTrackingViewState Then
                    CType(_addAllButtonStyle, IStateManager).TrackViewState()
                End If
            End If
            Return _addAllButtonStyle
        End Get
    End Property

    <Category("Style"), _
     Description("The style applied to the remove button."), _
     DesignerSerializationVisibility(DesignerSerializationVisibility.Content), _
     NotifyParentProperty(True), _
     PersistenceMode(PersistenceMode.InnerProperty)> _
    Public Overridable ReadOnly Property RemoveButtonStyle() As Style
        Get
            If (_removeButtonStyle Is Nothing) Then
                _removeButtonStyle = New Style
                If IsTrackingViewState Then
                    CType(_removeButtonStyle, IStateManager).TrackViewState()
                End If
            End If
            Return _removeButtonStyle
        End Get
    End Property

    <Category("Style"), _
     Description("The style applied to the remove all button."), _
     DesignerSerializationVisibility(DesignerSerializationVisibility.Content), _
     NotifyParentProperty(True), _
     PersistenceMode(PersistenceMode.InnerProperty)> _
    Public Overridable ReadOnly Property RemoveAllButtonStyle() As Style
        Get
            If (_removeAllButtonStyle Is Nothing) Then
                _removeAllButtonStyle = New Style
                If IsTrackingViewState Then
                    CType(_removeAllButtonStyle, IStateManager).TrackViewState()
                End If
            End If
            Return _removeAllButtonStyle
        End Get
    End Property

    <Category("Style"), _
     Description("The style applied to the available listbox's label."), _
     DesignerSerializationVisibility(DesignerSerializationVisibility.Content), _
     NotifyParentProperty(True), _
     PersistenceMode(PersistenceMode.InnerProperty)> _
    Public Overridable ReadOnly Property AvailableLabelStyle() As Style
        Get
            If (_availableLabelStyle Is Nothing) Then
                _availableLabelStyle = New Style
                If IsTrackingViewState Then
                    CType(_availableLabelStyle, IStateManager).TrackViewState()
                End If
            End If
            Return _availableLabelStyle
        End Get
    End Property

    <Category("Style"), _
     Description("The style applied to the selected listbox's label."), _
     DesignerSerializationVisibility(DesignerSerializationVisibility.Content), _
     NotifyParentProperty(True), _
     PersistenceMode(PersistenceMode.InnerProperty)> _
    Public Overridable ReadOnly Property SelectedLabelStyle() As Style
        Get
            If (_selectedLabelStyle Is Nothing) Then
                _selectedLabelStyle = New Style
                If IsTrackingViewState Then
                    CType(_selectedLabelStyle, IStateManager).TrackViewState()
                End If
            End If
            Return _selectedLabelStyle
        End Get
    End Property

    <Category("Style"), _
     Description("The style applied to the available listbox."), _
     DesignerSerializationVisibility(DesignerSerializationVisibility.Content), _
     NotifyParentProperty(True), _
     PersistenceMode(PersistenceMode.InnerProperty)> _
    Public Overridable ReadOnly Property AvailableListBoxStyle() As Style
        Get
            If (_availableListBoxStyle Is Nothing) Then
                _availableListBoxStyle = New Style
                If IsTrackingViewState Then
                    CType(_availableListBoxStyle, IStateManager).TrackViewState()
                End If
            End If
            Return _availableListBoxStyle
        End Get
    End Property

    <Category("Style"), _
     Description("The style applied to the selected listbox."), _
     DesignerSerializationVisibility(DesignerSerializationVisibility.Content), _
     NotifyParentProperty(True), _
     PersistenceMode(PersistenceMode.InnerProperty)> _
    Public Overridable ReadOnly Property SelectedListBoxStyle() As Style
        Get
            If (_selectedListBoxStyle Is Nothing) Then
                _selectedListBoxStyle = New Style
                If IsTrackingViewState Then
                    CType(_selectedListBoxStyle, IStateManager).TrackViewState()
                End If
            End If
            Return _selectedListBoxStyle
        End Get
    End Property
#End Region
#Region "Misc"

    <Category("Misc"), _
     Description("The collection of items between the two list boxes."), _
     Bindable(False), _
     DesignerSerializationVisibility(DesignerSerializationVisibility.Content), _
     NotifyParentProperty(True), _
     PersistenceMode(PersistenceMode.InnerDefaultProperty), _
     Editor(GetType(ListItemsCollectionEditor), GetType(System.Drawing.Design.UITypeEditor))> _
    Public ReadOnly Property Items() As ListItemCollection
        Get
            If (_items Is Nothing) Then
                _items = New ListItemCollection
                If IsTrackingViewState Then
                    CType(Me._items, IStateManager).TrackViewState()
                End If
            End If
            Return Me._items
        End Get
    End Property
#End Region

    Public Property SelectedValues() As String
        Get
            Return DirectCast(IIf(String.IsNullOrEmpty(Convert.ToString(ViewState(Me.ClientID & "SelectedValues"))), String.Empty, Convert.ToString(ViewState(Me.ClientID & "SelectedValues"))), String)
        End Get
        Set(ByVal value As String)
            ViewState(Me.ClientID & "SelectedValues") = value
            ViewState.SetItemDirty(Me.ClientID & "SelectedValues", True)
        End Set
    End Property
#End Region

    Private Sub WriteFilter(ByVal writer As HtmlTextWriter)
        Dim style As String = ""
        Dim className As String = ""
        'adding lable
        style = Me.GetCssStyle(_availableLabelStyle, className)
        writer.AddAttribute(HtmlTextWriterAttribute.Id, (Me.ClientID + ("_" + "FilterLabel")), False)
        If Not String.IsNullOrEmpty(style) Then
            writer.AddAttribute(HtmlTextWriterAttribute.Style, style)
        End If
        If (className.Length > 0) Then
            writer.AddAttribute(HtmlTextWriterAttribute.Class, className, False)
        End If
        writer.RenderBeginTag(HtmlTextWriterTag.Label)
        writer.WriteEncodedText(Me.FilterLabelText)
        writer.RenderEndTag()

        'adding text box
        writer.AddAttribute(HtmlTextWriterAttribute.Type, "Input", False)
        writer.AddAttribute(HtmlTextWriterAttribute.Id, (Me.ClientID + ("_" + "Filter")), False)
        writer.AddAttribute("Onkeyup", "filterList(this.value," & Me.ClientID & "_" & _leftListName & ")", False)
        style = Me.GetCssStyle(_availableLabelStyle, className)
        If Not String.IsNullOrEmpty(style) Then
            writer.AddAttribute(HtmlTextWriterAttribute.Style, style)
        End If
        If Not String.IsNullOrEmpty(className) Then
            writer.AddAttribute(HtmlTextWriterAttribute.Class, className)
        End If
        writer.RenderBeginTag(HtmlTextWriterTag.Input)
        writer.RenderEndTag()
        Me.WriteBreak(writer)
    End Sub
End Class


