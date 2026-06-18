Imports System.Web.Configuration
Imports Nexus.Library
Imports CMS.Library
Imports Nexus.Utils
Imports System.Web.HttpContext
Imports Nexus.Constants.Constant
Imports Nexus.Constants.Session
Imports System.Collections.Generic
Imports System.Data
Imports System.IO
Imports System.Threading

Namespace Nexus
    Partial Class secure_SystemEvents : Inherits CMS.Library.Frontend.clsCMSPage
        Private v_iModuleKey As Integer


        Public Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)
        End Sub

#Region "Page Load"
        ''' <summary>
        ''' In Page load fill the grid with the even list.
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub Page_Load1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            If Not IsPostBack AndAlso Me.Visible = True Then
                'Populate the EventList 
                Session.Remove(CNPolicyNumber)
                txtEventFromDate.Text = DateAdd("d", -7, Today.Date)
                txtEventToDate.Text = Today.Date

                FillModule()

                'Populate the User
                FillUser()
                ' Request.Form("__EVENTTARGET") = "btnExport"

            End If
        End Sub
#End Region
#Region "Controls's Fill Methods"

        Sub FillUser()
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oUser As NexusProvider.AuditTrailCollection = Nothing
            Dim oEventParams As New NexusProvider.AuditTrail
            'Fill the user dropdownlist.
            oUser = oWebService.GetAudittrailUser(oEventParams)
            ddlUserName.DataSource = oUser
            ddlUserName.DataTextField = "UserName"
            ddlUserName.DataValueField = "UserId"
            ddlUserName.DataBind()
            ddlUserName.Items.Insert(0, New ListItem("(All)", 0))
            ddlUserName.SelectedIndex = 0
        End Sub
        Sub FillModule()
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oModule As NexusProvider.AuditTrailCollection = Nothing
            Dim oEventParams As New NexusProvider.AuditTrail
            'Fill the user dropdownlist.
            oModule = oWebService.GetAuditTrailModule(oEventParams)
            ddlEventType.DataSource = oModule
            ddlEventType.DataTextField = "ModuleName"
            ddlEventType.DataValueField = "ModuleId"
            ddlEventType.DataBind()
            ddlEventType.Items.Insert(0, New ListItem("(All)", 0))
            ddlEventType.SelectedIndex = 0
        End Sub
        Protected Sub PopulateEventListGrid(Optional ByVal bUserSelection As Boolean = True)
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oEventParams As New NexusProvider.AuditTrail
            Dim oEventList As New NexusProvider.AuditTrailCollection

            If txtEventFromDate.Text.Trim.Length <> 0 Then
                oEventParams.EventFromDate = CDate(txtEventFromDate.Text.Trim)
            End If

            If txtEventToDate.Text.Trim.Length <> 0 Then
                oEventParams.EventToDate = CDate(txtEventToDate.Text.Trim)
            End If

            If ddlEventType.SelectedValue IsNot Nothing Then
                If ddlEventType.SelectedValue <> 0 Then
                    oEventParams.ModuleKey = ddlEventType.SelectedValue
                    v_iModuleKey = ddlEventType.SelectedValue
                End If
            End If

            If bUserSelection = True AndAlso ddlUserName.SelectedValue <> "" Then
                If ddlUserName.SelectedValue <> 0 Then
                    oEventParams.UserId = ddlUserName.SelectedValue
                End If
            End If


            EventSearchCriteria(oEventParams)
            Try
                oEventList = oWebService.GetAuditTrails(oEventParams)
            Catch ex As Exception
                ex.ToString()
            End Try

            If oEventList IsNot Nothing AndAlso oEventList.Count > 0 Then
                Session(CNAuditTrail) = oEventList
                'Bind the Grid with the EvenList Collection object.
                gvEventList.AllowPaging = True
                gvEventList.AllowSorting = True
                gvEventList.DataSource = oEventList
                gvEventList.DataBind()
                gvEventList.Visible = True
                btnExport.Visible = True
            Else
                gvEventList.Visible = True
                gvEventList.DataSource = Nothing
                gvEventList.DataBind()
                gvEventList.AllowPaging = False
                btnExport.Visible = False
            End If

            oWebService = Nothing
            oEventParams = Nothing
            oEventList = Nothing

        End Sub

