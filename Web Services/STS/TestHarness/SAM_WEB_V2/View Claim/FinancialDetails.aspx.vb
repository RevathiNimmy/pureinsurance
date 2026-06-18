Imports Microsoft.Web.Services3.Security.Tokens
Imports SAMForInsuranceV2
Partial Class OpenClaim_FinancialDetails
    Inherits System.Web.UI.Page
     Dim StartDate As Date
    Protected Sub Menu1_MenuItemClick(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.MenuEventArgs) Handles Menu1.MenuItemClick

        Dim oGetClaimPerilSummaryRequestType As New GetClaimPerilSummaryRequestType
        Dim oGetClaimPerilSummaryResponseType As New GetClaimPerilSummaryResponseType

        Dim oSAM As New SAMForInsuranceV2
        Dim iSelectedIndex As Integer
        iSelectedIndex = Menu1.Items.IndexOf(Menu1.SelectedItem)
        If (iSelectedIndex = 0) Then
            Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")
            oSAM.SetClientCredential(UserToken)
            oSAM.SetPolicy("SamClientPolicy")
            With oGetClaimPerilSummaryRequestType
                .BranchCode = "HeadOff"
                'JP 18/02/10
                .ClaimKey = (DirectCast(Session("SelectedClaim"), BaseGetVersionsForClaimResponseTypeRow)).ClaimKey
                .IncludeReserveTypes = chkIncludeReserveTypes.Checked
                .IncludeSalvageRecovery = chkIncludeSalvageecovery.Checked
                .IncludeTotals = chkIncludeTotals.Checked
                .IncludeTPRecovery = chkIncludeTPRecovery.Checked
            End With
            StartDate = Date.Now
            oGetClaimPerilSummaryResponseType = oSAM.GetClaimPerilSummary(oGetClaimPerilSummaryRequestType)
            WriteToLog(Session, "FinancialDetails.aspx", "SAMForInsuranceV2", "GetClaimPerilSummary", StartDate, Date.Now)
            If oGetClaimPerilSummaryResponseType.PerilTotals Is Nothing Then
                lblOutput.Text = "No Record found for Peril Totals"
            ElseIf oGetClaimPerilSummaryResponseType.PerilTotals.Length = 0 Then
                lblOutput.Text = "No Record found for Peril Totals"
            End If
            grd_Output.DataSource = oGetClaimPerilSummaryResponseType.PerilTotals
            grd_Output.DataBind()
        ElseIf (iSelectedIndex = 1) Then
            Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")
            oSAM.SetClientCredential(UserToken)
            oSAM.SetPolicy("SamClientPolicy")
            With oGetClaimPerilSummaryRequestType
                .BranchCode = "HeadOff"
                'JP 18/02/10
                .ClaimKey = (DirectCast(Session("SelectedClaim"), BaseGetVersionsForClaimResponseTypeRow)).ClaimKey
                .IncludeReserveTypes = chkIncludeReserveTypes.Checked
                .IncludeSalvageRecovery = chkIncludeSalvageecovery.Checked
                .IncludeTotals = chkIncludeTotals.Checked
                .IncludeTPRecovery = chkIncludeTPRecovery.Checked
            End With
            StartDate = Date.Now
            oGetClaimPerilSummaryResponseType = oSAM.GetClaimPerilSummary(oGetClaimPerilSummaryRequestType)
            WriteToLog(Session, "FinancialDetails.aspx", "SAMForInsuranceV2", "GetClaimPerilSummary", StartDate, Date.Now)
            If oGetClaimPerilSummaryResponseType.TPRecoveryPerils Is Nothing Then
                lblOutput.Text = "No Record found for TP Recovery"
            ElseIf oGetClaimPerilSummaryResponseType.TPRecoveryPerils.Length = 0 Then
                lblOutput.Text = "No Record found for TP Recovery"
            End If
            grd_Output.DataSource = oGetClaimPerilSummaryResponseType.TPRecoveryPerils
            grd_Output.DataBind()
        ElseIf (iSelectedIndex = 2) Then
            Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")
            oSAM.SetClientCredential(UserToken)
            oSAM.SetPolicy("SamClientPolicy")
            With oGetClaimPerilSummaryRequestType
                .BranchCode = "HeadOff"
                'JP 18/02/10
                .ClaimKey = (DirectCast(Session("SelectedClaim"), BaseGetVersionsForClaimResponseTypeRow)).ClaimKey
                .IncludeReserveTypes = chkIncludeReserveTypes.Checked
                .IncludeSalvageRecovery = chkIncludeSalvageecovery.Checked
                .IncludeTotals = chkIncludeTotals.Checked
                .IncludeTPRecovery = chkIncludeTPRecovery.Checked
            End With
            StartDate = Date.Now
            oGetClaimPerilSummaryResponseType = oSAM.GetClaimPerilSummary(oGetClaimPerilSummaryRequestType)
            WriteToLog(Session, "FinancialDetails.aspx", "SAMForInsuranceV2", "GetClaimPerilSummary", StartDate, Date.Now)
            If oGetClaimPerilSummaryResponseType.SalvageRecoveryPerils Is Nothing Then
                lblOutput.Text = "No Record found for Salvage Recovery"
            ElseIf oGetClaimPerilSummaryResponseType.SalvageRecoveryPerils.Length = 0 Then
                lblOutput.Text = "No Record found for Salvage Recovery"
            End If
            grd_Output.DataSource = oGetClaimPerilSummaryResponseType.SalvageRecoveryPerils
            grd_Output.DataBind()
        ElseIf (iSelectedIndex = 3) Then
            Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")
            oSAM.SetClientCredential(UserToken)
            oSAM.SetPolicy("SamClientPolicy")
            With oGetClaimPerilSummaryRequestType
                .BranchCode = "HeadOff"
                'JP 18/02/10
                .ClaimKey = (DirectCast(Session("SelectedClaim"), BaseGetVersionsForClaimResponseTypeRow)).ClaimKey
                .IncludeReserveTypes = chkIncludeReserveTypes.Checked
                .IncludeSalvageRecovery = chkIncludeSalvageecovery.Checked
                .IncludeTotals = chkIncludeTotals.Checked
                .IncludeTPRecovery = chkIncludeTPRecovery.Checked
            End With
            StartDate = Date.Now
            oGetClaimPerilSummaryResponseType = oSAM.GetClaimPerilSummary(oGetClaimPerilSummaryRequestType)
            WriteToLog(Session, "FinancialDetails.aspx", "SAMForInsuranceV2", "GetClaimPerilSummary", StartDate, Date.Now)
            If oGetClaimPerilSummaryResponseType.ReserveType Is Nothing Then
                lblOutput.Text = "No reserve types found"
            Else
                If (oGetClaimPerilSummaryResponseType.ReserveType(0).Perils.Length < 1) Then
                    lblOutput.Text = "No reserve type found"
                Else
                    lblCode.Text = "Code : " + oGetClaimPerilSummaryResponseType.ReserveType(0).Code.ToString
                    lblDescription.Text = "Description : " + oGetClaimPerilSummaryResponseType.ReserveType(0).Description.ToString
                    If oGetClaimPerilSummaryResponseType.ReserveType(0).Perils.Length = 0 Or oGetClaimPerilSummaryResponseType.ReserveType(0).Perils Is Nothing Then
                        lblOutput.Text = "No Record found for the reserve type: " + oGetClaimPerilSummaryResponseType.ReserveType(0).Description.ToString
                    Else
                        grd_Output.DataSource = oGetClaimPerilSummaryResponseType.ReserveType(0).Perils
                        grd_Output.DataBind()
                    End If

                End If

            End If
        ElseIf (iSelectedIndex = 4) Then
            Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")
            oSAM.SetClientCredential(UserToken)
            oSAM.SetPolicy("SamClientPolicy")
            With oGetClaimPerilSummaryRequestType
                .BranchCode = "HeadOff"
                'JP 18/02/10
                .ClaimKey = (DirectCast(Session("SelectedClaim"), BaseGetVersionsForClaimResponseTypeRow)).ClaimKey
                .IncludeReserveTypes = chkIncludeReserveTypes.Checked
                .IncludeSalvageRecovery = chkIncludeSalvageecovery.Checked
                .IncludeTotals = chkIncludeTotals.Checked
                .IncludeTPRecovery = chkIncludeTPRecovery.Checked
            End With
            StartDate = Date.Now
            oGetClaimPerilSummaryResponseType = oSAM.GetClaimPerilSummary(oGetClaimPerilSummaryRequestType)
            WriteToLog(Session, "FinancialDetails.aspx", "SAMForInsuranceV2", "GetClaimPerilSummary", StartDate, Date.Now)
            'If oGetClaimPerilSummaryResponseType.ReserveType.Length = 0 Or oGetClaimPerilSummaryResponseType.ReserveType Is Nothing Then
            If oGetClaimPerilSummaryResponseType.ReserveType Is Nothing Then

                lblOutput.Text = "No reserve types found"
            Else
                If (oGetClaimPerilSummaryResponseType.ReserveType.Length < 2) Then
                    lblOutput.Text = "No reserve type found"
                Else
                    lblCode.Text = "Code : " + oGetClaimPerilSummaryResponseType.ReserveType(1).Code.ToString
                    lblDescription.Text = "Description : " + oGetClaimPerilSummaryResponseType.ReserveType(1).Description.ToString
                    If oGetClaimPerilSummaryResponseType.ReserveType(1).Perils.Length = 0 Or oGetClaimPerilSummaryResponseType.ReserveType(1).Perils Is Nothing Then
                        lblOutput.Text = "No Record found for the reserve type: " + oGetClaimPerilSummaryResponseType.ReserveType(1).Description.ToString
                    Else
                        grd_Output.DataSource = oGetClaimPerilSummaryResponseType.ReserveType(1).Perils
                        grd_Output.DataBind()
                    End If

                End If

            End If

        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        lblOutput.Text = String.Empty
        lblCode.Text = String.Empty
        lblDescription.Text = String.Empty
    End Sub
End Class
