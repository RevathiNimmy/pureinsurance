Imports System.Web.Configuration.WebConfigurationManager
Imports System.Web.Configuration
Imports Nexus.Library
Imports CMS.Library
Imports Nexus.Utils
Imports System.Web.HttpContext
Imports Nexus.Constants
Imports Nexus.Constants.Session
Imports NexusProvider
Namespace Nexus
    Partial Class Modal_SelectFolders
        Inherits System.Web.UI.Page

        Public Const CNFolderListCollection As String = "FolderListCollection"

        ''' <summary>
        ''' Obtains the search result from database and populates the datagrid
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
            If Page.IsValid Then
                GetSubFolderList()
            End If
        End Sub
        Sub GetSubFolderList()
            Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oDMESearchCriteria As New NexusProvider.DMESearchCriteria
            Dim oDME As NexusProvider.DME

            'Initializing the values
            oDMESearchCriteria.FolderName = Trim(txtFolderName.Text)

            If (Request.QueryString("FolderNum") IsNot Nothing) Then
                oDMESearchCriteria.ParentNum = Request.QueryString("FolderNum")
            End If

            oDMESearchCriteria.DocumentDescription = ""
            oDMESearchCriteria.IncludeFiles = False

            ''Sam Call with Search criteria
            oDME = oWebservice.FindDMEDocuments(oDMESearchCriteria)

            'The results grid will show folders.
            Dim oTempListOfDME As New NexusProvider.DME
            Dim oFolderList As New NexusProvider.SubFolder
            If oDME.SubFolder IsNot Nothing Then
                For iCount As Integer = 0 To oDME.SubFolder.Count - 1
                    If grdvSearchClients.PageSize >= iCount Then
                        oFolderList = New NexusProvider.SubFolder
                        oFolderList.CreateDate = oDME.SubFolder(iCount).CreateDate
                        oFolderList.Name = oDME.SubFolder(iCount).Name
                        oFolderList.FolderNum = oDME.SubFolder(iCount).FolderNum
                        oTempListOfDME.SubFolder.Add(oFolderList)
                    End If
                Next
                'Show Apply Button
                If oDME.SubFolder.Count > 0 Then
                    btnApply.Visible = True
                End If
            End If

            'store the data in ViewState to use again for page indexing
            ViewState.Add(CNFolderListCollection, oTempListOfDME.SubFolder)

            'Populating the session with search results
            grdvSearchClients.Visible = True
            grdvSearchClients.DataSource = ViewState(CNFolderListCollection)
            grdvSearchClients.DataBind()
        End Sub
        Protected Sub btnApply_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnApply.Click
            Dim sSelectedRows As String = Nothing
            For Each GridViewRow In grdvSearchClients.Rows
                '  Dim arrayRows As ArrayList = New ArrayList()
                Dim cb As CheckBox = CType(GridViewRow.FindControl("cbSelect"), CheckBox)
                Dim lblFolderNum As Label = CType(GridViewRow.FindControl("lblFolderNum"), Label)
                If cb IsNot Nothing And lblFolderNum IsNot Nothing Then
                    If cb.Checked Then
                        sSelectedRows = sSelectedRows & Server.HtmlDecode(DirectCast(DirectCast(GridViewRow, System.Web.UI.WebControls.GridViewRow).Cells(1), System.Web.UI.WebControls.TableCell).Text) & ":"
                        sSelectedRows = sSelectedRows & lblFolderNum.Text & ";"
                    End If
                End If
            Next

            If (String.IsNullOrEmpty(Request.QueryString("SelectBranchDetail")) = False Or String.IsNullOrEmpty(sSelectedRows) = False) Then
                Page.ClientScript.RegisterStartupScript(GetType(String), "SetFolders", "self.parent.SetFolders('" & sSelectedRows & "','" & Request.QueryString("SelectBranchDetail") & "');", True)
            End If
        End Sub
        ''' <summary>
        ''' This event is fired on Row Data Bound of Grid View
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub grdvSearchClients_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdvSearchClients.RowDataBound
            If e.Row.RowType = DataControlRowType.DataRow Then
                If e.Row.FindControl("lblFolderNum") IsNot Nothing Then
                    If String.IsNullOrEmpty(CType(e.Row.DataItem, NexusProvider.SubFolder).FolderNum) = False Then
                        CType(e.Row.FindControl("lblFolderNum"), Label).Text = CType(e.Row.DataItem, NexusProvider.SubFolder).FolderNum
                    End If
                End If
            ElseIf e.Row.RowType = DataControlRowType.Header Then

            End If
        End Sub
        ''' <summary>
        ''' This event is fired on Page load
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            'Display Selected Branch Name
            If (Request.QueryString("SelectBranchDetail") IsNot Nothing) Then
                If Request.QueryString("SelectBranchDetail").Split(":").Length > 0 Then
                    txtCompany.Text = Request.QueryString("SelectBranchDetail").Split(":")(1)
                End If
            End If
        End Sub

        Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
            CMS.Library.Frontend.Functions.SetTheme(Page, AppSettings("ModalPageTemplate"))
        End Sub
    End Class
End Namespace