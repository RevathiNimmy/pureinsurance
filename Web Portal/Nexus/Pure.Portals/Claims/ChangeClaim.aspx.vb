Imports System.Web.Configuration.WebConfigurationManager
Imports Nexus
Imports Nexus.Constants
Imports Nexus.Constants.Session
Imports Nexus.Library
Imports CMS.Library

Namespace Nexus

    Partial Class Claims_ChangeClaim
        Inherits CMS.Library.Frontend.clsCMSPage

        Dim sRunAuthorizePayment As String
        Dim oRunClaimWorkFlow As NexusProvider.ProductClaimsWorkflowOptionsValue

        ''' <summary>
        ''' This method will be fired on press of OK button, after selecting the Reason For Change
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub btnOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOk.Click

            Dim oWebservice As NexusProvider.ProviderBase
            Dim bTimeStamp As Byte() = CType(Session(CNClaimTimeStamp), Byte())
            Dim oExclusiveLocking As NexusProvider.OptionTypeSetting
            Dim sddlReasonSelectedValue As String = String.Empty

            If ddlReason.SelectedItem.Text.ToString IsNot Nothing AndAlso ddlReason.SelectedItem.Text <> "" Then
                sddlReasonSelectedValue = ddlReason.SelectedItem.Text
            Else
                sddlReasonSelectedValue = ddlReason.SelectedValue
            End If

            Dim oInitialClaim As NexusProvider.ClaimOpen = CType(Session(CNClaim), NexusProvider.ClaimOpen)
            Select Case CType(Session(CNMode), Mode)
                Case Mode.EditClaim
                    Dim oClaimResponse As NexusProvider.ClaimResponse = Nothing
                    Dim oClaimRisk As NexusProvider.ClaimRisk = Nothing
                    Dim oClaimDetails As NexusProvider.ClaimDetails = Nothing

                    Dim oQuote As NexusProvider.Quote = Session(CNClaimQuote)
                    Dim oUserDetails As NexusProvider.UserDetails = CType(Session(CNAgentDetails), NexusProvider.UserDetails)
                    Dim sBranchCode As String = oQuote.BranchCode

                    'Make the Claim version description dynamically
                    oInitialClaim.ClaimVersionDescription = GetLocalResourceObject("lbl_ClaimReason_" & Session(CNMode)) & ddlReason.SelectedValue & " - " & txtIfOther.Text

                    'Call the SAM method MaintainClaim
                    'To Skip the posting
                    oInitialClaim.ReserveOnly = True
                    'arch issue 268
                    'oClaimResponse = oWebservice.MaintainClaim(oInitialClaim, bTimeStamp)
                    oClaimResponse = MaintainClaimCall(oInitialClaim, bTimeStamp, oQuote.BranchCode)

                    'Calling of Latest Claim Details after Maintain Claim
                    'arch issue 268
                    oClaimDetails = GetClaimDetailsCall(oClaimResponse.ClaimKey, sBranchCode)

                    'Check the Claim Builder Hidden product option
                    Dim oOptionType As New NexusProvider.OptionTypeSetting

                    oWebservice = New NexusProvider.ProviderManager().Provider
                    oOptionType = oWebservice.GetOptionSetting(NexusProvider.OptionType.ProductOption, 12)
                    If (oOptionType IsNot Nothing AndAlso String.IsNullOrEmpty(oOptionType.OptionValue) = False) _
                       AndAlso oOptionType.OptionValue = "1" Then
                        'Calling of the risk realted values after maintain claim
                        oClaimRisk = GetClaimRiskCall(oClaimDetails.BaseClaimKey, sBranchCode)
                        If oClaimRisk IsNot Nothing Then
                            Session.Item(CNDataSet) = oClaimRisk.XMLDataSet
                            Session(CNClaimRiskTimeStamp) = oClaimRisk.TimeStamp
                        End If
                    End If
                    'Updation of latest claim session variables
                    Session.Item(CNClaimTimeStamp) = oClaimResponse.TimeStamp
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
                        oInitialClaim.IsPolicyOutstanding = .IsPolicyOutstanding
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

                    'Check the claim builder ON or OFF
                    Dim sUrl As String = CheckClaimBuilder()
                    Response.Redirect(sUrl, False)


                    'Response.Redirect("~/Claims/perils.aspx")

                Case Mode.PayClaim

                    If Session(CNClaim) IsNot Nothing Then
                        oWebservice = New NexusProvider.ProviderManager().Provider

                        Dim sRunAuthorizePayment As String
                        Dim oRunClaimWorkFlow As NexusProvider.ProductClaimsWorkflowOptionsValue
                        Dim oQuote As NexusProvider.Quote = Session(CNClaimQuote)

                        'get the Product Risk option setting named Run Authorize Payment
                        sRunAuthorizePayment = oWebservice.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance, NexusProvider.ProductRiskOptions.RunAuthorisationScriptsClaimPayments, NexusProvider.RiskTypeOptions.None, oQuote.ProductCode, Nothing)

                        'get the Claim Workflow Setting
                        oRunClaimWorkFlow = oWebservice.GetProductClaimsWorkflowOptions(NexusProvider.ClaimProcessType.ClaimPayment, oQuote.ProductCode)
                        oExclusiveLocking = oWebservice.GetOptionSetting(NexusProvider.OptionType.SystemOption, NexusProvider.SystemOptions.ExclusiveLock)
                        If sRunAuthorizePayment = "1" Then
                            'Make the Claim version description dynamically
                            Session(CNChangeReason) = GetLocalResourceObject("lbl_ClaimReason_Auth") & sddlReasonSelectedValue & " - " & txtIfOther.Text
                        ElseIf oRunClaimWorkFlow.CashPaymentProcess = True Then
                            'Make the Claim version description dynamically
                            Session(CNChangeReason) = GetLocalResourceObject("lbl_ClaimReason_" & Session(CNMode)) & sddlReasonSelectedValue & " - " & txtIfOther.Text
                        Else
                            Session(CNChangeReason) = GetLocalResourceObject("lbl_ClaimReason_" & Session(CNMode)) & sddlReasonSelectedValue & " - " & txtIfOther.Text
                        End If
                        'Dummy Cal of PayClaim, to retreive back the latetst claim key
                        'PayClaimWithZeroAmount()

                        Dim sUrl As String = CheckClaimBuilder()
                        Response.Redirect(sUrl, False)


                    End If
                Case Mode.SalvageClaim, Mode.TPRecovery
                    If Session(CNClaim) IsNot Nothing Then
                        oWebservice = New NexusProvider.ProviderManager().Provider
                        Dim sDisplayReinsurance As String
                        Dim oRI2007 As NexusProvider.OptionTypeSetting = Nothing

                        sDisplayReinsurance = oWebservice.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.RiskTypeMaintenance, NexusProvider.ProductRiskOptions.Description, NexusProvider.RiskTypeOptions.DisplayClaimReinsurance, CType(Session(CNClaimQuote), NexusProvider.Quote).ProductCode, oInitialClaim.RiskType)
                        oExclusiveLocking = oWebservice.GetOptionSetting(NexusProvider.OptionType.SystemOption, NexusProvider.SystemOptions.ExclusiveLock)
                        'Check the User Authority to display of Reinsurance
                        Dim oUserAuthority As New NexusProvider.UserAuthority
                        oUserAuthority.UserCode = Session(CNLoginName)
                        oUserAuthority.UserAuthorityOption = NexusProvider.UserAuthority.UserAuthorityOptionType.DisplayClaimReinsurance
                        oWebservice = New NexusProvider.ProviderManager().Provider
                        oWebservice.GetUserAuthorityValue(oUserAuthority)

                        oRI2007 = oWebservice.GetOptionSetting(NexusProvider.OptionType.ProductOption, 88)

                        Dim oQuote As NexusProvider.Quote = Session(CNClaimQuote)
                        Dim sBranchCode As String = oQuote.BranchCode
                        Dim oClaimReceipt As New NexusProvider.ClaimReceipt
                        Dim oClaimReceiptCollection As New NexusProvider.ClaimReceiptCollection
                        Dim oClaim As New NexusProvider.Claim
                        If (Session(CNClaimMultiPerilIndex) IsNot Nothing) Then
                            Dim PerilsIndex As New System.Collections.Generic.List(Of Integer)
                            PerilsIndex = Session(CNClaimMultiPerilIndex)
                            Session(CNChangeReason) = GetLocalResourceObject("lbl_ClaimReason_" & Session(CNMode)) & sddlReasonSelectedValue & " - " & txtIfOther.Text
                            sddlReasonSelectedValue = String.Empty
                            If ddlReason.SelectedItem IsNot Nothing Then
                                sddlReasonSelectedValue = ddlReason.SelectedItem.Text
                            End If

                            Dim ReceiptItemIndex As Integer = 0
                            For Each PerilItemIndex In PerilsIndex
                                oClaimReceipt = CType(Session(CNClaim), NexusProvider.ClaimOpen).ClaimPeril(PerilItemIndex).Receipt
                                oClaimReceiptCollection.Add(oClaimReceipt)

                                If hidChlClaimClose.Value.Trim.ToUpper = "TRUE" Then
                                    oClaimReceiptCollection(ReceiptItemIndex).CloseClaimOnZeroReserveRecoveryBalance = True
                                End If

                                If Session(CNMode) = Mode.SalvageClaim Then
                                    oClaimReceiptCollection(ReceiptItemIndex).ClaimVersionDescription = "Salvage Recovery Comments - " & Session(CNChangeReason)
                                ElseIf Session(CNMode) = Mode.TPRecovery Then
                                    oClaimReceiptCollection(ReceiptItemIndex).ClaimVersionDescription = "Third Party Recovery Comments - " & Session(CNChangeReason)
                                End If
                                oClaimReceiptCollection(ReceiptItemIndex).TimeStamp = Session(CNClaimTimeStamp)
                                ReceiptItemIndex = ReceiptItemIndex + 1
                                'Update claim session with latest claim key(returned from ClaimReciept) as latest claim key would be required during bindclaim
                            Next
                            ClaimReceiptCall(Nothing, sBranchCode, oClaimReceiptCollection)
                            ReceiptItemIndex = 0
                            For Each PerilItemIndex In PerilsIndex
                                oClaimReceiptCollection(ReceiptItemIndex).TimeStamp = Session.Item(CNClaimTimeStamp)
                                ReceiptItemIndex = ReceiptItemIndex + 1
                            Next
                            oClaim = Session(CNClaim)
                            oClaim.ClaimKey = oClaimReceiptCollection(0).ClaimKey
                            oClaim.TimeStamp = oClaimReceiptCollection(oClaimReceiptCollection.Count - 1).TimeStamp
                            Session(CNClaim) = oClaim
                            Session.Item(CNClaimTimeStamp) = oClaimReceiptCollection(oClaimReceiptCollection.Count - 1).TimeStamp

                        ElseIf Session(CNClaimPerilIndex) IsNot Nothing Then
                            oClaimReceipt = CType(Session(CNClaim), NexusProvider.ClaimOpen).ClaimPeril(Session(CNClaimPerilIndex)).Receipt
                            If hidChlClaimClose.Value.Trim.ToUpper = "TRUE" Then
                                oClaimReceipt.CloseClaimOnZeroReserveRecoveryBalance = True
                            End If

                            sddlReasonSelectedValue = String.Empty
                            If ddlReason.SelectedItem IsNot Nothing Then
                                sddlReasonSelectedValue = ddlReason.SelectedItem.Text
                            End If

                            Session(CNChangeReason) = GetLocalResourceObject("lbl_ClaimReason_" & Session(CNMode)) & sddlReasonSelectedValue & " - " & txtIfOther.Text
                            If Session(CNMode) = Mode.SalvageClaim Then
                                oClaimReceipt.ClaimVersionDescription = "Salvage Recovery Comments - " & Session(CNChangeReason)
                            ElseIf Session(CNMode) = Mode.TPRecovery Then
                                oClaimReceipt.ClaimVersionDescription = "Third Party Recovery Comments - " & Session(CNChangeReason)
                            End If
                            oClaimReceipt.TimeStamp = Session(CNClaimTimeStamp)
                            oClaimReceipt.ClaimVersionDescription = GetLocalResourceObject("lbl_ClaimReason_" & Session(CNMode)) & sddlReasonSelectedValue & " - " & txtIfOther.Text
                            ClaimReceiptCall(oClaimReceipt, sBranchCode)
                            oClaimReceipt.TimeStamp = Session(CNClaimTimeStamp)

                            'Update claim session with latest claim key(returned from ClaimReciept) as latest claim key would be required during bindclaim
                            oClaim = Session(CNClaim)
                            oClaim.ClaimKey = oClaimReceipt.ClaimKey
                            oClaim.TimeStamp = oClaimReceipt.TimeStamp
                            Session(CNClaim) = oClaim
                            Session.Item(CNClaimTimeStamp) = oClaimReceipt.TimeStamp
                        End If


                        If sDisplayReinsurance = "1" AndAlso oUserAuthority.UserAuthorityValue = "1" Then
                            Response.Redirect("~/claims/ClaimReinsurance.aspx", False)
                        Else
                            Dim bClaimTimeStamp() As Byte = Session(CNClaimTimeStamp)

                            If BindClaimCall(oClaim, bClaimTimeStamp, 5, Nothing, sBranchCode) Is Nothing Then
                                Exit Sub
                            End If

                            'Check Make Furtehr Payments and hidden variable values to proceed further
                            If hidChkChoice.Value IsNot Nothing Then

                                If hidChkChoice.Value.Trim.Length <> 0 Then
                                    If hidChkChoice.Value.Trim.ToUpper = "TRUE" Then
                                        Session.Remove(CNEnablePayClaim) 'Remove the ReadOnly mode of the Salvage/TP Recovery
                                        GetLatestDetails()
                                        If oExclusiveLocking.OptionValue = "1" Then
                                            Dim oClaimDetails As NexusProvider.ClaimDetails = Nothing
                                            oClaimDetails = oWebservice.GetClaimDetails(v_iClaimKey:=oClaim.ClaimKey, v_sBranchCode:=sBranchCode, bExclusiveLock:=True)
                                        End If
                                        Response.Redirect("~/Claims/Perils.aspx", False)
                                    Else
                                        Response.Redirect("~/Claims/Complete.aspx", False)
                                    End If
                                Else
                                    Response.Redirect(AppSettings("WebRoot") & "/Claims/Complete.aspx")
                                End If
                            End If
                        End If
                    End If
            End Select
        End Sub

        ''' <summary>
        ''' This method will be fired on Page_Init for setting the javascript functions dynamically
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
            If Request.Browser.Browser = "IE" Then
                Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "Confirmation",
                                            "<script language=""JavaScript"" type=""text/javascript"">function Confirmation(){var r= newConfirm(""title"",'" & GetLocalResourceObject("msg_AnotherRecovery").ToString() & "',1,4,0); if(r==6){var ret=""TRUE"";} if(r==7){var ret=""FALSE"";} document.getElementById('" & hidChkChoice.ClientID & "').value=ret;}</script>")
            Else
                Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "Confirmation",
                                            "<script language=""JavaScript"" type=""text/javascript"">function Confirmation(){var r= confirm('" & GetLocalResourceObject("msg_AnotherRecovery").ToString() & "'); document.getElementById('" & hidChkChoice.ClientID & "').value=r;}</script>")
            End If
            Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "PaymentConfirmation",
                                                          "<script language=""JavaScript"" type=""text/javascript"">function PaymentConfirmation(){var r= newConfirm(""title"",'" & GetLocalResourceObject("msg_AnotherRecovery").ToString() & "',1,4,0); if(r==6){var ret=""TRUE"";} if(r==7){var ret=""FALSE"";} document.getElementById('" & hidChkChoice.ClientID & "').value=ret;}</script>")
            Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "ClaimCloseConfirmation",
                                                        "<script language=""JavaScript"" type=""text/javascript"">function ClaimCloseConfirmation(){var r= confirm('" & GetLocalResourceObject("msg_CloseClaim").ToString() & "'); document.getElementById('" & hidChlClaimClose.ClientID & "').value=r; document.getElementById('" & hidChkChoice.ClientID & "').value=false; if (document.getElementById('" & hidChkPaymentMsg.ClientID & "').value ==  '0' && r == false ) {PaymentConfirmation();}}</script>")

        End Sub

        ''' <summary>
        ''' This method will be fired on Page_Load
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub Page_Load1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            If Not IsPostBack Then
                'if User has not visited the change claim page then
                Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                Dim oQuote As NexusProvider.Quote = Session(CNClaimQuote)

                'To set the Focus
                Page.SetFocus(ddlReason)

                'Retreiving the available list of Reasons
                Dim olist As NexusProvider.LookupListCollection
                Dim oInitialClaim As NexusProvider.ClaimOpen = CType(Session(CNClaim), NexusProvider.ClaimOpen)

                olist = oWebservice.GetProductRiskEvents(oInitialClaim.InsuranceFileKey, Nothing, "CLAIM")

                ddlReason.DataSource = olist
                ddlReason.DataTextField = "Description"
                ddlReason.DataValueField = "Code"
                ddlReason.DataBind()
                If (Session(CNChangeReason) IsNot Nothing) Then
                    Dim sChangeReason As String = String.Empty
                    sChangeReason = Session(CNChangeReason)
                    Dim oChangeReason As String()
                    oChangeReason = sChangeReason.Split(New Char() {"-"c})

                    For nCount As Integer = 0 To ddlReason.Items.Count - 1
                        If ddlReason.Items(nCount).Text.Trim.ToUpper = oChangeReason(0).Trim.ToUpper Then
                            ddlReason.SelectedIndex = nCount
                            txtIfOther.Text = oChangeReason(1)
                            Exit For
                        End If
                    Next
                End If
                'get the Product Risk option setting named Run Authorize Payment
                sRunAuthorizePayment = oWebservice.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance, NexusProvider.ProductRiskOptions.RunAuthorisationScriptsClaimPayments, NexusProvider.RiskTypeOptions.None, oQuote.ProductCode, Nothing)

                'storing the value in Viewstate to again use on OK_Click and avoid the SAM call again
                ViewState("RunAuthorizePayment") = sRunAuthorizePayment

                'get the Claim Workflow Setting
                oRunClaimWorkFlow = oWebservice.GetProductClaimsWorkflowOptions(NexusProvider.ClaimProcessType.ClaimPayment, oQuote.ProductCode)

                'storing the value in Viewstate to again use on OK_Click and avoid the SAM call again
                ViewState("MakeFurtherPayments") = oRunClaimWorkFlow.MakeFurtherPayments
                ViewState("CashPaymentProcess") = oRunClaimWorkFlow.CashPaymentProcess

                'ToDO: Can't find the use of this session variable so we try to remove this
                Session(CNRunClaimWorkFlow) = oRunClaimWorkFlow

                Dim oPayment As NexusProvider.ClaimPerilReservePaymentTypeCollection
                Dim dAmount As Double = 0

                If Session(CNClaimPerilIndex) IsNot Nothing Then
                    oPayment = New NexusProvider.ClaimPerilReservePaymentTypeCollection

                    If oPayment IsNot Nothing Then
                        For Each oPaymentItem As NexusProvider.ClaimPerilReservePaymentType In oPayment
                            dAmount += oPaymentItem.ThisPaymentINCLTax
                        Next
                    End If
                ElseIf Session(CNClaimMultiPerilIndex) IsNot Nothing Then
                    oPayment = New NexusProvider.ClaimPerilReservePaymentTypeCollection
                    Dim PerilsIndex As New System.Collections.Generic.List(Of Integer)
                    PerilsIndex = Session(CNClaimMultiPerilIndex)
                    For PaymentItemIndex As Integer = 0 To PerilsIndex.Count - 1
                        oPayment = CType(Session(CNClaim), NexusProvider.ClaimOpen).ClaimPeril(PaymentItemIndex).ClaimReserve
                        If oPayment IsNot Nothing Then
                            For Each oPaymentItem As NexusProvider.ClaimPerilReservePaymentType In oPayment
                                dAmount += oPaymentItem.ThisPaymentINCLTax
                            Next
                        End If
                    Next
                End If


                If Session(CNMode) = Mode.SalvageClaim OrElse Session(CNMode) = Mode.TPRecovery Then

                    Dim sDisplayReinsurance As String
                    sDisplayReinsurance = oWebservice.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.RiskTypeMaintenance, NexusProvider.ProductRiskOptions.Description, NexusProvider.RiskTypeOptions.DisplayClaimReinsurance, CType(Session(CNClaimQuote), NexusProvider.Quote).ProductCode, oInitialClaim.RiskType)

                    'Check the User Authority to display of Reinsurance
                    Dim oUserAuthority As New NexusProvider.UserAuthority
                    oUserAuthority.UserCode = Session(CNLoginName)
                    oUserAuthority.UserAuthorityOption = NexusProvider.UserAuthority.UserAuthorityOptionType.DisplayClaimReinsurance
                    oWebservice = New NexusProvider.ProviderManager().Provider
                    oWebservice.GetUserAuthorityValue(oUserAuthority)
                    If Not (sDisplayReinsurance = "1" AndAlso oUserAuthority.UserAuthorityValue = "1") Then
                        hidChkPaymentMsg.Value = "0"
                        SetMessage()
                    End If
                End If

                'cleaning up
                oWebservice = Nothing
                oQuote = Nothing
                olist = Nothing
                oInitialClaim = Nothing
                oPayment = Nothing
                dAmount = Nothing
            End If
        End Sub

        ''' <summary>
        ''' This method retreives the latest claim version details and populate into claim session variables
        ''' </summary>
        ''' <remarks></remarks>
        Sub GetLatestDetails(Optional ByVal iClaimKey As Integer = 0)
            Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oClaimVersions As NexusProvider.VersionsCollections
            Dim iHighest As Integer = 0
            Dim oClaimDetails As NexusProvider.ClaimDetails = Nothing
            Dim oUserDetails As NexusProvider.UserDetails = CType(Session(CNAgentDetails), NexusProvider.UserDetails)
            Dim oOpenClaim As New NexusProvider.ClaimOpen
            Dim oClaimRisk As NexusProvider.ClaimRisk = Nothing
            Dim oQuote As NexusProvider.Quote = Session(CNClaimQuote)
            Dim sBranchCode As String = oQuote.BranchCode

            If iClaimKey > 0 Then
                'arch issue 268
                oClaimDetails = GetClaimDetailsCall(iClaimKey, sBranchCode)
            Else
                oClaimVersions = oWebservice.GetVersionsForClaim(Session(CNClaimNumber), sBranchCode)

                If oClaimVersions IsNot Nothing Then
                    'Find Highest Version
                    For iCount As Integer = 0 To oClaimVersions.Count - 1
                        If oClaimVersions(iCount).Version > iHighest Then
                            iHighest = oClaimVersions(iCount).Version
                        End If
                    Next
                End If
                For iCount As Integer = 0 To oClaimVersions.Count - 1
                    If oClaimVersions(iCount).Version = iHighest Then
                        'arch issue 268
                        oClaimDetails = GetClaimDetailsCall(oClaimVersions(iCount).ClaimKey, sBranchCode)
                        Exit For
                    End If
                Next
            End If

            'Updating claim session variables
            With oClaimDetails
                oOpenClaim.CatastropheCode = .CatastropheCode
                oOpenClaim.BaseCaseKey = .BaseCaseKey
                oOpenClaim.BaseClaimKey = .BaseClaimKey
                oOpenClaim.Claim = .Claim
                oOpenClaim.ClaimCoInsurer = .ClaimCoInsurer
                oOpenClaim.ClaimDescription = .ClaimDescription
                oOpenClaim.ClaimHandlerDescription = .ClaimHandlerDescription
                oOpenClaim.ClaimKey = .ClaimKey
                oOpenClaim.ClaimNumber = .ClaimNumber
                oOpenClaim.ClaimPeril = .ClaimPeril
                oOpenClaim.ClaimStatus = .ClaimStatus
                oOpenClaim.ClaimStatusDate = .ClaimStatusDate
                oOpenClaim.ClaimStatusID = .ClaimStatusID
                oOpenClaim.ClaimVersion = .ClaimVersion
                oOpenClaim.ClaimVersionDescription = .ClaimVersionDescription
                oOpenClaim.ClientClaimNumber = .ClientClaimNumber
                oOpenClaim.ClientEmail = .ClientEmail
                oOpenClaim.ClientFaxNo = .ClientFaxNo
                oOpenClaim.ClientMobileNo = .ClientMobileNo
                oOpenClaim.ClientName = .ClientName
                oOpenClaim.ClientShortName = oClaimVersions(0).ClientShortName 'IIf(.ClientShortName <> String.Empty, .ClientShortName, Trim(lblClientCode.Text))
                oOpenClaim.ClientTelNo = .ClientTelNo
                oOpenClaim.ClientTelNoOff = .ClientTelNoOff
                oOpenClaim.CloseClaimOnZeroReserveRecoveryBalance = .CloseClaimOnZeroReserveRecoveryBalance
                oOpenClaim.Comments = .Comments
                oOpenClaim.Contact = .Contact
                oOpenClaim.CurrencyISOCode = .CurrencyCode
                oOpenClaim.Description = .Description
                oOpenClaim.ExternalHandler = .ExternalHandler
                oOpenClaim.HandlerCode = .HandlerCode
                oOpenClaim.IgnoreClaimMaintain = .IgnoreClaimMaintain
                oOpenClaim.InfoOnly = .InfoOnly
                oOpenClaim.InsuranceFileKey = .InsuranceFileKey
                oOpenClaim.InsuranceRef = .InsuranceRef
                oOpenClaim.InsurerClaimNumber = .InsurerClaimNumber
                oOpenClaim.IsAllowedClosedClaims = .IsAllowedClosedClaims
                oOpenClaim.IsDeleted = .IsDeleted
                oOpenClaim.LastModifiedDate = .LastModifiedDate
                oOpenClaim.LikelyClaim = .LikelyClaim
                oOpenClaim.Location = .Location
                oOpenClaim.LossDate = .LossDate
                oOpenClaim.LossDateFrom = .LossDateFrom
                oOpenClaim.LossFromDate = .LossToDate
                oOpenClaim.LossToDate = .LossToDate
                oOpenClaim.LossToDateSpecified = .LossToDateSpecified
                oOpenClaim.Payments = .Payments
                oOpenClaim.PolicyNumber = .PolicyNumber
                oOpenClaim.PolicyType = .PolicyType
                oOpenClaim.PrimaryCause = .PrimaryCause
                oOpenClaim.PrimaryCauseCode = .PrimaryCauseCode
                oOpenClaim.PrimaryCauseDescription = .PrimaryCauseDescription
                oOpenClaim.ProductDescription = .ProductDescription
                oOpenClaim.ProgressStatusCode = .ProgressStatusCode
                oOpenClaim.ProgressStatusDescription = .ProgressStatusDescription
                oOpenClaim.ReportedDate = .ReportedDate
                oOpenClaim.Reserve = .Reserve
                oOpenClaim.RiskKey = .RiskKey
                oOpenClaim.RiskType = CType(Session(CNClaimQuote), NexusProvider.Quote).Risks.FindItemByRiskKey(.RiskKey).RiskTypeCode
                oOpenClaim.RiskTypeDescription = CType(Session(CNClaimQuote), NexusProvider.Quote).Risks.FindItemByRiskKey(.RiskKey).Description
                oOpenClaim.SecondaryCause = .SecondaryCause
                oOpenClaim.SecondaryCauseCode = .SecondaryCauseCode
                oOpenClaim.SecondaryCauseDescription = .SecondaryCauseDescription
                oOpenClaim.TotalCurrentShareValue = .TotalCurrentShareValue
                oOpenClaim.TotalShare = .TotalShare
                oOpenClaim.Town = .Town
                oOpenClaim.TownCode = .TownCode
                oOpenClaim.UnderwritingYearCode = .UnderwritingYearCode
                oOpenClaim.UserDefFldACode = .UserDefFldACode
                oOpenClaim.UserDefFldBCode = .UserDefFldBCode
                oOpenClaim.UserDefFldCCode = .UserDefFldCCode
                oOpenClaim.UserDefFldDCode = .UserDefFldECode
                oOpenClaim.UserDefFldECode = .UserDefFldECode
                'Added for Insurer
                oOpenClaim.Insurer = .Insurer
                Session.Item(CNClaimTimeStamp) = .TimeStamp
                oOpenClaim.CurrencyISOCode = .CurrencyCode
                Session.Item(CNCurrenyCode) = Trim(.CurrencyCode) 'Changed
                oOpenClaim.Client = .Client
                'Session(CNInsurer_Header) = .ClientName
                Session(CNClaimNumber) = .ClaimNumber
                Session(CNStatus) = .ClaimStatus

                'Retreiving the latest claim risk details
                'Arch issue 268
                ' oClaimRisk = oWebservice.GetClaimRisk(.BaseClaimKey, sBranchCode)
                Dim oOptionType As New NexusProvider.OptionTypeSetting
                oOptionType = oWebservice.GetOptionSetting(NexusProvider.OptionType.ProductOption, 12)
                If (oOptionType IsNot Nothing AndAlso String.IsNullOrEmpty(oOptionType.OptionValue) = False) _
                   AndAlso oOptionType.OptionValue = "1" Then
                    oClaimRisk = GetClaimRiskCall(.BaseClaimKey, .ClaimKey, sBranchCode)
                    Session(CNDataSet) = oClaimRisk.XMLDataSet
                End If

            End With
            Session(CNClaim) = oOpenClaim
        End Sub

        Sub SetMessage()

            Dim oQuote As NexusProvider.Quote = Session(CNClaimQuote)
            Dim oClaim As NexusProvider.ClaimOpen = Nothing
            Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim nClaimStatus As Integer
            Dim dCurrentReserve As Decimal
            Dim dCurrentRecovery As Decimal
            Dim sOptionSettings As String
            Dim bAllowNegativeReserve As Boolean = False
            Dim bStatus As Boolean = False
            Dim oClaimReceipt As NexusProvider.ClaimReceipt
            Dim dAmount As Decimal = 0
            If Session(CNClaimMultiPerilIndex) IsNot Nothing Then
                Dim PerilsIndex As New System.Collections.Generic.List(Of Integer)
                PerilsIndex = Session(CNClaimMultiPerilIndex)
                For ReceiptItemIndex As Integer = 0 To PerilsIndex.Count - 1
                    oClaimReceipt = New NexusProvider.ClaimReceipt
                    oClaimReceipt = CType(Session(CNClaim), NexusProvider.ClaimOpen).ClaimPeril(ReceiptItemIndex).Receipt
                    Dim i As Integer = 0

                    For i = 0 To oClaimReceipt.ReceiptItem.Count - 1
                        dAmount += oClaimReceipt.ReceiptItem(i).ThisReceiptINCLTaxAmount
                    Next
                Next
            ElseIf Session(CNClaimPerilIndex) IsNot Nothing Then
                oClaimReceipt = New NexusProvider.ClaimReceipt
                oClaimReceipt = CType(Session(CNClaim), NexusProvider.ClaimOpen).ClaimPeril(Session(CNClaimPerilIndex)).Receipt
                Dim i As Integer = 0
                For i = 0 To oClaimReceipt.ReceiptItem.Count - 1
                    dAmount += oClaimReceipt.ReceiptItem(i).ThisReceiptINCLTaxAmount
                Next
            End If


            btnOk.Attributes.Remove("onclick")
            sOptionSettings = oWebservice.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance, NexusProvider.ProductRiskOptions.AllowNegativeReserve, NexusProvider.RiskTypeOptions.None, oQuote.ProductCode, Nothing)

            If sOptionSettings <> "0" Then
                bAllowNegativeReserve = True
            End If

            oClaim = CType(Session.Item(CNClaim), NexusProvider.ClaimOpen)
            oWebservice.CheckReserveRecovery(oClaim.ClaimKey, nClaimStatus, dCurrentReserve, dCurrentRecovery)

            If nClaimStatus <> 3 AndAlso (dCurrentReserve <= 0 AndAlso dCurrentRecovery <= dAmount) AndAlso (bAllowNegativeReserve = False OrElse (dCurrentReserve = 0 AndAlso dCurrentRecovery = dAmount)) Then
                bStatus = True
            Else
                bStatus = False
            End If

            Dim oRunClaimWorkFlow As NexusProvider.ProductClaimsWorkflowOptionsValue
            oRunClaimWorkFlow = oWebservice.GetProductClaimsWorkflowOptions(NexusProvider.ClaimProcessType.ClaimPayment, oQuote.ProductCode)
            ViewState("RunClaimWorkFlow") = oRunClaimWorkFlow

            If Session(CNMode) = Mode.SalvageClaim Or Session(CNMode) = Mode.TPRecovery Then
                If oRunClaimWorkFlow.MakeFurtherPayments = True Then

                    If bStatus = False Then
                        'To make another receipt confirmation 
                        btnOk.Attributes.Add("onclick", "javascript:return Confirmation(this,'" + GetLocalResourceObject("msg_AnotherRecovery").ToString() + "');")
                    Else
                        hidChkPaymentMsg.Value = "0"
                        'If this value is set then paymentconfirmatio will be called from
                        'ClamCloseConfirmation javascript function
                        btnOk.Attributes.Add("onclick", "return ClaimCloseConfirmation(this,'" + GetLocalResourceObject("msg_CloseClaim").ToString() + "');")
                    End If
                ElseIf oRunClaimWorkFlow.MakeFurtherPayments = False Then

                    If bStatus = True Then
                        hidChkPaymentMsg.Value = String.Empty
                        btnOk.Attributes.Add("onclick", "ClaimCloseConfirmation(this,'" + GetLocalResourceObject("msg_CloseClaim").ToString() + "');")
                    End If
                End If

            ElseIf Session(CNMode) = Mode.PayClaim Then

                Dim sRunAuthorizePayment As String

                'get the Product Risk option setting named Run Authorize Payment
                sRunAuthorizePayment = oWebservice.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance, NexusProvider.ProductRiskOptions.RunAuthorisationScriptsClaimPayments, NexusProvider.RiskTypeOptions.None, oQuote.ProductCode, Nothing)
                'get the Claim Workflow Setting

                If oRunClaimWorkFlow.MakeFurtherPayments = True Then

                    If bStatus = False Then
                        btnOk.Attributes.Add("onclick", "javascript:return PaymentConfirmation();")
                    Else
                        hidChkPaymentMsg.Value = "0"
                        'If this value is set then paymentconfirmatio will be called from
                        'ClamCloseConfirmation javascript function
                        btnOk.Attributes.Add("onclick", "javascript:return ClaimCloseConfirmation();")
                    End If
                ElseIf oRunClaimWorkFlow.MakeFurtherPayments = False Then

                    If bStatus = True Then
                        hidChkPaymentMsg.Value = String.Empty
                        btnOk.Attributes.Add("onclick", "javascript:return ClaimCloseConfirmation();")
                    End If
                End If

            ElseIf Session(CNMode) = Mode.EditClaim Then
                If bStatus Then
                    hidChkPaymentMsg.Value = String.Empty
                    btnOk.Attributes.Add("onclick", "javascript:return ClaimCloseConfirmation();")
                End If

            End If

        End Sub
    End Class

End Namespace
