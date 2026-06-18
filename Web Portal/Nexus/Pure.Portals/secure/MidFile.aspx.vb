Imports System.Data
Imports Nexus.Constants.Constant
Imports Nexus.Constants.Session
Imports CMS.Library
Namespace Nexus


    Partial Class secure_MidFile : Inherits Frontend.clsCMSPage
        Dim iErrorCount As Integer
        Dim iWarningCount As Integer
        ''' <summary>
        ''' This method set the variable and call the SAM method on Page_Load
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Overrides Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            If Not IsPostBack Then
                'Reset the count's value
                iErrorCount = 0
                iWarningCount = 0

                Dim MidFileDetails As Guid ' To store the data in cache
                MidFileDetails = Guid.NewGuid()
                ViewState.Add("MidFileDetails", MidFileDetails.ToString)
                Dim dtMidFileDetails As DataTable = ValidateCache()

                'To Display the  File Sequence Number
                If Request.QueryString("FileSequenceNo") IsNot Nothing Then
                    lblFileSequenceNo.Text = Request.QueryString("FileSequenceNo")
                End If

                'To Display the  File Created
                If Request.QueryString("DateGenerated") IsNot Nothing Then
                    lblFileCreated.Text = Request.QueryString("DateGenerated")
                End If

                'To Display the  File Name
                If Request.QueryString("FileName") IsNot Nothing Then
                    lblImportingFile.Text = Request.QueryString("FileName")
                End If

                'Associate the data table with grid
                gvGetMIDFileDetails.DataSource = dtMidFileDetails
                gvGetMIDFileDetails.DataBind()

                'Display Value on screen
                lblWarnings.Text = iWarningCount.ToString
                lblErrors.Text = iErrorCount.ToString

            End If
        End Sub
        ''' <summary>
        '''  This method fires when user change the Page Index
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub gvGetMIDFileDetails_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvGetMIDFileDetails.PageIndexChanging
            gvGetMIDFileDetails.PageIndex = e.NewPageIndex
            gvGetMIDFileDetails.DataSource = ValidateCache()
            gvGetMIDFileDetails.DataBind()

        End Sub
        ''' <summary>
        ''' This execute or set the parameter on respective option selected by user
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub gvGetMIDFileDetails_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvGetMIDFileDetails.RowCommand
            'user should redirect to the client details page
            If e.CommandName = "Details" Then
                Dim sInsuranceFileKey As String = e.CommandArgument
                Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                Dim oQuote As NexusProvider.Quote
                Dim oBaseParty As NexusProvider.BaseParty

                'Based on Insurance File Key Party Key would be retreived using the SAM Methods
                If String.IsNullOrEmpty(sInsuranceFileKey) = False Then
                    oQuote = oWebService.GetHeaderAndSummariesByKey(CInt(sInsuranceFileKey))
                    oBaseParty = oWebService.GetParty(oQuote.PartyKey)

                    'Updating the session Variable to display the Client Screen
                    Session.Item(CNParty) = oBaseParty
                    Session(CNClientMode) = Mode.View

                    'Matching of Type of the Party
                    Select Case True
                        Case TypeOf oBaseParty Is NexusProvider.CorporateParty
                            With CType(oBaseParty, NexusProvider.CorporateParty)

                                Response.Redirect("~/secure/agent/CorporateClientDetails.aspx?PartyKey=" & .Key & "&Code=" & .ClientSharedData.ShortName.Trim(), False)

                            End With
                        Case TypeOf oBaseParty Is NexusProvider.PersonalParty
                            With CType(oBaseParty, NexusProvider.PersonalParty)

                                Response.Redirect("~/secure/agent/PersonalClientDetails.aspx?PartyKey=" & .Key & "&Code=" & .ClientSharedData.ShortName.Trim(), False)

                            End With
                    End Select

                End If
            End If
        End Sub
        ''' <summary>
        ''' This Method set the variaous parameter on when records are added into grid
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub gvGetMIDFileDetails_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvGetMIDFileDetails.RowDataBound
            If e.Row.RowType = DataControlRowType.DataRow Then
                'Population of the Command Argument, placing the InsuranceFileCnt into it for later use
                Dim lnkDetails As LinkButton = e.Row.FindControl("lnkDetails")
                Dim dtMidDetails As DataTable = ValidateCache()
                lnkDetails.CommandArgument = dtMidDetails.Rows(e.Row.RowIndex)("InsuranceFileCnt")
                lnkDetails.CommandName = "Details"
            End If
        End Sub
        ''' <summary>
        ''' This method sort the grid's record on user's choice
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub gvGetMIDFileDetails_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvGetMIDFileDetails.Sorting
            Dim dtMidDetails As DataTable = ValidateCache()
            Dim sSort As String = Nothing

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

            If _sortDirection = SortDirection.Ascending Then
                sSort = "Asc"
            Else
                sSort = "Desc"
            End If

            'Soring the records usinf dataview
            Dim dvMidDetails As New DataView(dtMidDetails, "", e.SortExpression & " " & sSort, DataViewRowState.CurrentRows)

            gvGetMIDFileDetails.DataSource = dvMidDetails
            gvGetMIDFileDetails.DataBind()
        End Sub
        ''' <summary>
        ''' This Method count the Warning and Error
        ''' </summary>
        ''' <param name="sCode"></param>
        ''' <remarks></remarks>
        Private Sub CountWarningAndError(ByVal sCode As String)
            'Count the error and warning to show on the screen
            If sCode IsNot Nothing Then
                If sCode.Substring(0, 1).ToUpper = "E" Then
                    iErrorCount += 1
                ElseIf sCode.Substring(0, 1).ToUpper = "W" Then
                    iWarningCount += 1
                End If
            End If
        End Sub
        ''' <summary>
        ''' This method will validate the cache and will return the collection
        ''' </summary>
        ''' <returns>dtMidFileDetails(DataTable)</returns>
        ''' <remarks></remarks>
        Protected Function ValidateCache() As DataTable
            'try to get the search results from the cache
            Dim dtMidFileDetails As DataTable = CType(Cache.Item(ViewState("MidFileDetails")), DataTable)
            Dim oMidFileDetails As NexusProvider.MidFileDetails
            Dim drMidFileDetail As DataRow
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim iMidFileKey As Integer = 0

            If dtMidFileDetails Is Nothing Then

                'Initialize the memory to add the columns
                dtMidFileDetails = New DataTable

                'Column Creation in Data Table
                dtMidFileDetails.Columns.Add(New DataColumn("RecordType"))
                dtMidFileDetails.Columns.Add(New DataColumn("Policy"))
                dtMidFileDetails.Columns.Add(New DataColumn("PPPC"))
                dtMidFileDetails.Columns.Add(New DataColumn("InsuranceFileCnt"))
                dtMidFileDetails.Columns.Add(New DataColumn("ExpectedPPPC"))
                dtMidFileDetails.Columns.Add(New DataColumn("Registration"))
                dtMidFileDetails.Columns.Add(New DataColumn("Errors"))
                'To Read the MIDFileKey from Request Query string
                If Request.QueryString("MIDFileKey") IsNot Nothing Then
                    iMidFileKey = CInt(Request.QueryString("MIDFileKey"))
                    oMidFileDetails = oWebService.GetMidFileDetails(True, iMidFileKey)

                    'If Result is availabe then only do the rest of the process
                    'If policy records are available
                    If oMidFileDetails IsNot Nothing And (oMidFileDetails.Policies IsNot Nothing AndAlso oMidFileDetails.Policies.Count > 0) Then

                        For iPolicyRowCount As Integer = 0 To oMidFileDetails.Policies.Count - 1
                            'Check Whether policy has any errors
                            If String.IsNullOrEmpty(oMidFileDetails.Policies(iPolicyRowCount).RejectErrorCodes) = False Then
                                'If errors are found then retreive the description based on code

                                Dim iStartIndex As Integer = 0
                                Dim iTotalLength As Integer = 0
                                'Retreive the description based on valid code
                                While iTotalLength < oMidFileDetails.Policies(iPolicyRowCount).RejectErrorCodes.Length
                                    Dim sCode, sDescription As String
                                    sCode = oMidFileDetails.Policies(iPolicyRowCount).RejectErrorCodes.Substring(iStartIndex, 4)
                                    'Retreive the description based on valid code
                                    If String.IsNullOrEmpty(sCode) = False Then
                                        iTotalLength += sCode.Length
                                        iStartIndex += sCode.Length
                                        sDescription = GetDescriptionForCode(NexusProvider.ListType.PMLookup, sCode, "UDL_MidCodes")
                                        If String.IsNullOrEmpty(sDescription) = False Then
                                            CountWarningAndError(sCode)

                                            'Add the record into Data Table
                                            drMidFileDetail = dtMidFileDetails.NewRow

                                            drMidFileDetail("RecordType") = "F" 'Policy Record
                                            drMidFileDetail("Policy") = oMidFileDetails.Policies(iPolicyRowCount).InsuranceFileRef
                                            drMidFileDetail("PPPC") = oMidFileDetails.Policies(iPolicyRowCount).PPPC
                                            drMidFileDetail("InsuranceFileCnt") = oMidFileDetails.Policies(iPolicyRowCount).InsuranceFileKey
                                            drMidFileDetail("ExpectedPPPC") = oMidFileDetails.Policies(iPolicyRowCount).ExpectedPPPC
                                            drMidFileDetail("Registration") = String.Empty
                                            drMidFileDetail("Errors") = sCode & ":  " & sDescription

                                            dtMidFileDetails.Rows.Add(drMidFileDetail)

                                        End If
                                    End If
                                End While
                            End If

                            'If Result is availabe then only do the rest of the process
                            'If Vehicle records are available
                            If oMidFileDetails.Policies(iPolicyRowCount).Vehicles IsNot Nothing AndAlso oMidFileDetails.Policies(iPolicyRowCount).Vehicles.Count > 0 Then

                                For iVehicleRowCount As Integer = 0 To oMidFileDetails.Policies(iPolicyRowCount).Vehicles.Count - 1

                                    If String.IsNullOrEmpty(oMidFileDetails.Policies(iPolicyRowCount).Vehicles(iVehicleRowCount).RejectErrorCodes) = False Then
                                        'If errors are found then retreive the description based on code

                                        Dim iStartIndex As Integer = 0
                                        Dim iTotalLength As Integer = 0
                                        'Retreive the description based on valid code
                                        While oMidFileDetails.Policies(iPolicyRowCount).Vehicles(iVehicleRowCount).RejectErrorCodes.Length > iTotalLength
                                            Dim sCode, sDescription As String
                                            sCode = oMidFileDetails.Policies(iPolicyRowCount).Vehicles(iVehicleRowCount).RejectErrorCodes.Substring(iStartIndex, 4)
                                            'Retreive the description based on valid code
                                            If String.IsNullOrEmpty(sCode) = False Then
                                                iTotalLength += sCode.Length
                                                iStartIndex += sCode.Length
                                                sDescription = GetDescriptionForCode(NexusProvider.ListType.PMLookup, sCode, "UDL_MidCodes")
                                                If String.IsNullOrEmpty(sDescription) = False Then
                                                    CountWarningAndError(sCode)

                                                    'Add the record into Data Table
                                                    drMidFileDetail = dtMidFileDetails.NewRow

                                                    drMidFileDetail("RecordType") = "V" 'Vehicle Record
                                                    drMidFileDetail("Policy") = oMidFileDetails.Policies(iPolicyRowCount).InsuranceFileRef
                                                    drMidFileDetail("PPPC") = oMidFileDetails.Policies(iPolicyRowCount).PPPC
                                                    drMidFileDetail("InsuranceFileCnt") = oMidFileDetails.Policies(iPolicyRowCount).InsuranceFileKey
                                                    drMidFileDetail("ExpectedPPPC") = oMidFileDetails.Policies(iPolicyRowCount).ExpectedPPPC
                                                    drMidFileDetail("Registration") = oMidFileDetails.Policies(iPolicyRowCount).Vehicles(iVehicleRowCount).Registration
                                                    drMidFileDetail("Errors") = sCode & ":  " & sDescription

                                                    dtMidFileDetails.Rows.Add(drMidFileDetail)

                                                End If
                                            End If
                                        End While
                                    End If
                                Next
                            End If
                        Next
                    End If
                End If

                'value inserted into cache
                Cache.Insert(ViewState("MidFileDetails"), dtMidFileDetails, Nothing, DateTime.MaxValue, TimeSpan.FromMinutes(5))

            End If

            Return dtMidFileDetails
        End Function
    End Class
End Namespace
