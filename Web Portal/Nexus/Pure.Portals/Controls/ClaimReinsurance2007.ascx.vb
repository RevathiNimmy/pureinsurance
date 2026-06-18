Imports NexusProvider.SAMForInsurance
Imports Nexus
Imports Nexus.Utils
Imports System.Data
Imports Nexus.Constants
Imports Nexus.Constants.Session
Imports System.Xml
Imports Nexus.Library
Imports System.Configuration.ConfigurationManager
Imports CMS.Library.Portal
Imports CMS.Library

Namespace Nexus
    Partial Class Controls_ClaimReinsurance2007
        Inherits System.Web.UI.UserControl

        Dim oWebService As NexusProvider.ProviderBase
        Dim oQuote As NexusProvider.Claim
        Dim iArrangementId As Integer
        Dim sKey As String
#Region "Page Events"

        ''' <summary>
        ''' This event is fired on page load
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Shadows Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            'To set the Focus
            If Me.Visible Then
                Page.SetFocus(ddlReinsurance)

                hfAnotherPaymentMessage.Value = GetLocalResourceObject("msg_AnotherPayment").ToString()

                If Not IsPostBack Then
                    'this will be used to cache the results of the SAM call
                    Dim RIXMLpageCacheID As Guid
                    RIXMLpageCacheID = Guid.NewGuid()
                    ViewState.Add("RIXMLpageCacheID", RIXMLpageCacheID.ToString)

                    'call method to populate band
                    PopulateBand()
                    ' call made for populating the grid  
                    PopulateGrid()

                    Dim oQuote As NexusProvider.Quote = Session(CNClaimQuote)
                    Dim oClaim As NexusProvider.ClaimOpen = Nothing
                    oWebService = New NexusProvider.ProviderManager().Provider
                    Dim nClaimStatus As Integer
                    Dim dCurrentReserve As Decimal
                    Dim dCurrentRecovery As Decimal
                    Dim sOptionSettings As String
                    Dim bAllowNegativeReserve As Boolean = False
                    Dim bAllowClaimClosureOnZeroReserve As NexusProvider.OptionTypeSetting
                    bAllowClaimClosureOnZeroReserve = Nothing
                    Dim bStatus As Boolean = False

                    sOptionSettings = oWebService.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance, NexusProvider.ProductRiskOptions.AllowNegativeReserve, NexusProvider.RiskTypeOptions.None, oQuote.ProductCode, Nothing)
                    If sOptionSettings <> "0" Then
                        bAllowNegativeReserve = True
                    End If
                    bAllowClaimClosureOnZeroReserve = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, 5073)
                    If bAllowClaimClosureOnZeroReserve.OptionValue Is Nothing Then
                        bAllowClaimClosureOnZeroReserve.OptionValue = "0"
                    End If
                    oClaim = CType(Session.Item(CNClaim), NexusProvider.ClaimOpen)
                    oWebService.CheckReserveRecovery(oClaim.ClaimKey, nClaimStatus, dCurrentReserve, dCurrentRecovery)
                    If nClaimStatus <> 3 AndAlso (dCurrentReserve <= 0 AndAlso dCurrentRecovery <= 0) AndAlso (bAllowNegativeReserve = False OrElse (dCurrentReserve = 0 AndAlso dCurrentRecovery = 0)) Then
                        bStatus = True
                    Else
                        bStatus = False
                    End If

                    Dim oRunClaimWorkFlow As NexusProvider.ProductClaimsWorkflowOptionsValue
                    oRunClaimWorkFlow = oWebService.GetProductClaimsWorkflowOptions(NexusProvider.ClaimProcessType.ClaimPayment, oQuote.ProductCode)
                    ViewState("RunClaimWorkFlow") = oRunClaimWorkFlow

                    If Session(CNMode) = Mode.SalvageClaim Or Session(CNMode) = Mode.TPRecovery Then
                        If oRunClaimWorkFlow.MakeFurtherPayments = True Then
                            If bStatus = False Then
                                'To make another receipt confirmation 
                                btnNext.Attributes.Add("onclick", "javascript:return RecoveriesConfirmation(this,'" + GetLocalResourceObject("msg_AnotherRecovery").ToString() + "');")
                            Else
                                hidChkPaymentMsg.Value = "0"
                                'If this value is set then paymentconfirmatio will be called from
                                'ClamCloseConfirmation javascript function
                                btnNext.Attributes.Add("onclick", "return ClaimCloseConfirmation(this,'" + GetLocalResourceObject("msg_CloseClaim").ToString() + "');")
                            End If
                        ElseIf oRunClaimWorkFlow.MakeFurtherPayments = False Then
                            If bStatus = True Then
                                hidChkPaymentMsg.Value = String.Empty
                                btnNext.Attributes.Add("onclick", "return ClaimCloseConfirmation(this,'" + GetLocalResourceObject("msg_CloseClaim").ToString() + "');")
                            End If
                        End If


                    ElseIf Session(CNMode) = Mode.PayClaim Then ''Claim Payment
                        Dim sRunAuthorizePayment As String

                        'get the Product Risk option setting named Run Authorize Payment
                        sRunAuthorizePayment = oWebService.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance, NexusProvider.ProductRiskOptions.RunAuthorisationScriptsClaimPayments, NexusProvider.RiskTypeOptions.None, oQuote.ProductCode, Nothing)
                        Dim isOutstandingReserveZero As Boolean = False
                        If oRunClaimWorkFlow.MakeFurtherPayments = True Then
                            Dim oPerilColl As NexusProvider.PerilCollection = CType(Session(CNClaim), NexusProvider.ClaimOpen).ClaimPeril
                            'if the outstanding Reserve is Zero then show close claim message
                            If nClaimStatus <> 3 Then ' claim is not closed
                                Dim bCheckZeroReserve As Boolean = False
                                If bAllowClaimClosureOnZeroReserve IsNot Nothing Then
                                    If bAllowClaimClosureOnZeroReserve.OptionValue = "1" Then
                                        bCheckZeroReserve = True
                                    End If
                                End If

                                If bCheckZeroReserve Then
                                    If dCurrentReserve <= 0 AndAlso dCurrentRecovery <= 0 Then
                                        For j As Integer = 0 To oPerilColl.Count - 1
                                            For i As Integer = 0 To oPerilColl(j).ClaimReserve.Count - 1
                                                If oPerilColl(j).ClaimReserve(i).CurrentReserve = 0 Then
                                                    isOutstandingReserveZero = True
                                                ElseIf oPerilColl(j).ClaimReserve(i).CurrentReserve < 0 Then
                                                    If bAllowNegativeReserve Then
                                                        isOutstandingReserveZero = True
                                                    End If
                                                Else
                                                    isOutstandingReserveZero = False
                                                    Exit For
                                                End If
                                            Next
                                            If isOutstandingReserveZero = False Then
                                                Exit For
                                            End If
                                        Next
                                    End If
                                End If
                                'If bAllowNegativeReserve = False Or (bAllowNegativeReserve = True AndAlso dCurrentReserve <= 0 AndAlso dCurrentRecovery <= 0 AndAlso bAllowClaimClosureOnZeroReserve IsNot Nothing AndAlso bAllowClaimClosureOnZeroReserve.OptionValue = "1") Then ''dCurrentReserve <= 0 AndAlso dCurrentRecovery <= 0 AndAlso
                                '    'Check the reserve amount vs Payment amount

                                'End If
                            End If

                            If isOutstandingReserveZero = False Then
                                btnNext.Attributes.Add("onclick", "return PaymentConfirmation(this,'" + GetLocalResourceObject("msg_AnotherPayment").ToString() + "')")
                            Else
                                hidChkPaymentMsg.Value = "0"
                                'If this value is set then paymentconfirmatio will be called from
                                'ClamCloseConfirmation javascript function
                                btnNext.Attributes.Add("onclick", "return ClaimCloseConfirmation(this,'" + GetLocalResourceObject("msg_CloseClaim").ToString() + "');")
                            End If
                        ElseIf oRunClaimWorkFlow.MakeFurtherPayments = False Then
                            If isOutstandingReserveZero = True Then
                                hidChkPaymentMsg.Value = String.Empty
                                btnNext.Attributes.Add("onclick", "return ClaimCloseConfirmation(this,'" + GetLocalResourceObject("msg_CloseClaim").ToString() + "');")
                            End If
                        End If
                    ElseIf Session(CNMode) = Mode.EditClaim Then
                        If bStatus Then
                            hidChkPaymentMsg.Value = String.Empty
                            btnNext.Attributes.Add("onclick", "return ClaimCloseConfirmation(this,'" + GetLocalResourceObject("msg_CloseClaim").ToString() + "');")
                        End If
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

        Protected Sub gvClaimReinsurance_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvClaimReinsurance.DataBound
            gvClaimReinsurance.Columns(0).Visible = False
            gvClaimReinsurance.Columns(1).Visible = False
            gvClaimReinsurance.Columns(2).Visible = False
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
            'Bind Claim
            Dim oExclusiveLocking As NexusProvider.OptionTypeSetting
            Dim bPaymentAuthorized As Boolean = False
            If Session(CNMode) = Mode.NewClaim Then
                'Update the Changed RI Arrangement to DB
                UdateRIArrangement()

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
                'Update the Changed RI Arrangement to DB (GAP 3 fix)
                UdateRIArrangement()

                'Fire the Bind Claim
                Dim oQuote As NexusProvider.Quote = Session(CNClaimQuote)
                Dim sBranchCode As String = oQuote.BranchCode
                Dim oOriginalClaim As NexusProvider.ClaimOpen = CType(Session.Item(CNClaim), NexusProvider.ClaimOpen)
                Dim bClaimTimeStamp() As Byte = Session.Item(CNClaimTimeStamp)
                oWebService = New NexusProvider.ProviderManager().Provider
                If hidChlClaimClose.Value.Trim.ToUpper = "TRUE" Then
                    oOriginalClaim.CloseClaimOnZeroReserveRecoveryBalance = True
                End If

                If BindClaimCall(oOriginalClaim, bClaimTimeStamp, 2, Nothing, sBranchCode) Is Nothing Then
                    Exit Sub
                End If
                Response.Redirect("~/Claims/Complete.aspx")
            ElseIf Session(CNMode) = Mode.PayClaim Then
                'Update the Changed RI Arrangement to DB (GAP 3 fix)
                UdateRIArrangement()

                'Fire the Bind Claim
                Dim oRunClaimWorkFlow As NexusProvider.ProductClaimsWorkflowOptionsValue
                Dim oQuote As NexusProvider.Quote = Session(CNClaimQuote)
                Dim oPayment As NexusProvider.ClaimPayment = CType(Session(CNClaim), NexusProvider.ClaimOpen).ClaimPeril(Session(CNClaimPerilIndex)).Payment
                Dim oOriginalClaim As NexusProvider.ClaimOpen = CType(Session.Item(CNClaim), NexusProvider.ClaimOpen)
                Dim bClaimTimeStamp() As Byte = Session.Item(CNClaimTimeStamp)
                Dim sBranchCode As String = oQuote.BranchCode
                oWebService = New NexusProvider.ProviderManager().Provider
                'Need to close the claim if Payment Amount is exceeding or equal to reserve amount for any peril
                If hidChlClaimClose.Value.Trim.ToUpper = "TRUE" Then
                    oOriginalClaim.CloseClaimOnZeroReserveRecoveryBalance = True
                    oPayment.CloseClaimOnZeroReserveRecoveryBalance = True
                Else
                    oOriginalClaim.CloseClaimOnZeroReserveRecoveryBalance = False
                    oPayment.CloseClaimOnZeroReserveRecoveryBalance = False
                End If

                Try

                    'Check to move to the accept another payment
                    oRunClaimWorkFlow = ViewState("RunClaimWorkFlow")

                    Dim bReserveEdited As Boolean = False
                    Dim oClaimReserve As NexusProvider.ReserveCollection = CType(Session(CNClaim), NexusProvider.ClaimOpen).ClaimPeril(Session(CNClaimPerilIndex)).Reserve
                    For Each oResrve As NexusProvider.Reserve In oClaimReserve
                        If oResrve.ReserveEdited Then
                            bReserveEdited = True
                            Exit For
                        End If
                    Next

                    If Session(CNMode) = Mode.PayClaim AndAlso (Session(CNLockPaymentGrid) IsNot Nothing AndAlso Session(CNLockPaymentGrid) = True) AndAlso bReserveEdited Then
                        'Fire the Bind Claim
                        If BindClaimCall(oOriginalClaim, bClaimTimeStamp, 4, oPayment, sBranchCode) Is Nothing Then
                            Exit Sub
                        End If
                    Else
                        'Fire the Bind Claim
                        If BindClaimCall(oOriginalClaim, bClaimTimeStamp, 3, oPayment, sBranchCode, bPaymentAuthorized) Is Nothing Then
                            Exit Sub
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

                    ' Confirmation for further payments 
                    Dim FurtherPaymentConfirm As String = hidChkChoice.Value
                    If FurtherPaymentConfirm = "True" Then
                        oRunClaimWorkFlow.MakeFurtherPayments = True
                    End If

                    If oRunClaimWorkFlow.MakeFurtherPayments = True And hidChkChoice.Value.Trim.ToUpper = "TRUE" Then
                        GetLatestDetails() 'Update the session with latest values
                        Session.Remove(CNEnablePayClaim) 'Remove the ReadOnly mode of the Pay Claim
                        Session(CNPayClaim) = Nothing ' Reset the pay claim to accept the another payment
                        'Dummy Cal of PayClaim, to retreive back the latetst claim key
                        PayClaimWithZeroAmount()

                        'Lock the claim 
                        oExclusiveLocking = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, NexusProvider.SystemOptions.ExclusiveLock)
                        If oExclusiveLocking.OptionValue = "1" Then
                            Dim oClaimDetails As NexusProvider.ClaimDetails = Nothing
                            oClaimDetails = oWebService.GetClaimDetails(v_iClaimKey:=oOriginalClaim.ClaimKey, v_sBranchCode:=sBranchCode, bExclusiveLock:=True)
                        End If

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
                    ElseIf CType(ex.Errors(0), NexusProvider.NexusError).Code = "1000158" Then 'Claim is locked for modification as already in use
                        'Show Claim locking error as alert
                        Dim sMessage As String = "alert('" + Replace(GetLocalResourceObject("lbl_ClaimLock_error"), "{1}", (ex.Errors(0).Detail.Split(":"))(2) + ".") + "')"
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "claimlocked", sMessage, True)
                        Server.ClearError()
                        'Clear all claim related sessions and throw the error
                        ClearClaims()
                        Exit Sub
                    Else
                        Throw
                    End If
                End Try

            ElseIf Session(CNMode) = Mode.SalvageClaim Or Session(CNMode) = Mode.TPRecovery Then
                'Update the Changed RI Arrangement to DB (GAP 3 fix)
                UdateRIArrangement()

                'Fire the Bind Claim
                Dim oQuote As NexusProvider.Quote = Session(CNClaimQuote)
                Dim sBranchCode As String = oQuote.BranchCode
                Dim oOriginalClaim As NexusProvider.ClaimOpen = CType(Session.Item(CNClaim), NexusProvider.ClaimOpen)
                Dim bClaimTimeStamp() As Byte = Session.Item(CNClaimTimeStamp)

                Try
                    If hidChlClaimClose.Value.Trim.ToUpper = "TRUE" Then
                        oOriginalClaim.CloseClaimOnZeroReserveRecoveryBalance = True
                    End If

                    oWebService = New NexusProvider.ProviderManager().Provider


                    If BindClaimCall(oOriginalClaim, bClaimTimeStamp, 5, Nothing, sBranchCode) Is Nothing Then
                        Exit Sub
                    End If
                    oExclusiveLocking = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, NexusProvider.SystemOptions.ExclusiveLock)

                Finally
                    oWebService = Nothing
                End Try

                'Check Make Furtehr Payments and hidden variable values to proceed further
                If hidChkChoice.Value IsNot Nothing Then
                    If hidChkChoice.Value.Trim.Length <> 0 Then
                        If hidChkChoice.Value.Trim.ToUpper = "TRUE" Then
                            Session.Remove(CNEnablePayClaim) 'Remove the ReadOnly mode of the Salvage/TP Recovery
                            'Lock the claim 
                            If oExclusiveLocking.OptionValue = "1" Then
                                GetLatestDetails(bExclusiveLock:=True)
                            Else
                                GetLatestDetails()
                            End If

                            RedirectCLaimBuilderScreen()
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
        Private Sub RedirectCLaimBuilderScreen()

            Dim oClaimResponse As NexusProvider.ClaimResponse = Nothing
            Dim oClaimRisk As NexusProvider.ClaimRisk = Nothing
            Dim oClaimDetails As NexusProvider.ClaimDetails = Nothing
            Dim oInitialClaim As NexusProvider.ClaimOpen = CType(Session(CNClaim), NexusProvider.ClaimOpen)
            Dim oQuote As NexusProvider.Quote = Session(CNClaimQuote)
            Dim oUserDetails As NexusProvider.UserDetails = CType(Session(CNAgentDetails), NexusProvider.UserDetails)
            Dim sBranchCode As String = oQuote.BranchCode
            Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider

            'Make the Claim version description dynamically

            Dim sDescription As String = "Third Party Recovery"

            If CType(Session.Item(CNMode), Mode) = Mode.SalvageClaim Then
                sDescription = "Salvage Recovery"
            End If
            oInitialClaim.ClaimVersionDescription = sDescription

            'Call the SAM method MaintainClaim
            'To Skip the posting
            oInitialClaim.ReserveOnly = True
            'arch issue 268
            'oClaimResponse = oWebservice.MaintainClaim(oInitialClaim, bTimeStamp)
            Dim bTimeStamp As Byte() = CType(Session(CNClaimTimeStamp), Byte())
            oClaimResponse = MaintainClaimCall(oInitialClaim, bTimeStamp)

            'Calling of Latest Claim Details after Maintain Claim
            'arch issue 268
            oClaimDetails = GetClaimDetailsCall(oClaimResponse.ClaimKey, sBranchCode)

            'Check the Claim Builder Hidden product option
            Dim oOptionType As New NexusProvider.OptionTypeSetting
            oOptionType = oWebservice.GetOptionSetting(NexusProvider.OptionType.ProductOption, 12)
            If (oOptionType IsNot Nothing AndAlso String.IsNullOrEmpty(oOptionType.OptionValue) = False) _
               AndAlso oOptionType.OptionValue = "1" Then
                'Calling of the risk realted values after maintain claim
                oClaimRisk = GetClaimRiskCall(oClaimDetails.BaseClaimKey, oClaimDetails.ClaimKey, sBranchCode)
                If oClaimRisk IsNot Nothing Then
                    Session.Item(CNDataSet) = oClaimRisk.XMLDataSet
                    Session(CNClaimRiskTimeStamp) = oClaimRisk.TimeStamp
                End If
            End If
            'Updation of latest claim session variables
            Session.Item(CNClaimTimeStamp) = oClaimDetails.TimeStamp
            Session.Item(CNBaseClaimKey) = oClaimResponse.BaseClaimKey
            Session.Item(CNClaimKey) = oClaimResponse.ClaimKey
            Session.Item(CNClaimNumber) = oClaimResponse.ClaimNumber
            'Updation of latest claim session variables
            With oClaimDetails
                oInitialClaim.CatastropheCode = .CatastropheCode
                oInitialClaim.BaseCaseKey = .BaseCaseKey
                oInitialClaim.BaseClaimKey = .BaseClaimKey
                oInitialClaim.Claim = .Claim
                oInitialClaim.ClaimCoInsurer = .ClaimCoInsurer
                oInitialClaim.ClaimDescription = .ClaimDescription
                oInitialClaim.ClaimHandlerDescription = .ClaimHandlerDescription
                oInitialClaim.ClaimKey = .ClaimKey
                oInitialClaim.ClaimNumber = .ClaimNumber
                oInitialClaim.ClaimPeril = .ClaimPeril
                oInitialClaim.ClaimStatus = .ClaimStatus
                oInitialClaim.ClaimStatusDate = .ClaimStatusDate
                oInitialClaim.ClaimStatusID = .ClaimStatusID
                oInitialClaim.ClaimVersion = .ClaimVersion
                oInitialClaim.ClaimVersionDescription = .ClaimVersionDescription
                oInitialClaim.ClientClaimNumber = .ClientClaimNumber
                oInitialClaim.ClientEmail = .ClientEmail
                oInitialClaim.ClientFaxNo = .ClientFaxNo
                oInitialClaim.ClientMobileNo = .ClientMobileNo
                oInitialClaim.ClientName = .ClientName
                oInitialClaim.ClientShortName = .ClientShortName
                oInitialClaim.ClientTelNo = .ClientTelNo
                oInitialClaim.ClientTelNoOff = .ClientTelNoOff
                oInitialClaim.CloseClaimOnZeroReserveRecoveryBalance = .CloseClaimOnZeroReserveRecoveryBalance
                oInitialClaim.Comments = .Comments
                oInitialClaim.Contact = .Contact
                oInitialClaim.CurrencyISOCode = .CurrencyISOCode
                oInitialClaim.Description = .Description
                oInitialClaim.ExternalHandler = .ExternalHandler
                oInitialClaim.HandlerCode = .HandlerCode
                oInitialClaim.IgnoreClaimMaintain = .IgnoreClaimMaintain
                oInitialClaim.InfoOnly = .InfoOnly
                oInitialClaim.InsuranceFileKey = .InsuranceFileKey
                oInitialClaim.InsuranceRef = .InsuranceRef
                oInitialClaim.InsurerClaimNumber = .InsurerClaimNumber
                oInitialClaim.IsAllowedClosedClaims = .IsAllowedClosedClaims
                oInitialClaim.IsDeleted = .IsDeleted
                oInitialClaim.LastModifiedDate = .LastModifiedDate
                oInitialClaim.LikelyClaim = .LikelyClaim
                oInitialClaim.Location = .Location
                oInitialClaim.LossDate = .LossDate
                oInitialClaim.LossDateFrom = .LossDateFrom
                oInitialClaim.LossFromDate = .LossToDate
                oInitialClaim.LossToDate = .LossToDate
                oInitialClaim.LossToDateSpecified = .LossToDateSpecified
                oInitialClaim.Payments = .Payments
                oInitialClaim.PolicyNumber = .PolicyNumber
                oInitialClaim.PolicyType = .PolicyType
                oInitialClaim.PrimaryCause = .PrimaryCause
                oInitialClaim.PrimaryCauseCode = .PrimaryCauseCode
                oInitialClaim.PrimaryCauseDescription = .PrimaryCauseDescription
                oInitialClaim.ProductDescription = .ProductDescription
                oInitialClaim.ProgressStatusCode = .ProgressStatusCode
                oInitialClaim.ProgressStatusDescription = .ProgressStatusDescription
                oInitialClaim.ReportedDate = .ReportedDate
                oInitialClaim.RiskKey = .RiskKey
                oInitialClaim.RiskType = CType(Session(CNClaimQuote), NexusProvider.Quote).Risks.FindItemByRiskKey(.RiskKey).RiskTypeCode
                oInitialClaim.RiskTypeDescription = CType(Session(CNClaimQuote), NexusProvider.Quote).Risks.FindItemByRiskKey(.RiskKey).Description
                oInitialClaim.SecondaryCause = .SecondaryCause
                oInitialClaim.SecondaryCauseCode = .SecondaryCauseCode
                oInitialClaim.SecondaryCauseDescription = .SecondaryCauseDescription
                oInitialClaim.TotalCurrentShareValue = .TotalCurrentShareValue
                oInitialClaim.TotalShare = .TotalShare
                oInitialClaim.Town = .Town
                oInitialClaim.TownCode = .TownCode
                oInitialClaim.UnderwritingYearCode = .UnderwritingYearCode
                oInitialClaim.UserDefFldACode = .UserDefFldACode
                oInitialClaim.UserDefFldBCode = .UserDefFldBCode
                oInitialClaim.UserDefFldCCode = .UserDefFldCCode
                oInitialClaim.UserDefFldDCode = .UserDefFldECode
                oInitialClaim.UserDefFldECode = .UserDefFldECode
            End With

            'take the latest claim information into Session
            Session.Item(CNClaim) = oInitialClaim

            'check the claim builder hidden product option
            oOptionType = oWebservice.GetOptionSetting(NexusProvider.OptionType.ProductOption, 12)
            If (oOptionType IsNot Nothing AndAlso String.IsNullOrEmpty(oOptionType.OptionValue) = False) _
                AndAlso oOptionType.OptionValue = "1" Then
                'Calling of the risk realted values after maintain claim
                'Arch issue 268
                oClaimRisk = GetClaimRiskCall(oClaimResponse.BaseClaimKey, oClaimResponse.ClaimKey, sBranchCode)

                Session.Item(CNDataSet) = oClaimRisk.XMLDataSet
                Session(CNClaimRiskTimeStamp) = oClaimRisk.TimeStamp
            End If

            Session(CNClaimNumber) = oClaimResponse.ClaimNumber
            Session(CNClaim) = oInitialClaim

            'Check the claim builder ON or OFF
            Dim sUrl As String = CheckClaimBuilder()
            If Trim(sUrl) <> "" Then
                Response.Redirect(sUrl, False)
            Else
                Response.Redirect("~/Claims/Perils.aspx")
            End If
        End Sub
