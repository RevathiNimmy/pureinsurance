Imports System.Web.Configuration.WebConfigurationManager
Imports Nexus
Imports Nexus.Constants
Imports Nexus.Constants.Session
Imports Nexus.Library
Imports CMS.Library
Partial Class Controls_ChangeClaim
    Inherits System.Web.UI.UserControl
    'Inherits CMS.Library.Frontend.clsCMSPage
    Dim sRunAuthorizePayment As String
    Dim oRunClaimWorkFlow As NexusProvider.ProductClaimsWorkflowOptionsValue


    Public Sub SetReasonForChange()

        Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
        Dim bTimeStamp As Byte() = CType(Session(CNClaimTimeStamp), Byte())
        Dim sddlReasonSelectedValue As String = String.Empty

        If Me.Visible = True Then
            If ddlReason.SelectedItem IsNot Nothing AndAlso ddlReason.SelectedItem.ToString <> "" Then
                sddlReasonSelectedValue = ddlReason.SelectedItem.Text
            Else
                sddlReasonSelectedValue = ddlReason.SelectedValue
            End If
        End If

        Select Case CType(Session(CNMode), Mode)
            Case Mode.EditClaim
                Dim oClaimResponse As NexusProvider.ClaimResponse = Nothing
                Dim oClaimRisk As NexusProvider.ClaimRisk = Nothing
                Dim oClaimDetails As NexusProvider.ClaimDetails = Nothing
                Dim oInitialClaim As NexusProvider.ClaimOpen = CType(Session(CNClaim), NexusProvider.ClaimOpen)
                Dim oQuote As NexusProvider.Quote = Session(CNClaimQuote)
                Dim oUserDetails As NexusProvider.UserDetails = CType(Session(CNAgentDetails), NexusProvider.UserDetails)
                Dim sBranchCode As String = oQuote.BranchCode

                'Make the Claim version description dynamically
                oInitialClaim.ClaimVersionDescription = GetLocalResourceObject("lbl_ClaimReason_" & Session(CNMode)) & sddlReasonSelectedValue & " - " & txtIfOther.Text

                'Call the SAM method MaintainClaim
                'To Skip the posting
                oInitialClaim.ReserveOnly = True
                'arch issue 268
                'oClaimResponse = oWebservice.MaintainClaim(oInitialClaim, bTimeStamp)
                oClaimResponse = MaintainClaimCall(oInitialClaim, bTimeStamp)
                If oClaimResponse Is Nothing Then
                    Exit Sub
                End If
                'Calling of Latest Claim Details after Maintain Claim
                'arch issue 268
                oClaimDetails = GetClaimDetailsCall(oClaimResponse.ClaimKey, sBranchCode)

                'Calling of the risk realted values after maintain claim
                'Arch issue 268
                oClaimRisk = GetClaimRiskCall(oClaimDetails.BaseClaimKey, oClaimDetails.ClaimKey, sBranchCode)

                'Updation of latest claim session variables
                Session.Item(CNClaimTimeStamp) = oClaimDetails.TimeStamp
                Session.Item(CNBaseClaimKey) = oClaimResponse.BaseClaimKey
                Session.Item(CNClaimKey) = oClaimResponse.ClaimKey
                Session.Item(CNClaimNumber) = oClaimResponse.ClaimNumber

                If oClaimRisk IsNot Nothing Then
                    Session.Item(CNDataSet) = oClaimRisk.XMLDataSet
                    Session(CNClaimRiskTimeStamp) = oClaimRisk.TimeStamp
                End If

                'Updation of latest claim session variables
                With oClaimDetails
                    oInitialClaim.CatastropheCode = .CatastropheCode
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
                    oInitialClaim.CurrencyISOCode = .CurrencyCode
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
                    oInitialClaim.ClientShortName = .ClientShortName

                End With

                'take the latest claim information into Session
                Session.Item(CNClaim) = oInitialClaim

                'check the claim builder hidden product option
                Dim oOptionType As New NexusProvider.OptionTypeSetting
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
                Dim sScreenCode As String = ""
                Dim sUrl As String = CheckClaimBuilder(sScreenCode)
                If Session(CNMode) = Mode.EditClaim AndAlso Session(CNInfoOnly) IsNot Nothing AndAlso Convert.ToBoolean(Session(CNInfoOnly)) AndAlso sScreenCode.Length > 0 Then
                    Session.Item(CNDataSet) = oWebservice.RunDefaultRulesAdd(sScreenCode, Session.Item(CNDataSet), sBranchCode, False)
                End If
                Response.Redirect(sUrl, False)


                'Response.Redirect("~/Claims/perils.aspx")

            Case Mode.PayClaim

                If Session(CNClaim) IsNot Nothing Then
                    Dim sRunAuthorizePayment As String
                    Dim oRunClaimWorkFlow As NexusProvider.ProductClaimsWorkflowOptionsValue
                    Dim oQuote As NexusProvider.Quote = Session(CNClaimQuote)

                    'get the Product Risk option setting named Run Authorize Payment
                    sRunAuthorizePayment = oWebservice.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance, NexusProvider.ProductRiskOptions.RunAuthorisationScriptsClaimPayments, NexusProvider.RiskTypeOptions.None, oQuote.ProductCode, Nothing)

                    'get the Claim Workflow Setting
                    oRunClaimWorkFlow = oWebservice.GetProductClaimsWorkflowOptions(NexusProvider.ClaimProcessType.ClaimPayment, oQuote.ProductCode)

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
                    PayClaimWithZeroAmount()

                    If CBool(Session(CNIsClaimLocked)) <> True Then

                        Dim sUrl As String = ""
                        sUrl = RedirectShowCheckUnpaidPremium("CLAIMBUILDER")

                        If sUrl <> "" Then
                            Response.Redirect(sUrl, False)
                        Else
                            'Check the claim builder ON or OFF
                            sUrl = CheckClaimBuilder()
                            Response.Redirect(sUrl, False)
                        End If


                        Exit Sub
                    End If


                    'Response.Redirect(AppSettings("WebRoot") & "/Claims/Perils.aspx")
                End If
            Case Mode.SalvageClaim, Mode.TPRecovery

                Session(CNChangeReason) = sddlReasonSelectedValue & " - " & txtIfOther.Text
                If Session(CNClaim) IsNot Nothing Then
                    Dim oQuote As NexusProvider.Quote = Session(CNClaimQuote)
                    Dim sBranchCode As String = oQuote.BranchCode
                    Dim oClaimReceipt As NexusProvider.ClaimReceipt = CType(Session(CNClaim), NexusProvider.ClaimOpen).ClaimPeril(Session(CNClaimPerilIndex)).Receipt
                    'arch issue 268
                    'oWebservice.ClaimReceipt(oClaimReceipt, sBranchCode)
                    If Not ClaimReceiptCall(oClaimReceipt, sBranchCode) Then
                        Exit Sub
                    End If
                    'Check Make Furtehr Payments and hidden variable values to proceed further
                    If hidChkChoice.Value IsNot Nothing Then

                        If hidChkChoice.Value.Trim.Length <> 0 Then
                            If hidChkChoice.Value.Trim.ToUpper = "TRUE" Then
                                Session.Remove(CNEnablePayClaim) 'Remove the ReadOnly mode of the Salvage/TP Recovery
                                GetLatestDetails()
                                Response.Redirect(AppSettings("WebRoot") & "/Claims/Perils.aspx")
                            Else
                                Response.Redirect(AppSettings("WebRoot") & "/Claims/Complete.aspx")
                            End If
                        End If
                    End If
                End If
            Case Mode.ViewClaim
                Dim sClaimVersionDescription = CType(Session(CNClaim), NexusProvider.ClaimOpen).ClaimVersionDescription
                Dim sDefaultTextOther = GetLocalResourceObject("ClaimReasonDefaultTextOther").ToString()
                If Session(CNClaim) IsNot Nothing Then
                    ddlReason.Enabled = False
                    txtIfOther.Enabled = False
                    If sClaimVersionDescription IsNot Nothing AndAlso sClaimVersionDescription.ToString.ToUpper.StartsWith(sDefaultTextOther.ToString.ToUpper) Then
                        txtIfOther.Text = sClaimVersionDescription.Substring(sDefaultTextOther.Length, sClaimVersionDescription.Length - sDefaultTextOther.Length)
                    Else
                        txtIfOther.Text = String.Empty
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
        Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "Confirmation",
                                        "<script language=""JavaScript"" type=""text/javascript"">function Confirmation(){var r= confirm('" & GetLocalResourceObject("msg_AnotherRecovery").ToString() & "'); document.getElementById('" & hidChkChoice.ClientID & "').value=r;}</script>")
    End Sub


    ''' <summary>
    ''' This method will be fired on Page_Load
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Load1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If (Session(CNMode) = Mode.ViewClaim) Then
            ddlReason.Enabled = False
            txtIfOther.Enabled = False
        End If
        If Not IsPostBack Then
            'if User has not visited the change claim page then
            Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oQuote As NexusProvider.Quote = Session(CNClaimQuote)
            Dim oPayment As NexusProvider.ClaimPerilReservePaymentTypeCollection
            If CType(Session(CNClaim), NexusProvider.ClaimOpen).ClaimPeril.Count > 0 Then
                oPayment = CType(Session(CNClaim), NexusProvider.ClaimOpen).ClaimPeril(CInt(Session(CNClaimPerilIndex))).ClaimReserve
            End If
            Dim dAmount As Double = 0

            'To set the Focus
            Page.SetFocus(ddlReason)

            'Retreiving the available list of Reasons
            Dim olist As NexusProvider.LookupListCollection
            Dim oInitialClaim As NexusProvider.ClaimOpen = CType(Session(CNClaim), NexusProvider.ClaimOpen)

            olist = oWebservice.GetProductRiskEvents(oInitialClaim.InsuranceFileKey, Nothing, "CLAIM")

            Dim nIndex As Integer = 0
            For Each Item In olist
                If olist.Item(nIndex).IsDefault = True Then
                    ddlReason.SelectedIndex = nIndex
                    Exit For
                End If
                nIndex = nIndex + 1
            Next

            ddlReason.DataSource = olist
            ddlReason.DataTextField = "Description"
            ddlReason.DataValueField = "Code"
            ddlReason.DataBind()

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

            If oPayment IsNot Nothing Then
                For Each oPaymentItem As NexusProvider.ClaimPerilReservePaymentType In oPayment
                    dAmount += oPaymentItem.ThisPaymentINCLTax
                Next
            End If

            'If Session(CNMode) = Mode.SalvageClaim Or Session(CNMode) = Mode.TPRecovery Then

            '    'To make another receipt confirmation 
            '    btnOk.Attributes.Add("onclick", "javascript:return Confirmation();")
            'End If

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
            oClaimRisk = GetClaimRiskCall(.BaseClaimKey, .ClaimKey, sBranchCode)
            Session(CNDataSet) = oClaimRisk.XMLDataSet
        End With
        Session(CNClaim) = oOpenClaim
    End Sub
End Class
