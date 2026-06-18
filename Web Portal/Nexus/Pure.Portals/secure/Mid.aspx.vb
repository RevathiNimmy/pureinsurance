Imports CMS.Library
Imports Nexus.Constants.Session
Namespace Nexus

    Partial Class secure_Mid : Inherits Frontend.clsCMSPage
        ''' <summary>
        ''' This method fires when user change the Page Index
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub gvGetMIDFile_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvGetMIDFile.PageIndexChanging
            'Retreiving the values from cache
            gvGetMIDFile.PageIndex = e.NewPageIndex
            'Grid binding with source
            gvGetMIDFile.DataSource = ValidateCache()
            gvGetMIDFile.DataBind()
        End Sub
        ''' <summary>
        ''' This Method set the variaous parameter on when records are added into grid
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub gvGetMIDFile_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvGetMIDFile.RowDataBound
            If e.Row.RowType = DataControlRowType.DataRow Then
                Dim oLnkDetails As HyperLink = e.Row.FindControl("lnkDetails")
                oLnkDetails.NavigateUrl = "~/secure/MidFile.aspx?DateGenerated=" & CType(e.Row.DataItem, NexusProvider.MidFile).DateGenerated.ToShortDateString & "&FileSequenceNo=" & CType(e.Row.DataItem, NexusProvider.MidFile).FileSequenceNumber & "&MIDFileKey=" & CType(e.Row.DataItem, NexusProvider.MidFile).MIDFileKey & "&FileName=" & CType(e.Row.DataItem, NexusProvider.MidFile).FileName
                'Each different status need to be depicted in different color
                If CType(e.Row.DataItem, NexusProvider.MidFile).StatusDescription IsNot Nothing Then

                    Select Case CType(e.Row.DataItem, NexusProvider.MidFile).StatusDescription.Trim.ToUpper
                        Case "GENERATED"
                            e.Row.Cells(2).CssClass = "AspNet-GridView-StatusGenerated"
                        Case "RECEIVED"
                            e.Row.Cells(2).CssClass = "AspNet-GridView-StatusReceived"
                        Case "LOADED"
                            e.Row.Cells(2).CssClass = "AspNet-GridView-StatusLoaded"
                        Case "REJECTED"
                            e.Row.Cells(2).CssClass = "AspNet-GridView-StatusRejected"
                            'Details links are need to be displayed only when status is error or rejected
                            oLnkDetails.Visible = True
                        Case "ERROR"
                            e.Row.Cells(2).CssClass = "AspNet-GridView-StatusError"
                            'Details links are need to be displayed only when status is error or rejected
                            oLnkDetails.Visible = True
                    End Select
                End If
            End If
        End Sub
        ''' <summary>
        ''' This method set the variable and call the SAM method on Page_Load
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Overrides Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            If Not IsPostBack Then
                'Cleaning of the session values
                ClearQuote()
                ClearClaims()
                ClearHeader()
                Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                Dim oMidFileColl As NexusProvider.MidFileCollection
                Dim MidFileColl As Guid ' To store the data in cache
                MidFileColl = Guid.NewGuid()
                ViewState.Add("MidFileColl", MidFileColl.ToString)

                'Retreive the value from SAM if PostBack is set to false
                oMidFileColl = ValidateCache()

                'Grid binding with source
                gvGetMIDFile.DataSource = oMidFileColl
                gvGetMIDFile.DataBind()

                'Dispose the variable
                oWebService = Nothing
                oMidFileColl = Nothing
                MidFileColl = Nothing
            End If
        End Sub
        ''' <summary>
        ''' This Method does the sorting if user presses any field from Grid's Header
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub gvGetMIDFile_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvGetMIDFile.Sorting
            'sort the work manager entries according to the column clicked

            Dim oMidFileCollection As NexusProvider.MidFileCollection = ValidateCache()
            oMidFileCollection.SortColumn = e.SortExpression

            'we need to store the current sort order in viewstate, and reverse it each time
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
            oMidFileCollection.SortingOrder = _sortDirection
            oMidFileCollection.Sort()

            CType(sender, GridView).DataSource = oMidFileCollection
            CType(sender, GridView).DataBind()

            'Dispose the variable
            oMidFileCollection = Nothing

        End Sub
        ''' <summary>
        ''' This method will validate the cache and will return the collection
        ''' </summary>
        ''' <returns>oMidFileCollection(NexusProvider.MidFileCollection)</returns>
        ''' <remarks></remarks>
        Protected Function ValidateCache() As NexusProvider.MidFileCollection
            'try to get the search results from the cache
            Dim oMidFileCollection As NexusProvider.MidFileCollection = CType(Cache.Item(ViewState("MidFileColl")), NexusProvider.MidFileCollection)
            If oMidFileCollection Is Nothing Then
                Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                Dim dtStart_Date, dtEnd_Date As Date

                'As per the PS we need to show the history of last 30 days only
                dtStart_Date = Now.AddDays(-30)
                dtEnd_Date = Now
                oMidFileCollection = oWebService.GetMidFiles(dtStart_Date, dtEnd_Date, False, 0)

                'value inserted into cache
                Cache.Insert(ViewState("MidFileColl"), oMidFileCollection, Nothing, DateTime.MaxValue, TimeSpan.FromMinutes(5))

                'Dispose the variable
                oWebService = Nothing
            End If
         
            Return oMidFileCollection
        End Function
    End Class
End Namespace