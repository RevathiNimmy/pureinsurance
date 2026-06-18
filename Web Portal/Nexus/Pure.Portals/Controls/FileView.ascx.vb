Imports System
Imports System.IO
Imports System.Web.Configuration
Imports System.Web.Configuration.WebConfigurationManager
Imports CMS.Library
Imports Nexus
Imports Nexus.Constants
Imports Nexus.Constants.Session
Imports Nexus.Library
Imports Nexus.Utils
Partial Class Controls_FileView
    Inherits System.Web.UI.UserControl

    Dim oFileTypes As Config.FileTypes = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID()).FileTypes
    Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
    Dim oDMEFolder As New NexusProvider.DME
    Dim b_EnableFileUpload As Boolean = True ' Default Set as true
    Public Const CNDocCollection As String = "CNDocCollection"
    Dim sCurrentBranchName As String = ""

    ''' <summary>
    ''' Page Load Event- Display DocumentList based on  CurrentFolder Path
    ''' </summary> 
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Load1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Set FileUpload Panel based on EnableFileUpload Properties Value
        If Session(CNMode) = Mode.View OrElse Session(CNMode) = Mode.ViewClaim Then
            b_EnableFileUpload = False
        End If
        docFrame1.Attributes.Remove("src")
        ' Clear upload status message on every load (btnUploadDocument_Click will re-set it after upload)
        lblFileUploadMessage.Text = ""
        ' Clear filter session on first load of DMEDocumentManager to prevent carry-over from main page
        If Not IsPostBack AndAlso Request.Url.ToString().Contains("DMEDocumentManager.aspx") Then
            Session("DME_FilterCategory") = Nothing
            Session("DME_FilterSubCategory") = Nothing
            Session("DME_FilterDocName") = Nothing
        End If
        If Request.QueryString("BranchCode") <> "" Then
            sCurrentBranchName = GetDescriptionForCode(NexusProvider.ListType.PMLookup, Request.QueryString("BranchCode").Trim(), "SOURCE")
        ElseIf Session(CNBranchCode) IsNot Nothing AndAlso Session(CNBranchCode) <> "" Then
            sCurrentBranchName = GetDescriptionForCode(NexusProvider.ListType.PMLookup, Session(CNBranchCode).Trim(), "SOURCE")
        Else
            sCurrentBranchName = Convert.ToString(Session("BranchName"))
        End If

        ' Keep Session("BranchName") in sync with the resolved branch name
        If Not String.IsNullOrEmpty(sCurrentBranchName) Then
            Session("BranchName") = sCurrentBranchName
        End If

        If Not String.IsNullOrEmpty(Request.QueryString("FolderNum")) And Not String.IsNullOrEmpty(Request.QueryString("FolderName")) And sCurrentBranchName.Trim() <> "" Then
            If HidCurrentFolder.Value = "" Then
                HidCurrentFolder.Value = Session("BranchName").ToString() + "|" + Request.QueryString("FolderName").ToString()

                If (Session("PolicyFolderName") IsNot Nothing And Request.QueryString("fromlink").ToString() = "policy") Then
                    HidCurrentFolder.Value = HidCurrentFolder.Value + "|" + Session("PolicyFolderName").ToString()
                ElseIf (Session("ClaimFolderName") IsNot Nothing And Request.QueryString("fromlink").ToString() = "claim") Then
                    HidCurrentFolder.Value = HidCurrentFolder.Value + "|" + Session("ClaimFolderName").ToString()
                ElseIf (Session("GeneralFolderExist") IsNot Nothing And Request.QueryString("fromlink").ToString() = "client") Then
                    HidCurrentFolder.Value = HidCurrentFolder.Value + "|" + "GENERAL"
                End If
                ' Set FolderNum from query string so upload works on first load
                If String.IsNullOrEmpty(HidFolderNum.Value) Then
                    Dim sRawFolderNum As String = Request.QueryString("FolderNum").ToString().Replace("null", "").Replace("?", "").Trim()
                    ' Extract only leading digits (strip trailing non-numeric chars like ',null) from legacy URLs)
                    HidFolderNum.Value = System.Text.RegularExpressions.Regex.Match(sRawFolderNum, "^\d+").Value
                End If
            End If
            GetDocumentTree(Server.UrlDecode(HidCurrentFolder.Value))
            ' On first load, override HidFolderNum with the actual subfolder number from session
            ' On postback, HidFolderNum is set by JavaScript setFolder — don't override
            If Not IsPostBack Then
                If Request.QueryString("fromlink") IsNot Nothing Then
                    Dim sFromLink As String = Request.QueryString("fromlink").ToString()
                    If sFromLink = "client" AndAlso Session("GeneralFolderNum") IsNot Nothing Then
                        HidFolderNum.Value = Session("GeneralFolderNum").ToString()
                    ElseIf sFromLink = "policy" AndAlso Session("PolicyFolderNum") IsNot Nothing Then
                        HidFolderNum.Value = Session("PolicyFolderNum").ToString()
                    ElseIf sFromLink = "claim" AndAlso Session("ClaimFolderNum") IsNot Nothing Then
                        HidFolderNum.Value = Session("ClaimFolderNum").ToString()
                    End If
                End If
            Else
                Session("GeneralFolderNum") = ""
                Session("PolicyFolderNum") = ""
                Session("ClaimFolderNum") = ""
            End If
        End If
        If Request("__EVENTARGUMENT") = "RefreshDocumentList" Then
            'Restore filter values from Session when navigating between folders
            RestoreFiltersFromSession()
            Dim iCatFilter As Integer = 0
            Dim iSubCatFilter As Integer = 0
            If Not String.IsNullOrEmpty(drpFilterCategory.Value) Then Integer.TryParse(drpFilterCategory.Value, iCatFilter)
            If Not String.IsNullOrEmpty(drpFilterSubCategory.Value) Then Integer.TryParse(drpFilterSubCategory.Value, iSubCatFilter)
            GetDocumentTree(Server.UrlDecode(HidCurrentFolder.Value), iCatFilter, iSubCatFilter, txtFilterDocName.Text.Trim())
        ElseIf Not String.IsNullOrEmpty(HidCurrentFolder.Value) Then
            'Restore filter values from Session on regular load
            If Not IsPostBack Then RestoreFiltersFromSession()
            Dim iCatFilter2 As Integer = 0
            Dim iSubCatFilter2 As Integer = 0
            If Not String.IsNullOrEmpty(drpFilterCategory.Value) Then Integer.TryParse(drpFilterCategory.Value, iCatFilter2)
            If Not String.IsNullOrEmpty(drpFilterSubCategory.Value) Then Integer.TryParse(drpFilterSubCategory.Value, iSubCatFilter2)
            GetDocumentTree(Server.UrlDecode(HidCurrentFolder.Value), iCatFilter2, iSubCatFilter2, txtFilterDocName.Text.Trim())
        End If

        'Display Selected Folder Path 
        If String.IsNullOrEmpty(HidCurrentFolder.Value) Then
            lblSubHeader.Text = GetLocalResourceObject("lbl_DeselectedHeader")
            liUpload.Visible = False
        Else
            liUpload.Visible = True
            lblSubHeader.Text = GetLocalResourceObject("lbl_SubHeader")
            lblSubHeader.Text = Replace(lblSubHeader.Text, "#TransactionNode", Server.UrlDecode(HidCurrentFolder.Value))
        End If
    End Sub
    ''' <summary>
    ''' Upload button click - validates file, reads category/sub-category, and uploads document
    ''' </summary>
    Protected Sub btnUploadDocument_Click(ByVal sender As Object, ByVal e As EventArgs)
        If fuDocumentUpload.HasFile Then
            ' Validate file type server-side
            Dim sExtension As String = Path.GetExtension(fuDocumentUpload.FileName).ToUpper()
            Dim allowedExtensions() As String = {".DOC", ".DOCX", ".JPG", ".XLS", ".XLSX",
                                                  ".PDF", ".TIFF", ".TIF", ".TXT", ".MSG"}
            If Array.IndexOf(allowedExtensions, sExtension) < 0 Then
                lblFileUploadMessage.Text = GetLocalResourceObject("Msg_FileTypeError")
                Return
            End If

            ' Read Category and Sub-Category from dropdowns
            Dim iCategoryId As Integer = 0
            Dim iSubCategoryId As Integer = 0
            If Not String.IsNullOrEmpty(drpUploadCategory.Value) Then
                Integer.TryParse(drpUploadCategory.Value, iCategoryId)
            End If
            If Not String.IsNullOrEmpty(drpUploadSubCategory.Value) Then
                Integer.TryParse(drpUploadSubCategory.Value, iSubCategoryId)
            End If

            Dim RequestedPageURL As String = LCase(Request.Url.Segments(Request.Url.Segments.Length - 1).ToString)
            Dim sPageName = GetLocalResourceObject("sRestrictEventlockFile")
            Dim AddDocumentToDocumasterType As New NexusProvider.AddDocumentToDocumasterType
            Dim oEventDetails As New NexusProvider.EventDetails
            Dim iInsuranceFileKey As Integer = 0
            Dim sTempFolderName As String = Nothing
            Dim sFileFullPath As String = Nothing

            ' Create temp folder and save file
            sTempFolderName = Guid.NewGuid.ToString
            If Not (Directory.Exists(Server.MapPath("~/") + sTempFolderName) AndAlso String.IsNullOrEmpty(sTempFolderName)) Then
                Directory.CreateDirectory(Server.MapPath("~/") + sTempFolderName)
                sFileFullPath = Server.MapPath("~/") + sTempFolderName + "\" + Path.GetFileName(fuDocumentUpload.FileName)
                fuDocumentUpload.SaveAs(sFileFullPath)
            End If

            ' Safety check - if file save failed, show error
            If String.IsNullOrEmpty(sFileFullPath) OrElse Not File.Exists(sFileFullPath) Then
                lblFileUploadMessage.Text = GetLocalResourceObject("Msg_FileUploadError")
                Return
            End If

            ' Always set common fields regardless of session context
            With AddDocumentToDocumasterType
                .v_bVisibleFromWeb = True
                .v_sDescription = Path.GetFileNameWithoutExtension(sFileFullPath)
                .v_sFileName = sFileFullPath
            End With

            ' Populate DTO based on session context
            If Session(CNParty) IsNot Nothing Then
                Dim oParty As NexusProvider.BaseParty = Session.Item(CNParty)
                AddDocumentToDocumasterType.v_iPartyKey = oParty.Key
            End If

            If Session(CNClaim) IsNot Nothing Then
                With AddDocumentToDocumasterType
                    .v_iClaimKey = CType(Session(CNClaim), NexusProvider.ClaimOpen).ClaimKey
                    .v_iInsuranceFolderKey = CType(Session(CNClaimQuote), NexusProvider.Quote).InsuranceFolderKey
                End With
            ElseIf Session(CNQuote) IsNot Nothing Then
                With AddDocumentToDocumasterType
                    .v_iInsuranceFolderKey = CType(Session(CNQuote), NexusProvider.Quote).InsuranceFolderKey
                    .v_iPartyKey = CType(Session(CNQuote), NexusProvider.Quote).PartyKey
                End With
                iInsuranceFileKey = CType(Session(CNQuote), NexusProvider.Quote).InsuranceFileKey
            End If

            ' Always set FolderNum from HidFolderNum if available — this is the folder the user clicked
            ' FolderNum takes priority in the SAM backend (uses AddDocumentDirect)
            Dim sFolderNum As String = System.Text.RegularExpressions.Regex.Match(HidFolderNum.Value.Replace("null", "").Replace("?", "").Trim(), "^\d+").Value
            If Not String.IsNullOrEmpty(sFolderNum) AndAlso IsNumeric(sFolderNum) AndAlso CInt(sFolderNum) > 0 Then
                AddDocumentToDocumasterType.v_iFolderNum = CInt(sFolderNum)
                ' Clear InsuranceFolderKey so SAM uses FolderNum (AddDocumentDirect) instead
                AddDocumentToDocumasterType.v_iInsuranceFolderKey = 0
            ElseIf AddDocumentToDocumasterType.v_iInsuranceFolderKey = 0 AndAlso AddDocumentToDocumasterType.v_iClaimKey = 0 Then
                ' Fallback: try query string FolderNum
                If Request.QueryString("FolderNum") IsNot Nothing Then
                    Dim sQsFolderNum As String = Request.QueryString("FolderNum").ToString().Replace("null", "").Trim()
                    If Not String.IsNullOrEmpty(sQsFolderNum) AndAlso IsNumeric(sQsFolderNum) Then
                        AddDocumentToDocumasterType.v_iFolderNum = CInt(sQsFolderNum)
                    End If
                End If
            End If

            ' Set Category and Sub-Category
            AddDocumentToDocumasterType.v_iDocumentTemplateGroupId = iCategoryId
            AddDocumentToDocumasterType.v_iDocumentTemplateSubGroupId = iSubCategoryId

            ' Call AddDocumentToDocumaster SAM Method
            oWebService.AddDocumentToDocumaster(AddDocumentToDocumasterType)

            ' Display success
            lblFileUploadMessage.Text = GetLocalResourceObject("Msg_FileUpload")

            ' Create event (same logic as existing UploadComplete)
            If LCase(sPageName.Contains(RequestedPageURL)) Then
                'SAM Will Generate an Event
            Else
                With oEventDetails
                    .EventDate = Now()
                    .PartyKey = AddDocumentToDocumasterType.v_iPartyKey
                    .InsuranceFileKey = iInsuranceFileKey
                    .InsuranceFolderKey = AddDocumentToDocumasterType.v_iInsuranceFolderKey
                    .ClaimKey = AddDocumentToDocumasterType.v_iClaimKey
                    .RtfText = GetLocalResourceObject("Msg_EventRtfText")
                    .Description = GetLocalResourceObject("Msg_EventDescription")
                    .UserName = Session(CNLoginName)
                    .EventTypeKey = 10
                    .EventLogSubjectKey = 1
                End With
                oWebService.AddEvent(oEventDetails)
            End If

            ' Refresh document list
            GetDocumentTree(Server.UrlDecode(HidCurrentFolder.Value))

            ' Reset dropdowns after successful upload
            drpUploadCategory.Value = ""
            drpUploadSubCategory.Value = ""
        Else
            lblFileUploadMessage.Text = GetLocalResourceObject("Msg_FileUploadError")
        End If
    End Sub
    ''' <summary>
    ''' This Function is fired when Any Folder has been Selected
    ''' </summary>
    ''' <remarks></remarks>
    Sub GetDocumentTree(ByVal sFolderPath As String, Optional ByVal iFilterCategoryId As Integer = 0, Optional ByVal iFilterSubCategoryId As Integer = 0, Optional ByVal sFilterDocName As String = Nothing)
        'Call GetDMEFolder to get the contents of that folder
        If sFolderPath <> sCurrentBranchName Then
            oDMEFolder = oWebService.GetDMEFolder(0, sFolderPath, True, Nothing, iFilterCategoryId, iFilterSubCategoryId, sFilterDocName)
        End If

        If oDMEFolder.DocumentList IsNot Nothing Then
            If oDMEFolder.DocumentList.Count > 0 Then
                grdvDocumentResults.DataSource() = oDMEFolder.DocumentList
                grdvDocumentResults.AllowPaging = True
            Else
                grdvDocumentResults.DataSource() = Nothing
                grdvDocumentResults.AllowPaging = False
            End If
        End If

        'store the data in ViewState to use again for page indexing and sorting
        ViewState(CNDocCollection) = oDMEFolder.DocumentList

        'Apply existing sort if user had previously sorted a column
        If Not String.IsNullOrEmpty(SortExpression) AndAlso ViewState(CNDocCollection) IsNot Nothing Then
            grdvDocumentResults.DataSource = GetSortedDocumentList(ViewState(CNDocCollection))
        End If

        grdvDocumentResults.DataBind()
    End Sub

    ''' <summary>
    ''' Search/Filter button click - saves filter values to Session for cross-folder persistence
    ''' </summary>
    Protected Sub btnFilter_Click(sender As Object, e As EventArgs)
        If Not String.IsNullOrEmpty(HidCurrentFolder.Value) Then
            Dim iCategoryId As Integer = 0
            Dim iSubCategoryId As Integer = 0
            If Not String.IsNullOrEmpty(drpFilterCategory.Value) Then Integer.TryParse(drpFilterCategory.Value, iCategoryId)
            If Not String.IsNullOrEmpty(drpFilterSubCategory.Value) Then Integer.TryParse(drpFilterSubCategory.Value, iSubCategoryId)
            Dim sDocName As String = txtFilterDocName.Text.Trim()
            'Save filter values to Session so they persist across folder navigation
            Session("DME_FilterCategory") = drpFilterCategory.Value
            Session("DME_FilterSubCategory") = drpFilterSubCategory.Value
            Session("DME_FilterDocName") = sDocName
            GetDocumentTree(Server.UrlDecode(HidCurrentFolder.Value), iCategoryId, iSubCategoryId, sDocName)
        End If
    End Sub

    ''' <summary>
    ''' Clear filter button click - clears Session filter values
    ''' </summary>
    Protected Sub btnClearFilter_Click(sender As Object, e As EventArgs)
        drpFilterCategory.Value = ""
        drpFilterSubCategory.Value = ""
        txtFilterDocName.Text = ""
        'Clear Session filter values
        Session("DME_FilterCategory") = Nothing
        Session("DME_FilterSubCategory") = Nothing
        Session("DME_FilterDocName") = Nothing
        If Not String.IsNullOrEmpty(HidCurrentFolder.Value) Then
            GetDocumentTree(Server.UrlDecode(HidCurrentFolder.Value))
        End If
    End Sub

    ''' <summary>
    ''' Restores filter textbox values from Session for cross-folder persistence
    ''' </summary>
    Private Sub RestoreFiltersFromSession()
        If Session("DME_FilterCategory") IsNot Nothing Then
            drpFilterCategory.Value = Session("DME_FilterCategory").ToString()
        End If
        If Session("DME_FilterSubCategory") IsNot Nothing Then
            drpFilterSubCategory.Value = Session("DME_FilterSubCategory").ToString()
        End If
        If Session("DME_FilterDocName") IsNot Nothing Then
            txtFilterDocName.Text = Session("DME_FilterDocName").ToString()
        End If
    End Sub

    ''' <summary>
    ''' This is fired on Load of the Grid View
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub grdvDocumentResults_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles grdvDocumentResults.Load
        'No Need to Show page Number When result not more then one page
        If grdvDocumentResults.PageCount = 1 Then
            grdvDocumentResults.AllowPaging = False
        End If
    End Sub
    ''' <summary>
    ''' ViewState property to persist the current sort expression
    ''' </summary>
    Private Property SortExpression() As String
        Get
            If ViewState("SortExpression") Is Nothing Then
                Return String.Empty
            End If
            Return ViewState("SortExpression").ToString()
        End Get
        Set(ByVal value As String)
            ViewState("SortExpression") = value
        End Set
    End Property

    ''' <summary>
    ''' ViewState property to persist the current sort direction
    ''' </summary>
    Private Property SortDirection() As System.Web.UI.WebControls.SortDirection
        Get
            If ViewState("SortDirection") Is Nothing Then
                Return System.Web.UI.WebControls.SortDirection.Ascending
            End If
            Return CType(ViewState("SortDirection"), System.Web.UI.WebControls.SortDirection)
        End Get
        Set(ByVal value As System.Web.UI.WebControls.SortDirection)
            ViewState("SortDirection") = value
        End Set
    End Property

    ''' <summary>
    ''' Sorts the document list by the specified property and direction
    ''' </summary>
    Private Function GetSortedDocumentList(ByVal docList As Object) As Object
        If docList Is Nothing OrElse String.IsNullOrEmpty(SortExpression) Then
            Return docList
        End If

        Dim dt As New System.Data.DataTable()
        Dim oDocList As NexusProvider.DocumentListCollection = CType(docList, NexusProvider.DocumentListCollection)

        dt.Columns.Add("DocNum", GetType(Integer))
        dt.Columns.Add("DocDescription", GetType(String))
        dt.Columns.Add("CreateDate", GetType(DateTime))
        dt.Columns.Add("DocumentType", GetType(String))
        dt.Columns.Add("UploadedBy", GetType(String))
        dt.Columns.Add("Category", GetType(String))
        dt.Columns.Add("SubCategory", GetType(String))

        For Each doc As NexusProvider.DocumentList In oDocList
            Dim dtCreateDate As DateTime = DateTime.MinValue
            DateTime.TryParse(doc.CreateDate, dtCreateDate)
            dt.Rows.Add(doc.DocNum, doc.DocDescription, dtCreateDate, doc.DocumentType, If(doc.UploadedBy, String.Empty), If(doc.Category, String.Empty), If(doc.SubCategory, String.Empty))
        Next

        Dim sSortDir As String = If(SortDirection = System.Web.UI.WebControls.SortDirection.Ascending, "ASC", "DESC")
        dt.DefaultView.Sort = SortExpression & " " & sSortDir
        Return dt
    End Function

    ''' <summary>
    ''' This is fired when a column header is clicked to sort the Grid View
    ''' </summary>
    Protected Sub grdvDocumentResults_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles grdvDocumentResults.Sorting
        ' Toggle direction if same column clicked again
        If SortExpression = e.SortExpression Then
            If SortDirection = System.Web.UI.WebControls.SortDirection.Ascending Then
                SortDirection = System.Web.UI.WebControls.SortDirection.Descending
            Else
                SortDirection = System.Web.UI.WebControls.SortDirection.Ascending
            End If
        Else
            SortExpression = e.SortExpression
            SortDirection = System.Web.UI.WebControls.SortDirection.Ascending
        End If

        grdvDocumentResults.DataSource = GetSortedDocumentList(ViewState(CNDocCollection))
        grdvDocumentResults.DataBind()
    End Sub

    ''' <summary>
    ''' This is fired on Page Index Change of the Grid View
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub grdvDocumentResults_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdvDocumentResults.PageIndexChanging
        grdvDocumentResults.PageIndex = e.NewPageIndex
        If Not String.IsNullOrEmpty(SortExpression) Then
            grdvDocumentResults.DataSource = GetSortedDocumentList(ViewState(CNDocCollection))
        Else
            grdvDocumentResults.DataSource = ViewState(CNDocCollection)
        End If
        grdvDocumentResults.DataBind()
    End Sub
    ''' <summary>
    ''' This event is fired on Row Data Bound of Grid View
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub grdvDocumentResults_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdvDocumentResults.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then

            ' Extract field values - DataItem is always DataRowView since we use DataTable
            Dim sDocNum As String = String.Empty
            Dim sDocDescription As String = String.Empty
            Dim sDocumentType As String = String.Empty
            Dim sUploadedBy As String = String.Empty

            If TypeOf e.Row.DataItem Is NexusProvider.DocumentList Then
                Dim oDoc As NexusProvider.DocumentList = CType(e.Row.DataItem, NexusProvider.DocumentList)
                sDocNum = oDoc.DocNum.ToString()
                sDocDescription = oDoc.DocDescription
                sDocumentType = oDoc.DocumentType.ToString()
                sUploadedBy = If(oDoc.UploadedBy, String.Empty)
            ElseIf TypeOf e.Row.DataItem Is System.Data.DataRowView Then
                Dim drv As System.Data.DataRowView = CType(e.Row.DataItem, System.Data.DataRowView)
                sDocNum = drv("DocNum").ToString()
                sDocDescription = drv("DocDescription").ToString()
                sDocumentType = drv("DocumentType").ToString()
                sUploadedBy = drv("UploadedBy").ToString()
            End If

            e.Row.Attributes.Add("id", sDocNum)

            If e.Row.FindControl("hypDocDescription") IsNot Nothing Then
                If (sDocumentType = "PDF") Then
                    Dim sUrl As String = ""
                    sUrl = AppSettings("WebRoot") & "/secure/document.aspx?modal=true&KeepThis=true&TB_iframe=true&height=500&width=750"
                    CType(e.Row.FindControl("hypDocDescription"), LinkButton).CommandArgument = sUrl & "|" & sDocNum & "|" & sDocumentType
                Else
                    CType(e.Row.FindControl("hypDocDescription"), LinkButton).CommandArgument = "~/secure/document.aspx?filename=" & Server.UrlEncode(Trim(sDocDescription)) & "&doctype=" & sDocumentType & "|" & sDocNum & "|" & sDocumentType
                End If
                CType(e.Row.FindControl("hypDocDescription"), LinkButton).Text = sDocDescription
                If oFileTypes.FileType(UCase(sDocumentType)) IsNot Nothing AndAlso oFileTypes.FileType(UCase(sDocumentType)).DocType IsNot Nothing Then
                    CType(e.Row.FindControl("hypDocDescription"), LinkButton).CssClass = oFileTypes.FileType(UCase(sDocumentType)).CssClass
                End If
            End If
            If e.Row.FindControl("lblDocumentType") IsNot Nothing Then
                If oFileTypes.FileType(UCase(sDocumentType)) IsNot Nothing AndAlso oFileTypes.FileType(UCase(sDocumentType)).DocType IsNot Nothing Then
                    CType(e.Row.FindControl("lblDocumentType"), Label).Text = oFileTypes.FileType(UCase(sDocumentType)).Display
                End If
            End If
        ElseIf e.Row.RowType = DataControlRowType.Header Then

        End If
    End Sub
    Protected Sub lnkDocDescription_Command(sender As Object, e As CommandEventArgs)
        Dim args As String() = e.CommandArgument.ToString().Split("|"c)
        Dim docUrl As String = args(0)
        Dim docNumber As String = args(1)
        Dim docType As String = args(2)
        Session("SelectedDocId") = docNumber
        ' Redirect to full URL
        If (docType = "PDF") Then
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "tb_show",
                                  "<script language=""JavaScript"" type=""text/javascript"">$(document).ready(function(){window.open('" & docUrl & "', '_blank' );});</script>", False)
        Else
            docFrame1.Attributes.Add("src", docUrl)
            Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "UnblockUI", "$.unblockUI();", True)
        End If
    End Sub

    ''' <summary>
    '''  Add a New Properties “EnableFileUpload” (Boolean) 
    ''' </summary>
    ''' <remarks></remarks>
    Public Property EnableFileUpload() As Boolean
        Get
            Return b_EnableFileUpload
        End Get
        Set(ByVal Value As Boolean)
            b_EnableFileUpload = Value
        End Set
    End Property
    ''' <summary>
    '''  Add a New Properties “CurrentFolder” (String)  
    ''' </summary>
    ''' <remarks></remarks>
    Public Property CurrentFolder() As String
        Get
            Return Server.UrlDecode(HidCurrentFolder.Value)
        End Get
        Set(ByVal Value As String)
            HidCurrentFolder.Value = Server.UrlDecode(Value)
        End Set
    End Property

End Class
