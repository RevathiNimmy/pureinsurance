Imports NexusProvider.SAMForInsurance
Imports Nexus
Imports Nexus.Utils
Imports System.Data
Imports Nexus.Constants
Imports Nexus.Constants.Session
Imports Nexus.Library
Imports System.Configuration.ConfigurationManager
Imports CMS.Library.Portal
Namespace Nexus
    Partial Class Controls_ClaimReinsurance
        Inherits System.Web.UI.UserControl

        Dim oWebService As NexusProvider.ProviderBase
        Dim oQuote As NexusProvider.Claim
        Dim iArrangementId As Integer
        Dim sKey As String
#Region "Page Events"

        Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
            Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "Confirmation",
                                         "<script language=""JavaScript"" type=""text/javascript"">function Confirmation(){var r= confirm('" & GetLocalResourceObject("msg_AnotherRecovery").ToString() & "'); document.getElementById('" & hidChkChoice.ClientID & "').value=r;}</script>")
            Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "PaymentConfirmation",
                                                        "<script language=""JavaScript"" type=""text/javascript"">function PaymentConfirmation(){var r= confirm('" & GetLocalResourceObject("msg_AnotherPayment").ToString() & "'); document.getElementById('" & hidChkChoice.ClientID & "').value=r;}</script>")
            Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "ClaimCloseConfirmation",
                                                        "<script language=""JavaScript"" type=""text/javascript"">function ClaimCloseConfirmation(){var r= confirm('" & GetLocalResourceObject("msg_CloseClaim").ToString() & "'); document.getElementById('" & hidChlClaimClose.ClientID & "').value=r; if (document.getElementById('" & hidChkPaymentMsg.ClientID & "').value ==  '0' && r == false) {PaymentConfirmation();}}</script>")
        End Sub
        ''' <summary>
        ''' This event is fired on page load
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Shadows Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            If Me.Visible Then
                'To set the Focus
                Page.SetFocus(ddlReinsurance)

                oWebService = New NexusProvider.ProviderManager().Provider
                Dim oUserDetails As NexusProvider.UserDetails = CType(Session(CNAgentDetails), NexusProvider.UserDetails)
                Dim sBranchCode As String = oUserDetails.ListOfBranches(0).Code
                Dim oClaim As NexusProvider.ClaimOpen = Nothing
                If Not IsPostBack Then
                    oClaim = CType(Session.Item(CNClaim), NexusProvider.ClaimOpen)
                    ' Obtaining the value of Reinsurance bands from SAM
                    Dim oReinsurarerBandCollection As NexusProvider.ReinsuranceBandsCollection = Nothing
                    ' Checking whether we have value for Current Risk Key
                    ' if we have then get the Reinsurance band values for the Current risk key else 
                    ' obtain it for the first risk option available
                    If oClaim.ClaimKey <> 0 Then
                        oReinsurarerBandCollection = oWebService.GetClaimReinsurancebands(oClaim.ClaimKey)

                        ' adding value from the collection to the dropdown
                        For Each oReinsurarerBand As NexusProvider.ReinsuranceBands In oReinsurarerBandCollection
                            ddlReinsurance.Items.Add(New ListItem(oReinsurarerBand.Band, oReinsurarerBand.BandID))
                        Next
                    End If

                    ' call made for populating the grid   
                    Dim ReinsurancepageCacheID As Guid
                    ReinsurancepageCacheID = Guid.NewGuid()
                    ViewState.Add("ClaimReinsurancepageCacheID", ReinsurancepageCacheID.ToString)
                    PopulateGrid()

                    If Session(CNMode) = Mode.SalvageClaim Or Session(CNMode) = Mode.TPRecovery Then
                        'To make another receipt confirmation 
                        btnNext.Attributes.Add("onclick", "javascript:return Confirmation();")

                    ElseIf Session(CNMode) = Mode.PayClaim Then
                        Dim sRunAuthorizePayment As String
                        Dim oRunClaimWorkFlow As NexusProvider.ProductClaimsWorkflowOptionsValue
                        Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                        Dim oQuote As NexusProvider.Quote = Session(CNClaimQuote)

                        'get the Product Risk option setting named Run Authorize Payment
                        sRunAuthorizePayment = oWebservice.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance, NexusProvider.ProductRiskOptions.RunAuthorisationScriptsClaimPayments, NexusProvider.RiskTypeOptions.None, oQuote.ProductCode, Nothing)
                        'get the Claim Workflow Setting
                        oRunClaimWorkFlow = oWebservice.GetProductClaimsWorkflowOptions(NexusProvider.ClaimProcessType.ClaimPayment, oQuote.ProductCode)
                        ViewState("RunClaimWorkFlow") = oRunClaimWorkFlow
                        If oRunClaimWorkFlow.MakeFurtherPayments = True Then
                            Dim oPerilColl As NexusProvider.PerilCollection = CType(Session(CNClaim), NexusProvider.ClaimOpen).ClaimPeril

                            If oRunClaimWorkFlow.MakeFurtherPayments = True Then
                                'Check the reserve amount vs Payment amount
                                Dim bStatus As Boolean = False
                                For j As Integer = 0 To oPerilColl.Count - 1
                                    For i As Integer = 0 To oPerilColl(j).ClaimReserve.Count - 1
                                        If oPerilColl(j).ClaimReserve(i).CurrentReserve <= 0 Then
                                            bStatus = True
                                        Else
                                            bStatus = False
                                            Exit For
                                        End If
                                    Next
                                    If bStatus = False Then
                                        Exit For
                                    End If
                                Next
                                If bStatus = False Then
                                    btnNext.Attributes.Add("onclick", "javascript:return PaymentConfirmation();")
                                Else
                                    hidChkPaymentMsg.Value = "0"
                                    'If this value is set then paymentconfirmatio will be called from
                                    'ClamCloseConfirmation javascript function
                                    btnNext.Attributes.Add("onclick", "javascript:return ClaimCloseConfirmation();")
                                End If
                            ElseIf oRunClaimWorkFlow.MakeFurtherPayments = False Then
                                'Check the reserve amount vs Payment amount
                                Dim bFound As Boolean = False
                                For j As Integer = 0 To oPerilColl.Count - 1
                                    For i As Integer = 0 To oPerilColl(j).ClaimReserve.Count - 1
                                        If oPerilColl(j).ClaimReserve(i).CurrentReserve <= 0 Then
                                            bFound = True
                                        Else
                                            bFound = False
                                            Exit For
                                        End If
                                    Next
                                    If bFound = False Then
                                        Exit For
                                    End If
                                Next
                                If bFound = True Then
                                    hidChkPaymentMsg.Value = String.Empty
                                    btnNext.Attributes.Add("onclick", "javascript:return ClaimCloseConfirmation();")
                                End If
                            End If
                        End If
                    ElseIf CType(Session.Item(CNMode), Mode) = Mode.ViewClaim Then
                        btnNext.Visible = False

                    End If
                End If
            End If
        End Sub