#End Region
#Region "Protected Methods"
        ''' <summary>
        ''' populate RI band
        ''' </summary>
        ''' <remarks></remarks>
        Sub PopulateBand()
            oWebService = New NexusProvider.ProviderManager().Provider
            Dim oClaim As NexusProvider.ClaimOpen = Nothing
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
        End Sub
        ''' <summary>
        ''' update Ri Arrangement
        ''' </summary>
        ''' <remarks></remarks>
        Sub UdateRIArrangement()
            oWebService = New NexusProvider.ProviderManager().Provider
            Dim oXMLData As String = Convert.ToString(Session(CNRIXMLData))
            Dim RIBANDID As String = "Current_" & ddlReinsurance.SelectedValue
            Dim oArrangementLineColl As New NexusProvider.ArrangementLinesTypeCollection
            Dim oArrangementLine As NexusProvider.ArrangementLinesType
            Dim iRIArrangementLineKey As Integer

            If String.IsNullOrEmpty(oXMLData) = False Then
                Dim oXMLDoc As New XmlDocument
                oXMLDoc.LoadXml(oXMLData)
                Dim xmlNodes As XmlNodeList = oXMLDoc.SelectNodes("/rows/RIBAND[@Name='" & RIBANDID & "']/ArrangementRow[@IsEdited='True']")

                If xmlNodes IsNot Nothing AndAlso xmlNodes.Count > 0 Then

                    For Each oArrangeLineNode As XmlNode In xmlNodes
                        oArrangementLine = New NexusProvider.ArrangementLinesType

                        Integer.TryParse(oArrangeLineNode.Attributes("RIArrangementLineKey").Value, iRIArrangementLineKey)
                        If iRIArrangementLineKey <> 0 Then
                            oArrangementLine = oWebService.GetRIArrangementLineFromXML(iRIArrangementLineKey, oXMLData, True)
                        End If
                        oArrangementLineColl.Add(oArrangementLine)
                    Next
                End If

                'If any record is found changed then update it in DB
                If oArrangementLineColl IsNot Nothing AndAlso oArrangementLineColl.Count > 0 Then
                    oWebService.UpdateClaimRIArrangementLinesRI2007(oArrangementLineColl)
                End If

            End If
        End Sub
        ''' <summary>
        ''' Populate the grid
        ''' </summary>
        ''' <remarks></remarks>
        Protected Sub PopulateGrid()
            oWebService = New NexusProvider.ProviderManager().Provider
            Dim oClaim As NexusProvider.ClaimOpen = Nothing
            oClaim = CType(Session.Item(CNClaim), NexusProvider.ClaimOpen)
            Dim oXMLSource As New XmlDataSource
            Dim oXMLData As String = Session(CNRIXMLData)
            oXMLSource.EnableCaching = False
            oQuote = Session(CNQuote)

            If String.IsNullOrEmpty(oXMLData) = True Then
                ' GAP 6: Load the RI model for the specific claim version so that any overridden
                ' model (IsEdited/OverrideReasonId) is picked up rather than the default settings.
                Dim iClaimKey As Integer = 0
                If Session(CNMode) = Mode.PayClaim Then
                    iClaimKey = oClaim.ClaimKey
                   ' oXMLData = oWebService.GetClaimReinsurance2007(oClaim.ClaimKey)

                ElseIf Session(CNMode) = Mode.SalvageClaim Or Session(CNMode) = Mode.TPRecovery Then
                    Dim oClaimReceipt As NexusProvider.ClaimReceipt = CType(Session(CNClaim), NexusProvider.ClaimOpen).ClaimPeril(Session(CNClaimPerilIndex)).Receipt
                    'oXMLData = oWebService.GetClaimReinsurance2007(oClaimReceipt.ClaimKey)
                    iClaimKey = oClaimReceipt.ClaimKey
                Else
                    'oXMLData = oWebService.GetClaimReinsurance2007(oClaim.ClaimKey)
                    iClaimKey = oClaim.ClaimKey
                End If
                oXMLData = oWebService.GetClaimReinsurance2007(iClaimKey)
                Session(CNRIXMLData) = oXMLData
            End If

            oXMLSource.Data = oXMLData
            oXMLSource.XPath = ".//RIBAND[@Name='Current_" & ddlReinsurance.SelectedValue & "']/ArrangementRow"

            gvClaimReinsurance.DataSource = oXMLSource
            gvClaimReinsurance.DataBind()
        End Sub
