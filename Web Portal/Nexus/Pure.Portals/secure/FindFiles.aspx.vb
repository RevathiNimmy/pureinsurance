Imports CMS.Library
Imports Nexus.Library
Imports Nexus.Utils
Imports System.Web.Configuration
Imports System.Web.Configuration.WebConfigurationManager
Imports Nexus.Constants.Constant
Imports Nexus.Constants.Session

Namespace Nexus

    Partial Class secure_FindFiles
        Inherits Frontend.clsCMSPage

        Public Const CNDocumentListCollection As String = "DocumentListCollection"
        Dim oFileTypes As Config.FileTypes = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID()).FileTypes

        ''' <summary>
        ''' This event is Fired on Search Button Click
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
            If Page.IsValid Then
                BindGrid()
            End If
        End Sub
        ''' <summary>
        '''  To Get the search results 
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub BindGrid()
            Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oDMESearchCriteria As New NexusProvider.DMESearchCriteria
            Dim oDME As NexusProvider.DME

            'Initializing the values
            oDMESearchCriteria.PartyCode = Trim(txtClientCode.Text)
            oDMESearchCriteria.PartyName = Trim(txtClientName.Text)
            oDMESearchCriteria.PostCode = Trim(txtPostCode.Text)
            oDMESearchCriteria.PolicyNumber = Trim(txtPolicyNumber.Text)
            oDMESearchCriteria.ClaimNumber = Trim(txtClaimNumber.Text)
            oDMESearchCriteria.RiskIndex = Trim(txtRiskIndex.Text)
            oDMESearchCriteria.DocumentDescription = Trim(txtFilename.Text)
            oDMESearchCriteria.IncludeFiles = True

            'Sam Call with Search criteria
            oDME = oWebservice.FindDMEDocuments(oDMESearchCriteria)

            'The results grid will show Document and folders. 
           
            Dim oTempListOfDME As New NexusProvider.DME
            Dim oFolderList As New NexusProvider.DocumentList
            If oDME.SubFolder IsNot Nothing Then
                For iCount As Integer = 0 To oDME.SubFolder.Count - 1
                    oFolderList = New NexusProvider.DocumentList()
                    oFolderList.CreateDate = oDME.SubFolder(iCount).CreateDate
                    'If a folder is displayed then the “FolderPath” and “DocDescription” column set as empty 
                    oFolderList.FolderPath = ""
                    oFolderList.DocDescription = ""
                    oTempListOfDME.DocumentList.Add(oFolderList)
                Next
            End If
            'Get the search results of DocumentList Collection
            Dim oDocumentList As New NexusProvider.DocumentList
            If oDME.DocumentList IsNot Nothing Then
                For jCount As Integer = 0 To oDME.DocumentList.Count - 1
                    oDocumentList = New NexusProvider.DocumentList()
                    oDocumentList.CreateDate = oDME.DocumentList(jCount).CreateDate
                    oDocumentList.DocDescription = oDME.DocumentList(jCount).DocDescription
                    oDocumentList.DocNum = oDME.DocumentList(jCount).DocNum
                    oDocumentList.DocumentType = oDME.DocumentList(jCount).DocumentType
                    oDocumentList.FolderNum = oDME.DocumentList(jCount).FolderNum
                    oDocumentList.FolderPath = oDME.DocumentList(jCount).FolderPath
                    oTempListOfDME.DocumentList.Add(oDocumentList)
                Next
            End If

            'store the data in ViewState to use again for page indexing
            ViewState.Add(CNDocumentListCollection, oTempListOfDME.DocumentList)

            'Populating the session with search results
            grdvDocumentResults.Visible = True
            grdvDocumentResults.AllowPaging = True
            grdvDocumentResults.DataSource = ViewState(CNDocumentListCollection)
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
            grdvDocumentResults.DataSource = ViewState(CNDocumentListCollection)
            grdvDocumentResults.DataBind()
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
        ''' This event is fired on Row Data Bound of Grid View
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub grdvDocumentResults_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdvDocumentResults.RowDataBound
            If e.Row.RowType = DataControlRowType.DataRow Then
                'NOTE - this will need to be changed to give each row a unique id
                'this needs to be matched in markup for the menu (id="Menu_<%# Eval("DocNum") %>")
                e.Row.Attributes.Add("id", CType(e.Row.DataItem, NexusProvider.DocumentList).DocNum)

                If e.Row.FindControl("hypDocDescription") IsNot Nothing Then
                    If (CType(e.Row.DataItem, NexusProvider.DocumentList).DocumentType.ToString() = "PDF") Then
                        Dim sUrl As String = ""
                        sUrl = AppSettings("WebRoot") & "/secure/document.aspx?modal=true&KeepThis=true&TB_iframe=true&height=500&width=750"
                        CType(e.Row.FindControl("hypDocDescription"), LinkButton).CommandArgument = sUrl & "|" & CType(e.Row.DataItem, NexusProvider.DocumentList).DocNum.ToString() & "|" & CType(e.Row.DataItem, NexusProvider.DocumentList).DocumentType.ToString()
                    Else
                        CType(e.Row.FindControl("hypDocDescription"), LinkButton).CommandArgument = "~/secure/document.aspx?filename=" & Trim(CType(e.Row.DataItem, NexusProvider.DocumentList).DocDescription) & "&doctype=" & CType(e.Row.DataItem, NexusProvider.DocumentList).DocumentType.ToString() & "|" & CType(e.Row.DataItem, NexusProvider.DocumentList).DocNum.ToString() & "|" & CType(e.Row.DataItem, NexusProvider.DocumentList).DocumentType.ToString()
                    End If
                    CType(e.Row.FindControl("hypDocDescription"), LinkButton).Text = CType(e.Row.DataItem, NexusProvider.DocumentList).DocDescription
                    If oFileTypes.FileType(UCase(CType(e.Row.DataItem, NexusProvider.DocumentList).DocumentType.ToString)).DocType IsNot Nothing Then
                        CType(e.Row.FindControl("hypDocDescription"), LinkButton).CssClass = oFileTypes.FileType(UCase(CType(e.Row.DataItem, NexusProvider.DocumentList).DocumentType.ToString)).CssClass
                    End If
                End If

                'SAM Return DocumentType Label will match Web.Config DocumentType, Selected FileType 'display' tag  used for DocumentType Label
                If e.Row.FindControl("lblDocumentType") IsNot Nothing Then
                    'If folder Collection is displayed 
                    If String.IsNullOrEmpty(CType(e.Row.DataItem, NexusProvider.DocumentList).DocDescription) = True Then
                        'The “Type” column contain the word “Folder” from ResourceObject
                        CType(e.Row.FindControl("lblDocumentType"), Label).Text = GetLocalResourceObject("lbl_FolderDocumentType")
                    Else
                        'If Document Collection is displayed 

                        If oFileTypes.FileType(UCase(CType(e.Row.DataItem, NexusProvider.DocumentList).DocumentType.ToString)).DocType IsNot Nothing Then
                            '“DocType” which will match an entry in the FileType of Selected “DocType”
                            CType(e.Row.FindControl("lblDocumentType"), Label).Text = oFileTypes.FileType(UCase(CType(e.Row.DataItem, NexusProvider.DocumentList).DocumentType.ToString)).Display
                        End If
                       
                    End If

                End If

                'This hyperlinks which open the DocumentManager Page , passing the FolderPath of DocumentCollection
                If e.Row.FindControl("hypFolderPath") IsNot Nothing Then
                    CType(e.Row.FindControl("hypFolderPath"), HyperLink).NavigateUrl = "~/Secure/DocumentManager.aspx?path=" & CType(e.Row.DataItem, NexusProvider.DocumentList).FolderPath
                    CType(e.Row.FindControl("hypFolderPath"), HyperLink).Text = CType(e.Row.DataItem, NexusProvider.DocumentList).FolderPath
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
        ''' This is fired on clicking on the column heading of the Grid View
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub grdvDocumentResults_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles grdvDocumentResults.Sorting
            'sort the Quote & Policy according to the column clicked
            'we need to store the current sort order in viewstate, and reverse it each time
            Dim oDocumentListCollection As NexusProvider.DocumentListCollection = ViewState(CNDocumentListCollection)
            oDocumentListCollection.SortColumn = e.SortExpression
            'check that the sort expression is the same as stored in viewstate as we should start again if reordering by a new column
            Dim _sortDirection As New SortDirection
            If ViewState("SortDirection") = SortDirection.Ascending And ViewState("SortExpression") = e.SortExpression Then
                _sortDirection = SortDirection.Descending
            Else
                _sortDirection = SortDirection.Ascending
            End If
            'store the current sortdirection for comparison on the next sort
            ViewState("SortDirection") = _sortDirection
            'store the SortExpression in viewstate so that we can check if we are sorting by a new column on the next sort
            ViewState("SortExpression") = e.SortExpression
            oDocumentListCollection.SortingOrder = _sortDirection
            oDocumentListCollection.Sort()
            CType(sender, GridView).DataSource = oDocumentListCollection
            CType(sender, GridView).DataBind()
        End Sub

        Protected Sub Page_Load1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            'Clear the session if not then it will load left bar controls (cliamInfo)
            Session(CNMode) = Nothing
        End Sub
    End Class
End Namespace