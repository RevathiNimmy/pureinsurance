Imports CMS.Library
Imports Nexus.Constants.Constant
Imports Nexus.Constants.Session
Imports System.Configuration.ConfigurationManager
Imports System.Diagnostics.Eventing.Reader
Imports NexusProvider
Imports System.Web.Configuration
Imports Nexus.Library

Namespace Nexus
    Partial Class ExternalTaskManager
        Inherits BaseFindParty

        Dim smessageId As String
        Dim sPureKey As String
        Dim sOperation As String
        Dim sOriginatingSource As String
        Dim sPartyCode As String
        Dim sPolicyNumber As String
        Dim sPartyType As String
        Dim sClientCode As String
        Dim sCliamNumber As String
        Dim sMode As String
        Dim sClaimDate As String

        Protected Shadows Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            smessageId = Request.QueryString("messageId")
            sOriginatingSource = Request.QueryString("originatingSource").ToUpper()
            sOperation = Request.QueryString("operation").ToUpper()
            sMode = Request.QueryString("mode").ToUpper()

            If sOriginatingSource.Trim().Length < 1 OrElse sMode.Trim().Length < 1 Then
                Response.Redirect("~/secure/agent/FindClient.aspx")
            End If

            'Select Case sOriginatingSource
            'Case "WORKITEM"
            If (sMode = "CLAIMOPEN") Then
                sClaimDate = Request.QueryString("claimDate").ToUpper()
                sPolicyNumber = Request.QueryString("policyNumber").ToUpper()
            ElseIf (sMode = "CLAIMMAINTAIN" OrElse sMode = "CLAIMVIEW" OrElse sMode = "CLAIMPAYMENT") Then
                sCliamNumber = Request.QueryString("claimNumber").ToUpper()
            ElseIf (sMode = "CLIENTNEW") Then
                sPartyType = Request.QueryString("partyType").ToUpper()
            ElseIf (sMode = "CLIENTEDIT") Then
                sClientCode = Request.QueryString("clientCode").ToUpper()
            ElseIf (sMode = "QUOTEEDIT") Then
                sPolicyNumber = Request.QueryString("policyNumber").ToUpper()
                sClientCode = Request.QueryString("clientCode").ToUpper()
            ElseIf (sMode = "POLICYNEW") Then
                sClientCode = Request.QueryString("clientCode").ToUpper()
            ElseIf _
                (sMode = "POLICYMTA" OrElse sMode = "POLICYCAN" OrElse sMode = "POLICYREINS" OrElse _
                 sMode = "POLICYREN" OrElse sMode = "POLICYLAPSE") Then
                sPolicyNumber = Request.QueryString("policyNumber").ToUpper()
            End If
            'End Select

            Select Case sMode
                Case "CLIENTNEW"

                    If (sPartyType = "PC") Then
                        Response.Redirect("~/secure/agent/PersonalClientDetails.aspx?mode=add")
                    ElseIf (sPartyType = "CC") Then
                        Response.Redirect("~/secure/agent/CorporateClientDetails.aspx?mode=add")
                    Else
                        Response.Redirect("~/secure/agent/FindClient.aspx")
                    End If

                Case "CLIENTEDIT", "POLICYNEW"

                    Dim oWebService As NexusProvider.ProviderBase = Nothing
                    Try

                        oWebService = New NexusProvider.ProviderManager().Provider
                        Dim oPartyCollection As NexusProvider.PartyCollection
                        Dim oPartySearchCriteria As NexusProvider.PartySearchCriteria
                        oPartySearchCriteria = New NexusProvider.PartySearchCriteria()
                        With oPartySearchCriteria
                            .ShortName = sClientCode
                            .PartyType = PartyType.GC
                            .PartyTypes.Add(NexusProvider.PartyTypeType.PC)
                            .PartyTypes.Add(NexusProvider.PartyTypeType.CC)
                        End With
                        oPartyCollection = oWebService.FindParty(oPartySearchCriteria, Session(CNTransBranchCode))

                        If oPartyCollection.Count > 0 Then
                            Dim bPartyTypeCast As Boolean = True
                            Try
                                If _
                                    (DirectCast(DirectCast(oPartyCollection(0), NexusProvider.BaseParty),  _
                                                NexusProvider.PersonalParty) IsNot Nothing) Then
                                    sPartyType = "PC"
                                    bPartyTypeCast = False
                                End If
                            Catch ex As System.Exception
                            End Try

                            If (bPartyTypeCast) Then
                                If _
                                    (DirectCast(DirectCast(oPartyCollection(0), NexusProvider.BaseParty),  _
                                                NexusProvider.CorporateParty) IsNot Nothing) Then
                                    sPartyType = "CC"
                                End If
                            End If

                            If (sMode = "CLIENTEDIT") Then
                                Session(CNClientMode) = Mode.Edit
                            Else
                                Session(CNClientMode) = Nothing
                            End If

                            If (sPartyType = "PC") Then
                                Response.Redirect( _
                                    "~/secure/agent/PersonalClientDetails.aspx?PartyKey=" & oPartyCollection(0).Key & _
                                    "&Code=" & oPartyCollection(0).UserName & "")
                            ElseIf (sPartyType = "CC") Then
                                Response.Redirect( _
                                    "~/secure/agent/CorporateClientDetails.aspx?PartyKey=" & oPartyCollection(0).Key & _
                                    "&Code=" & oPartyCollection(0).UserName & "")
                            Else
                                Response.Redirect("~/secure/agent/FindClient.aspx")
                            End If
                        End If

                    Catch ex As System.Exception

                    Finally
                        oWebService = Nothing
                    End Try

                Case "QUOTEEDIT"

                    Dim iCurrentRiskKey As Integer
                    Dim sRedirectPath As String = String.Empty
                    Dim sOnErrorRedirectPath As String = String.Empty
                    Dim oQuote As NexusProvider.Quote = Nothing
                    Dim oWebService As NexusProvider.ProviderBase = Nothing

                    Try

                        Session(CNClientMode) = Nothing
                        oWebService = New NexusProvider.ProviderManager().Provider
                        Dim oPartyCollection As NexusProvider.PartyCollection
                        Dim oPartySearchCriteria As NexusProvider.PartySearchCriteria
                        oPartySearchCriteria = New NexusProvider.PartySearchCriteria()
                        With oPartySearchCriteria
                            .ShortName = sClientCode
                            .PartyType = PartyType.GC
                            .PartyTypes.Add(NexusProvider.PartyTypeType.PC)
                            .PartyTypes.Add(NexusProvider.PartyTypeType.CC)
                        End With
                        oPartyCollection = oWebService.FindParty(oPartySearchCriteria, Session(CNTransBranchCode))

                        If oPartyCollection.Count > 0 Then
                            Dim bPartyTypeCast As Boolean = True
                            Try
                                If _
                                    (DirectCast(DirectCast(oPartyCollection(0), NexusProvider.BaseParty),  _
                                                NexusProvider.PersonalParty) IsNot Nothing) Then
                                    sPartyType = "PC"
                                    bPartyTypeCast = False
                                End If
                            Catch ex As System.Exception
                            End Try

                            If (bPartyTypeCast) Then
                                If _
                                    (DirectCast(DirectCast(oPartyCollection(0), NexusProvider.BaseParty),  _
                                                NexusProvider.CorporateParty) IsNot Nothing) Then
                                    sPartyType = "CC"
                                End If
                            End If

                            If (sPartyType <> "PC" AndAlso sPartyType <> "CC") Then
                                sOnErrorRedirectPath = "~/secure/agent/FindClient.aspx"
                                Throw New ApplicationException
                            End If
                        End If

                        Session.Remove(CNOldPremium) 'Remove the old premium from session
                        Session.Remove(CNRiskType) 'Reset the Risk Type
                        ClearClaims() 'to Clear the claim session variable if any
                        oWebService = New NexusProvider.ProviderManager().Provider
                        oQuote = oWebService.GetHeaderAndSummariesByRef(sPolicyNumber, Nothing)
                        oQuote = oWebService.GetHeaderAndSummariesByKey(CInt(oQuote.InsuranceFileKey))
                        Dim oFindParty As NexusProvider.BaseParty
                        oWebService = New NexusProvider.ProviderManager().Provider
                        oFindParty = oWebService.GetParty(oQuote.PartyKey)
                        Session(CNParty) = oFindParty

                        'Put highest risk key into Session
                        For i As Integer = 0 To oQuote.Risks.Count - 1
                            oWebService.GetRisk(oQuote.Risks(i).Key, i, oQuote)
                            iCurrentRiskKey = oQuote.Risks(i).Key
                        Next

                        oWebService.GetHeaderAndRisksByKey(oQuote)

                        Session(CNQuote) = oQuote
                        DataSetFunctions.GetScreens()

                        Session.Remove(CNOI)
                        Session(CNMode) = Mode.Edit
                        Session(CNRenewal) = Nothing
                        Session(CNInsuranceFileKey) = oQuote.InsuranceFileKey
                        Session(CNQuoteMode) = QuoteMode.FullQuote
                        Session(CNQuoteInSync) = False
                        Session(CNMtaReasonSelected) = Nothing

                        If IsDataSetQuickQuote() = False Then
                            Session(CNQuoteMode) = QuoteMode.FullQuote
                        Else
                            Session(CNQuoteMode) = QuoteMode.QuickQuote
                        End If

                        If DataSetFunctions.sSummaryOfCover.ToLower = "true" Then
                            sRedirectPath = DataSetFunctions.sSummaryOfCoverURL
                        Else
                            sRedirectPath = "~/secure/PremiumDisplay.aspx"
                        End If

                        If (sPartyType = "PC") Then
                            sOnErrorRedirectPath = "~/secure/agent/PersonalClientDetails.aspx?PartyKey=" & _
                                                   oPartyCollection(0).Key & "&Code=" & oPartyCollection(0).UserName
                        ElseIf (sPartyType = "CC") Then
                            sOnErrorRedirectPath = "~/secure/agent/CorporateClientDetails.aspx?PartyKey=" & _
                                                   oPartyCollection(0).Key & "&Code=" & oPartyCollection(0).UserName
                        End If

                    Catch ex As System.Exception
                        Response.Redirect(sOnErrorRedirectPath, False)
                    Finally
                        oQuote = Nothing
                        oWebService = Nothing
                    End Try

                    Response.Redirect(sRedirectPath, False)

                Case "POLICYMTA", "POLICYCAN", "POLICYREINS"

                    Dim oQuote As NexusProvider.Quote = Nothing
                    Dim oWebService As NexusProvider.ProviderBase = Nothing

                    Try
                        oWebService = New NexusProvider.ProviderManager().Provider
                        oQuote = oWebService.GetHeaderAndSummariesByRef(sPolicyNumber, Nothing)
                        oQuote = oWebService.GetHeaderAndSummariesByKey(CInt(oQuote.InsuranceFileKey))
                        For iCount As Integer = 0 To oQuote.Risks.Count - 1
                            oWebService.GetRisk(oQuote.Risks(iCount).Key, iCount, oQuote)
                        Next
                        oWebService.GetHeaderAndRisksByKey(oQuote)
                        'TO retreive the selected status after btn Print
                        For iCount As Integer = 0 To oQuote.Risks.Count - 1
                            If oQuote.Risks(iCount).IsRisk = True Then
                                oQuote.Risks(iCount).IsRisk = True
                            Else
                                oQuote.Risks(iCount).IsRisk = False
                            End If
                        Next

                        If (sMode = "POLICYREINS") Then
                            Session(CNMTAType) = MTAType.REINSTATEMENT
                            Dim oFindParty As NexusProvider.BaseParty
                            oWebService = New NexusProvider.ProviderManager().Provider
                            oFindParty = oWebService.GetParty(oQuote.PartyKey)
                            Session(CNParty) = oFindParty
                        End If
                        Session(CNQuote) = oQuote
                        Session.Remove(CNOI)
                        Session(CNMode) = Mode.Edit
                        Session(CNRenewal) = Nothing
                        Session(CNInsuranceFileKey) = oQuote.InsuranceFileKey
                        Session(CNQuoteMode) = QuoteMode.FullQuote
                        Session(CNQuoteInSync) = False
                        Session(CNMtaReasonSelected) = Nothing

                    Catch ex As System.Exception
                        Response.Redirect("~/secure/agent/FindClient.aspx")
                    Finally
                        oQuote = Nothing
                        oWebService = Nothing
                    End Try

                    Response.Redirect("~/secure/MTAReason.aspx")

                Case "POLICYLAPSE", "POLICYREN"
                    If (sPolicyNumber.Length > 0) Then
                        Response.Redirect("~/secure/agent/RenewalSelection.aspx?ref=" & sPolicyNumber)
                    Else
                        Response.Redirect("~/secure/agent/FindClient.aspx")
                    End If

                Case "CLAIMOPEN"
                    Dim iInsuranceFileKey As Integer
                    Dim sInsuranceRef As String, sTypeCode As String
                    Dim dLossDate As Date
                    Dim dtStartDate As Date, dtEndDate As Date, dtInceptionDate As Date
                    Dim iReturnCode As Integer
                    Dim oNexusConfig As Library.Config.NexusFrameWork = _
                            CType(ConfigurationManager.GetSection("NexusFrameWork"), Library.Config.NexusFrameWork)
                    Dim oPortal As Nexus.Library.Config.Portal = oNexusConfig.Portals.Portal(Portal.GetPortalID())
                    Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider

                    Dim oInsuranceFileSearchCriteria As New NexusProvider.InsuranceFileDetails
                    With oInsuranceFileSearchCriteria
                        .InsuranceRef = sPolicyNumber
                        .LossDate = CDate(sClaimDate)
                        .SearchDate = CDate(sClaimDate)
                    End With

                    Dim oInsuranceFileDetails = oWebService.FindInsuranceFileForClaims(oInsuranceFileSearchCriteria)
                    Session.Remove(CNQuote)

                    For i As Integer = 0 To oInsuranceFileDetails.Count - 1

                        Dim oOpenClaim As New NexusProvider.ClaimOpen
                        Dim oAgentDetailsPolicy As NexusProvider.AgentDetailsForPolicy = Nothing
                        Dim oPartyDetails As NexusProvider.InsuranceFileDetails = Nothing

                        Dim oInsuranceFileCollection As NexusProvider.PolicyCollection

                        iInsuranceFileKey = oInsuranceFileDetails(i).InsuranceFileKey
                        sInsuranceRef = oInsuranceFileDetails(i).InsuranceFileRef
                        dLossDate = CDate(sClaimDate)

                        'btnNext.Attributes.Remove("onclick")
                        Dim oQuote1 As NexusProvider.Quote = oWebService.GetHeaderAndSummariesByKey(iInsuranceFileKey)
                        oInsuranceFileCollection = oWebService.GetAllPolicyVersions(oQuote1.InsuranceFolderKey)
                        GetPolicyForClaimDate(oInsuranceFileCollection, dLossDate, iInsuranceFileKey, sInsuranceRef, _
                                              dtStartDate, dtEndDate, iReturnCode, dtInceptionDate, sTypeCode)

                        If iInsuranceFileKey = 0 Then
                            iInsuranceFileKey = oInsuranceFileDetails(i).InsuranceFileKey
                        End If

                        Try
                            'Populating the Agent(insurer) Details if Policy is associated with agent
                            If oInsuranceFileDetails(i).LeadAgentKey > 0 Then
                                oAgentDetailsPolicy = oWebService.GetAgentDetailsForPolicy(iInsuranceFileKey)
                            End If
                            'Populating the Client details
                            oPartyDetails = oWebService.GetClaimPartyDetails(iInsuranceFileKey)
                        Catch ex As System.Exception
                            Response.Redirect("~/Claims/FindInsuranceFile.aspx")
                        End Try

                        With oOpenClaim
                            If oPartyDetails IsNot Nothing Then
                                .ClientName = oPartyDetails.ClientName
                                .Client.ClientName = oPartyDetails.ClientName
                                .ClientShortName = oPartyDetails.ClientShortName
                                .Client.ShortName = oPartyDetails.ClientShortName
                                .Client.Address = oPartyDetails.Address
                                .Client.PartyKey = oPartyDetails.PartyKey
                                If .Client.PartyKey > 0 Then
                                    Dim oBaseParty As NexusProvider.BaseParty
                                    oBaseParty = oWebService.GetParty(.Client.PartyKey)
                                    Session.Item(CNParty) = oBaseParty
                                End If
                                .Client.Address.CountryCode = GetCodeForKey(NexusProvider.ListType.PMLookup, _
                                                                            oPartyDetails.CountryKey, "Country", True)
                                .ClientEmail = oPartyDetails.Email
                                .ClientFaxNo = oPartyDetails.Fax
                                .ClientMobileNo = oPartyDetails.Mobile
                                .ClientTelNo = oPartyDetails.TelHome
                                .ClientTelNoOff = oPartyDetails.TelOff
                            Else
                                .Client = Nothing
                            End If

                            If oAgentDetailsPolicy IsNot Nothing Then
                                .Insurer.ContactName = oAgentDetailsPolicy.Name
                                .Insurer.ShortName = oAgentDetailsPolicy.Shortname
                                .Insurer.Address.Address1 = oAgentDetailsPolicy.Address1
                                .Insurer.Address.Address2 = oAgentDetailsPolicy.Address2
                                .Insurer.Address.Address3 = oAgentDetailsPolicy.Address3
                                .Insurer.Address.Address4 = oAgentDetailsPolicy.Address4
                                .Insurer.Address.CountryCode = GetCodeForKey(NexusProvider.ListType.PMLookup, _
                                                                             oAgentDetailsPolicy.CountryKey, "Country", _
                                                                             True)
                                .Insurer.Address.CountryDescription = _
                                    GetDescriptionForCode(NexusProvider.ListType.PMLookup, .Insurer.Address.CountryCode, _
                                                          "COUNTRY")
                                .Insurer.Address.PostCode = oAgentDetailsPolicy.PostalCode
                                .Insurer.Address.CountryCode = .Insurer.Address.CountryCode
                                .Insurer.Address.CountryDescription = .Insurer.Address.CountryDescription

                                If oAgentDetailsPolicy.Contacts IsNot Nothing Then
                                    .Insurer.Contact = oAgentDetailsPolicy.Contacts
                                End If
                            Else
                                .Insurer = Nothing
                            End If
                        End With
                        Session(CNLapseDate) = oInsuranceFileDetails(i).LapseDate
                        Session(CNClaim) = oOpenClaim
                        Exit For
                    Next

                    Session(CNInsuranceFileKey) = iInsuranceFileKey
                    Dim oQuote As NexusProvider.Quote = Nothing
                    Dim oUserDetails As NexusProvider.UserDetails = CType(Session(CNAgentDetails),  _
                                                                          NexusProvider.UserDetails)
                    Dim sBranchCode As String = oUserDetails.ListOfBranches(0).Code

                    Try
                        'Retreival of the quote information and storing in session
                        oQuote = oWebService.GetHeaderAndSummariesByKey(iInsuranceFileKey, sBranchCode)
                        'If policy is CAN and MTACAN then cover start date is less than equal to current date then claim can not be processed
                        If oQuote.InsuranceFileStatusCode IsNot Nothing Then
                            If oQuote.InsuranceFileStatusCode.Trim.ToUpper = "CAN" And oQuote.CoverStartDate <= Date.Now _
                                Then
                                'IsPolicyCancelled.IsValid = False

                            End If
                        ElseIf oQuote.InsuranceFileTypeCode IsNot Nothing Then
                            If _
                                oQuote.InsuranceFileTypeCode.Trim.ToUpper = "MTACAN" And _
                                oQuote.CoverStartDate <= Date.Now Then
                                'IsPolicyCancelled.IsValid = False
                                'Exit Sub
                            End If
                        ElseIf oQuote.PolicyStatusCode IsNot Nothing Then
                            'on Policy status LAP, claim can not be processed
                            If oQuote.PolicyStatusCode.Trim.ToUpper = "LAP" Then
                                '   IsPolicyCancelled.IsValid = False
                                Exit Sub
                            End If
                        Else
                            'IsPolicyCancelled.IsValid = True
                        End If
                        'Updation of the session variables

                        Session(CNLossDate) = FormatDateTime(sClaimDate, DateFormat.LongDate) & " " & _
                                              Date.Now.ToShortTimeString
                        Session(CNRisks) = oQuote.Risks
                        Session.Item(CNPartyKey) = oQuote.PartyKey
                        Session.Item(CNDate_Header) = oQuote.CoverStartDate.ToShortDateString & " - " & _
                                                      oQuote.CoverEndDate.ToShortDateString
                        Session.Item(CNPolicyNumber) = oQuote.InsuranceFileRef
                        Session.Item(CNCurrenyCode) = oQuote.CurrencyCode
                        Session.Item(CNClaimNumber) = GetLocalResourceObject("lbl_ClaimNumber").ToString()
                        Session.Item(CNStatus) = oPortal.Claims.NewClaimStatus
                        Session(CNInsurer_Header) = oQuote.InsuredName
                        Session(CNProductCode) = oQuote.ProductCode
                        Session(CNPolicyNumber) = oQuote.InsuranceFileRef
                        Session(CNMode) = Mode.NewClaim
                        Session(CNClaimQuote) = oQuote
                        Session(CNInsuranceFileStatus) = oQuote.InsuranceFileStatusCode

                    Catch ex As System.Exception
                        Response.Redirect("~/Claims/FindInsuranceFile.aspx")
                    Finally
                        oWebService = Nothing
                        oUserDetails = Nothing
                    End Try

                    Response.Redirect("~/claims/overview.aspx")

                Case "CLAIMMAINTAIN", "CLAIMVIEW", "CLAIMPAYMENT"

                    Dim oWebService As NexusProvider.ProviderBase = Nothing
                    Dim oClaimVersions As NexusProvider.VersionsCollections = Nothing
                    Dim oQuote As NexusProvider.Quote = Nothing
                    Dim oBaseParty As NexusProvider.BaseParty = Nothing
                    Dim oOpenClaim As NexusProvider.ClaimOpen = Nothing
                    Dim oClaimDetails As NexusProvider.ClaimDetails = Nothing
                    Dim oCashListItem As NexusProvider.CashListItemsCollection = Nothing
                    Dim oClaimRisk As NexusProvider.ClaimRisk = Nothing
                    Dim bUnlockRequired As Boolean = False
                    Dim bExclusiveLock As Boolean = True
                    Try
                        ClearHeader()
                        ClearQuote()
                        ClearClaims()
                        If sMode = "CLAIMMAINTAIN" Then
                            Session(CNMode) = Mode.EditClaim
                        ElseIf sMode = "CLAIMPAYMENT" Then
                            Session(CNMode) = Mode.PayClaim
                        ElseIf sMode = "CLAIMVIEW" Then
                            Session(CNMode) = Mode.ViewClaim
                            bExclusiveLock = False
                            bUnlockRequired = False
                        End If

                        oWebService = New NexusProvider.ProviderManager().Provider
                        oOpenClaim = New NexusProvider.ClaimOpen

                        Dim sClaimNumber As String = sCliamNumber.Trim()
                        Dim iHighest As Integer = 0

                        oClaimVersions = oWebService.GetVersionsForClaim(sClaimNumber)
                        If oClaimVersions IsNot Nothing Then
                            'Find Highest Version

                            For iCount As Integer = 0 To oClaimVersions.Count - 1
                                If oClaimVersions(iCount).Version > iHighest Then
                                    iHighest = oClaimVersions(iCount).Version
                                End If
                            Next

                            'Updating of claim quote oQuote
                            oQuote = oWebService.GetHeaderAndSummariesByKey(oClaimVersions(0).InsuranceFileKey)
                            If oQuote IsNot Nothing Then
                                oBaseParty = oWebService.GetParty(oQuote.PartyKey)
                                Session.Item(CNParty) = oBaseParty
                                Session.Item(CNRisks) = oQuote.Risks
                                Session.Item(CNRenewalDate) = oQuote.RenewalDate
                                Session.Item(CNAddress) = oBaseParty.Addresses(0).Address1 & ", " & _
                                                          oBaseParty.Addresses(0).Address4
                                Session.Item(CNDate_Header) = oQuote.CoverStartDate.ToShortDateString & " - " & _
                                                              oQuote.CoverEndDate.ToShortDateString
                                Session(CNInsurer_Header) = oQuote.InsuredName
                                Session(CNProductCode) = oQuote.ProductCode
                                Session(CNClaimQuote) = oQuote
                            End If
                            Session(CNClaimVersion) = oClaimVersions
                            Session.Item(CNInsuranceFileKey) = oClaimVersions(0).InsuranceFileKey
                            Session.Item(CNPolicyNumber) = oClaimVersions(0).InsuranceRef
                            'Response.Redirect("FindClaim.aspx")
                        End If

                        'As per the discussion with Gaurav, Not required for CBL
                        'Unlock claim of same user
                        'For iCount As Integer = 0 To oClaimVersions.Count - 1
                        '    UnlockClaim(oClaimVersions(iCount).ClaimKey)
                        'Next

                        'Retreival of claim details
                        Dim sBranchCode As String = oQuote.BranchCode
                        For iCount As Integer = 0 To oClaimVersions.Count - 1
                            If oClaimVersions(iCount).Version = iHighest Then
                                'To Check whether Payment is pending for Authorization
                                If sMode = "CLAIMPAYMENT" Or sMode = "CLAIMMAINTAIN" Then
                                    oCashListItem = oWebService.GetReferredPayments()
                                    For Each oCashList As NexusProvider.CashListItems In oCashListItem
                                        If oClaimVersions(iCount).ClaimNumber = oCashList.ClaimNumber Then
                                            'AllowClaimPayment.IsValid = False
                                            Dim sMessage As String = Replace(GetLocalResourceObject("lbl_ClaimPaymentPendAuth_error"), "{1}", oCashList.ClaimNumber)
                                            Dim sReturnUrl As String = "~/Claims/FindClaim.aspx?ExternalWrkMgrMsg=" & sMessage
                                            Response.Redirect(sReturnUrl, False)
                                            Context.ApplicationInstance.CompleteRequest()
                                            Exit Sub
                                        End If
                                    Next

                                    Try
                                        'This is expected to through an error if claim is locked in BO
                                        oClaimDetails = GetClaimDetailsCall(oClaimVersions(iCount).ClaimKey, sBranchCode)
                                    Catch ex As NexusProvider.NexusException
                                        'Claim locking error
                                        Select Case CType(ex.Errors(0), NexusProvider.NexusError).Code
                                            Case "1000160" 'Claim is locked for modification as already in use
                                                'Show Claim locking error as alert
                                                Dim sMessage As String = "alert('" + Replace(GetLocalResourceObject("lbl_ClaimLock_error"), "{1}", (ex.Errors(0).Detail.Split(":"))(1) + ".") + "')"
                                                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "claimlocked", sMessage, True)
                                                bUnlockRequired = False
                                                Exit Sub
                                            Case "200" 'Claim Locking
                                                'Show Claim locking error as alert
                                                Dim sMessage As String = "alert('" + ex.Errors(0).Description + ".\n" + ex.Errors(0).Detail + "')"
                                                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "claimlocked", sMessage, True)
                                                Server.ClearError()
                                                bUnlockRequired = True
                                                Exit Sub
                                            Case Else
                                                Throw ex
                                        End Select
                                        Server.ClearError()
                                        'Clear all claim related sessions and throw the error
                                        ClearQuote()
                                        ClearClaims()
                                    End Try
                                Else
                                    'This is expected to through an error if claim is locked in BO
                                    oClaimDetails = GetClaimDetailsCall(v_iClaimKey:=oClaimVersions(iCount).ClaimKey, v_sBranchCode:=sBranchCode, bExclusiveLock:=bExclusiveLock)
                                End If

                                'check for closed claim
                                If sMode = "CLAIMPAYMENT" AndAlso oClaimDetails IsNot Nothing Then
                                    If _
                                        Not String.IsNullOrEmpty(oClaimDetails.ClaimStatus) AndAlso _
                                        oClaimDetails.ClaimStatus.Trim.ToUpper = "CLOSED" Then
                                        'ChkClosedClaim.IsValid = False
                                        Exit Sub
                                    End If
                                End If

                                'Updation of session with claim details
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
                                    oOpenClaim.ClientShortName = oClaimVersions(0).ClientShortName _
                                    'IIf(.ClientShortName <> String.Empty, .ClientShortName, Trim(lblClientCode.Text))
                                    oOpenClaim.ClientTelNo = .ClientTelNo
                                    oOpenClaim.ClientTelNoOff = .ClientTelNoOff
                                    oOpenClaim.CloseClaimOnZeroReserveRecoveryBalance = _
                                        .CloseClaimOnZeroReserveRecoveryBalance
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
                                    oOpenClaim.LossFromDate = .LossFromDate
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
                                    oOpenClaim.RiskType = _
                                        CType(Session(CNClaimQuote), NexusProvider.Quote).Risks.FindItemByRiskKey( _
                                            .RiskKey).RiskTypeCode
                                    oOpenClaim.RiskTypeDescription = _
                                        CType(Session(CNClaimQuote), NexusProvider.Quote).Risks.FindItemByRiskKey( _
                                            .RiskKey).Description
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
                                    oOpenClaim.UserDefFldDCode = .UserDefFldDCode
                                    oOpenClaim.UserDefFldECode = .UserDefFldECode
                                    oOpenClaim.TPA = .TPA
                                    'Added for Insurer
                                    oOpenClaim.Insurer = .Insurer
                                    Session.Item(CNClaimTimeStamp) = .TimeStamp
                                    oOpenClaim.CurrencyISOCode = .CurrencyCode
                                    Session.Item(CNCurrenyCode) = Trim(.CurrencyCode) 'Changed
                                    oOpenClaim.Client = .Client
                                    'this needs to be removed after SAM issue is resolved
                                    If oOpenClaim.Client.PartyKey = 0 Then
                                        oOpenClaim.Client.PartyKey = oQuote.PartyKey
                                    End If
                                    'Session(CNInsurer_Header) = .ClientName
                                    Session(CNClaimNumber) = .ClaimNumber
                                    Session(CNStatus) = .ClaimStatus

                                    If sMode = "CLAIMPAYMENT" Or sMode = "CLAIMVIEW" Then
                                        'Retreival of the risk related values 
                                        'Arch issue 268

                                        Dim oOptionType As New NexusProvider.OptionTypeSetting
                                        oOptionType = _
                                            oWebService.GetOptionSetting(NexusProvider.OptionType.ProductOption, 12)
                                        If _
                                            (oOptionType IsNot Nothing AndAlso _
                                             String.IsNullOrEmpty(oOptionType.OptionValue) = False) _
                                            AndAlso oOptionType.OptionValue = "1" Then
                                            oClaimRisk = GetClaimRiskCall(.BaseClaimKey, .ClaimKey, sBranchCode)
                                            Session(CNDataSet) = oClaimRisk.XMLDataSet
                                        End If
                                    End If
                                End With
                            End If
                        Next
                        bUnlockRequired = False
                        Session(CNClaim) = oOpenClaim

                    Catch ex As System.Exception
                        Response.Redirect("~/Claims/FindInsuranceFile.aspx")
                    Finally
                        If bUnlockRequired Then
                            Dim oLock As New NexusProvider.Locks
                            Dim oLockCollection As New NexusProvider.LockCollection

                            oLock.LockName = "claim_id"                    ''It is equivalent to lockname for locking claims in SAM
                            oLock.LockValue = oOpenClaim.BaseClaimKey

                            oLockCollection.Add(oLock)
                            oWebService.MaintainLock(oLockCollection, False, False, Session(CNBranchCode).ToString())
                        End If
                        oClaimDetails = Nothing
                        oWebService = Nothing
                        oClaimRisk = Nothing
                    End Try

                    Response.Redirect("~/Claims/Overview.aspx")
            End Select
        End Sub

        Public Function GetPolicyForClaimDate(ByVal v_oPolicies As NexusProvider.PolicyCollection, _
                                              ByVal v_dtClaimDate As Date, ByRef r_lInsuranceFileCnt As Integer, _
                                              ByRef r_sPolicyNumber As String, ByRef r_dtStartDate As Date, _
                                              ByRef r_dtEndDate As Date, Optional ByRef r_lReturnCode As Integer = 0, _
                                              Optional ByRef r_dtInceptionDate As Date = #12:00:00 PM#, _
                                              Optional ByRef r_sTypeCode As String = "") As Integer

            Dim result As Integer = 0
            Dim lCurrentPosition As Integer

            Dim dtFirstStartDate, dtLastExpiryDate As Date
            Dim bFoundDate As Boolean

            Const ReturnCode_Error As Integer = 0
            Const ReturnCode_Ok As Integer = 1
            Const ReturnCode_TooEarly As Integer = 2
            Const ReturnCode_TooLate As Integer = 3


            For lCount As Integer = 0 To v_oPolicies.Count - 1

                If r_lInsuranceFileCnt = v_oPolicies.Item(lCount).InsuranceFileKey Then

                    r_dtStartDate = CDate(v_oPolicies.Item(lCount).CoverStartDate)

                    r_dtEndDate = CDate(v_oPolicies.Item(lCount).ExpiryDate)
                    bFoundDate = True
                    Exit For
                End If
            Next lCount

            ' if we havent found a date yet then
            If Not bFoundDate Then

                For lCount As Integer = 0 To v_oPolicies.Count - 1
                    If _
                        (v_oPolicies.Item(lCount).InsuranceFileTypeKey = 2 Or _
                         v_oPolicies.Item(lCount).InsuranceFileTypeKey = 5 Or _
                         v_oPolicies.Item(lCount).InsuranceFileTypeKey = 6 Or _
                         v_oPolicies.Item(lCount).InsuranceFileTypeKey = 8 Or _
                         v_oPolicies.Item(lCount).InsuranceFileTypeKey = 9) Then

                        r_dtStartDate = CDate(v_oPolicies.Item(lCount).CoverStartDate)

                        r_dtEndDate = CDate(v_oPolicies.Item(lCount).ExpiryDate)
                        bFoundDate = True
                        Exit For
                    End If
                Next lCount
            End If

            ' Find a version of the policy which encompasses the claim date
            lCurrentPosition = -1

            For lCount As Integer = v_oPolicies.Count - 1 To 0 Step -1

                If _
                    (v_oPolicies.Item(lCount).InsuranceFileTypeKey = 2 Or _
                     v_oPolicies.Item(lCount).InsuranceFileTypeKey = 5 Or _
                     v_oPolicies.Item(lCount).InsuranceFileTypeKey = 6 Or _
                     v_oPolicies.Item(lCount).InsuranceFileTypeKey = 8 Or _
                     v_oPolicies.Item(lCount).InsuranceFileTypeKey = 9) Then
                    If _
                        (v_oPolicies.Item(lCount).CoverStartDate <= v_dtClaimDate) And _
                        (v_dtClaimDate <= v_oPolicies.Item(lCount).ExpiryDate) Then

                        ' We found a valid one, note its position and exit!
                        lCurrentPosition = lCount
                        Exit For
                    End If

                    ' Save the earliest start date and the latest expiry date

                    If CDate(v_oPolicies.Item(lCount).CoverStartDate) < dtFirstStartDate Or lCount = 0 Then

                        dtFirstStartDate = CDate(v_oPolicies.Item(lCount).CoverStartDate)
                    End If

                    If CDate(v_oPolicies.Item(lCount).ExpiryDate) > dtLastExpiryDate Or lCount = 0 Then

                        dtLastExpiryDate = CDate(v_oPolicies.Item(lCount).ExpiryDate)
                    End If
                End If
            Next lCount

            r_lInsuranceFileCnt = 0
            r_dtInceptionDate = dtFirstStartDate

            If lCurrentPosition <> -1 Then

                ' Found one, we returns its details

                r_sPolicyNumber = v_oPolicies.Item(lCurrentPosition).InsuranceFileRef

                r_lInsuranceFileCnt = v_oPolicies.Item(lCurrentPosition).InsuranceFileKey

                r_sTypeCode = v_oPolicies.Item(lCurrentPosition).InsuranceFileTypeCode

                r_lReturnCode = ReturnCode_Ok

            Else

                ' Else, we check why we haven't found one, to report to user
                If v_dtClaimDate > dtLastExpiryDate Then
                    ' The claim date if after the latest expiry date
                    r_lReturnCode = ReturnCode_TooLate
                ElseIf v_dtClaimDate < dtFirstStartDate Then
                    ' The claim date is before the earliest start date
                    r_lReturnCode = ReturnCode_TooEarly
                Else
                    ' We can't find any policy, probably because the claim date is in between
                    ' two valid policy versions
                    r_lReturnCode = ReturnCode_Error
                End If

            End If

            Return result
        End Function

        Private Sub UnlockClaim(nClaimKey As Integer)
            Dim oLockCollection As NexusProvider.LockCollection
            Dim oWebService As NexusProvider.ProviderBase = Nothing
            Dim sUserName As String = String.Empty
            Dim bMaintainedSuccess As Boolean = False
            Dim bLogout As Boolean = False
            Dim bAllClear As Boolean = False
            Dim oLock As New NexusProvider.Locks
            oWebService = New NexusProvider.ProviderManager().Provider
            oLockCollection = oWebService.GetLockDetails(Session(CNBranchCode).ToString())

            For Each oLockItem As NexusProvider.Locks In oLockCollection
                If HttpContext.Current.User.Identity.Name.Trim().ToUpper = oLockItem.LockUserName.Trim().ToUpper AndAlso oLockItem.LockName.Trim() = "claim_id" AndAlso oLockItem.LockValue = nClaimKey Then
                    oLock.LockName = oLockItem.LockName
                    oLock.LockValue = oLockItem.LockValue
                    oLockCollection.Add(oLock)
                    bMaintainedSuccess = oWebService.MaintainLock(oLockCollection, bAllClear, bLogout, Session(CNBranchCode).ToString())
                    Exit For
                End If
            Next

        End Sub
    End Class
End Namespace