#End Region
#Region "Control Events"
        ''' <summary>
        ''' This event is fired on the drop down list selected  Index change
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub ddlReinsurance_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlReinsurance.SelectedIndexChanged
            PopulateGrid()
        End Sub

        Protected Sub gvClaimReinsurance_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvClaimReinsurance.Load
            If gvClaimReinsurance.PageCount = 1 Then
                gvClaimReinsurance.AllowPaging = False
            End If
        End Sub

        ''' <summary>
        ''' This event is fired on the next button click
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub btnNext_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNext.Click
            If Session(CNMode) = Mode.NewClaim Then

                'Fire the Bind Claim
                Dim oQuote As NexusProvider.Quote = Session(CNClaimQuote)
                Dim sBranchCode As String = oQuote.BranchCode
                Dim oOriginalClaim As NexusProvider.ClaimOpen = CType(Session.Item(CNClaim), NexusProvider.ClaimOpen)
                Dim bClaimTimeStamp() As Byte = Session.Item(CNClaimTimeStamp)
                oWebService = New NexusProvider.ProviderManager().Provider

                If BindClaimCall(oOriginalClaim, bClaimTimeStamp, 1, Nothing, sBranchCode) Is Nothing Then
                    Exit Sub
                End If

                Response.Redirect("~/Claims/Complete.aspx")
            ElseIf Session(CNMode) = Mode.EditClaim Then
                'Fire the Bind Claim
                Dim oQuote As NexusProvider.Quote = Session(CNClaimQuote)
                Dim sBranchCode As String = oQuote.BranchCode
                Dim oOriginalClaim As NexusProvider.ClaimOpen = CType(Session.Item(CNClaim), NexusProvider.ClaimOpen)
                Dim bClaimTimeStamp() As Byte = Session.Item(CNClaimTimeStamp)
                oWebService = New NexusProvider.ProviderManager().Provider

                If BindClaimCall(oOriginalClaim, bClaimTimeStamp, 2, Nothing, sBranchCode) Is Nothing Then
                    Exit Sub
                End If

                Response.Redirect("~/Claims/Complete.aspx")
            ElseIf Session(CNMode) = Mode.PayClaim Then
                'Fire the Bind Claim
                Dim PerilsIndex As New System.Collections.Generic.List(Of Integer)
                Dim oClaimPerilPayment As New NexusProvider.ClaimPaymentCollection
                Dim oRunClaimWorkFlow As NexusProvider.ProductClaimsWorkflowOptionsValue
                Dim oQuote As NexusProvider.Quote = Session(CNClaimQuote)
                Dim oPayment As New NexusProvider.ClaimPayment
                Dim oOriginalClaim As NexusProvider.ClaimOpen = CType(Session.Item(CNClaim), NexusProvider.ClaimOpen)
                Dim bClaimTimeStamp() As Byte = Session.Item(CNClaimTimeStamp)
                Dim sBranchCode As String = oQuote.BranchCode
                Dim bPaymentAuthorized As Boolean = False
                oWebService = New NexusProvider.ProviderManager().Provider

                If Session(CNClaimMultiPerilIndex) IsNot Nothing Then
                    PerilsIndex = Session(CNClaimMultiPerilIndex)
                    For Each PerilItemIndex As Integer In PerilsIndex
                        oPayment = CType(Session(CNClaim), NexusProvider.ClaimOpen).ClaimPeril(PerilItemIndex).Payment
                        'Need to close the claim if Payment Amount is exceeding or equal to reserve amount for any peril
                        If hidChlClaimClose.Value.Trim.ToUpper = "TRUE" Then
                            oOriginalClaim.CloseClaimOnZeroReserveRecoveryBalance = True
                            oPayment.CloseClaimOnZeroReserveRecoveryBalance = True
                        Else
                            oOriginalClaim.CloseClaimOnZeroReserveRecoveryBalance = False
                            oPayment.CloseClaimOnZeroReserveRecoveryBalance = False
                        End If
                        oClaimPerilPayment.Add(oPayment)
                    Next
                Else
                    oPayment = CType(Session(CNClaim), NexusProvider.ClaimOpen).ClaimPeril(Session(CNClaimPerilIndex)).Payment
                    'Need to close the claim if Payment Amount is exceeding or equal to reserve amount for any peril
                    If hidChlClaimClose.Value.Trim.ToUpper = "TRUE" Then
                        oOriginalClaim.CloseClaimOnZeroReserveRecoveryBalance = True
                        oPayment.CloseClaimOnZeroReserveRecoveryBalance = True
                    Else
                        oOriginalClaim.CloseClaimOnZeroReserveRecoveryBalance = False
                        oPayment.CloseClaimOnZeroReserveRecoveryBalance = False
                    End If
                End If

                Try
                    'Check to move to the accept another payment
                    oRunClaimWorkFlow = ViewState("RunClaimWorkFlow")
                    If Session(CNMode) = Mode.PayClaim AndAlso (Session(CNLockPaymentGrid) IsNot Nothing AndAlso Session(CNLockPaymentGrid) = True) Then
                        'Fire the Bind Claim
                        If Session(CNClaimMultiPerilIndex) IsNot Nothing Then
                            If BindClaimCall(oOriginalClaim, bClaimTimeStamp, 4, Nothing, sBranchCode, oPaymentCollection:=oClaimPerilPayment) Is Nothing Then
                                Exit Sub
                            End If
                        Else
                            If BindClaimCall(oOriginalClaim, bClaimTimeStamp, 4, oPayment, sBranchCode) Is Nothing Then
                                Exit Sub
                            End If
                        End If

                    Else
                        'Fire the Bind Claim
                        If Session(CNClaimMultiPerilIndex) IsNot Nothing Then
                            If BindClaimCall(oOriginalClaim, bClaimTimeStamp, 3, Nothing, sBranchCode, bPaymentAuthorized, oPaymentCollection:=oClaimPerilPayment) Is Nothing Then
                                Exit Sub
                            End If
                        Else
                            If BindClaimCall(oOriginalClaim, bClaimTimeStamp, 3, oPayment, sBranchCode, bPaymentAuthorized) Is Nothing Then
                                Exit Sub
                            End If
                        End If

                    End If
                    Dim oClaimDetail As NexusProvider.ClaimOpen = Session.Item(CNClaim)
                    Dim oclaimRisk As New NexusProvider.ClaimRisk
                    Dim bClaimBuilder As Boolean = False
                    Boolean.TryParse(Session(CNClaimBuilder), bClaimBuilder)
                    If bClaimBuilder Then
                        ' get latest TimeStamp
                        oclaimRisk = GetClaimRiskCall(oClaimDetail.BaseClaimKey, oClaimDetail.ClaimKey, sBranchCode)
                    End If

                    'Claim Risk has wrong argument called ClaimKey, actually it is BaseClaimKey
                    oclaimRisk.ClaimKey = oClaimDetail.BaseClaimKey
                    oclaimRisk.TimeStamp = Session.Item(CNClaimRiskTimeStamp)
                    oclaimRisk.XMLDataSet = Session.Item(CNDataSet)


                    If oOriginalClaim IsNot Nothing AndAlso oOriginalClaim.CloseClaimOnFinalPayment = True Then
                        GetClaimDetails(oClaimDetail.ClaimKey, oclaimRisk)
                        Dim oOpenClaim As NexusProvider.ClaimOpen = CType(Session(CNClaim), NexusProvider.ClaimOpen)
                        oOpenClaim.ProgressStatusCode = "CLOSED"
                        Dim iTimestamp As Byte() = CType(Session.Item(CNClaimTimeStamp), Byte())
                        MaintainClaimCall(oOpenClaim, iTimestamp, sBranchCode)
                    End If
                    'Check to move to the accept another payment
                    oRunClaimWorkFlow = ViewState("RunClaimWorkFlow")

                    If oRunClaimWorkFlow.MakeFurtherPayments = True And hidChkChoice.Value.Trim.ToUpper = "TRUE" Then
                        GetLatestDetails() 'Update the session with latest values
                        Session.Remove(CNEnablePayClaim) 'Remove the ReadOnly mode of the Pay Claim
                        Session(CNPayClaim) = Nothing ' Reset the pay claim to accept the another payment
                        'Dummy Cal of PayClaim, to retreive back the latetst claim key
                        PayClaimWithZeroAmount()

                        Dim sUrl As String = CheckClaimBuilder()
                        Response.Redirect(sUrl)
                    Else
                        If bPaymentAuthorized Then
                            Session(CNAuthorizeStatus) = ""
                        End If
                        Response.Redirect("~/Claims/Complete.aspx")
                    End If
                Catch ex As NexusProvider.NexusException
                    If ex.Errors(0).Code = "331" Then   'Code : 331 :: Description: DebtorUserGroupsAreNotSetup

                        Dim cstDebtorUserGroups As New CustomValidator
                        cstDebtorUserGroups.IsValid = False
                        'look for a validation message in the page resources, but if there is not one defined add a default message
                        cstDebtorUserGroups.ErrorMessage = IIf(GetLocalResourceObject("cstDebtorUserGroups") Is Nothing, "Debtor User Groups are not setup. Please contact your system administrator", GetLocalResourceObject("cstDebtorUserGroups"))
                        cstDebtorUserGroups.Display = ValidatorDisplay.None 'we only want the error messages in the validation summary
                        'add the validator to the page, this will have the effect of making the page invalid
                        Page.Validators.Add(cstDebtorUserGroups)
                        Exit Sub
                    Else
                        Throw
                    End If
                End Try
            ElseIf Session(CNMode) = Mode.SalvageClaim Or Session(CNMode) = Mode.TPRecovery Then
                'Fire the Bind Claim
                Dim oQuote As NexusProvider.Quote = Session(CNClaimQuote)
                Dim sBranchCode As String = oQuote.BranchCode
                Dim bClaimTimeStamp() As Byte = Session.Item(CNClaimTimeStamp)
                Dim PerilsIndex As New System.Collections.Generic.List(Of Integer)
                Dim oPayment As New NexusProvider.ClaimPayment
                Dim oClaimPerilPayment As New NexusProvider.ClaimPaymentCollection
                Dim oOriginalClaim As NexusProvider.ClaimOpen = CType(Session.Item(CNClaim), NexusProvider.ClaimOpen)
                Try
                    If hidChlClaimClose.Value.Trim.ToUpper = "TRUE" Then
                        oOriginalClaim.CloseClaimOnZeroReserveRecoveryBalance = True
                    End If
                    oWebService = New NexusProvider.ProviderManager().Provider
                    GetClaimDetails(CType(Session.Item(CNClaim), NexusProvider.ClaimOpen).ClaimKey, Nothing)
                    If Session(CNClaimMultiPerilIndex) IsNot Nothing Then
                        PerilsIndex = Session(CNClaimMultiPerilIndex)
                        For Each PerilItemIndex As Integer In PerilsIndex
                            oPayment = CType(Session(CNClaim), NexusProvider.ClaimOpen).ClaimPeril(PerilItemIndex).Payment
                            oPayment.BaseClaimPerilKey = CType(Session.Item(CNClaim), NexusProvider.ClaimOpen).ClaimPeril(PerilItemIndex).ClaimPerilKey
                            oClaimPerilPayment.Add(oPayment)
                        Next
                    End If

                    If oClaimPerilPayment IsNot Nothing Then
                        Session.Item(CNClaim) = oOriginalClaim
                        BindClaimCall(oOriginalClaim, bClaimTimeStamp, 5, Nothing, sBranchCode, oPaymentCollection:=oClaimPerilPayment)
                    Else
                        BindClaimCall(oOriginalClaim, bClaimTimeStamp, 5, Nothing, sBranchCode)
                    End If

                Finally
                    oWebService = Nothing
                End Try

                'Check Make Furtehr Payments and hidden variable values to proceed further
                If hidChkChoice.Value IsNot Nothing Then
                    If hidChkChoice.Value.Trim.Length <> 0 Then
                        If hidChkChoice.Value.Trim.ToUpper = "TRUE" Then
                            Session.Remove(CNEnablePayClaim) 'Remove the ReadOnly mode of the Salvage/TP Recovery
                            GetLatestDetails()
                            Response.Redirect("~/Claims/Perils.aspx")
                        Else
                            Response.Redirect("~/Claims/Complete.aspx")
                        End If
                    Else
                        Response.Redirect("~/Claims/Complete.aspx")
                    End If
                Else
                    Response.Redirect("~/Claims/Complete.aspx")
                End If
            Else
                Response.Redirect("~/claims/complete.aspx")
            End If

        End Sub
