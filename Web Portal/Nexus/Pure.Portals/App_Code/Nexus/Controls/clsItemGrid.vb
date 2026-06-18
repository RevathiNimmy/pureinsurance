Imports System.Web.HttpContext
Imports System.Configuration.ConfigurationManager
Imports Nexus.Constants
Imports Nexus.Constants.Session
Imports System.Resources
Imports Nexus.Constants.Constant

Namespace Nexus

    ''' <summary>
    ''' Risk control to show list of items from child screen
    ''' </summary>
    ''' <remarks>This is just an overloaded GridView to make use of the existing functionality.</remarks>
    Public Class ItemGrid : Inherits System.Web.UI.WebControls.GridView

        Private bLinkTypeAsImage As Boolean = False

        'Either ChildPage or ChildContainer, NOT both, but ChildContainer will take priority if both exist.
        Private sChildPage As String = String.Empty
        Private sChildContainer As String = String.Empty

        Private sParentElement As String = String.Empty
        Private sChildElement As String = String.Empty

        Private sScreenCode As String = String.Empty

        Private obtnAdd As Object = Nothing

        Protected oEmptyFooterRow As GridViewRow
        Protected oEmptyHeaderRow As GridViewRow

        Dim oResource As ResXResourceReader
        Dim en As IDictionaryEnumerator
        Dim sbtnAddText As String
        Dim sbtnAddToolTipText As String
        Dim sbtnEditText As String
        Dim sbtnDeleteText As String
        Dim sbtnViewText As String
        Dim sbtnSelectText As String

        ''' <summary>
        ''' The event that is raised whenever a item link is clicked in the grid,
        ''' if the itemgrid is NOT using 'inline' child controls
        ''' </summary>
        ''' <param name="v_sPath">Path to the child page, that needs to be redirected to</param>
        ''' <param name="v_sOI">SAM dataset ID of the child item</param>
        ''' <remarks></remarks>
        Public Event EditItem(ByVal v_sPath As String, ByVal v_sOI As String, ByVal v_sScreenCode As String)

        ''' <summary>
        ''' Raised when a item link is clicked in the grid and the itemgrid is using 'inline' child controls
        ''' </summary>
        ''' <param name="v_sContainer">Container control ID of the inline child controls</param>
        ''' <param name="v_sOI">SAM dataset ID of the child item</param>
        ''' <remarks></remarks>
        Public Event EditItemInRiskContainer(ByVal v_sContainer As String, ByVal v_sOI As String)

        ''' <summary>
        ''' Event raised when the add item link is clicked
        ''' </summary>
        ''' <param name="v_sScreenCode"></param>
        ''' <param name="v_sPath">Path to the child page</param>
        ''' <param name="v_sParentElement"></param>
        ''' <param name="v_sChildElement"></param>
        ''' <remarks></remarks>
        Public Event AddItem(ByVal v_sScreenCode As String, ByVal v_sPath As String, ByVal v_sParentElement As String, ByVal v_sChildElement As String)

        ''' <summary>
        ''' Event raised when the delete item link is clicked
        ''' </summary>
        ''' <param name="v_sOI">SAM dataset ID of the child item</param>
        ''' <param name="v_sChildElement"></param>
        ''' <remarks></remarks>
        Public Event DeleteItem(ByVal v_sOI As String, ByVal v_sChildElement As String)

        Private Sub ItemGrid_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
            oResource = New ResXResourceReader(HttpContext.Current.Server.MapPath(AppSettings("WebRoot") & "App_LocalResources/ItemGrid.resx"))
            en = oResource.GetEnumerator()

            While (en.MoveNext)
                If en.Key.ToString.Trim = "btn_AddText" Then
                    sbtnAddText = en.Value
                End If
                If en.Key.ToString.Trim = "btn_AddToolTipText" Then
                    sbtnAddToolTipText = en.Value
                End If
                If en.Key.ToString.Trim = "btn_EditText" Then
                    sbtnEditText = en.Value
                End If
                If en.Key.ToString.Trim = "btn_DeleteText" Then
                    sbtnDeleteText = en.Value
                End If
                If en.Key.ToString.Trim = "btn_ViewText" Then
                    sbtnViewText = en.Value
                End If
                If en.Key.ToString.Trim = "btn_SelectText" Then
                    sbtnSelectText = en.Value
                End If
            End While

            ShowFooter = True
            DataKeyNames = Split("OI") 'Nice, Why cant you initialize an array easily !?
            EnableViewState = False

            Dim sTmp() As String = Regex.Split(ID, "__")
            If sTmp.Length > 1 Then
                sParentElement = sTmp(0) 'Parent Element Name
                sChildElement = sTmp(1) 'Child Element Name
            End If

            Dim cmdfButtons As New CommandField

            'Create Command Links
            Select Case CType(Current.Session.Item(CNMode), Mode)
                Case Mode.Add, Mode.Edit, Mode.Buy, Mode.NewClaim, Mode.EditClaim, Mode.PayClaim

                    If bLinkTypeAsImage Then
                        cmdfButtons.ButtonType = ButtonType.Image
                        cmdfButtons.EditImageUrl = "~/App_Themes/" & Page.Theme & "/images/edit.gif"
                        cmdfButtons.DeleteImageUrl = "~/App_Themes/" & Page.Theme & "/images/delete.gif"
                    Else
                        cmdfButtons.ButtonType = ButtonType.Link
                        cmdfButtons.EditText = sbtnEditText
                        cmdfButtons.DeleteText = sbtnDeleteText
                    End If

                    cmdfButtons.ShowEditButton = True
                    cmdfButtons.ShowDeleteButton = True

                    'Case Mode.View, Mode.ViewClaim, Mode.SalvageClaim, Mode.TPRecovery, Mode.Review

                Case Mode.View, Mode.ViewClaim, Mode.SalvageClaim, Mode.TPRecovery, Mode.Review, Mode.Authorise, Mode.ViewClaimPayment, Mode.Recommend, Mode.DeclinePayment

                    If bLinkTypeAsImage Then
                        cmdfButtons.ButtonType = ButtonType.Image
                        cmdfButtons.SelectImageUrl = "~App_Themes/" & Page.Theme & "/images/view.gif"
                    Else
                        cmdfButtons.ButtonType = ButtonType.Link
                        cmdfButtons.SelectText = sbtnViewText


                    End If

                    cmdfButtons.ShowSelectButton = True

            End Select

            If Current.Request.Url.ToString().ToUpper().Contains("PERSONALCLIENTDETAILS") OrElse Current.Request.Url.ToString().ToUpper().Contains("CORPORATECLIENTDETAILS") Then

                Select Case CType(Current.Session.Item(CNClientMode), Mode)
                    Case Mode.View
                        cmdfButtons.ButtonType = ButtonType.Link
                        cmdfButtons.SelectText = sbtnSelectText
                        cmdfButtons.ShowSelectButton = True
                End Select
            End If

            cmdfButtons.CausesValidation = False
            Columns.Add(cmdfButtons)

        End Sub

        ''' <summary>
        ''' What type of item links should be generated? text or image links
        ''' </summary>
        ''' <value>type of link, either 'image' or 'text'</value>
        ''' <remarks></remarks>
        Public WriteOnly Property LinkType() As String
            Set(ByVal value As String)
                Select Case LCase(value)
                    Case "image"
                        bLinkTypeAsImage = True
                    Case "text"
                        bLinkTypeAsImage = False
                    Case Else
                        bLinkTypeAsImage = False
                End Select
            End Set
        End Property

        ''' <summary>
        ''' The risk child page that the items will be linked to
        ''' </summary>
        ''' <value></value>
        ''' <remarks></remarks>
        Public WriteOnly Property ChildPage() As String
            Set(ByVal value As String)
                sChildPage = value
            End Set
        End Property

        ''' <summary>
        ''' If using an 'inline' child page, as opposed to an independent page, the control
        ''' id of the container containing the child controls must be specified
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks>The ChildContainer property takes priority over the ChildPage property
        ''' if both are specified</remarks>
        Public Property ChildContainer() As String
            Get
                Return sChildContainer
            End Get
            Set(ByVal value As String)
                sChildContainer = value
            End Set
        End Property

        ''' <summary>
        ''' The BackOffice screen of the child page
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ScreenCode() As String
            Get
                Return sScreenCode
            End Get
            Set(ByVal value As String)
                sScreenCode = value
            End Set
        End Property

        Public Overrides ReadOnly Property FooterRow() As System.Web.UI.WebControls.GridViewRow
            Get
                Dim oFooterRow As GridViewRow = MyBase.FooterRow
                If (oFooterRow IsNot Nothing) Then
                    Return oFooterRow
                Else
                    Return oEmptyFooterRow
                End If
            End Get
        End Property

        Public Overrides ReadOnly Property HeaderRow() As System.Web.UI.WebControls.GridViewRow
            Get
                Dim oHeaderRow As GridViewRow = MyBase.HeaderRow
                If (oHeaderRow IsNot Nothing) Then
                    Return oHeaderRow
                Else
                    Return oEmptyHeaderRow
                End If
            End Get
        End Property

        Protected Overrides Function CreateChildControls(ByVal dataSource As System.Collections.IEnumerable, ByVal dataBinding As Boolean) As Integer

            Dim numRows As Integer = MyBase.CreateChildControls(dataSource, dataBinding)

            'no data rows created, create empty table if enabled
            If numRows = 0 Then ' And ShowWhenEmpty Then

                'create table
                Dim table As Table = New Table()
                table.ID = Me.ID
                table.CssClass = "grid-table"





                'convert the exisiting columns into an array and initialize
                Dim fields As DataControlField() = New DataControlField(Me.Columns.Count - 1) {}
                Me.Columns.CopyTo(fields, 0)

                If Me.ShowHeader Then
                    'create a new header row
                    oEmptyHeaderRow = MyBase.CreateRow(-1, -1, DataControlRowType.Header, DataControlRowState.Normal)

                    Me.InitializeRow(oEmptyHeaderRow, fields)
                    table.Rows.Add(oEmptyHeaderRow)
                End If

                'create the empty row
                Dim emptyRow As GridViewRow = New GridViewRow(-1, -1, DataControlRowType.EmptyDataRow, DataControlRowState.Normal)

                Dim cell As TableCell = New TableCell()
                cell.ColumnSpan = Me.Columns.Count
                cell.Width = Unit.Percentage(100)
                If (Not String.IsNullOrEmpty(EmptyDataText)) Then cell.Controls.Add(New LiteralControl(EmptyDataText))

                If Me.EmptyDataTemplate IsNot Nothing Then EmptyDataTemplate.InstantiateIn(cell)

                emptyRow.Cells.Add(cell)
                table.Rows.Add(emptyRow)

                If (Me.ShowFooter) Then
                    'create footer row
                    oEmptyFooterRow = MyBase.CreateRow(-1, -1, DataControlRowType.Footer, DataControlRowState.Normal)

                    Me.InitializeRow(oEmptyFooterRow, fields)

                    'DONT DISPLAY ADD BUTTON IN ITEMGRID IF CONTAINER IS DEFINED - MB-08 JUNE 2007
                    If sChildContainer Is String.Empty Then
                        If CType(Current.Session.Item(CNMode), Mode) <> Mode.View And _
                            CType(Current.Session.Item(CNMode), Mode) <> Mode.ViewClaim And _
                            CType(Current.Session.Item(CNMode), Mode) <> Mode.SalvageClaim And _
                            CType(Current.Session.Item(CNMode), Mode) <> Mode.TPRecovery And _
                            CType(Current.Session.Item(CNMode), Mode) <> Mode.Review Then
                            If bLinkTypeAsImage Then
                                obtnAdd = New ImageButton
                                obtnAdd.ImageUrl = "~/App_Themes/" & Page.Theme & "/images/add.gif"
                                obtnAdd.ToolTip = sbtnAddToolTipText
                                obtnAdd.CausesValidation = False

                            Else
                                obtnAdd = New LinkButton
                                obtnAdd.SkinID = "btnGrid"
                                obtnAdd.Text = sbtnAddText
                                obtnAdd.CausesValidation = False
                            End If

                            obtnAdd.CommandName = "Add"
                            oEmptyFooterRow.Cells(oEmptyFooterRow.Cells.Count - 1).Controls.Add(obtnAdd)

                        End If
                    End If

                    table.Rows.Add(oEmptyFooterRow)
                End If

                Me.Controls.Clear()
                Me.Controls.Add(table)



            End If

            Return numRows

        End Function

        ''' <summary>
        ''' ItemGrid render method
        ''' </summary>
        ''' <param name="writer"></param>
        ''' <remarks>We need to override the render method of the GridView to register the Add Item button</remarks>
        Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
            MyBase.Render(writer)

            Select Case CType(Current.Session.Item(CNMode), Mode)
                Case Mode.Add, Mode.Edit, Mode.NewClaim, Mode.EditClaim, Mode.PayClaim
                    If obtnAdd IsNot Nothing Then
                        Page.ClientScript.RegisterForEventValidation(obtnAdd.UniqueID, Nothing)
                    End If
            End Select

        End Sub

        Protected Overrides Sub DataBind(ByVal raiseOnDataBinding As Boolean)

            MyBase.DataBind(raiseOnDataBinding)

        End Sub

        Private Sub ItemGrid_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles Me.RowCommand

            If e.CommandName <> "Page" Then
                Select Case LCase(e.CommandName)
                    Case "add"
                        Current.Session.Item(CNChildMode) = Nothing
                        RaiseEvent AddItem(sScreenCode, sChildPage, sParentElement, sChildElement)
                    Case "delete"
                        RaiseEvent DeleteItem(Me.DataKeys(e.CommandArgument).Value, sChildElement)
                    Case Else
                        If Not String.IsNullOrEmpty(sChildContainer) Then
                            RaiseEvent EditItemInRiskContainer(sChildContainer, Me.DataKeys(e.CommandArgument).Value)
                        Else
                            RaiseEvent EditItem(sChildPage, Me.DataKeys(e.CommandArgument).Value, sScreenCode)
                        End If
                End Select
            End If
        End Sub

        Private Sub ItemGrid_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles Me.PageIndexChanging
            If Me.AllowPaging = True Then
                Me.PageIndex = e.NewPageIndex
                MyBase.DataBind()
            End If
        End Sub
        Private Sub ItemGrid_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles Me.RowDeleting
            'Have to refresh the page when delete event is fired because,
            'This will avoid Add item issue if ItemGrid is empty.

            If Not HttpContext.Current.Request.QueryString("modal") = "true" Then
                HttpContext.Current.Response.Redirect(HttpContext.Current.Request.RawUrl, False)
            End If


        End Sub

        Private Sub ItemGrid_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles Me.RowEditing
            'If inline child then we need to set editindex=-1 otherwise grid row will be editable and will show text boxes instead of label
            If Not String.IsNullOrEmpty(sChildContainer) Then
                e.NewEditIndex = -1
            End If
        End Sub

        Private Sub ItemGrid_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles Me.RowCreated

            If e.Row.RowType = DataControlRowType.Footer Then
                If CType(Current.Session.Item(CNMode), Mode) <> Mode.View And _
                    CType(Current.Session.Item(CNMode), Mode) <> Mode.ViewClaim And _
                    CType(Current.Session.Item(CNMode), Mode) <> Mode.SalvageClaim And _
                    CType(Current.Session.Item(CNMode), Mode) <> Mode.TPRecovery And _
                    CType(Current.Session.Item(CNMode), Mode) <> Mode.Review Then
                    If sChildContainer Is String.Empty Then
                        If bLinkTypeAsImage Then
                            obtnAdd = New ImageButton
                            obtnAdd.ImageUrl = "~/App_Themes/" & Page.Theme & "/images/add.gif"
                            obtnAdd.ToolTip = sbtnAddToolTipText
                            obtnAdd.CausesValidation = False
                        Else
                            obtnAdd = New LinkButton
                            obtnAdd.SkinID = "btnGrid"
                            obtnAdd.Text = sbtnAddText
                            obtnAdd.CausesValidation = False
                        End If

                        obtnAdd.CommandName = "Add"
                        e.Row.Cells(e.Row.Cells.Count - 1).Controls.Add(obtnAdd)

                    End If

                End If
            End If

        End Sub

        Private Sub ItemGrid_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles Me.RowDataBound
            If e.Row.RowType = DataControlRowType.DataRow Then
                If e.Row.RowState = 0 OrElse e.Row.RowState = 1 Then
                    If Not bLinkTypeAsImage Then
                        Dim oControl As Control
                        Dim oLinkButtons As LinkButton
                        For nCells = 0 To e.Row.Cells.Count - 1
                            For nControls = 0 To e.Row.Cells(nCells).Controls.Count - 1
                                If e.Row.Cells(nCells).Controls.Count > 0 Then
                                    oControl = e.Row.Cells(nCells).Controls(nControls)
                                    If TypeOf oControl Is LinkButton Then
                                        oLinkButtons = CType(oControl, LinkButton)
                                        oLinkButtons.CssClass = "btn btn-xs btn-primary"
                                    End If
                                End If
                            Next
                        Next
                    End If
                End If
            End If
        End Sub
    End Class

    ''' <summary>
    ''' Template field for the ItemGrid which lookups up text/description of attributes
    ''' </summary>
    ''' <remarks>Uses the SAM GetList method to retrieve a more readable result of a dataset attribute
    ''' e.g country code of 'GBR' would be displayed as 'United Kingdom'</remarks>
    Public Class GISLookupField : Inherits DataControlField

        ' 21-11-07 - DH - Rewrote to integrate with the Nexus Provider

        Private oListType As NexusProvider.ListType
        Private sListCode As String
        Private oDataItemValue As NexusProvider.DataItemTypes

        ''' <summary>
        ''' Define the type of list that will be retrived from SAM for matching the data field attribute
        ''' </summary>
        ''' <value>Uses the ListType enumerator, only 3 possible values, GIS, UserDefined and PMLookup</value>
        ''' <remarks></remarks>
        Public WriteOnly Property ListType() As NexusProvider.ListType
            Set(ByVal value As NexusProvider.ListType)
                oListType = value
            End Set
        End Property

        ''' <summary>
        ''' Define the SAM list identifier/code
        ''' </summary>
        ''' <value></value>
        ''' <remarks></remarks>
        Public WriteOnly Property ListCode() As String
            Set(ByVal value As String)
                sListCode = value
            End Set
        End Property

        ''' <summary>
        ''' Defines the field in the list items that the current datafield will be matched against.
        ''' </summary>
        ''' <value>Uses the DataItemTypes enumerator e.g Key, Code or Description</value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Property DataItemValue() As NexusProvider.DataItemTypes
            Get
                Return oDataItemValue
            End Get
            Set(ByVal value As NexusProvider.DataItemTypes)
                oDataItemValue = value
            End Set
        End Property

        ''' <summary>
        ''' The dataset child item attribute to be retrived
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overridable Property DataField() As String
            Get
                Dim o As Object = MyBase.ViewState("DataField")
                If (Not o Is Nothing) Then
                    Return CType(o, String)
                End If
                Return String.Empty
            End Get
            Set(ByVal value As String)
                MyBase.ViewState("DataField") = value
                OnFieldChanged()
            End Set
        End Property

        Protected Overrides Function CreateField() As System.Web.UI.WebControls.DataControlField
            Return New GISLookupField
        End Function

        Public Overrides Sub ExtractValuesFromCell(ByVal dictionary As System.Collections.Specialized.IOrderedDictionary, _
                                                    ByVal cell As System.Web.UI.WebControls.DataControlFieldCell, _
                                                    ByVal rowState As System.Web.UI.WebControls.DataControlRowState, _
                                                    ByVal includeReadOnly As Boolean)

            MyBase.ExtractValuesFromCell(dictionary, cell, rowState, includeReadOnly)
        End Sub

        Public Overrides Sub InitializeCell(ByVal cell As DataControlFieldCell, _
                                            ByVal cellType As DataControlCellType, _
                                            ByVal rowState As DataControlRowState, _
                                            ByVal rowIndex As Integer)

            MyBase.InitializeCell(cell, cellType, rowState, rowIndex)

            If cellType = DataControlCellType.DataCell Then
                InitializeDataCell(cell, rowState)
            End If

        End Sub

        Protected Overridable Sub InitializeDataCell(ByVal cell As DataControlFieldCell, _
                                                        ByVal rowState As DataControlRowState)

            Dim ctrl As Control = Nothing

            If rowState <> DataControlRowState.Edit And rowState <> DataControlRowState.Insert Then
                ctrl = cell
            End If

            If Not ctrl Is Nothing And Visible Then
                AddHandler ctrl.DataBinding, New EventHandler(AddressOf OnBindingField)
            End If

        End Sub

        Protected Overridable Sub OnBindingField(ByVal sender As Object, ByVal e As EventArgs)

            Dim target As Control = CType(sender, Control)

            If TypeOf (target) Is TableCell Then
                'View
                Dim tcTmp As TableCell = CType(target, TableCell)

                If DesignMode Then
                    tcTmp.Text = "SAM Lookup"
                Else
                    Dim diTmp As Object = DataBinder.GetDataItem(target.NamingContainer)

                    If diTmp IsNot Nothing Then

                        Try
                            If DataBinder.GetPropertyValue(diTmp, DataField).ToString() IsNot Nothing Then
                                tcTmp.Text = GetGISValue(DataBinder.GetPropertyValue(diTmp, DataField).ToString())
                            End If
                        Catch ex As Exception
                            tcTmp.Text = String.Empty
                        End Try

                    Else
                        tcTmp.Text = String.Empty
                    End If
                End If
            End If

        End Sub

        ''' <summary>
        ''' Retrives the description of the datafield from the specified list from SAM
        ''' </summary>
        ''' <param name="v_sSelectedValue">Datafield value</param>
        ''' <returns>description</returns>
        ''' <remarks></remarks>
        Private Function GetGISValue(ByVal v_sSelectedValue As String) As String

            Dim sReturnValue As String

            If v_sSelectedValue Is Nothing Or v_sSelectedValue = "0" Or v_sSelectedValue = String.Empty Then
                sReturnValue = String.Empty
            Else

                Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider

                Try
                    Dim oList As NexusProvider.LookupListCollection = oWebService.GetList(oListType, sListCode, True, False)
                    Dim oItem As NexusProvider.LookupListItem = Nothing

                    Select Case oDataItemValue
                        Case NexusProvider.DataItemTypes.Code
                            oItem = oList.Item(v_sSelectedValue)
                        Case NexusProvider.DataItemTypes.Description
                            oItem = oList.FindItemByDescription(v_sSelectedValue)
                        Case NexusProvider.DataItemTypes.Key
                            oItem = oList.FindItemByKey(CInt(v_sSelectedValue))
                    End Select

                    If oItem Is Nothing Then
                        sReturnValue = String.Empty
                    Else
                        sReturnValue = oItem.Description
                    End If

                Finally
                    oWebService = Nothing
                End Try

            End If

            Return sReturnValue

        End Function

    End Class
    Public Class PartyLookupField : Inherits DataControlField

        Private oDataItemValue As NexusProvider.DataItemTypes

        ''' <summary>
        ''' Defines the field in the list items that the current datafield will be matched against.
        ''' </summary>
        ''' <value>Uses the DataItemTypes enumerator e.g Key, Code or Description</value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Property DataItemValue() As NexusProvider.DataItemTypes
            Get
                Return oDataItemValue
            End Get
            Set(ByVal value As NexusProvider.DataItemTypes)
                oDataItemValue = value
            End Set
        End Property

        ''' <summary>
        ''' The dataset child item attribute to be retrived
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overridable Property DataField() As String
            Get
                Dim o As Object = MyBase.ViewState("DataField")
                If (Not o Is Nothing) Then
                    Return CType(o, String)
                End If
                Return String.Empty
            End Get
            Set(ByVal value As String)
                MyBase.ViewState("DataField") = value
                OnFieldChanged()
            End Set
        End Property

        Protected Overrides Function CreateField() As System.Web.UI.WebControls.DataControlField
            Return New PartyLookupField
        End Function

        Public Overrides Sub ExtractValuesFromCell(ByVal dictionary As System.Collections.Specialized.IOrderedDictionary, _
                                                    ByVal cell As System.Web.UI.WebControls.DataControlFieldCell, _
                                                    ByVal rowState As System.Web.UI.WebControls.DataControlRowState, _
                                                    ByVal includeReadOnly As Boolean)

            MyBase.ExtractValuesFromCell(dictionary, cell, rowState, includeReadOnly)
        End Sub

        Public Overrides Sub InitializeCell(ByVal cell As DataControlFieldCell, _
                                            ByVal cellType As DataControlCellType, _
                                            ByVal rowState As DataControlRowState, _
                                            ByVal rowIndex As Integer)

            MyBase.InitializeCell(cell, cellType, rowState, rowIndex)

            If cellType = DataControlCellType.DataCell Then
                InitializeDataCell(cell, rowState)
            End If

        End Sub

        Protected Overridable Sub InitializeDataCell(ByVal cell As DataControlFieldCell, _
                                                        ByVal rowState As DataControlRowState)

            Dim ctrl As Control = Nothing

            If rowState <> DataControlRowState.Edit And rowState <> DataControlRowState.Insert Then
                ctrl = cell
            End If

            If Not ctrl Is Nothing And Visible Then
                AddHandler ctrl.DataBinding, New EventHandler(AddressOf OnBindingField)
            End If

        End Sub

        Protected Overridable Sub OnBindingField(ByVal sender As Object, ByVal e As EventArgs)

            Dim target As Control = CType(sender, Control)

            If TypeOf (target) Is TableCell Then
                'View
                Dim tcTmp As TableCell = CType(target, TableCell)

                If DesignMode Then
                    tcTmp.Text = "SAM Lookup"
                Else
                    Dim diTmp As Object = DataBinder.GetDataItem(target.NamingContainer)

                    If diTmp IsNot Nothing Then

                        Try
                            If DataBinder.GetPropertyValue(diTmp, DataField).ToString() IsNot Nothing Then
                                tcTmp.Text = GetPartyValue(DataBinder.GetPropertyValue(diTmp, DataField).ToString())
                            End If
                        Catch ex As Exception
                            tcTmp.Text = String.Empty
                        End Try

                    Else
                        tcTmp.Text = String.Empty
                    End If
                End If
            End If

        End Sub

        ''' <summary>
        ''' Retrives the description of the datafield from the specified list from SAM
        ''' </summary>
        ''' <param name="v_sSelectedValue">Datafield value</param>
        ''' <returns>description</returns>
        ''' <remarks></remarks>
        Private Function GetPartyValue(ByVal v_sSelectedValue As String) As String

            Dim sReturnValue As String = Nothing

            If v_sSelectedValue Is Nothing Or v_sSelectedValue = "0" Or v_sSelectedValue = String.Empty Then
                sReturnValue = String.Empty
            Else

                Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider

                Try
                    Dim oParty As NexusProvider.BaseParty
                    oParty = oWebService.GetParty(CInt(v_sSelectedValue))

                    Select Case True
                        Case TypeOf oParty Is NexusProvider.CorporateParty
                            With CType(oParty, NexusProvider.CorporateParty)
                                sReturnValue = .CompanyName
                            End With
                        Case TypeOf oParty Is NexusProvider.PersonalParty
                            With CType(oParty, NexusProvider.PersonalParty)
                                sReturnValue = .Title & " " & .Forename & " " & .Lastname
                            End With
                        Case TypeOf oParty Is NexusProvider.OtherParty
                            With CType(oParty, NexusProvider.OtherParty)
                                sReturnValue = .Name.Trim
                            End With
                    End Select

                Finally
                    oWebService = Nothing
                End Try

            End If

            Return sReturnValue

        End Function

    End Class
    Public Class RiskAttribute : Inherits BoundField
        Dim sFilterControl As String
        'Added New property to enable the filer on risk screen
        Public Property FilterByControl() As String
            Get
                Return sFilterControl
            End Get
            Set(ByVal value As String)
                sFilterControl = value
            End Set
        End Property

        Protected Overrides Function CreateField() As System.Web.UI.WebControls.DataControlField
            Return New RiskAttribute
        End Function

        Public Overrides Sub ExtractValuesFromCell(ByVal dictionary As System.Collections.Specialized.IOrderedDictionary, _
                                                    ByVal cell As System.Web.UI.WebControls.DataControlFieldCell, _
                                                    ByVal rowState As System.Web.UI.WebControls.DataControlRowState, _
                                                    ByVal includeReadOnly As Boolean)

            MyBase.ExtractValuesFromCell(dictionary, cell, rowState, includeReadOnly)
        End Sub

        Public Overrides Sub InitializeCell(ByVal cell As DataControlFieldCell, _
                                            ByVal cellType As DataControlCellType, _
                                            ByVal rowState As DataControlRowState, _
                                            ByVal rowIndex As Integer)

            MyBase.InitializeCell(cell, cellType, rowState, rowIndex)

            If cellType = DataControlCellType.DataCell Then
                InitializeDataCell(cell, rowState)
            End If

        End Sub

        Protected Overrides Sub InitializeDataCell(ByVal cell As DataControlFieldCell, _
                                                        ByVal rowState As DataControlRowState)

            Dim ctrl As Control = Nothing

            If rowState <> DataControlRowState.Edit And rowState <> DataControlRowState.Insert Then
                ctrl = cell
            End If

            If Not ctrl Is Nothing And Visible Then
                AddHandler ctrl.DataBinding, New EventHandler(AddressOf OnBindingField)
            End If

        End Sub

        Protected Overridable Sub OnBindingField(ByVal sender As Object, ByVal e As EventArgs)

            Dim target As Control = CType(sender, Control)

            If TypeOf (target) Is TableCell Then
                'View
                Dim tcTmp As TableCell = CType(target, TableCell)

                If DesignMode Then
                    tcTmp.Text = "Risk Attribute"
                Else

                    Dim diTmp As Object = Nothing
                    Dim oValue As Object = Nothing

                    Try
                        diTmp = DataBinder.GetDataItem(target.NamingContainer)
                        oValue = DataBinder.Eval(diTmp, DataField)
                    Catch ex As Exception

                        'Attribute does not exist in XML so use null value
                        tcTmp.Text = NullDisplayText
                    End Try

                    If DataFormatString IsNot String.Empty Then

                        'Find out what the data types is reformat according to the dataformatstring
                        Dim dtValue As DateTime
                        If DateTime.TryParseExact(oValue, "yyyy-MM-dd HH:mm:ss",
                           System.Globalization.CultureInfo.InvariantCulture,
                           System.Globalization.DateTimeStyles.None, dtValue) OrElse
                           DateTime.TryParseExact(oValue, "yyyy-MM-ddTHH:mm:ssZ",
                           System.Globalization.CultureInfo.InvariantCulture,
                           System.Globalization.DateTimeStyles.None, dtValue) Then

                            tcTmp.Text = String.Format(DataFormatString, dtValue)
                        Else
                            Dim dValue As Double
                            If Double.TryParse(oValue, dValue) Then
                                If DataFormatString = "{0:0}%" Then
                                    tcTmp.Text = String.Format("{0:N2}%", dValue)
                                Else
                                    tcTmp.Text = String.Format(DataFormatString, dValue)
                                End If

                            Else
                                Dim iValue As Integer
                                If Integer.TryParse(oValue, iValue) Then
                                    tcTmp.Text = String.Format(DataFormatString, iValue)
                                Else
                                    'tcTmp.Text = String.Format(DataFormatString, oValue)
                                    tcTmp.Text = "&nbsp;"
                                End If
                            End If
                        End If

                    Else
                        If String.IsNullOrEmpty(oValue) Then
                            tcTmp.Text = "&nbsp;"
                        Else
                            tcTmp.Text = oValue.ToString
                        End If
                    End If

                    'If HtmlEncode Then
                    '    tcTmp.Text = Current.Server.HtmlEncode(tcTmp.Text)
                    'End If

                End If
            End If

        End Sub

    End Class

End Namespace