#End Region
#Region "Public Properties"
        ''' <summary>
        ''' Build Event Search Criteria.
        ''' </summary>
        ''' <param name="v_oEvent"></param>
        ''' <remarks></remarks>
        Private Sub EventSearchCriteria(ByRef v_oEvent As NexusProvider.AuditTrail)
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            With (v_oEvent)
                .ModuleKey = v_oEvent.ModuleKey
                .ModuleKeySpecified = True
                .UserKeySpecified = False
                .DateToSpecified = False
                .FromDateSpecified = False
                If ddlUserName.SelectedValue <> 0 Then
                    .UserKey = Convert.ToInt32(ddlUserName.SelectedValue)
                    .UserKeySpecified = True
                Else
                    .UserKeySpecified = False
                End If

                If (txtEventFromDate.Text <> String.Empty) Then
                    .FromDate = CDate(txtEventFromDate.Text)
                    .FromDateSpecified = True
                Else
                    .FromDateSpecified = False
                End If

                If (txtEventToDate.Text <> String.Empty) Then
                    .DateTo = CDate(txtEventToDate.Text)
                    .DateToSpecified = True
                Else
                    .DateToSpecified = False
                End If
            End With
        End Sub
#End Region
#Region "Button Click"
        Protected Sub btnNewSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNewSearch.Click
            txtEventFromDate.Text = DateAdd("d", -7, Today.Date)
            txtEventToDate.Text = Today.Date
            ddlEventType.SelectedIndex = 0
            ddlUserName.SelectedIndex = 0
            gvEventList.DataSource = Nothing
            gvEventList.DataBind()
            gvEventList.AllowPaging = False
            gvEventList.Visible = False
            Session(CNAuditTrail) = Nothing
            btnExport.Visible = False
        End Sub

        Protected Sub btnRefresh_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRefresh.Click
            PopulateEventListGrid(True)
        End Sub
        Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExport.Click
            Try
                Response.Clear()
                gvEventList.PageSize = 1000
                gvEventList.AllowSorting = False
                gvEventList.AllowPaging = False
                Dim oEventCollection As NexusProvider.AuditTrailCollection = Session(CNAuditTrail)
                gvEventList.DataSource = oEventCollection
                gvEventList.DataBind()
                Response.ClearContent()
                Response.ClearHeaders()
                Response.Buffer = True
                Dim filename As String = ddlEventType.SelectedItem.ToString() + "_" + txtEventFromDate.Text + "_" + txtEventToDate.Text + ".xls"
                Response.AddHeader("content-disposition", String.Format("attachment; filename={0}", filename))
                Response.ContentEncoding = Encoding.UTF8
                Response.ContentType = "application/ms-excel"
                Dim sw As New StringWriter()
                Dim htw As New HtmlTextWriter(sw)
                gvEventList.RenderControl(htw)
                Response.Write(sw.ToString())
                ' sw.Close()
                Response.Flush()
                Response.End()


            Catch ex As Exception
            Finally

            End Try
        End Sub
#End Region
#Region "Grid Events "

        ''' <summary>
        ''' For sorting events grid
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub gvEventList_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvEventList.Sorting
            'sort the Quote & Policy according to the column clicked
            'we need to store the current sort order in viewstate, and reverse it each time
            Dim oEventCollection As NexusProvider.AuditTrailCollection = Session(CNAuditTrail)
            oEventCollection.SortColumn = e.SortExpression
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
            oEventCollection.SortingOrder = _sortDirection
            oEventCollection.Sort()
            gvEventList.DataSource = oEventCollection
            gvEventList.DataBind()
        End Sub

        ''' <summary>
        ''' Page navigation in the Gridview.
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub gvEventList_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvEventList.PageIndexChanging

            gvEventList.PageIndex = e.NewPageIndex
            gvEventList.DataSource = Session(CNAuditTrail)
            gvEventList.DataBind()
        End Sub


#End Region

    End Class

End Namespace