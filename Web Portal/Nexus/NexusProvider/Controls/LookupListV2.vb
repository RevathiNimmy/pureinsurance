Imports System
Imports System.ComponentModel
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.Configuration.WebConfigurationManager
Imports System.Web.Configuration

''' <summary>
''' LookupListV2 is enhanced  control to LookupList
''' </summary>
''' <remarks></remarks>
<DefaultProperty("Text"), ToolboxData("<{0}:LookupListV2 runat=server></{0}:LookupListV2>"), ValidationProperty("Value")>
Public Class LookupListV2 : Inherits WebControl : Implements IPostBackEventHandler

#Region "Private Types"

#End Region

#Region "Private Constants"

#End Region

#Region "Private Variables"
    Private Property localList() As LookupListCollection
        Get
            Return ViewState("sspList")
        End Get
        Set(ByVal Value As LookupListCollection)
            ViewState("sspList") = Value
        End Set
    End Property
#End Region

#Region "Constructors"

    Public Sub New()

        ViewState("LookupListV2DataItemText") = DataItemTypes.Description
        ViewState("LookupListV2DataItemValue") = DataItemTypes.Code

    End Sub

#End Region

#Region "Public Properties"

    ''' <summary>
    ''' Boolean to determine whether changes in the selection causes a postback or not
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property CausesValidation() As Boolean
        Get
            Return CBool(ViewState("LookupListV2CausesValidation"))
        End Get
        Set(ByVal Value As Boolean)
            ViewState("LookupListV2CausesValidation") = Value
        End Set
    End Property

    ''' <summary>
    ''' AutoPostBack Property
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property AutoPostBack() As Boolean
        Get
            Return CBool(ViewState("LookupListV2AutoPostBack"))
        End Get
        Set(ByVal Value As Boolean)
            ViewState("LookupListV2AutoPostBack") = Value
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
            Dim oWebService As ProviderBase = New ProviderManager().Provider
            Dim oList As LookupListCollection
            Dim oItem As LookupListItem = Nothing

            If (ListType = ListType.PMSystemLookup) Then
                oList = oWebService.GetList(ListType.PMLookup, ListCode, True, False, Nothing, 0, , , EffectiveDate)
            Else
                oList = oWebService.GetList(ListType, ListCode, True, False, Nothing, 0, , , EffectiveDate)
            End If

            Select Case DataItemValue
                Case DataItemTypes.Code
                    If Not String.IsNullOrEmpty(Value) Then
                        oItem = oList.FindItemByKey(CInt(Value))
                    End If
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
    ''' LookupListV2 controls that link to this control as their parent.
    ''' </summary>  
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Bindable(True), Category("Appearance"), DefaultValue("")> Property Value() As String
        Get
            Dim sValue As String = CStr(ViewState("LookupListV2Value"))
            Return sValue
        End Get
        Set(ByVal value As String)
            ViewState("LookupListV2Value") = value
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
            Return ViewState("LookupListV2DataItemText")
        End Get
        Set(ByVal value As DataItemTypes)
            ViewState("LookupListV2DataItemText") = value
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
            Return ViewState("LookupListV2DataItemValue")
        End Get
        Set(ByVal value As DataItemTypes)
            ViewState("LookupListV2DataItemValue") = value
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
            Return ViewState("LookupListV2DefaultText")
        End Get
        Set(ByVal value As String)
            ViewState("LookupListV2DefaultText") = value
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
            If ViewState("LookupListV2ListType") Is Nothing Then
                Return Nothing
            Else
                Return CType(ViewState("LookupListV2ListType"), ListType)
            End If
        End Get
        Set(ByVal value As ListType)
            ViewState("LookupListV2ListType") = value
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
            Dim sListCode As String = CStr(ViewState("LookupListV2ListCode"))
            If sListCode Is Nothing Then
                Return String.Empty
            Else
                Return sListCode
            End If
        End Get
        Set(ByVal value As String)
            ViewState("LookupListV2ListCode") = value
        End Set
    End Property

    ''' <summary>
    ''' ParentFieldName Property
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Category("Data")> Property ParentFieldName() As String
        Get
            Dim sParentFieldName As String = CStr(ViewState("LookupListV2ParentFieldName"))
            If String.IsNullOrEmpty(sParentFieldName) Then
                Return String.Empty
            Else
                Return sParentFieldName
            End If
        End Get
        Set(ByVal value As String)
            ViewState("LookupListV2ParentFieldName") = value
        End Set
    End Property

    ''' <summary>
    ''' If LookupListV2 is part of a filtered set of lists then the link between parent and child
    ''' need to be defined.
    ''' </summary>
    ''' <value>The control ID of the parent LookupListV2</value>
    ''' <returns></returns>
    ''' <remarks>ParentLookupList control needs to be within the same container as the child control
    ''' and parent DataItemValue attribute must be set to key</remarks>
    <Category("Data")> Property ParentLookupListV2ID() As String
        Get
            Dim s As String = CStr(ViewState("ParentLookupListV2ID"))
            If String.IsNullOrEmpty(s) Then
                Return String.Empty
            Else
                Return s
            End If
        End Get
        Set(ByVal value As String)
            ViewState("ParentLookupListV2ID") = value
        End Set
    End Property
    <Category("Data")> Property ParentLookupListID() As String
        Get
            Dim s As String = CStr(ViewState("ParentLookupListV2ID"))
            If String.IsNullOrEmpty(s) Then
                Return String.Empty
            Else
                Return s
            End If
        End Get
        Set(ByVal value As String)
            ViewState("ParentLookupListV2ID") = value
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
            If Parent.FindControl(ParentLookupListV2ID) IsNot Nothing Then
                Dim sValue As String = CType(Parent.FindControl(ParentLookupListV2ID), LookupListV2).Value
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
            Return CType(ViewState("LookupListV2Sort"), Direction)
        End Get
        Set(ByVal value As Direction)
            ViewState("LookupListV2Sort") = value
        End Set
    End Property

    ''' <summary>
    ''' Items Property
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Category("Data")> ReadOnly Property Items() As LookupListCollection
        Get

            Dim oLocalList As Boolean = False
            If WebConfigurationManager.AppSettings("UseLocalList") IsNot Nothing AndAlso WebConfigurationManager.AppSettings("UseLocalList").ToLower() = "true" Then
                oLocalList = True
            End If
            Dim oWebService As ProviderBase = New ProviderManager().Provider
            Dim oList As LookupListCollection

            If localList Is Nothing Or oLocalList = False Then
                If (ListType = ListType.PMSystemLookup) Then
                    oList = oWebService.GetList(ListType.PMLookup, ListCode, True, False, Nothing, 0, , , EffectiveDate)
                Else
                    oList = oWebService.GetList(ListType, ListCode, True, False, Nothing, 0, , , EffectiveDate)
                End If
                If oLocalList Then
                    localList = New LookupListCollection()
                    For iCount As Integer = 0 To oList.Count - 1
                        Dim source As LookupListItem = oList(iCount)
                        localList.Add(New LookupListItem(
                            v_iKey:=source.Key,
                            v_iParentKey:=source.ParentKey,
                            v_sCode:=source.Code,
                            v_sDescription:=source.Description,
                            v_dtEffectiveDate:=source.EffectiveDate,
                            v_bIsDeleted:=source.IsDeleted,
                            v_bIsDefault:=False))
                    Next
                End If
            End If
            If oLocalList Then
                localList.Sort(DataItemText, Sort)
                Return localList
            Else
                oList.Sort(DataItemText, Sort)
                Return oList
            End If

        End Get
    End Property

    ''' <summary>
    ''' If EffectiveDate is part of a filtered set of lists then the link between parent and child
    ''' need to be defined.
    ''' </summary>
    ''' <value>The Effective Date LookupListV2</value>
    ''' <returns></returns>
    ''' <remarks>ParentLookupList control needs to be within the same container as the child control
    ''' and parent DataItemValue attribute must be set to key</remarks>
    <Category("Data")> Property EffectiveDate() As Date
        Get
            Return CType(ViewState("LookupListV2EffectiveDate"), Date)
        End Get
        Set(ByVal value As Date)
            ViewState("LookupListV2EffectiveDate") = value
        End Set
    End Property

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
            If (Int32.TryParse(ViewState("LookupListV2Value"), nNumericKey)) Then
                Return Items.FindItemByKey(CInt(ViewState("LookupListV2Value"))).Code
            Else
                Return String.Empty
            End If
        End Get
    End Property

