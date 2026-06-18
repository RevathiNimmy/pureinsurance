Imports Microsoft.Web.Services3.Security.Tokens
Imports System.Data
Partial Class PolicyRenewal_wfrmRenewals
    Inherits System.Web.UI.Page
  Dim StartDate As Date


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        lblOutput.Text = String.Empty
        If (Session("StatusMessage") IsNot Nothing) Then
            lblOutput.Text = Session("StatusMessage").ToString
            Session("StatusMessage") = String.Empty
        End If
        If (Not IsPostBack) Then
            PopulateGrid()
            PopulateRenewalStatus()
            PopulateLaspseReasons()

        End If
    End Sub

    Protected Sub chkSelect_SelectChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        ProcessUIControls(MarkSelectedItems())
    End Sub
    Private Function MarkSelectedItems() As Integer
        Dim iSelectedIndices() As Integer = Nothing
        Dim iLength As Integer = 0
        For iCnt As Integer = 0 To gvRenewals.Rows.Count - 1
            Dim chkTemp As CheckBox = CType(gvRenewals.Rows(iCnt).FindControl("chkSelect"), CheckBox)
            If (chkTemp.Checked) Then
                gvRenewals.Rows(iCnt).BackColor = Drawing.Color.SlateBlue
                ReDim Preserve iSelectedIndices(iLength)
                iSelectedIndices(iLength) = iCnt
                iLength = iSelectedIndices.Length
            Else
                gvRenewals.Rows(iCnt).BackColor = Drawing.Color.White
            End If
        Next
        Session("SelectedIndices") = iSelectedIndices
        Session("SelectedCount") = iLength
        Return iLength
    End Function
    Private Sub ProcessUIControls(ByVal iCount As Integer)
        Select Case iCount
            Case 0
                btnAmend.Enabled = False
                btnLapse.Enabled = False
                btnDelete.Enabled = False
                btnStatus.Enabled = False
            Case 1
                btnAmend.Enabled = True
                btnLapse.Enabled = True
                btnDelete.Enabled = True
                btnStatus.Enabled = True
            Case Is > 1
                btnAmend.Enabled = True
                btnLapse.Enabled = True
                btnDelete.Enabled = False
                btnStatus.Enabled = False

        End Select

    End Sub
    Protected Sub btnSelectAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSelectAll.Click
        Dim iSelectedIndices() As Integer = Nothing
        Dim iLength As Integer = gvRenewals.Rows.Count
        If (iLength > 0) Then
            ReDim iSelectedIndices(iLength - 1)
        End If
        For iCnt As Integer = 0 To gvRenewals.Rows.Count - 1
            Dim chkTemp As CheckBox = CType(gvRenewals.Rows(iCnt).FindControl("chkSelect"), CheckBox)
            chkTemp.Checked = True
            gvRenewals.Rows(iCnt).BackColor = Drawing.Color.SlateBlue
            iSelectedIndices(iCnt) = iCnt
        Next
        Session("SelectedIndices") = iSelectedIndices
        Session("SelectedCount") = iLength
        ProcessUIControls(iLength)
    End Sub

    Protected Sub btnStatus_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnStatus.Click
        ddlRenewalStatus.SelectedIndex = 0
        pnlRenewal.Visible = False
        pnlSetStatus.Visible = True
        pnlLapseReason.Visible = False
        Page.Title = "Renewal Status"
    End Sub

    Protected Sub btnSetStatus_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSetStatus.Click
        Try
            Dim iInsuranceFileKeys() As Integer = GetSelectedInsuranceFileKeys()
            If (iInsuranceFileKeys IsNot Nothing AndAlso iInsuranceFileKeys.Length > 0) Then
                'Currently only first selected item is used
                Session("InsuranceFileKey") = iInsuranceFileKeys(0)
                SetStatus(iInsuranceFileKeys(0))
            End If
        Catch ex As Exception
            lblOutput.Text = ex.Message
        End Try
        pnlRenewal.Visible = True
        pnlSetStatus.Visible = False
        pnlLapseReason.Visible = False
        Page.Title = "Renewals"
        PopulateGrid()
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        pnlRenewal.Visible = True
        pnlSetStatus.Visible = False
        pnlLapseReason.Visible = False
        Page.Title = "Renewals"
    End Sub
    Private Sub PopulateGrid()
        Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")

        'set up the proxy object
        Dim oSAM As New SAMForInsuranceV2
        oSAM.SetClientCredential(UserToken)
        oSAM.SetPolicy("SamClientPolicy")

        'create the request and response objects
        Dim oGetPoliciesInRenewalRequestType As New GetPoliciesInRenewalRequestType
        Dim oGetPoliciesInRenewalResponseType As New GetPoliciesInRenewalResponseType

        'set up request object with some values
        With oGetPoliciesInRenewalRequestType
            If Session("BranchCode") IsNot Nothing Then
                .BranchCode = Convert.ToString(Session("BranchCode"))
            Else
                .BranchCode = "HeadOff"
            End If

            If Session("ProductCode") IsNot Nothing Then
                .ProductCode = Convert.ToString(Session("ProductCode"))

            End If

            If Session("AgentKey") IsNot Nothing Then
                .AgentKey = Convert.ToInt32(Session("AgentKey"))
                .AgentKeySpecified = True
            End If
            If Session("PartyKey") IsNot Nothing Then
                .PartyKey = Convert.ToInt32(Session("PartyKey"))
                .PartyKeySpecified = True
            End If
            If (Session("RenewalDate") IsNot Nothing) Then
                .RenewalDate = Convert.ToDateTime(Session("RenewalDate"))
                .RenewalDateSpecified = True
            End If
            If (Session("OnlyDirect") IsNot Nothing) Then
                .DirectOnly = True
                .DirectOnlySpecified = True
            End If

        End With

        Try
            StartDate = Date.Now
            oGetPoliciesInRenewalResponseType = oSAM.GetPoliciesInRenewal(oGetPoliciesInRenewalRequestType)
            WriteToLog(Session, "wfrmRenewals_Amend.aspx", "SAMForInsuranceV2", "GetPoliciesInRenewal", StartDate, Date.Now)
            With oGetPoliciesInRenewalResponseType
                If Not (.Errors) Is Nothing Then
                    'errors returned, so throw an exception
                    Throw New SamResponseException(.Errors)
                End If

                Dim dt As New DataTable
                dt.Columns.Clear()
                Dim dc1 As New DataColumn("RenewalStatusKey", GetType(System.Int32))
                Dim dc2 As New DataColumn("PartyKey", GetType(System.Int32))
                Dim dc3 As New DataColumn("BranchCode", GetType(System.String))
                Dim dc4 As New DataColumn("PartyName", GetType(System.String))
                Dim dc5 As New DataColumn("InsuranceFileRef", GetType(System.String))
                Dim dc6 As New DataColumn("InsuranceFileKey", GetType(System.Int32))
                Dim dc7 As New DataColumn("InsuranceFolderKey", GetType(System.Int32))
                Dim dc8 As New DataColumn("InsuranceFileStatusDescription", GetType(System.String))
                Dim dc9 As New DataColumn("InsuranceFileTypeDescription", GetType(System.String))
                Dim dc10 As New DataColumn("RenewalStatusTypeCode", GetType(System.String))
                Dim dc11 As New DataColumn("RenewalStatusTypeDescription", GetType(System.String))
                Dim dc12 As New DataColumn("CoverStartDate", GetType(System.DateTime))
                Dim dc13 As New DataColumn("CoverEndDate", GetType(System.DateTime))
                Dim dc14 As New DataColumn("RenewalDate", GetType(System.DateTime))
                Dim dc15 As New DataColumn("RenewalPremium", GetType(System.Decimal))
                Dim dc16 As New DataColumn("ProductCode", GetType(System.String))
                Dim dc17 As New DataColumn("ProductDescription", GetType(System.String))
                Dim dc18 As New DataColumn("LeadAgentKey", GetType(System.Int32))
                Dim dc19 As New DataColumn("LeadAgent", GetType(System.String))
                Dim dc20 As New DataColumn("AccHandler", GetType(System.String))
                Dim dc21 As New DataColumn("ClaimIndicator", GetType(System.Boolean))
                Dim dc22 As New DataColumn("IsClosed", GetType(System.Boolean))
                Dim dc23 As New DataColumn("IsTrueMonthlyPolicy", GetType(System.Boolean))
                Dim dc24 As New DataColumn("AnniversaryCopy", GetType(System.Boolean))

                dt.Columns.Add(dc1)
                dt.Columns.Add(dc2)
                dt.Columns.Add(dc3)
                dt.Columns.Add(dc4)
                dt.Columns.Add(dc5)
                dt.Columns.Add(dc6)
                dt.Columns.Add(dc7)
                dt.Columns.Add(dc8)
                dt.Columns.Add(dc9)
                dt.Columns.Add(dc10)
                dt.Columns.Add(dc11)
                dt.Columns.Add(dc12)
                dt.Columns.Add(dc13)
                dt.Columns.Add(dc14)
                dt.Columns.Add(dc15)
                dt.Columns.Add(dc16)
                dt.Columns.Add(dc17)
                dt.Columns.Add(dc18)
                dt.Columns.Add(dc19)
                dt.Columns.Add(dc20)
                dt.Columns.Add(dc21)
                dt.Columns.Add(dc22)
                dt.Columns.Add(dc23)
                dt.Columns.Add(dc24)

                If .Policies IsNot Nothing Then
                    Dim dr As DataRow
                    For Each drow As BaseGetPoliciesInRenewalResponseTypeRow In .Policies
                        dr = dt.NewRow()
                        dr.Item("RenewalStatusKey") = drow.RenewalStatusKey
                        dr.Item("PartyKey") = drow.PartyKey
                        dr.Item("BranchCode") = drow.BranchCode
                        dr.Item("PartyName") = drow.PartyName
                        dr.Item("InsuranceFileRef") = drow.InsuranceFileRef
                        dr.Item("InsuranceFileKey") = drow.InsuranceFileKey
                        dr.Item("InsuranceFolderKey") = drow.InsuranceFolderKey
                        dr.Item("InsuranceFileStatusDescription") = drow.InsuranceFileStatusDescription
                        dr.Item("InsuranceFileTypeDescription") = drow.InsuranceFileTypeDescription
                        dr.Item("RenewalStatusTypeCode") = drow.RenewalStatusTypeCode
                        dr.Item("RenewalStatusTypeDescription") = drow.RenewalStatusTypeDescription
                        dr.Item("CoverStartDate") = drow.CoverStartDate
                        dr.Item("CoverEndDate") = drow.CoverEndDate
                        dr.Item("RenewalDate") = drow.RenewalDate
                        dr.Item("RenewalPremium") = drow.RenewalPremium
                        dr.Item("ProductCode") = drow.ProductCode
                        dr.Item("ProductDescription") = drow.ProductDescription
                        dr.Item("LeadAgentKey") = drow.LeadAgentKey
                        dr.Item("LeadAgent") = drow.LeadAgent
                        dr.Item("AccHandler") = drow.AccHandler
                        dr.Item("ClaimIndicator") = drow.ClaimIndicator
                        dr.Item("IsClosed") = drow.IsClosed
                        dr.Item("IsTrueMonthlyPolicy") = drow.IsTrueMonthlyPolicy
                        dr.Item("AnniversaryCopy") = drow.AnniversaryCopy
                        dt.Rows.Add(dr)
                        dr = Nothing
                    Next
                End If

                gvRenewals.DataSource = dt
                gvRenewals.DataBind()
                lblItemCount.Text = gvRenewals.Rows.Count
                Session("Policies") = dt
                Session("SelectedCount") = 0
                Session("SelectedIndices") = Nothing
                ProcessUIControls(0)

            End With

        Catch os As SamResponseException
            'should do some error handling here. Just output error for now
            lblOutput.Text = "An error occured calling SAM:<br>" & os.Message
        Catch oe As Exception
            'should do some error handling here. Just output error for now
            lblOutput.Text = "An error occured:<br>" & oe.Message
        Finally
            'clean up any objects here
        End Try

    End Sub

    Protected Sub btnFilter_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFilter.Click
        Session("Process") = "Amend"
        Response.Redirect("wfrmFilterRenewals.aspx")
    End Sub

    Protected Sub btnAmend_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAmend.Click

        Session("Process") = "REN"
        Session("ReturnPage") = "~/PolicyRenewal/wfrmRenewals_Amend.aspx"
        Session("BranchCode") = "HeadOff"
        Try
            Dim iInsuranceFileKeys() As Integer = GetSelectedInsuranceFileKeys()
            Dim iPartyKeys() As Integer = GetSelectedPartyKeys()
            If (iInsuranceFileKeys IsNot Nothing AndAlso iInsuranceFileKeys.Length > 0) Then
                'Currently only first selected item is used
                Session("InsuranceFileKey") = iInsuranceFileKeys(0)
                'Session("PartyKey") = iPartyKeys(0)
            End If
        Catch ex As Exception
            lblOutput.Text = ex.Message
        End Try
        Response.Redirect("~/MTA/PolicyHeader.aspx")
        pnlRenewal.Visible = False
        pnlSetStatus.Visible = False
        pnlLapseReason.Visible = True
        Page.Title = "Change Policy Status"
    End Sub

    Protected Sub btnLapse_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLapse.Click
        lstLapseReason.SelectedIndex = 0
        pnlRenewal.Visible = False
        pnlSetStatus.Visible = False
        pnlLapseReason.Visible = True
        Page.Title = "Lapse Reason"
    End Sub

    Protected Sub btnLapseOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLapseOK.Click
        Try
            Dim iInsuranceFileKeys() As Integer = GetSelectedInsuranceFileKeys()
            If (iInsuranceFileKeys IsNot Nothing AndAlso iInsuranceFileKeys.Length > 0) Then
                Session("InsuranceFileKey") = iInsuranceFileKeys(0)
                LapseRenewal(iInsuranceFileKeys(0))
            End If
        Catch ex As Exception
            lblOutput.Text = ex.Message
        End Try
        pnlRenewal.Visible = True
        pnlSetStatus.Visible = False
        pnlLapseReason.Visible = False
        Page.Title = "Renewals"
        PopulateGrid()
    End Sub

    Protected Sub btnLapseCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLapseCancel.Click
        pnlRenewal.Visible = True
        pnlSetStatus.Visible = False
        pnlLapseReason.Visible = False
        Page.Title = "Renewals"
    End Sub
    Private Sub PopulateRenewalStatus()
        Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")

        'set up the proxy object
        Dim oSAM As New SAMForInsuranceV2
        oSAM.SetClientCredential(UserToken)
        oSAM.SetPolicy("SamClientPolicy")

        'create the request and response objects
        Dim oGetListRequest As New GetListRequestType
        Dim oGetListResponse As New GetListResponseType

        oGetListRequest.BranchCode = "HeadOff"
        oGetListRequest.ListType = STSListType.PMLookup
        oGetListRequest.ListCode = "renewal_status_type"
        Try
            StartDate = Date.Now
            oGetListResponse = oSAM.GetList(oGetListRequest)
            WriteToLog(Session, "wfrmRenewals_Amend.aspx", "SAMForInsuranceV2", "GetList", StartDate, Date.Now)

            If Not (oGetListResponse.Errors) Is Nothing Then
                'errors returned, so throw an exception
                Throw New SamResponseException(oGetListResponse.Errors)
            End If
            'Filter Awaiting Broker Transfer
            ddlRenewalStatus.Items.Clear()
            Dim ddlListItem As ListItem
            For Each lstItem As BaseGetListResponseTypeRow In oGetListResponse.List
                If lstItem.Code.Trim <> "BROKERXFER" Then
                    ddlListItem = New ListItem(lstItem.Description, lstItem.Code)
                    ddlRenewalStatus.Items.Add(ddlListItem)
                End If
            Next
            ddlRenewalStatus.SelectedIndex = 0

        Catch os As SamResponseException
            'should do some error handling here. Just output error for now
            lblOutput.Text = "An error occured calling SAM:<br>" & os.Message
        Catch oe As Exception
            'should do some error handling here. Just output error for now
            lblOutput.Text = "An error occured:<br>" & oe.Message
        Finally
            'clean up any objects here
        End Try
        
    End Sub

    Private Sub PopulateLaspseReasons()
        Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")

        'set up the proxy object
        Dim oSAM As New SAMForInsuranceV2
        oSAM.SetClientCredential(UserToken)
        oSAM.SetPolicy("SamClientPolicy")

        'create the request and response objects
        Dim oGetListRequest As New GetListRequestType
        Dim oGetListResponse As New GetListResponseType

        oGetListRequest.BranchCode = "HeadOff"
        oGetListRequest.ListType = STSListType.PMLookup
        oGetListRequest.ListCode = "lapsed_reason"
        Try
            StartDate = Date.Now
            oGetListResponse = oSAM.GetList(oGetListRequest)
            WriteToLog(Session, "wfrmRenewals_Amend.aspx", "SAMForInsuranceV2", "GetList", StartDate, Date.Now)

            If Not (oGetListResponse.Errors) Is Nothing Then
                'errors returned, so throw an exception
                Throw New SamResponseException(oGetListResponse.Errors)
            End If
            lstLapseReason.DataSource = oGetListResponse.List
            lstLapseReason.DataTextField = "Description"
            lstLapseReason.DataValueField = "Code"
            lstLapseReason.DataBind()
            lstLapseReason.SelectedIndex = 0
            lblLapseReasonCount.Text = lstLapseReason.Items.Count

        Catch os As SamResponseException
            'should do some error handling here. Just output error for now
            lblOutput.Text = "An error occured calling SAM:<br>" & os.Message
        Catch oe As Exception
            'should do some error handling here. Just output error for now
            lblOutput.Text = "An error occured:<br>" & oe.Message
        Finally
            'clean up any objects here
        End Try

    End Sub
    Private Sub LapseRenewal(ByVal iInsuranceFileKey As Integer)
        Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")

        'set up the proxy object
        Dim oSAM As New SAMForInsuranceV2
        oSAM.SetClientCredential(UserToken)
        oSAM.SetPolicy("SamClientPolicy")

        'create the request and response objects
        Dim oLapseRenewalRequest As New LapseRenewalRequestType
        Dim oLapseRenewalResponse As New LapseRenewalResponseType
        'Use GetHeaderAndSummariesByKey to get timestamp on InsuranceFolderKey that has to be passed on request of GenerateInvite
        Dim oGetHeaderAndSummariesByKeyRequest As New GetHeaderAndSummariesByKeyRequestType
        Dim oGetHeaderAndSummariesByKeResponse As New GetHeaderAndSummariesByKeyResponseType

        If Session("BranchCode") IsNot Nothing Then
            oGetHeaderAndSummariesByKeyRequest.BranchCode = Convert.ToString(Session("BranchCode"))
        Else
            oGetHeaderAndSummariesByKeyRequest.BranchCode = "HeadOff"
        End If
        oGetHeaderAndSummariesByKeyRequest.InsuranceFileKey = iInsuranceFileKey
        Try
            StartDate = Date.Now
            oGetHeaderAndSummariesByKeResponse = oSAM.GetHeaderAndSummariesByKey(oGetHeaderAndSummariesByKeyRequest)
            WriteToLog(Session, "wfrmRenewals.aspx", "SAMForInsuranceV2", "GetHeaderAndSummariesByKey", StartDate, Date.Now)

            If Not (oGetHeaderAndSummariesByKeResponse.Errors) Is Nothing Then
                'errors returned, so throw an exception
                Throw New SamResponseException(oGetHeaderAndSummariesByKeResponse.Errors)
            End If

            'set up request object with some values
            With oLapseRenewalRequest
                If Session("BranchCode") IsNot Nothing Then
                    .BranchCode = Convert.ToString(Session("BranchCode"))
                Else
                    .BranchCode = "HeadOff"

                End If
                .InsuranceFileKey = iInsuranceFileKey
                .LapseReasonCode = lstLapseReason.SelectedValue
                .QuoteTimeStamp = oGetHeaderAndSummariesByKeResponse.QuoteTimeStamp
            End With

            StartDate = Date.Now
            oLapseRenewalResponse = oSAM.LapseRenewal(oLapseRenewalRequest)
            WriteToLog(Session, "wfrmRenewals_Amend.aspx", "SAMForInsuranceV2", "LapseRenewal", StartDate, Date.Now)
            'oGenerateInviteResponseType.

            With oLapseRenewalResponse
                If Not (.Errors) Is Nothing Then
                    'errors returned, so throw an exception
                    Throw New SamResponseException(.Errors)
                End If
                lblOutput.Text = "Process completed successfully"
            End With


        Catch os As SamResponseException
            'should do some error handling here. Just output error for now
            lblOutput.Text = "An error occured calling SAM:<br>" & os.Message
        Catch oe As Exception
            'should do some error handling here. Just output error for now
            lblOutput.Text = "An error occured:<br>" & oe.Message
        Finally
            'clean up any objects here
        End Try

    End Sub

    Private Sub SetStatus(ByVal iInsuranceFileKey As Integer)
        Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")

        'set up the proxy object
        Dim oSAM As New SAMForInsuranceV2
        oSAM.SetClientCredential(UserToken)
        oSAM.SetPolicy("SamClientPolicy")

        'create the request and response objects
        Dim oUpdateRenewalStatusRequest As New UpdateRenewalStatusRequestType
        Dim oUpdateRenewalStatusResponse As New UpdateRenewalStatusResponseType
        'Use GetHeaderAndSummariesByKey to get timestamp on InsuranceFolderKey that has to be passed on request of GenerateInvite
        Dim oGetHeaderAndSummariesByKeyRequest As New GetHeaderAndSummariesByKeyRequestType
        Dim oGetHeaderAndSummariesByKeResponse As New GetHeaderAndSummariesByKeyResponseType

        oGetHeaderAndSummariesByKeyRequest.BranchCode = "HeadOff"
        oGetHeaderAndSummariesByKeyRequest.InsuranceFileKey = iInsuranceFileKey
        Try
            
            StartDate = Date.Now
            oGetHeaderAndSummariesByKeResponse = oSAM.GetHeaderAndSummariesByKey(oGetHeaderAndSummariesByKeyRequest)
            WriteToLog(Session, "wfrmRenewals_Amend.aspx", "SAMForInsuranceV2", "GetHeaderAndSummariesByKey", StartDate, Date.Now)
            
            If Not (oGetHeaderAndSummariesByKeResponse.Errors) Is Nothing Then
                'errors returned, so throw an exception
                Throw New SamResponseException(oGetHeaderAndSummariesByKeResponse.Errors)
            End If

            'set up request object with some values
            With oUpdateRenewalStatusRequest
                If Session("BranchCode") IsNot Nothing Then
                    .BranchCode = Convert.ToString(Session("BranchCode"))
                Else
                    .BranchCode = "HeadOff"
                End If

                .InsuranceFileKey = iInsuranceFileKey
                .QuoteTimeStamp = oGetHeaderAndSummariesByKeResponse.QuoteTimeStamp
                .RenewalStatusCode = ddlRenewalStatus.SelectedValue
            End With

            StartDate = Date.Now
            oUpdateRenewalStatusResponse = oSAM.UpdateRenewalStatus(oUpdateRenewalStatusRequest)
            WriteToLog(Session, "wfrmRenewals_Amend.aspx", "SAMForInsuranceV2", "UpdateRenewalStatus", StartDate, Date.Now)
            'oGenerateInviteResponseType.

            With oUpdateRenewalStatusResponse
                If Not (.Errors) Is Nothing Then
                    'errors returned, so throw an exception
                    Throw New SamResponseException(.Errors)
                End If
                lblOutput.Text = "Process completed successfully"
            End With
        Catch os As SamResponseException
            'should do some error handling here. Just output error for now
            lblOutput.Text = "An error occured calling SAM:<br>" & os.Message
        Catch oe As Exception
            'should do some error handling here. Just output error for now
            lblOutput.Text = "An error occured:<br>" & oe.Message
        Finally
            'clean up any objects here
        End Try

    End Sub
    Private Function GetSelectedInsuranceFileKeys() As Integer()
        Dim iInsuranceFileKeys() As Integer = Nothing
        'Put selected policy to Session
        If Session("SelectedIndices") Is Nothing Or Session("Policies") Is Nothing Then
            Throw New Exception("Session values corrupted. Unable to proceed")
        Else
            Dim iLength = 0
            ReDim Preserve iInsuranceFileKeys(iLength)
            Dim iSelectedIndices() As Integer = CType(Session("SelectedIndices"), Integer())
            Dim dtPolicies As DataTable = CType(Session("Policies"), DataTable)
            For Each iCnt As Integer In iSelectedIndices
                iInsuranceFileKeys(iLength) = Convert.ToInt32(dtPolicies.Rows(iCnt).Item("InsuranceFileKey").ToString)
                iLength = iInsuranceFileKeys.Length
            Next
        End If
        Return iInsuranceFileKeys
    End Function
    Private Function GetSelectedPartyKeys() As Integer()
        Dim iPartyKeys() As Integer = Nothing
        'Put selected policy to Session
        If Session("SelectedIndices") Is Nothing Or Session("Policies") Is Nothing Then
            Throw New Exception("Session values corrupted. Unable to proceed")
        Else
            Dim iLength = 0
            ReDim Preserve iPartyKeys(iLength)
            Dim iSelectedIndices() As Integer = CType(Session("SelectedIndices"), Integer())
            Dim dtPolicies As DataTable = CType(Session("Policies"), DataTable)
            For Each iCnt As Integer In iSelectedIndices
                iPartyKeys(iLength) = Convert.ToInt32(dtPolicies.Rows(iCnt).Item("PartyKey").ToString)
                iLength = iPartyKeys.Length
            Next
        End If
        Return iPartyKeys
    End Function

    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        Try
            Dim iInsuranceFileKeys() As Integer = GetSelectedInsuranceFileKeys()
            If (iInsuranceFileKeys IsNot Nothing AndAlso iInsuranceFileKeys.Length > 0) Then
                Session("InsuranceFileKey") = iInsuranceFileKeys(0)
                DeleteRenewal(iInsuranceFileKeys(0))
            End If
        Catch ex As Exception
            lblOutput.Text = ex.Message
        End Try
        PopulateGrid()
    End Sub
    Private Sub DeleteRenewal(ByVal iInsuranceFileKey As Integer)
        Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")

        'set up the proxy object
        Dim oSAM As New SAMForInsuranceV2
        oSAM.SetClientCredential(UserToken)
        oSAM.SetPolicy("SamClientPolicy")

        'create the request and response objects
        Dim oDeleteRenewalRequest As New DeleteRenewalRequestType
        Dim oDeleteRenewalResponse As New DeleteRenewalResponseType
        'Use GetHeaderAndSummariesByKey to get timestamp on InsuranceFolderKey that has to be passed on request of GenerateInvite
        Dim oGetHeaderAndSummariesByKeyRequest As New GetHeaderAndSummariesByKeyRequestType
        Dim oGetHeaderAndSummariesByKeResponse As New GetHeaderAndSummariesByKeyResponseType

        oGetHeaderAndSummariesByKeyRequest.BranchCode = "HeadOff"
        oGetHeaderAndSummariesByKeyRequest.InsuranceFileKey = iInsuranceFileKey
        Try
            StartDate = Date.Now
            oGetHeaderAndSummariesByKeResponse = oSAM.GetHeaderAndSummariesByKey(oGetHeaderAndSummariesByKeyRequest)
            WriteToLog(Session, "wfrmRenewals_Amend.aspx", "SAMForInsuranceV2", "GetHeaderAndSummariesByKey", StartDate, Date.Now)

            If Not (oGetHeaderAndSummariesByKeResponse.Errors) Is Nothing Then
                'errors returned, so throw an exception
                Throw New SamResponseException(oGetHeaderAndSummariesByKeResponse.Errors)
            End If

            'set up request object with some values
            With oDeleteRenewalRequest
                If Session("BranchCode") IsNot Nothing Then
                    .BranchCode = Convert.ToString(Session("BranchCode"))
                Else
                    .BranchCode = "HeadOff"
                End If
                .InsuranceFileKey = iInsuranceFileKey
                .QuoteTimeStamp = oGetHeaderAndSummariesByKeResponse.QuoteTimeStamp
            End With

            StartDate = Date.Now
            oDeleteRenewalResponse = oSAM.DeleteRenewal(oDeleteRenewalRequest)
            WriteToLog(Session, "wfrmRenewals_Amend.aspx", "SAMForInsuranceV2", "DeleteRenewal", StartDate, Date.Now)
            'oGenerateInviteResponseType.

            With oDeleteRenewalResponse
                If Not (.Errors) Is Nothing Then
                    'errors returned, so throw an exception
                    Throw New SamResponseException(.Errors)
                Else
                    lblOutput.Text = "Process completed successfully"
                End If
            End With

        Catch os As SamResponseException
            'should do some error handling here. Just output error for now
            lblOutput.Text = "An error occured calling SAM:<br>" & os.Message
        Catch oe As Exception
            'should do some error handling here. Just output error for now
            lblOutput.Text = "An error occured:<br>" & oe.Message
        Finally
            'clean up any objects here
        End Try
    End Sub

    Protected Sub btnOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOK.Click
        Response.Redirect("~/UIIC_Demo/HomePage.aspx")
    End Sub
End Class
