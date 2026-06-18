Imports System.Web.Configuration.WebConfigurationManager
Imports Nexus.Utils
Imports Nexus.Library
Imports CMS.Library
Imports System
Imports System.Data
Imports Nexus.Constants
Imports Nexus.Constants.Session
Imports Nexus.Web.UI.WebControls
Imports System.Collections.Generic
Imports System.Reflection



Namespace Nexus
    Partial Class secure_Controls_Events : Inherits System.Web.UI.UserControl
        Private v_iModuleKey As Integer
        Dim Ds As DataSet

#Region "Page Load"
        ''' <summary>
        ''' In Page load fill the grid with the even list.
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            If Not IsPostBack AndAlso Me.Visible = True Then
                'Populate the EventList 
                Session.Remove(CNPolicyNumber)
                txtEventFromDate.Text = DateAdd("d", -365, Today.Date)
                txtEventToDate.Text = Today.Date

                FillModule()

                'Populate the User
                FillUser()


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
            ddlUserName.Items.Insert(0, New ListItem("(all)", 0))
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
            ddlEventType.Items.Insert(0, New ListItem("(Please select)", ""))
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
                If ddlUserName.SelectedValue > 0 Then
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
                btnExport.Visible = True
                'Bind the Grid with the EvenList Collection object.
                gvEventList.AllowPaging = True
                gvEventList.AllowSorting = True
                gvEventList.DataSource = oEventList
                gvEventList.DataBind()
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
                If ddlUserName.SelectedValue > 0 Then
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
            txtEventFromDate.Text = String.Empty
            txtEventToDate.Text = String.Empty
            ddlEventType.SelectedIndex = 0
            ddlUserName.SelectedIndex = 0
            gvEventList.Visible = False
            gvEventList.DataSource = Nothing
            gvEventList.DataBind()
            Session(CNAuditTrail) = Nothing
        End Sub

        Protected Sub btnRefresh_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRefresh.Click
            PopulateEventListGrid(True)
        End Sub
        Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExport.Click


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