#End Region

#Region "Public Methods"

    ''' <summary>
    ''' Add additional attributes to the control before render
    ''' </summary>
    ''' <param name="writer"></param>
    ''' <remarks></remarks>
    Public Overrides Sub RenderBeginTag(ByVal writer As HtmlTextWriter)

        writer.AddAttribute("name", ClientID)

        'Disable the List if its associated with a parent list but the parent key has not been set yet
        If Not String.IsNullOrEmpty(ParentLookupListV2ID) And ParentKey = 0 Then
            Enabled = False
        End If

        AddAttributesToRender(writer)
        writer.RenderBeginTag(HtmlTextWriterTag.Select)

    End Sub

#End Region

#Region "Protected Methods"

    ''' <summary>
    ''' Render the list items as a dropdown list
    ''' </summary>
    ''' <param name="writer"></param>
    ''' <remarks></remarks>
    Protected Overrides Sub RenderContents(ByVal writer As HtmlTextWriter)
        Dim oLocalList As Boolean = False
        If WebConfigurationManager.AppSettings("UseLocalList") IsNot Nothing AndAlso WebConfigurationManager.AppSettings("UseLocalList").ToLower() = "true" Then
            oLocalList = True
        End If

        Dim oWebService As ProviderBase = New ProviderManager().Provider
        Dim oList As LookupListCollection
        If localList Is Nothing Or oLocalList = False Then
            If (ListType = ListType.PMSystemLookup) Then
                oList = oWebService.GetList(ListType.PMLookup, ListCode, True, False, ParentFieldName, ParentKey, , , EffectiveDate)
            Else
                oList = oWebService.GetList(ListType, ListCode, True, False, ParentFieldName, ParentKey, , , EffectiveDate)
            End If
            If oLocalList Then
                localList = New LookupListCollection()

                For iCount As Integer = 0 To oList.Count - 1
                    Dim source As LookupListItem = oList(iCount)
                    localList.Add(New LookupListItem(
                        v_iKey:=source.Key,
                        v_iParentKey:=source.ParentKey,
                        v_sCode:=source.Code,
                        v_sDescription:=source.Description,
                        v_dtEffectiveDate:=source.EffectiveDate,
                        v_bIsDeleted:=source.IsDeleted,
                        v_bIsDefault:=False))
                Next
            End If
        End If
        Dim oTmpList As LookupListCollection
        If oLocalList Then
            localList.Sort(DataItemText, Sort)
            oTmpList = localList
        Else
            oList.Sort(DataItemText, Sort)
            oTmpList = oList
        End If

        If oTmpList IsNot Nothing Then
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

            If oList IsNot Nothing Then
                For Each oItem As LookupListItem In oList

                    'Don't render deleted items
                    If Not oItem.IsDeleted Then

                        writer.Write(writer.NewLine)
                        writer.Write(vbTab & vbTab)
                        writer.WriteBeginTag("option")

                        Select Case DataItemValue
                            Case DataItemTypes.Code
                                If (ListType = ListType.UserDefined Or ListType = ListType.PMLookup) Then
                                    writer.WriteAttribute("value", oItem.Key)

                                    writer.WriteAttribute("code", oItem.Code)
                                    If Value = CStr(oItem.Key) Then
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
                Next
            End If
        End If
    End Sub