#End Region
#Region "Protected Methods"
        Protected Sub PopulateGrid()
            oWebService = New NexusProvider.ProviderManager().Provider
            Dim dsArrangementGridData As DataSet = CType(Cache.Item(ViewState("ClaimReinsurancepageCacheID")), DataSet)

            If dsArrangementGridData Is Nothing Then
                Dim oClaim As NexusProvider.ClaimOpen = Nothing
                oClaim = CType(Session.Item(CNClaim), NexusProvider.ClaimOpen)

                dsArrangementGridData = oWebService.GetClaimReinsurance(oClaim.ClaimKey)
                Cache.Insert(ViewState("ClaimReinsurancepageCacheID"), dsArrangementGridData, Nothing, DateTime.MaxValue, TimeSpan.FromMinutes(5))
            End If

            If Session(CNMode) = Mode.PayClaim Or Session(CNMode) = Mode.ViewClaimPayment Then
                gvClaimReinsurance.Columns(6).Visible = True
                gvClaimReinsurance.Columns(7).Visible = False
            End If

            If Session(CNMode) = Mode.PayClaim AndAlso Session(CNLockPaymentGrid) IsNot Nothing AndAlso Session(CNLockPaymentGrid) = True Then
                gvClaimReinsurance.Columns(7).Visible = True
            End If

            ' assigning the dataset to the grid
            gvClaimReinsurance.DataSource = dsArrangementGridData.Tables("Current_" & ddlReinsurance.SelectedValue)
            gvClaimReinsurance.DataBind()
        End Sub

#End Region

        ''' <summary>
        ''' TO handle rowdata bound event. Few rows for totals will be marked as bold and yellow
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub gvClaimReinsurance_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvClaimReinsurance.RowDataBound
            If Trim(e.Row.Cells(0).Text) = GetLocalResourceObject("lbl_BandTotal").ToString() Or
                    Trim(e.Row.Cells(0).Text) = GetLocalResourceObject("lbl_Allocated").ToString() Or
                    Trim(e.Row.Cells(0).Text) = GetLocalResourceObject("lbl_TreatyTotal").ToString() Or
                    Trim(e.Row.Cells(0).Text) = GetLocalResourceObject("lbl_FacTotal").ToString() Then
                e.Row.CssClass = "summary"
            End If
        End Sub
    End Class
End Namespace
