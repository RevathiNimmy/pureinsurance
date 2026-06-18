Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Text
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls

''' <summary>
''' 
''' </summary>
''' <remarks></remarks>
<DefaultProperty("Text"), ToolboxData("<{0}:LookupList runat=server></{0}:LookupList>"), ValidationProperty("Value")> _
Public Class LookupList : Inherits WebControl : Implements IPostBackEventHandler

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Public Event SelectedIndexChange(ByVal sender As Object, ByVal e As System.EventArgs)

    Public Sub New()

        ViewState("DataItemText") = DataItemTypes.Description
        ViewState("DataItemValue") = DataItemTypes.Code

    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <exception cref="ArgumentException">Parent Lookup List, DefaultItemValue must be set to Key</exception>
    ''' <exception cref="ArgumentException">Unable to find Parent Lookup List, parent control must be within the same container as the child control</exception>
    ''' <remarks></remarks>
    Private Sub LookupList_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        If String.IsNullOrEmpty(ListType) Then
            Throw New ArgumentNullException("ListType")
        End If

        If String.IsNullOrEmpty(ListCode) Then
            Throw New ArgumentNullException("ListCode")
        End If

        EnableViewState = True

        If Not String.IsNullOrEmpty(ParentLookupListID) Then

            Dim oParentLookupList As LookupList = Parent.FindControl(ParentLookupListID)

            If oParentLookupList IsNot Nothing Then
                If oParentLookupList.DataItemValue = DataItemTypes.Key Or oParentLookupList.DataItemValue = DataItemTypes.Code Then
                    'set the parent lookup list to auto postback
                    oParentLookupList.AutoPostBack = True
                    ' AddHandler oParentLookupList.SelectedIndexChange, AddressOf ParentIndexChangeHandler
                Else
                    Throw New ArgumentException("Parent Lookup List, DefaultItemValue must be set to Key", "ParentLookupListID")
                End If
            Else
                Throw New ArgumentException("Unable to find Parent Lookup List, parent control must be " _
                    & "within the same container as the child control", "ParentLookupListID")
            End If

        End If

    End Sub

    Private Sub LookupList_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not String.IsNullOrEmpty(Page.Request.Form(ClientID)) Then
            ViewState("Value") = Page.Request.Form(ClientID)
        ElseIf Page.Request.Form(ClientID) IsNot Nothing Then
            If Page.Request.Form(ClientID).Trim.Length = 0 Then
                ViewState("Value") = Page.Request.Form(ClientID)
            End If
        End If
    End Sub

    Public Sub RaisePostBackEvent(ByVal eventArgument As String) Implements System.Web.UI.IPostBackEventHandler.RaisePostBackEvent

        Select Case eventArgument
            Case "onchange"
                RaiseEvent SelectedIndexChange(Me, New EventArgs())
        End Select

    End Sub

    ''' <summary>
    ''' Boolean to determine whether changes in the selection causes a postback or not
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property CausesValidation() As Boolean
        Get
            Return CBool(ViewState("CausesValidation"))
        End Get
        Set(ByVal Value As Boolean)
            ViewState("CausesValidation") = Value
        End Set
    End Property

    Public Property AutoPostBack() As Boolean
        Get
            Return CBool(ViewState("AutoPostBack"))
        End Get
        Set(ByVal Value As Boolean)
            ViewState("AutoPostBack") = Value
        End Set
    End Property

    ''' <summary>
    ''' Retrieve the text of the currently selected item in the list
    ''' </summary>
    ''' <value></value>
    ''' <returns>string returned is determined by DataItemText value, this can be either the item code, key or description</returns>
    ''' <remarks></remarks>
    <Bindable(True), Category("Appearance"), DefaultValue(""), Localizable(True)> ReadOnly Property Text() As String
        Get
            Dim oWebService As ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oList As LookupListCollection = oWebService.GetList(ListType, ListCode, True, False, Nothing, 0, , , EffectiveDate)
            Dim oItem As LookupListItem = Nothing

            Select Case DataItemValue
                Case DataItemTypes.Code
                    oItem = oList.Item(Value)
                Case DataItemTypes.Description
                    oItem = oList.FindItemByDescription(Value)
                Case DataItemTypes.Key
                    If Not String.IsNullOrEmpty(Value) Then
                        oItem = oList.FindItemByKey(CInt(Value))
                    End If
            End Select

            If oItem IsNot Nothing Then

                Select Case DataItemText
                    Case DataItemTypes.Code
                        Return oItem.Code
                    Case DataItemTypes.Description
                        Return oItem.Description
                    Case DataItemTypes.Key
                        Return CStr(oItem.Key)
                End Select

            End If

            Return String.Empty

        End Get
    End Property

    ''' <summary>
    ''' Retrieves or sets the currently selected list item, if the currently selected item is
    ''' changed then the SelectedIndexChange event is raised as this event is handled by any other
    ''' LookupList controls that link to this control as their parent.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Bindable(True), Category("Appearance"), DefaultValue("")> Property Value() As String
        Get
            Dim sValue As String = CStr(ViewState("Value"))
            Return sValue
        End Get
        Set(ByVal value As String)
            ViewState("Value") = value
            RaiseEvent SelectedIndexChange(Me, New EventArgs)
        End Set
    End Property

    ''' <summary>
    ''' Determines the text displayed to the user in the dropdown list, can either, be the code,
    ''' key or description from the SAM list items
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Category("Data"), DefaultValue(DataItemTypes.Description)> Property DataItemText() As DataItemTypes
        Get
            Return ViewState("DataItemText")
        End Get
        Set(ByVal value As DataItemTypes)
            ViewState("DataItemText") = value
        End Set
    End Property

    ''' <summary>
    ''' Determines the value used for the item values in the dropdown list, can either be code, key or description
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Category("Data"), DefaultValue(DataItemTypes.Code)> Property DataItemValue() As DataItemTypes
        Get
            Return ViewState("DataItemValue")
        End Get
        Set(ByVal value As DataItemTypes)
            ViewState("DataItemValue") = value
        End Set
    End Property

    ''' <summary>
    ''' Optional attribute, if specfied will add an item to the top of the list,
    ''' usually used as a prompt to the user e.g 'Please Select'
    ''' </summary>
    ''' <value>Text string to display as the first item as the list</value>
    ''' <returns></returns>
    ''' <remarks>The new item has no value, only a text description</remarks>
    <Category("Data")> Property DefaultText() As String
        Get
            Return ViewState("DefaultText")
        End Get
        Set(ByVal value As String)
            ViewState("DefaultText") = value
        End Set
    End Property

    ''' <summary>
    ''' Determines the type of list to be retrieve from BackOffice via a SAM call
    ''' </summary>
    ''' <value>Three different types of list can be retrieved, GIS, PMLookup or UserDefined</value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Category("Data")> Property ListType() As ListType
        Get
            If ViewState("ListType") Is Nothing Then
                Return Nothing
            Else
                Return CType(ViewState("ListType"), ListType)
            End If
        End Get
        Set(ByVal value As ListType)
            ViewState("ListType") = value
        End Set
    End Property

    ''' <summary>
    ''' The identifier of the list within BackOffice
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Category("Data")> Property ListCode() As String
        Get
            Dim s As String = CStr(ViewState("ListCode"))
            If s Is Nothing Then
                Return String.Empty
            Else
                Return s
            End If
        End Get
        Set(ByVal value As String)
            ViewState("ListCode") = value
        End Set
    End Property


    <Category("Data")> Property ParentFieldName() As String
        Get
            Dim sParentFieldName As String = CStr(ViewState("ParentFieldName"))
            If String.IsNullOrEmpty(sParentFieldName) Then
                Return String.Empty
            Else
                Return sParentFieldName
            End If
        End Get
        Set(ByVal value As String)
            ViewState("ParentFieldName") = value
        End Set
    End Property

    ''' <summary>
    ''' ParentFieldValue Property
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Category("Data")> Property ParentFieldValue() As Integer
        Get
            Dim nParentFieldValue As Integer = 0
            If IsNumeric(ViewState("ParentFieldValue")) Then
                nParentFieldValue = CInt(ViewState("ParentFieldValue"))
            End If

            Return nParentFieldValue

        End Get
        Set(ByVal value As Integer)
            ViewState("ParentFieldValue") = value
        End Set
    End Property

    ''' <summary>
    ''' If LookupList is part of a filtered set of lists then the link between parent and child
    ''' need to be defined.
    ''' </summary>
    ''' <value>The control ID of the parent LookupList</value>
    ''' <returns></returns>
    ''' <remarks>ParentLookupList control needs to be within the same container as the child control
    ''' and parent DataItemValue attribute must be set to key</remarks>
    <Category("Data")> Property ParentLookupListID() As String
        Get
            Dim s As String = CStr(ViewState("ParentLookupListID"))
            If String.IsNullOrEmpty(s) Then
                Return String.Empty
            Else
                Return s
            End If
        End Get
        Set(ByVal value As String)
            ViewState("ParentLookupListID") = value
        End Set
    End Property

    ''' <summary>
    ''' ParentKey is the filter value for the current list, if the ParentKey is
    ''' changed the current selection is also cleared
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Category("Data")> ReadOnly Property ParentKey() As Integer
        Get
            Dim iValue As Integer = 0
            If Parent.FindControl(ParentLookupListID) IsNot Nothing Then
                Dim sValue As String = CType(Parent.FindControl(ParentLookupListID), LookupList).Value
                If Not Integer.TryParse(sValue, iValue) Then
                    iValue = 0
                End If
            End If
            Return iValue
        End Get
    End Property

    ''' <summary>
    ''' Sort direction of list items
    ''' </summary>
    ''' <value>Asc or Desc are the only valid values</value>
    ''' <returns></returns>
    ''' <remarks>If nothing is specified the list remains in the order returned from SAM</remarks>
    <Category("Data")> Property Sort() As Direction
        Get
            Return CType(ViewState("Sort"), Direction)
        End Get
        Set(ByVal value As Direction)
            ViewState("Sort") = value
        End Set
    End Property

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Category("Data")> ReadOnly Property Items() As LookupListCollection
        Get

            Dim oWebService As ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oList As LookupListCollection = oWebService.GetList(ListType, ListCode, True, False, Nothing, 0, , , EffectiveDate)

            oList.Sort(DataItemText, Sort)

            Return oList

        End Get
    End Property
    ''' <summary>
    ''' If EffectiveDate is part of a filtered set of lists then the link between parent and child
    ''' need to be defined.
    ''' </summary>
    ''' <value>The Effective Date LookupList</value>
    ''' <returns></returns>
    ''' <remarks>ParentLookupList control needs to be within the same container as the child control
    ''' and parent DataItemValue attribute must be set to key</remarks>
    <Category("Data")> Property EffectiveDate() As Date
        Get
            Return CType(ViewState("EffectiveDate"), Date)
        End Get
        Set(ByVal value As Date)
            ViewState("EffectiveDate") = value
        End Set
    End Property


    Private Sub LookupList_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender

        If CBool(ViewState("AutoPostBack")) Then
            If CausesValidation Then
                'register for event validation so that auto postback causes validation to fire
                Attributes.Add("onchange", Page.ClientScript.GetPostBackEventReference(Me, "onchange", True))
            Else
                'don't register for event validation so that auto postback doesn't trigger page validation
                Attributes.Add("onchange", Page.ClientScript.GetPostBackEventReference(Me, "onchange", False))
            End If

        End If

        DataBind()

    End Sub

    ''' <summary>
    ''' Override the TagName, otherwise .NET will render a span tag
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Protected Overrides ReadOnly Property TagName() As String
        Get
            Return "select"
        End Get
    End Property

    ''' <summary>
    ''' Retrieves the currently selected list item code
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Bindable(True), Category("Appearance"), DefaultValue("")> ReadOnly Property Code() As String
        Get
            Dim nNumericKey As Integer
            If (Int32.TryParse(ViewState("Value"), nNumericKey)) Then
                Return Me.Items.FindItemByKey(CInt(ViewState("Value"))).Code
            End If
        End Get
    End Property
    ''' <summary>
    ''' Add additional attributes to the control before render
    ''' </summary>
    ''' <param name="writer"></param>
    ''' <remarks></remarks>
    Public Overrides Sub RenderBeginTag(ByVal writer As System.Web.UI.HtmlTextWriter)

        writer.AddAttribute("name", ClientID)

        'Disable the List if its associated with a parent list but the parent key has not been set yet
        If Not String.IsNullOrEmpty(ParentLookupListID) And ParentKey = 0 Then
            Me.Enabled = False
        End If

        AddAttributesToRender(writer)
        writer.RenderBeginTag(HtmlTextWriterTag.Select)

    End Sub

    ''' <summary>
    ''' Render the list items as a dropdown list
    ''' </summary>
    ''' <param name="writer"></param>
    ''' <remarks></remarks>
    Protected Overrides Sub RenderContents(ByVal writer As HtmlTextWriter)
        Dim CNMode As String = "MODE"
        Dim oWebService As ProviderBase = New NexusProvider.ProviderManager().Provider

        Dim oList As LookupListCollection
        Dim sSubLookDataCache As String = "LookupSubListData"
        If ParentKey = 0 Then
            oList = oWebService.GetList(ListType, ListCode, True, False, ParentFieldName, ParentKey, , , EffectiveDate)
        Else
            sSubLookDataCache = sSubLookDataCache + Convert.ToString(ListCode) + ParentKey.ToString()
            If HttpContext.Current.Cache(sSubLookDataCache) Is Nothing Then
                oList = oWebService.GetList(ListType, ListCode, True, False, ParentFieldName, ParentKey, , , EffectiveDate)
                HttpContext.Current.Cache.Insert(sSubLookDataCache, oList, Nothing, Now.AddHours(4), TimeSpan.Zero)
            Else
                'Use cache data
                oList = CType(HttpContext.Current.Cache(sSubLookDataCache), LookupListCollection)
            End If
        End If
        If (ListCode = "employeeband") Then
            oList.Sort(DataItemValue, Sort)
        Else
            oList.Sort(DataItemText, Sort)
        End If


        If oList IsNot Nothing Then

            'Add Default Text if available
            If Not String.IsNullOrEmpty(DefaultText) Then

                writer.Write(vbTab)
                writer.WriteBeginTag("option")
                writer.WriteAttribute("value", "")
                If Value = DefaultText Then
                    writer.WriteAttribute("selected", "selected")
                End If
                writer.Write(">")
                writer.Write(DefaultText)
                writer.WriteEndTag("option")

            End If


            '  For Each oItem As LookupListItem In oList
            Dim oItem As LookupListItem
            For i As Integer = 0 To oList.Count - 1
                oItem = oList.Item(i)

                'Don't render deleted items
                Select Case HttpContext.Current.Session(CNMode)
                    Case Modes.View, Modes.ViewClaim
                        writer.Write(writer.NewLine)
                        writer.Write(vbTab & vbTab)
                        writer.WriteBeginTag("option")

                        Select Case DataItemValue
                            Case DataItemTypes.Code
                                If (ListType = ListType.UserDefined) Then
                                    writer.WriteAttribute("value", oItem.Key)

                                    writer.WriteAttribute("code", oItem.Code)
                                    If Value = CStr(oItem.Key) Then
                                        writer.WriteAttribute("selected", "selected")
                                    ElseIf Value = CStr(oItem.Code) Then
                                        writer.WriteAttribute("selected", "selected")
                                    End If
                                Else
                                    writer.WriteAttribute("value", oItem.Code)
                                    If Value = CStr(oItem.Code) Then
                                        writer.WriteAttribute("selected", "selected")
                                    End If
                                End If
                            Case DataItemTypes.Description
                                writer.WriteAttribute("value", oItem.Description)
                                If Value = oItem.Description Then
                                    writer.WriteAttribute("selected", "selected")
                                End If
                            Case DataItemTypes.Key
                                writer.WriteAttribute("value", oItem.Key)
                                If Value = CStr(oItem.Key) Then
                                    writer.WriteAttribute("selected", "selected")
                                End If
                        End Select

                        writer.Write(">")

                        Select Case DataItemText
                            Case DataItemTypes.Code
                                writer.Write(oItem.Code)
                            Case DataItemTypes.Description
                                writer.Write(oItem.Description)
                            Case DataItemTypes.Key
                                writer.Write(oItem.Key)
                        End Select

                        writer.WriteEndTag("option")
                    Case Else
                        If Not oItem.IsDeleted Then

                            writer.Write(writer.NewLine)
                            writer.Write(vbTab & vbTab)
                            writer.WriteBeginTag("option")

                            Select Case DataItemValue
                                Case DataItemTypes.Code
                                    If (ListType = ListType.UserDefined) Then
                                        writer.WriteAttribute("value", oItem.Key)

                                        writer.WriteAttribute("code", oItem.Code)
                                        If Value = CStr(oItem.Key) Then
                                            writer.WriteAttribute("selected", "selected")
                                        ElseIf Value = CStr(oItem.Code) Then
                                            writer.WriteAttribute("selected", "selected")
                                        End If
                                    Else
                                        writer.WriteAttribute("value", oItem.Code)
                                        If Value = CStr(oItem.Code) Then
                                            writer.WriteAttribute("selected", "selected")
                                        End If
                                    End If
                                Case DataItemTypes.Description
                                    writer.WriteAttribute("value", oItem.Description)
                                    If Value = oItem.Description Then
                                        writer.WriteAttribute("selected", "selected")
                                    End If
                                Case DataItemTypes.Key
                                    writer.WriteAttribute("value", oItem.Key)
                                    If Value = CStr(oItem.Key) Then
                                        writer.WriteAttribute("selected", "selected")
                                    End If
                            End Select

                            writer.Write(">")

                            Select Case DataItemText
                                Case DataItemTypes.Code
                                    writer.Write(oItem.Code)
                                Case DataItemTypes.Description
                                    writer.Write(oItem.Description)
                                Case DataItemTypes.Key
                                    writer.Write(oItem.Key)
                            End Select

                            writer.WriteEndTag("option")

                        End If
                End Select
            Next
        End If
    End Sub
End Class

''' <summary>
''' Type of List within BackOffice
''' </summary>
Public Enum ListType

    ''' <summary>
    ''' GIS
    ''' </summary>
    GIS = 1

    ''' <summary>
    ''' PM Lookup
    ''' </summary>
    PMLookup = 2

    ''' <summary>
    ''' User Defined
    ''' </summary>
    UserDefined = 3

    ''' <summary>
    ''' PM System Lookup
    ''' </summary>
    PMSystemLookup = 4


End Enum

''' <summary>
''' Type of data within the list item
''' </summary>
Public Enum DataItemTypes

    ''' <summary>
    ''' Item Key
    ''' </summary>
    Key = 0

    ''' <summary>
    ''' Item Code
    ''' </summary>
    Code = 1

    ''' <summary>
    ''' Item Description
    ''' </summary>
    Description = 2

End Enum
Public Enum Modes
    Add = 0 'New policy
    Edit = 1 'Edit an existing policy
    View = 2 'View an existing policy
    Save = 5 'Save a Quick Quote
    Buy = 6 'Buy a Quick Quote ( .. carry onto Full Quote)
    'Added for Claims
    NewClaim = 7
    EditClaim = 8
    ViewClaim = 9
    PayClaim = 10
    SalvageClaim = 11
    TPRecovery = 12
    Review = 13
    Authorise = 14
    Recommend = 15
    DeclinePayment = 16
    ViewClaimPayment = 17
End Enum
