Imports CMS.Library
Imports System.Web.Configuration.WebConfigurationManager
Imports Nexus.Library
Imports Nexus.Utils
Imports Nexus.Constants.Constant
Imports Nexus.Constants.Session

Namespace Nexus

    Partial Class secure_CoverNoteBook
        Inherits Frontend.clsCMSPage

        Dim oWebService As NexusProvider.ProviderBase
        Dim sEdit As String = "Edit"
        Dim sDelete As String = "Delete"

        Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click
            CoverNoteSheet()
        End Sub

        Protected Sub grdvCoverNoteBook_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdvCoverNoteBook.PageIndexChanging
            CType(sender, GridView).PageIndex = e.NewPageIndex
            CType(sender, GridView).DataSource = Session(CNCoverNoteSheetData)
            CType(sender, GridView).DataBind()
        End Sub


        Protected Sub grdvCoverNoteBook_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grdvCoverNoteBook.RowCommand
            If e.CommandName = "Delete" Then
                ' Instansiating object for use
                oWebService = New NexusProvider.ProviderManager().Provider
                Dim oCovernote As New NexusProvider.CoverNote
                Dim oCoverNoteSheet As New NexusProvider.CoverNoteSheetType

                Try
                    ' assigning values to properties from controls if populated
                    oCovernote.CoverNoteBookKey = Request.QueryString("CoverNoteBookKey")
                    oCoverNoteSheet.CoverNoteSheetNumber = CInt(e.CommandArgument)
                    oCovernote.CoverNoteSheets.Add(oCoverNoteSheet)
                    oCovernote.CoverNoteBookTimestamp = Session(CNTimeStamp)

                    ' calling SAM to delete CoverNoteBook
                    oWebService.DeleteCoverNoteSheet(oCovernote)
                    Session(CNTimeStamp) = oCovernote.CoverNoteBookTimestamp
                    ' after delete refresh the page to obtain latest information
                    PopulatePage()

                    'Catch ex As Exception
                Finally

                    oWebService = Nothing
                    oCovernote = Nothing

                End Try
            End If
        End Sub

        Protected Sub grdvCoverNoteBook_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdvCoverNoteBook.RowDataBound

            If e.Row.RowType = DataControlRowType.DataRow Then
                'NOTE - this will need to be changed to give each row a unique id
                'this needs to be matched in markup for the menu (id="Menu_<%# Eval("CoverNoteSheetKey") %>")
                e.Row.Attributes.Add("id", CType(e.Row.DataItem, NexusProvider.CoverNoteSheetType).CoverNoteSheetKey)

                ' assigns values to properties of hyperlink 
                Dim oliEditLink As HtmlGenericControl = CType(e.Row.Cells(7).FindControl("liEdit"), HtmlGenericControl)
                Dim oliDeleteLink As HtmlGenericControl = CType(e.Row.Cells(7).FindControl("liDelete"), HtmlGenericControl)

                Dim oHyperLink As LinkButton = CType(e.Row.Cells(7).FindControl("lnkEdit"), LinkButton)
                Dim oHyperLinkButtonDelete As LinkButton = CType(e.Row.Cells(7).FindControl("lnkDelete"), LinkButton)
                oHyperLinkButtonDelete.CommandName = "Delete"
                oHyperLinkButtonDelete.CommandArgument = CType(e.Row.DataItem, NexusProvider.CoverNoteSheetType).CoverNoteSheetKey


                ' begin: Confirmation for delete
                oHyperLinkButtonDelete.Attributes.Add("OnClick", "javascript:return " + ("confirm('Are you sure you want to delete this cover sheet from the Cover Note Book?')"))
                ' end: Confirmation for delete
                Dim oItem As NexusProvider.CoverNoteSheetType = CType(e.Row.DataItem, NexusProvider.CoverNoteSheetType)

                ' begin: Pass startnumber and endnumber as querystring
                If HttpContext.Current.Session.IsCookieless Then
                    oHyperLink.OnClientClick = "tb_show(null , " & AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & "/Modal/CoverNoteSheet.aspx?PostbackTo=" & UpdCNB.ClientID.ToString & "&Mode=Edit&CoverNoteBookKey=" + Request.QueryString("CoverNoteBookKey") + "&CoverNoteSheetNumber=" + oItem.CoverNoteSheetNumber.ToString() + "&SN=" + ViewState.Item(CNStartNumber) + "&EN=" + ViewState.Item(CNEndNumber) + "&modal=true&KeepThis=true&TB_iframe=true&height=500&width=650' , null);return false;"
                Else
                    oHyperLink.OnClientClick = "tb_show(null , '../Modal/CoverNoteSheet.aspx?PostbackTo=" & UpdCNB.ClientID.ToString & "&Mode=Edit&CoverNoteBookKey=" + Request.QueryString("CoverNoteBookKey") + "&CoverNoteSheetNumber=" + oItem.CoverNoteSheetNumber.ToString() + "&SN=" + ViewState.Item(CNStartNumber) + "&EN=" + ViewState.Item(CNEndNumber) + "&modal=true&KeepThis=true&TB_iframe=true&height=500&width=650' , null);return false;"
                End If
                ' end: Pass startnumber and endnumber as querystring

                ' Hidden feild to store "CoverNoteSheetKey" so it can be obtained when required
                Dim TextHiddenFeild As HiddenField = CType(e.Row.Cells(7).FindControl("hiddenKey"), HiddenField)
                TextHiddenFeild.Value = oItem.CoverNoteSheetKey

                If CType(e.Row.DataItem, NexusProvider.CoverNoteSheetType).CoverNoteSheetStatusCode IsNot Nothing Then
                    If CType(e.Row.DataItem, NexusProvider.CoverNoteSheetType).CoverNoteSheetStatusCode.Trim.ToUpper = "ISSUED" Then
                        oHyperLinkButtonDelete.Visible = False
                        oliDeleteLink.Visible = False
                    End If
                End If
            End If

        End Sub

        Protected Shadows Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            ' instansiating objects
            oWebService = New NexusProvider.ProviderManager().Provider
            Dim oCovernote As New NexusProvider.CoverNote
            txtAgentCode.Attributes.Add("readonly", "readonly")

            If Not IsPostBack Then

                If ViewState.Item(CNCoverMode) = sEdit Then
                   
                    'To set the Focus
                    Page.SetFocus(txtEffectiveDate)

                    ' Populate page with information related to the "CovernoteBookkey" 
                    PopulatePage()
                    btnAdd.Text = "Add Sheet"
                    ' begin: Pass startnumber and endnumber as querystring
                    If HttpContext.Current.Session.IsCookieless Then
                        btnAdd.OnClientClick = "tb_show(null ," & AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & "/Modal/CoverNoteSheet.aspx?PostbackTo=" & UpdCNB.ClientID.ToString & "&Mode=Add&CoverNoteBookKey=" + Request.QueryString("CoverNoteBookKey") + "&SN=" + ViewState.Item(CNStartNumber) + "&EN=" + ViewState.Item(CNEndNumber) + "&modal=true&KeepThis=true&TB_iframe=true&height=500&width=650' , null);return false;"
                    Else
                        btnAdd.OnClientClick = "tb_show(null , '../Modal/CoverNoteSheet.aspx?PostbackTo=" & UpdCNB.ClientID.ToString & "&Mode=Add&CoverNoteBookKey=" + Request.QueryString("CoverNoteBookKey") + "&SN=" + ViewState.Item(CNStartNumber) + "&EN=" + ViewState.Item(CNEndNumber) + "&modal=true&KeepThis=true&TB_iframe=true&height=500&width=650' , null);return false;"
                    End If
                    ' end: Pass startnumber and endnumber as querystring
                Else
                   
                    'To set the Focus
                    Page.SetFocus(txtBookNumber)

                    txtCreatedDate.Text = CDate(DateTime.Now.ToShortDateString)
                    'oCovernote.CoverNoteBookKey = CType(ViewState.Item("CoverNoteBookKey"), Integer)
                    oCovernote.CoverNoteBookKey = CType(Request.QueryString("CoverNoteBookKey"), Integer)

                    PopulateNewBookProduct()
                    btnAdd.Visible = False

                End If
            End If

            'Update The Grid
            If Request("__EVENTARGUMENT") = "RefreshGrid" Then
                oWebService = New NexusProvider.ProviderManager().Provider
                oCovernote.CoverNoteBookKey = CType(Request.QueryString("CoverNoteBookKey"), Integer)
                ' Obtain data related to CoverNoteBookKey
                oWebService.GetCoverNoteBook(oCovernote)
                ' assigning obtained values from SAM to grid
                grdvCoverNoteBook.DataSource = oCovernote.CoverNoteSheets
                grdvCoverNoteBook.DataBind()

                Session(CNCoverNoteSheetData) = oCovernote.CoverNoteSheets
                Session(CNTimeStamp) = oCovernote.CoverNoteBookTimestamp
            End If

        End Sub

        Protected Sub PopulatePage()
            If Request.QueryString("CoverNoteBookKey") IsNot Nothing Then
                ' instansiating objects
                oWebService = New NexusProvider.ProviderManager().Provider
                Dim oCovernote As New NexusProvider.CoverNote

                'Branch Population
                FillBranch()
                
                Try
                    ' assigning CoverNoteBookKey to property from viewstate

                    oCovernote.CoverNoteBookKey = CType(Request.QueryString("CoverNoteBookKey"), Integer)

                    ' Obtain data related to CoverNoteBookKey
                    oWebService.GetCoverNoteBook(oCovernote)

                    ' Populating products in List
                    PopulateProductList(oCovernote)

                    ' assigning obtained values from SAM to grid
                    grdvCoverNoteBook.DataSource = oCovernote.CoverNoteSheets
                    grdvCoverNoteBook.DataBind()

                    Session(CNCoverNoteSheetData) = oCovernote.CoverNoteSheets

                    ' assigning values to controls which are obtained from SAM
                    txtBookNumber.Text = oCovernote.BookNumber
                    txtStartNumber.Text = oCovernote.StartNumber
                    txtEndNumber.Text = oCovernote.EndNumber

                    'Check for availabillity of date, if not exists set current date as default
                    If IsDate(oCovernote.EffectiveDate) And CDate(oCovernote.EffectiveDate.ToShortDateString) <> Date.MinValue _
                    And CDate(oCovernote.EffectiveDate) >= CDate("01/01/1900") Then
                        txtEffectiveDate.Text = CDate(oCovernote.EffectiveDate)
                    Else
                        txtEffectiveDate.Text = CDate(Date.Today.ToShortDateString)
                    End If

                    'end: Bug fixing
                    txtAgentCode.Attributes.Add("readonly", "readonly")

                    If oCovernote.AgentKey > 0 Then
                        hiddenAgentCode.Value = oCovernote.AgentKey
                        txtAgentCode.Text = oCovernote.AgentName
                    End If

                    txtCreatedDate.Text = CDate(oCovernote.DateCreated.ToShortDateString)
                    Session(CNTimeStamp) = oCovernote.CoverNoteBookTimestamp
                    ddlBranch.SelectedValue = oCovernote.CoverNoteBranchCode.Trim()

                    'Ram begin: Bug fixing
                    'Check for availabillity of book status, if not exists set "Not Issued" as default
                    If oCovernote.CoverNoteBookStatusCode = "" Then
                        ddlCoverNoteStatus.Value = "Not Issued"
                    Else
                        ddlCoverNoteStatus.Value = oCovernote.CoverNoteBookStatusCode
                    End If
                    'Ram end: Bug fixing

                    ' disabling controls
                    txtBookNumber.Enabled = False
                    txtStartNumber.Enabled = False
                    txtEndNumber.Enabled = False

                Finally
                    oWebService = Nothing
                    oCovernote = Nothing
                End Try
            End If
        End Sub

        Protected Sub PopulateProductList(ByRef oCovernote As NexusProvider.CoverNote)
            If oCovernote.CoverNoteBookProducts.Count > 0 Then
                Dim oProductCollection As New NexusProvider.ProductCollection

                'Retreive only selected products
                For Each oProduct As NexusProvider.Product In oCovernote.CoverNoteBookProducts
                    If oProduct.Chosen Then
                        oProductCollection.Add(oProduct)
                    End If
                Next
                'Calling on controls functionalities SetSelectedValues to set the values in Pick list
                pckProduct.SetSelectedValues(oProductCollection)
            End If

        End Sub

        Protected Sub Page_PreInit1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit

            'CoverNoteBookkey = Nothing
            ViewState.Add(CNCoverMode, Request.QueryString("Mode"))

            'Ram begin: Add the startnumber and endnumber to viewstate
            ViewState.Add(CNStartNumber, Request.QueryString("SN"))
            ViewState.Add(CNEndNumber, Request.QueryString("EN"))
            'Ram end: Add the startnumber and endnumber to viewstate

            'CMS.Library.Frontend.Functions.SetTheme(Page, AppSettings("ModalPageTemplate"))

        End Sub
        Sub FillBranch()
            If Session(CNAgentDetails) IsNot Nothing Then
                Dim oUserDetails As NexusProvider.UserDetails
                oUserDetails = Session(CNAgentDetails)
                ddlBranch.DataSource = oUserDetails.ListOfBranches
                ddlBranch.DataTextField = "Description"
                ddlBranch.DataValueField = "Code"
                ddlBranch.DataBind()
                ddlBranch.Items.Insert(0, New ListItem("(Please Select)", ""))
                ddlBranch.SelectedIndex = 0
            End If
        End Sub
        Protected Sub PopulateNewBookProduct()

            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oList As NexusProvider.LookupListCollection = oWebService.GetList(NexusProvider.ListType.PMLookup, "Product", True, False)

            'Branch Population
            FillBranch()
            ' instansiating objects
            Dim ProductListItem As ListItem

            ' clearing items in list if already exists
            '   ChkListProducts.Items.Clear()

            For Each oListitem As NexusProvider.LookupListItem In oList
                ProductListItem = New ListItem
                ProductListItem.Text = oListitem.Description
                ProductListItem.Value = oListitem.Code
                '   ChkListProducts.Items.Add(ProductListItem)
                ProductListItem = Nothing
            Next

            txtEffectiveDate.Text = CDate(Date.Today.ToShortDateString)
            ddlCoverNoteStatus.Value = "NOTISS"

        End Sub
        Private Sub ValidatePage()
            'Validation of Product
            Dim checkedCount As Integer = 0
            Dim oProductListColl As ListItemCollection
            Dim iProductCount As Integer = 0
            oProductListColl = pckProduct.GetSelectedItems()

            If oProductListColl IsNot Nothing Then
                checkedCount = oProductListColl.Count
            End If

            If checkedCount = 0 Then
                ProductCustomValidator.Enabled = True
                ProductCustomValidator.IsValid = False
            End If

            'Validation of Agent
            If hiddenAgentCode.Value.Trim.Length = 0 Then
                AgentCustomvalidator.IsValid = False
                AgentCustomvalidator.Enabled = True
            End If

            'Validate Branch Code
            If ddlBranch.SelectedValue.Trim.Length = 0 Then
                RqdBranch.Enabled = True
                RqdBranch.IsValid = False
            End If
        End Sub
        Protected Sub btnOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOk.Click
            Dim checkedCount As Integer = 0
            Dim oProductListColl As ListItemCollection
            Dim iProductCount As Integer = 0
            oProductListColl = pckProduct.GetSelectedItems()

            If oProductListColl IsNot Nothing Then
                checkedCount = oProductListColl.Count
            End If

            If hiddenAgentCode.Value.Trim.Length = 0 And checkedCount = 0 And ddlBranch.SelectedValue.Trim.Length = 0 Then
                AgentCustomvalidator.IsValid = True
                AgentCustomvalidator.Enabled = False
                RqdBranch.Enabled = False
                RqdBranch.IsValid = True
                ProductCustomValidator.Enabled = False
                ProductCustomValidator.IsValid = True
            Else
                ValidatePage()
            End If

            If Page.IsValid Then
                CoverNoteSheet()
            End If
        End Sub

        Protected Sub CoverNoteSheet()
            If Page.IsValid Then

                ' Instansiating object for use
                oWebService = New NexusProvider.ProviderManager().Provider
                Dim oCovernote As New NexusProvider.CoverNote
                Dim oProduct As NexusProvider.Product
                'Dim bProductedSelected As Boolean = False

                Try

                    Dim oCoverNoteCollection As New NexusProvider.CoverNoteCollection
                    oCoverNoteCollection = oWebService.FindCoverNoteBooks(oCovernote)

                    If Request.QueryString("CoverNoteBookKey") Is Nothing Then
                        For Each coverNoteItem As NexusProvider.CoverNote In oCoverNoteCollection
                            If coverNoteItem.BookNumber = txtBookNumber.Text Then
                                PnlError.Visible = True
                                lblError.Text = "BookNumber already exists"
                                Exit Sub
                            End If
                        Next
                    End If

                    oCovernote.CoverNoteBookKey = Request.QueryString("CoverNoteBookKey")

                    ' assigning values to properties from controls if populated
                    If Not txtBookNumber.Text.Trim().Length = 0 Then
                        oCovernote.BookNumber = txtBookNumber.Text.Trim()
                    End If

                    If Not txtStartNumber.Text.Trim().Length = 0 Then
                        oCovernote.StartNumber = txtStartNumber.Text.Trim()
                    End If

                    If Not txtEndNumber.Text.Trim().Length = 0 Then
                        oCovernote.EndNumber = txtEndNumber.Text.Trim()
                    End If

                    If Not txtEffectiveDate.Text.Trim.Length = 0 Then
                        oCovernote.EffectiveDate = txtEffectiveDate.Text.Trim()
                    End If

                    If Not hiddenAgentCode.Value.Trim.Length = 0 Then
                        oCovernote.AgentKey = hiddenAgentCode.Value
                    End If

                    If Not ddlBranch.SelectedValue.Trim.Length = 0 Then
                        oCovernote.CoverNoteBranchCode = ddlBranch.SelectedValue
                    End If

                    If Not ddlCoverNoteStatus.Text.Trim.Length = 0 Then
                        oCovernote.CoverNoteStatusCode = ddlCoverNoteStatus.Value
                    End If

                    ' Obtaining values from the productlist
                    Dim oProductListColl As ListItemCollection
                    oProductListColl = pckProduct.GetSelectedItems()
                    For Each oListItem As ListItem In oProductListColl

                        If oListItem.Selected Then
                            oProduct = New NexusProvider.Product
                            oProduct.Description = oListItem.Text
                            oProduct.ProductCode = oListItem.Value

                            oCovernote.CoverNoteBookProducts.Add(oProduct)
                            oProduct = Nothing
                        End If
                    Next

                    If ViewState.Item(CNCoverMode) = sEdit Then
                        oCovernote.CoverNoteBookTimestamp = Session(CNTimeStamp)
                        ' calling SAM to update CoverNoteBook
                        oWebService.UpdateCoverNoteBook(oCovernote)
                        PopulatePage()
                    Else
                        ' calling SAM to add new CoverNoteBook
                        oWebService.AddCoverNoteBook(oCovernote)

                    End If
                    'Move to the FindCoverNote Page
                    Response.Redirect("../secure/FindCoverNoteBook.aspx?NewCoverNote=true", False)
                Finally
                    oWebService = Nothing
                    oCovernote = Nothing
                End Try
            End If
        End Sub

        Protected Sub grdvCoverNoteBook_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles grdvCoverNoteBook.RowDeleting

        End Sub

        Protected Sub cusValidNumber_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles cusValidNumber.ServerValidate
            Dim iResult As Integer
            Dim iStartNo, iEndNo As Integer
            If Integer.TryParse(txtStartNumber.Text.Trim, iResult) = False Then
                args.IsValid = False
                cusValidNumber.ErrorMessage = GetLocalResourceObject("Err_InvalidStartNo")
            ElseIf Integer.TryParse(txtEndNumber.Text.Trim, iResult) = False Then
                args.IsValid = False
                cusValidNumber.ErrorMessage = GetLocalResourceObject("Err_InvalidEndNo")
            ElseIf Integer.TryParse(txtStartNumber.Text.Trim, iStartNo) = True And Integer.TryParse(txtEndNumber.Text.Trim, iEndNo) = True Then
                If iStartNo <= 0 Then
                    args.IsValid = False
                    cusValidNumber.ErrorMessage = GetLocalResourceObject("Err_InvalidStartNo")
                ElseIf iEndNo < iStartNo Then
                    args.IsValid = False
                    cusValidNumber.ErrorMessage = GetLocalResourceObject("Err_InvalidEndNoLessThan")
                End If
            End If
        End Sub

        Protected Sub VldEffectiveDate_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles VldEffectiveDate.ServerValidate
            If txtEffectiveDate.Text.Trim.Length = 0 Then
                args.IsValid = False
                VldEffectiveDate.ErrorMessage = GetLocalResourceObject("Err_InvalidEffectiveDate")
            ElseIf txtEffectiveDate.Text.Trim.Length <> 0 And IsDate(txtEffectiveDate.Text.Trim) = False Then
                args.IsValid = False
                VldEffectiveDate.ErrorMessage = GetLocalResourceObject("Err_InvalidEffectiveDate")
            End If
            If args.IsValid = True And IsDate(txtEffectiveDate.Text.Trim) = True Then
                If CDate(txtEffectiveDate.Text.Trim) < CDate("01/01/1900") Or CDate(txtEffectiveDate.Text.Trim) > CDate("01/12/9998") Then
                    args.IsValid = False
                    VldEffectiveDate.ErrorMessage = GetLocalResourceObject("Err_InvalidEffectiveDate")
                End If
            End If
        End Sub
    End Class

End Namespace
