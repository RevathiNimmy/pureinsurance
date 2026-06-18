Imports Nexus.Library
Imports CMS.Library
Imports System.Data
Imports System.Web.Configuration.WebConfigurationManager
Imports Nexus.Utils
Imports Nexus.Constants
Imports Nexus.Constants.Session

Namespace Nexus

    Partial Class Controls_ClientEvents : Inherits System.Web.UI.UserControl

        Private iPartyKey As Integer

        Public Property PartyKey() As Integer
            Set(ByVal value As Integer)

                iPartyKey = value
                Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                Dim oEventDetailsCollection As New NexusProvider.EventDetailsCollection
                Dim oEventDetails As New NexusProvider.EventDetails

                Try
                    oEventDetails.PartyKey = value
                    oEventDetailsCollection = oWebService.GetEventDetails(oEventDetails)
                    'put in the session to use it on another page
                    Session.Item(CNEvent) = oEventDetailsCollection

                    grdvEvents.DataSource = oEventDetailsCollection
                    If Session("PageIndex") Is Nothing Then
                        Session("PageIndex") = 0
                    End If
                    grdvEvents.PageIndex = Convert.ToInt32(Session("PageIndex"))
                    grdvEvents.DataBind()
                    Session("PageIndex") = 0
                Finally
                    oWebService = Nothing
                    oEventDetailsCollection = Nothing
                    oEventDetails = Nothing
                End Try
            End Set
            Get
                Return iPartyKey
            End Get
        End Property
        ''' <summary>
        ''' As per system option, make CaseNumber column visible false.
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub grdvEvents_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles grdvEvents.DataBound
            If CType(ViewState("bDisplayCaseOption"), Boolean) = True Then
                grdvEvents.Columns(3).Visible = True
            Else
                grdvEvents.Columns(3).Visible = False
            End If
        End Sub
        Protected Sub grdvEvents_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles grdvEvents.Load
            If grdvEvents.PageCount = 1 Then
                grdvEvents.AllowPaging = False
            End If

        End Sub

        Protected Sub grdvEvents_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdvEvents.PageIndexChanging
            grdvEvents.PageIndex = e.NewPageIndex
            Session("PageIndex") = e.NewPageIndex
            Dim oEventDetailsCollection As NexusProvider.EventDetailsCollection = CType(Session.Item(CNEvent), NexusProvider.EventDetailsCollection)
            grdvEvents.DataSource = oEventDetailsCollection
            grdvEvents.PageIndex = e.NewPageIndex
            grdvEvents.DataBind()
        End Sub

        Protected Sub grdvEvents_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdvEvents.RowDataBound
            If e.Row.RowType = DataControlRowType.DataRow Then
                Dim RowIndex As Integer
                If (grdvEvents.PageIndex > 0) Then
                    RowIndex = grdvEvents.PageIndex * grdvEvents.PageSize + e.Row.RowIndex
                Else
                    RowIndex = e.Row.RowIndex
                End If
                Dim sUrl As String = ""
                
                If HttpContext.Current.Session.IsCookieless Then
                    sUrl = AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & "/Modal/EventDetails.aspx?EventDetailID=" & CType(e.Row.DataItem, NexusProvider.EventDetails).EventKey & "&modal=true&KeepThis=true&TB_iframe=true&height=500&width=750"
                Else
                    sUrl = AppSettings("WebRoot") & "Modal/EventDetails.aspx?EventDetailID=" & CType(e.Row.DataItem, NexusProvider.EventDetails).EventKey & "&modal=true&KeepThis=true&TB_iframe=true&height=500&width=750"
                End If

                Dim hypDetails As HyperLink = e.Row.FindControl("hypEventDetails")
                hypDetails.Attributes.Add("onClick", "tb_show( null,'" & sUrl & "' , null);return false;")
                hypDetails.CssClass = "thickbox"

                'NOTE - this will need to be changed to give each row a unique id
                'this needs to be matched in markup for the menu (id="Menu_<%# Eval("EventKey") %>")
                e.Row.Attributes.Add("id", CType(e.Row.DataItem, NexusProvider.EventDetails).EventKey)

                If e.Row.FindControl("lblEventDescription") IsNot Nothing Then
                    Dim oLabelDesc As Label = e.Row.FindControl("lblEventDescription")

                    If DataBinder.Eval(e.Row.DataItem, "EventDescription") = "" _
                    And DataBinder.Eval(e.Row.DataItem, "Description") = "" _
                    And DataBinder.Eval(e.Row.DataItem, "EventType") = "New Policy" Then
                        oLabelDesc.Text = "Added Policy " + DataBinder.Eval(e.Row.DataItem, "PolicyCode")
                    ElseIf Session(CNClaim) IsNot Nothing And String.IsNullOrEmpty(HttpContext.Current.Request.QueryString("ReturnUrl")) = False Then
                        If DataBinder.Eval(e.Row.DataItem, "EventType") = "Notes - Claims" Then
                            oLabelDesc.Text = DataBinder.Eval(e.Row.DataItem, "EventDescription")
                        End If
                    ElseIf DataBinder.Eval(e.Row.DataItem, "EventType") = "Notes - Customer" Then
                        oLabelDesc.Text = "Note:" & DataBinder.Eval(e.Row.DataItem, "EventDescription")
                    ElseIf DataBinder.Eval(e.Row.DataItem, "EventType") = "New Client" _
                    And DataBinder.Eval(e.Row.DataItem, "EventDescription") = "" _
                    And DataBinder.Eval(e.Row.DataItem, "Description") = "" Then
                        oLabelDesc.Text = "Client created"
                    End If
                End If

                'To show the status
                If CType(e.Row.DataItem, NexusProvider.EventDetails).EventType = "Notes - Customer Warning" _
                And String.IsNullOrEmpty(CType(e.Row.DataItem, NexusProvider.EventDetails).Priority) = False Then
                    If e.Row.FindControl("lblStatus") IsNot Nothing Then
                        Dim olblStatus As Label = e.Row.FindControl("lblStatus")
                        If CType(e.Row.DataItem, NexusProvider.EventDetails).StatusKey = 0 Then
                            olblStatus.Text = "Outstanding"
                        ElseIf CType(e.Row.DataItem, NexusProvider.EventDetails).StatusKey = 1 Then
                            olblStatus.Text = "Completed"
                        End If
                    End If
                End If
            End If
        End Sub
        ''' <summary>
        ''' Initializing control with system option.
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oOptionSettings As NexusProvider.OptionTypeSetting
            'If System Option for "Enhanced Case Search" is ON then we need to visible case related search criteria and grid column
            oOptionSettings = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, 5099)
            If oOptionSettings IsNot Nothing AndAlso Not String.IsNullOrEmpty(oOptionSettings.OptionValue) Then
                If oOptionSettings.OptionValue(0) <> "0" Then
                    ViewState("bDisplayCaseOption") = True
                Else
                    ViewState("bDisplayCaseOption") = False
                End If
            Else
                ViewState("bDisplayCaseOption") = False
            End If
        End Sub
        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        End Sub

        Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
            If Not IsPostBack AndAlso Me.Visible = True Then
                Session.Remove(CNEvent)
                Dim oEventDetailsCollection As NexusProvider.EventDetailsCollection = CType(Session.Item(CNEvent), NexusProvider.EventDetailsCollection)
                If oEventDetailsCollection Is Nothing Or (oEventDetailsCollection IsNot Nothing AndAlso oEventDetailsCollection.Count = 0) Then
                    If Session(CNParty) IsNot Nothing AndAlso Me.PartyKey = 0 AndAlso Session(CNClientMode) = Mode.View _
                    AndAlso CType(Session(CNParty), NexusProvider.BaseParty).Key <> 0 Then
                        Dim oParty As NexusProvider.BaseParty = Session(CNParty)
                        PartyKey = oParty.Key
                    End If
                End If
            End If
        End Sub
        ''' <summary>
        ''' For sorting events grid
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub grdvEvents_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles grdvEvents.Sorting
            'sort the Quote & Policy according to the column clicked
            'we need to store the current sort order in viewstate, and reverse it each time
            Dim oEventCollection As NexusProvider.EventDetailsCollection = Session(CNEvent)
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
            CType(sender, GridView).DataSource = oEventCollection
            CType(sender, GridView).DataBind()
        End Sub
    End Class

End Namespace
