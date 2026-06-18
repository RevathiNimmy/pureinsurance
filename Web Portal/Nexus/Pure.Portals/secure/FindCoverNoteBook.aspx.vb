Imports CMS.Library
Imports System.Web.Configuration.WebConfigurationManager
Imports Nexus.Library
Imports Nexus.Utils
Imports Nexus.Constants.Constant
Imports Nexus.Constants.Session

Namespace Nexus

    Partial Class secure_FindCoverNoteBook : Inherits Frontend.clsCMSPage

        Dim oWebService As NexusProvider.ProviderBase
        Dim oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
        Dim oPortal As Nexus.Library.Config.Portal = oNexusConfig.Portals.Portal(Portal.GetPortalID())

        Protected Shadows Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            grdvSearchResults.Visible = True
            'Make Agnet textbox Read Only
            txtAgentCode.Attributes.Add("readonly", "readonly")

            If Not IsPostBack Then
                'Cleaning of the session values
                ClearQuote()
                ClearClaims()
                ClearHeader()
                ' assigning default dates 
                txtCoverNoteLastUpdate.Text = DateTime.Now.ToShortDateString
                txtAssignedDate.Text = DateTime.Now.ToShortDateString

                FillBranch()
                FillCoverNoteStatus()
            Else
                If ChkAssignedDate.Checked = False Then
                    txtAssignedDate.Text = DateTime.Now.ToShortDateString
                End If
                If ChkLastUpdate.Checked = False Then
                    txtCoverNoteLastUpdate.Text = DateTime.Now.ToShortDateString
                End If
            End If

            'set focus on book number
            txtBookNumber.Focus()

            'Need to Show the Newly CReate CoverNote
            If Request.QueryString("NewCoverNote") IsNot Nothing Then
                Dim bNewCoverNote As Boolean = Request.QueryString("NewCoverNote")
                If bNewCoverNote AndAlso ViewState("SearchData") Is Nothing Then
                    Dim oCovernote As New NexusProvider.CoverNote
                    Dim oCoverNoteCollection As New NexusProvider.CoverNoteCollection
                    oWebService = New NexusProvider.ProviderManager().Provider
                    'to limit the search return from SAM
                    oCovernote.MaxRowsToFetch = oPortal.MaxSearchResults

                    ' Obtains value using SAM call and assigns the value to grid
                    oCoverNoteCollection = oWebService.FindCoverNoteBooks(oCovernote)
                    ViewState("SearchData") = oCoverNoteCollection
                    grdvSearchResults.DataSource = oCoverNoteCollection
                    grdvSearchResults.DataBind()

                End If
            End If
        End Sub
        Sub FillBranch()
            If Session(CNAgentDetails) IsNot Nothing Then
                Dim oUserDetails As NexusProvider.UserDetails
                oUserDetails = Session(CNAgentDetails)
                ddlBranch.DataSource = oUserDetails.ListOfBranches
                ddlBranch.DataTextField = "Description"
                ddlBranch.DataValueField = "Code"
                ddlBranch.DataBind()
                ddlBranch.Items.Insert(0, New ListItem("(all)", ""))
                ddlBranch.SelectedIndex = 0
            End If
        End Sub
        Sub FillCoverNoteStatus()
            Dim olistCoverNoteStatus As NexusProvider.LookupListCollection
            oWebService = New NexusProvider.ProviderManager().Provider
            olistCoverNoteStatus = oWebService.GetList(NexusProvider.ListType.PMLookup, "cover_note_book_status", True, False)
            ddlCoverNoteStatus.DataSource = olistCoverNoteStatus
            ddlCoverNoteStatus.DataTextField = "Description"
            ddlCoverNoteStatus.DataValueField = "Code"
            ddlCoverNoteStatus.DataBind()
            ddlCoverNoteStatus.Items.Insert(0, New ListItem("(all)", ""))
            ddlCoverNoteStatus.SelectedIndex = 0
        End Sub
        Protected Sub btnNewSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNewSearch.Click

            ' Clearing all the values in controls.
            txtEndNumber.Text = String.Empty
            txtStartNumber.Text = String.Empty
            txtAgentCode.Text = String.Empty
            txtBookNumber.Text = String.Empty
            txtPolicyNumber.Text = String.Empty
            ChkLastUpdate.Checked = False
            ChkAssignedDate.Checked = False
            grdvSearchResults.DataSource = Nothing
            grdvSearchResults.DataBind()
            grdvSearchResults.Visible = False
            grdvSearchResults.AllowPaging = True
            'Cleared the hidden value for new search
            hiddenAgentCode.Value = String.Empty
            'Cleared the hidden value for new search
            FillBranch()
            FillCoverNoteStatus()
            ' assigning default dates 
            txtCoverNoteLastUpdate.Text = DateTime.Now.ToShortDateString
            txtAssignedDate.Text = DateTime.Now.ToShortDateString

        End Sub

        Protected Sub btnFindNow_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFindNow.Click
            ' calls the search function 
            If Page.IsValid Then
                SearchCoverNoteBook()
            End If
        End Sub

        Protected Sub SearchCoverNoteBook()

            ' initalizing objects
            oWebService = New NexusProvider.ProviderManager().Provider
            Dim oCovernote As New NexusProvider.CoverNote
            Dim oCoverNoteCollection As New NexusProvider.CoverNoteCollection
            ' checks whether the controls have any values and assigns to 
            ' respective parameters

            If Not txtBookNumber.Text.Trim().Length = 0 Then
                oCovernote.BookNumber = txtBookNumber.Text.Trim()
            End If

            If Not txtStartNumber.Text.Trim().Length = 0 Then
                oCovernote.StartNumber = CType(txtStartNumber.Text.Trim(), Integer)
            End If

            If Not txtEndNumber.Text.Trim().Length = 0 Then
                oCovernote.EndNumber = CType(txtEndNumber.Text.Trim(), Integer)
            End If

            If Not hiddenAgentCode.Value.Trim().Length = 0 Then
                oCovernote.AgentKey = CType(hiddenAgentCode.Value.Trim(), Integer)
            End If

            If ChkLastUpdate.Checked Then
                If Not txtCoverNoteLastUpdate.Text.Trim().Length = 0 Then
                    oCovernote.LastUpdated = txtCoverNoteLastUpdate.Text.Trim()
                End If
            End If

            If ChkAssignedDate.Checked Then
                If Not txtAssignedDate.Text.Trim().Length = 0 Then
                    oCovernote.AssignedDate = txtAssignedDate.Text.Trim()
                End If
            End If

            If ddlBranch.SelectedValue.Trim.Length <> 0 Then

                oCovernote.CoverNoteBranchCode = ddlBranch.SelectedValue
            End If

            If Not ddlCoverNoteStatus.SelectedValue.Trim().Length = 0 Then
                oCovernote.CoverNoteBookStatusCode = ddlCoverNoteStatus.SelectedValue
            End If

            If Not txtPolicyNumber.Text.Trim().Length = 0 Then
                oCovernote.PolicyNumber = txtPolicyNumber.Text.Trim()
            End If

            'to limit the search return from SAM
            oCovernote.MaxRowsToFetch = oPortal.MaxSearchResults

            ' Obtains value using SAM call and assigns the value to grid
            oCoverNoteCollection = oWebService.FindCoverNoteBooks(oCovernote)
            ViewState("SearchData") = oCoverNoteCollection
            grdvSearchResults.Visible = True
            grdvSearchResults.AllowPaging = True
            grdvSearchResults.DataSource = oCoverNoteCollection
            grdvSearchResults.DataBind()

            'validate size of dataset. if 500(configured at portal level) or more results are returned then add a validation message to the screen
            If oCoverNoteCollection.Count >= oPortal.MaxSearchResults Then
                'create a custom validator
                Dim cstMaxResults As New CustomValidator
                cstMaxResults.IsValid = False
                'look for a validation message in the page resources, but if there is not one defined add a default message
                cstMaxResults.ErrorMessage = IIf(GetLocalResourceObject("cstMaxResults") Is Nothing, "Maximum number of search results exceeded, please refine your search criteria", GetLocalResourceObject("cstMaxResults"))
                cstMaxResults.Display = ValidatorDisplay.None 'we only want the error messages in the validation summary
                'add the validator to the page, this will have the effect of making the page invalid
                Page.Validators.Add(cstMaxResults)
            End If

        End Sub

        Protected Sub grdvSearchResults_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles grdvSearchResults.DataBound
            If grdvSearchResults.Rows.Count = 0 Or grdvSearchResults.PageCount = 1 Then
                grdvSearchResults.AllowPaging = False
            End If
        End Sub

        Protected Sub grdvSearchResults_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdvSearchResults.PageIndexChanging
            CType(sender, GridView).PageIndex = e.NewPageIndex
            ''line commented by Simpy Bhatia on dated 06-June
            CType(sender, GridView).DataSource = ViewState("SearchData")
            CType(sender, GridView).DataBind()
        End Sub

        Protected Sub grdvSearchResults_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdvSearchResults.RowDataBound
            ' checks if the row exists
            If e.Row.RowType = DataControlRowType.DataRow Then
                'NOTE - this will need to be changed to give each row a unique id
                'this needs to be matched in markup for the menu (id="Menu_<%# Eval("CoverNoteBookKey") %>")
                e.Row.Attributes.Add("id", CType(e.Row.DataItem, NexusProvider.CoverNote).CoverNoteBookKey)

                ' assigns values to properties of hyperlink 
                Dim oHyperLink As LinkButton = CType(e.Row.Cells(8).FindControl("lnkEdit"), LinkButton)
                Dim oItem As NexusProvider.CoverNote = CType(e.Row.DataItem, NexusProvider.CoverNote)
                ' begin: Pass start number and end number as query string
                oHyperLink.PostBackUrl = "../secure/CoverNoteBook.aspx?Mode=Edit&CoverNoteBookKey=" + oItem.CoverNoteBookKey.ToString() + "&SN=" + oItem.StartNumber.ToString() + "&EN=" + oItem.EndNumber.ToString() + ""
                ' end: Pass start number and end number as query string

            End If

        End Sub

        Protected Sub VldDate_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles VldDate.ServerValidate
            Dim iResult As Integer
            If ChkAssignedDate.Checked = True And IsDate(txtAssignedDate.Text.Trim) = False Then
                args.IsValid = False
                VldDate.ErrorMessage = GetLocalResourceObject("Err_AssignedDate")
            ElseIf args.IsValid = True And ChkAssignedDate.Checked = True And IsDate(txtAssignedDate.Text.Trim) = True Then
                If CDate(txtAssignedDate.Text.Trim) < CDate("01/01/1900") Or CDate(txtAssignedDate.Text.Trim) > CDate("01/12/9998") Then
                    args.IsValid = False
                    VldDate.ErrorMessage = GetLocalResourceObject("Err_AssignedDate")
                End If
            ElseIf args.IsValid = True And ChkLastUpdate.Checked = True And IsDate(txtCoverNoteLastUpdate.Text.Trim) = False Then
                args.IsValid = False
                VldDate.ErrorMessage = GetLocalResourceObject("Err_LastUpdateDate")
            ElseIf args.IsValid = True And ChkLastUpdate.Checked = True And IsDate(txtCoverNoteLastUpdate.Text.Trim) = True Then
                If CDate(txtCoverNoteLastUpdate.Text.Trim) < CDate("01/01/1900") Or CDate(txtCoverNoteLastUpdate.Text.Trim) > CDate("01/12/9998") Then
                    args.IsValid = False
                    VldDate.ErrorMessage = GetLocalResourceObject("Err_LastUpdateDate")
                End If
            ElseIf args.IsValid = True And txtStartNumber.Text.Trim.Length <> 0 And Integer.TryParse(txtStartNumber.Text.Trim, iResult) = False Then
                args.IsValid = False
                VldDate.ErrorMessage = GetLocalResourceObject("Err_InvalidStartNo")
            ElseIf args.IsValid = True And txtEndNumber.Text.Trim.Length <> 0 And Integer.TryParse(txtEndNumber.Text.Trim, iResult) = False Then
                args.IsValid = False
                VldDate.ErrorMessage = GetLocalResourceObject("Err_InvalidEndNo")
            End If
        End Sub

        Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
            If HttpContext.Current.Session.IsCookieless Then
                btnAgent.OnClientClick = "tb_show(null ,'../Modal/FindAgent.aspx?modal=true&KeepThis=true&TB_iframe=true&height=500&width=750' , null);return false;"
            Else
                btnAgent.OnClientClick = "tb_show(null ,'" & AppSettings("WebRoot") & "/Modal/FindAgent.aspx?modal=true&KeepThis=true&TB_iframe=true&height=500&width=750' , null);return false;"
            End If
        End Sub
    End Class

End Namespace