#End Region

#Region "Event Handlers"

    ''' <summary>
    ''' Event SelectedIndexChange
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Public Event SelectedIndexChange(ByVal sender As Object, ByVal e As EventArgs)


    ''' <summary>
    ''' Init event of LookupListV2 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <exception cref="ArgumentException">Parent Lookup List, DefaultItemValue must be set to Key</exception>
    ''' <exception cref="ArgumentException">Unable to find Parent Lookup List, parent control must be within the same container as the child control</exception>
    ''' <remarks></remarks>
    ''' 
    Private Sub LookupListV2_Init(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Init

        If String.IsNullOrEmpty(ListType) Then
            Throw New ArgumentNullException("ListType")
        End If

        If String.IsNullOrEmpty(ListCode) Then
            Throw New ArgumentNullException("ListCode")
        End If

        EnableViewState = True

        If Not String.IsNullOrEmpty(ParentLookupListV2ID) Then

            Dim oParentLookupListV2 As LookupListV2 = Parent.FindControl(ParentLookupListV2ID)

            If oParentLookupListV2 IsNot Nothing Then
                If oParentLookupListV2.DataItemValue = DataItemTypes.Key Or oParentLookupListV2.DataItemValue = DataItemTypes.Code Then
                    'set the parent lookup list to auto postback
                    oParentLookupListV2.AutoPostBack = True
                    ' AddHandler oParentLookupList.SelectedIndexChange, AddressOf ParentIndexChangeHandler
                Else
                    Throw New ArgumentException("Parent Improved Lookup List, DefaultItemValue must be set to Key", "ParentLookupListID")
                End If
            Else
                Throw New ArgumentException("Unable to find Parent Improved Lookup List, parent control must be " _
                    & "within the same container as the child control", "ParentLookupListV2ID")
            End If

        End If

    End Sub

    ''' <summary>
    ''' Load event of LookupListV2 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub LookupListV2_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load

        If Not String.IsNullOrEmpty(Page.Request.Form(ClientID)) Then
            ViewState("LookupListV2Value") = Page.Request.Form(ClientID)
        ElseIf Page.Request.Form(ClientID) IsNot Nothing Then
            If Page.Request.Form(ClientID).Trim.Length = 0 Then
                ViewState("LookupListV2Value") = Page.Request.Form(ClientID)
            End If
        End If
    End Sub

    ''' <summary>
    ''' PreRender event of LookupListV2 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub LookupList_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles Me.PreRender

        If CBool(ViewState("LookupListV2AutoPostBack")) Then
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
    ''' Raise Event SelectedIndexChange
    ''' </summary>
    ''' <param name="eventArgument"></param>
    ''' <remarks></remarks>
    Public Sub RaisePostBackEvent(ByVal eventArgument As String) Implements IPostBackEventHandler.RaisePostBackEvent

        Select Case eventArgument
            Case "onchange"
                RaiseEvent SelectedIndexChange(Me, New EventArgs())
        End Select

    End Sub

#End Region

End Class