#End Region

        Protected Sub gvClaimReinsurance_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvClaimReinsurance.RowDataBound
            Dim oClaim As NexusProvider.Claim = Session(CNClaim)
            If e.Row.RowType = DataControlRowType.Header Then
                gvClaimReinsurance.Columns(0).Visible = True
                gvClaimReinsurance.Columns(1).Visible = True
                gvClaimReinsurance.Columns(2).Visible = True

                'This Payment Label should be changed in case of Recovery 
                If Session(CNMode) = Mode.SalvageClaim Or Session(CNMode) = Mode.TPRecovery Or oClaim.IsRecovery = True Then
                    e.Row.Cells(14).Text = Replace(GetLocalResourceObject("lblThisRecovery"), "[!Currency!]", TransactionCurrency.Symbol)
                End If
            ElseIf e.Row.RowType = DataControlRowType.DataRow Then

                Dim oRngThisPerc As RangeValidator = CType(e.Row.FindControl("rngThisPerc"), RangeValidator)
                Dim txtThisPerc As TextBox = e.Row.FindControl("txtThisPerc")
                Dim lblThisPerc As Label = e.Row.FindControl("lblThisPerc")
                Dim lnkView As LinkButton = e.Row.FindControl("lnkView")
                Dim sPlacement As String = e.Row.Cells(3).Text
                Dim iRIArrangementLineKey, iRIArrangementKey As Integer
                Dim bIsRIBroker As Boolean = False

                Dim oFormatStringPercentage As String = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(GetPortalID()).FormatStrings.FormatString("Percentage").DataFormatString
                Dim oFormatStringCurrency As String = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(GetPortalID()).FormatStrings.FormatString("Currency").DataFormatString

                'Formatting
                Dim dDefaultPerc As Decimal
                Decimal.TryParse(e.Row.Cells(6).Text, dDefaultPerc)
                e.Row.Cells(6).Text = String.Format("{0:N4}%", dDefaultPerc)

                Dim dRetPerc As Decimal
                Decimal.TryParse(e.Row.Cells(5).Text, dRetPerc)
                e.Row.Cells(5).Text = String.Format("{0:N0}%", dRetPerc)

                'TextBox
                Dim dThisPerc As Decimal
                Decimal.TryParse(txtThisPerc.Text, dThisPerc)
                txtThisPerc.Text = String.Format("{0:N4}%", dThisPerc)
                'Label
                Decimal.TryParse(lblThisPerc.Text, dThisPerc)
                lblThisPerc.Text = String.Format("{0:N4}%", dThisPerc)

                Dim dLowerLimit As Decimal
                Decimal.TryParse(e.Row.Cells(8).Text, dLowerLimit)
                e.Row.Cells(8).Text = String.Format(oFormatStringCurrency, dLowerLimit)

                Dim dLineLimit As Decimal
                Decimal.TryParse(e.Row.Cells(9).Text, dLineLimit)
                e.Row.Cells(9).Text = String.Format(oFormatStringCurrency, dLineLimit)

                Dim dSumInsured As Decimal
                Decimal.TryParse(e.Row.Cells(10).Text, dSumInsured)
                e.Row.Cells(10).Text = String.Format(oFormatStringCurrency, dSumInsured)

                Dim dReserveToDate As Decimal
                Decimal.TryParse(e.Row.Cells(11).Text, dReserveToDate)
                e.Row.Cells(11).Text = String.Format(oFormatStringCurrency, dReserveToDate)

                Dim dThisReserve As Decimal
                Decimal.TryParse(e.Row.Cells(12).Text, dThisReserve)
                e.Row.Cells(12).Text = String.Format(oFormatStringCurrency, dThisReserve)

                Dim dPaymentToDate As Decimal
                Decimal.TryParse(e.Row.Cells(13).Text, dPaymentToDate)
                e.Row.Cells(13).Text = String.Format(oFormatStringCurrency, dPaymentToDate)

                Dim dThisPayment As Decimal
                Decimal.TryParse(e.Row.Cells(14).Text, dThisPayment)
                e.Row.Cells(14).Text = String.Format(oFormatStringCurrency, dThisPayment)

                Dim dRecoverToDate As Decimal
                Decimal.TryParse(e.Row.Cells(15).Text, dRecoverToDate)
                e.Row.Cells(15).Text = String.Format(oFormatStringCurrency, dRecoverToDate)

                Dim dBalance As Decimal
                Decimal.TryParse(e.Row.Cells(16).Text, dBalance)
                e.Row.Cells(16).Text = String.Format(oFormatStringCurrency, dBalance)
                '----- End Formatting----------

                Integer.TryParse(e.Row.Cells(0).Text, iRIArrangementKey)
                Integer.TryParse(e.Row.Cells(1).Text, iRIArrangementLineKey)
                Boolean.TryParse(e.Row.Cells(2).Text, bIsRIBroker)

                If String.IsNullOrEmpty(sPlacement) = False AndAlso (sPlacement.Trim.ToUpper = "FAC PROP" _
                Or sPlacement.Trim.ToUpper = "TREATY QSH" Or sPlacement.Trim.ToUpper = "TREATY SURPLUS") Then

                    'Lower Limit
                    e.Row.Cells(8).Text = String.Empty
                    'Upper Limit
                    e.Row.Cells(9).Text = String.Empty
                    'Retained
                    e.Row.Cells(5).Text = String.Empty

                    lblThisPerc.Visible = True

                    If String.IsNullOrEmpty(sPlacement) = False AndAlso sPlacement.Trim.ToUpper = "FAC PROP" Then

                        lnkView.Visible = True
                        If CheckBrokerExist(iRIArrangementLineKey) = False Then
                            Dim sMessage As String = Nothing
                            sMessage = GetLocalResourceObject("lbl_FAC_Placment_Err")
                            lnkView.Attributes.Add("onclick", "javascript:return ShowMsg('" & sMessage & "','null')")
                        ElseIf CheckBrokerExist(iRIArrangementLineKey) = True Then
                            lnkView.PostBackUrl = "~/Modal/RIBrokerParticipant.aspx?Type=FACPROP&Mode=VIEW&RIArrangementLineKey=" & iRIArrangementLineKey
                        Else
                            Dim sMessage As String = Nothing
                            sMessage = GetLocalResourceObject("lbl_FAC_Broker_Err")
                            lnkView.Attributes.Add("onclick", "javascript:ShowMsg('" & sMessage & "','" & iRIArrangementLineKey & "')")
                        End If
                    End If

                ElseIf String.IsNullOrEmpty(sPlacement) = False AndAlso sPlacement.Trim.ToUpper = "FAC XOL" Then
                    lnkView.Visible = True
                    e.Row.Cells(6).Text = String.Empty
                    e.Row.Cells(7).Text = String.Empty
                    lnkView.PostBackUrl = "~/Modal/FACPlacement.aspx?Type=FACXOL&Mode=VIEW&RIArrangementLineKey=" & iRIArrangementLineKey
                    lblThisPerc.Visible = True

                    'Retained
                    e.Row.Cells(5).Text = String.Empty
                ElseIf sPlacement.Trim.ToUpper = "NET" Then
                    'Lower Limit
                    e.Row.Cells(8).Text = String.Empty
                    'Upper Limit
                    e.Row.Cells(9).Text = String.Empty
                    'For This Perc
                    'Retained
                    lblThisPerc.Text = String.Empty
                ElseIf sPlacement.Trim.ToUpper = "TREATY XOL" Or sPlacement.Trim.ToUpper = "TREATY CAT" Then
                    'For This Perc
                    lblThisPerc.Text = String.Empty
                    'Retained
                    e.Row.Cells(5).Text = String.Empty
                Else
                    'Retained
                    e.Row.Cells(5).Text = String.Empty
                    lnkView.Visible = False
                End If
                'Applying the CSS for styling
                If Trim(e.Row.Cells(3).Text).ToUpper = "GROSS NET" Or Trim(e.Row.Cells(4).Text).ToUpper = "ALLOCATED" Or Trim(e.Row.Cells(4).Text).ToUpper = "UNALLOCATED" Or Trim(e.Row.Cells(3).Text).ToUpper = "GROSS" Or Trim(e.Row.Cells(3).Text).ToUpper = "NET" Then
                    e.Row.CssClass = "summary"
                    If Trim(e.Row.Cells(3).Text).ToUpper <> "NET" Then
                        e.Row.Cells(5).Text = String.Empty
                        e.Row.Cells(6).Text = String.Empty

                    End If
                    'Lower Limit
                    e.Row.Cells(8).Text = String.Empty
                    'Upper Limit
                    e.Row.Cells(9).Text = String.Empty

                End If

            End If
        End Sub

        Protected Sub txtThisPerc_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
            Dim value As String = CType(sender, TextBox).Text
            Dim oReinsurance As New Reinsurance.Reinsurance
            Dim RIBANDID As String = "Current_" & ddlReinsurance.SelectedValue
            Dim oXMLData As String = Convert.ToString(Session(CNRIXMLData))
            If String.IsNullOrEmpty(value) Then
                value = 0.0
            End If
            oReinsurance.UpdateClaimThisPercentage(oXMLData, value, RIBANDID, CType(sender, TextBox).ToolTip)

            Session(CNRIXMLData) = oXMLData
            'Update the Grid with latest calculation
            PopulateGrid()
        End Sub
        ''' <summary>
        ''' This method checks the Broker Existence in FAC PROP
        ''' </summary>
        ''' <param name="iRIArrangementLineKey"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function CheckBrokerExist(ByVal iRIArrangementLineKey As Integer) As Boolean
            Dim bReturn As Boolean = False
            Dim oArrangementType As NexusProvider.ArrangementLinesType
            oWebService = New NexusProvider.ProviderManager().Provider
            oArrangementType = oWebService.GetRIArrangementLineFromXML(iRIArrangementLineKey, Session(CNRIXMLData), True)

            If oArrangementType.BrokerParticipants IsNot Nothing AndAlso oArrangementType.BrokerParticipants.Count > 0 Then
                bReturn = True
            End If

            Return bReturn
        End Function

        Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
            If Session(CNMode) = Mode.NewClaim Then
                Response.Redirect("~\claims\FindInsuranceFile.aspx")
            Else
                Response.Redirect("~\claims\FindClaim.aspx")
            End If
        End Sub
    End Class
End Namespace
