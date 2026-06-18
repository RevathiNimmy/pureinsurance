Imports System.Web.Configuration.WebConfigurationManager
Imports Nexus
Imports Nexus.Constants
Imports Nexus.Constants.Session
Imports CMS.Library
Imports Nexus.Library
Namespace Nexus

    Partial Class Framework_Complete
        Inherits CMS.Library.Frontend.clsCMSPage

#Region " Page Events "

        Protected Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init

            ' Load the claimcomplete control on page init otherwise view state for Document manager will not get maintained
            Dim oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
            Dim oPortal As Nexus.Library.Config.Portal = oNexusConfig.Portals.Portal(Portal.GetPortalID())
            Dim WebControlPath As String
            Dim sFolder As String = "~/Claims/ClientPages/" & oPortal.Claims.ScreenLocation
            WebControlPath = sFolder & "/claimcomplete.ascx"
            If (System.IO.File.Exists(Request.MapPath(WebControlPath))) Then
                Dim tempControl As Control = LoadControl(WebControlPath)
                TransactionConfirmation.Controls.Clear()
                TransactionConfirmation.Controls.Add(tempControl)
            End If

        End Sub
        ''' <summary>
        ''' This event is fired on page load
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Shadows Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            'Check the link of case with claim
            Dim oOpenClaim As NexusProvider.ClaimOpen = Session(CNClaim)
            If Session(CNMode) <> Mode.ViewClaim Then
                If oOpenClaim IsNot Nothing AndAlso oOpenClaim.BaseCaseKey > 0 Then
                    btnReturnToCase.Visible = True
                    If Session(CNCaseKey) IsNot Nothing Then
                        btnReturnToCase.PostBackUrl = "~/Claims/ClaimCase.aspx?CaseKey=" & Session(CNCaseKey).ToString()
                    Else
                        btnReturnToCase.PostBackUrl = "~/Claims/ClaimCase.aspx?BaseCaseKey=" & oOpenClaim.BaseCaseKey
                    End If
                End If
            End If

            If Session.Item(CNClaimNumber) IsNot Nothing Then
                hypClaimNumber.Text = Trim(Session.Item(CNClaimNumber))
                hypClaimNumber.PostBackUrl = "~/Claims/FindClaim.aspx?Claimno=" & Trim(Session.Item(CNClaimNumber))
                If Session.Item(CNMode) = Mode.NewClaim Then
                    ltrlThankyouRegister.Visible = True
                ElseIf Session.Item(CNMode) = Mode.EditClaim Then
                    If Session(CNClaimStatus) IsNot Nothing Then
                        'If claim is "CLOSED"
                        If Session(CNClaimStatus).ToString.Trim.ToUpper = "CLOSED" Then
                            ltrlCloseClaim.Visible = True
                            Session(CNClaimStatus) = Nothing
                        ElseIf Session(CNClaimStatus).ToString.Trim.ToUpper = "UNABLETOCLOSE" Then
                            ltrlUnableToCloseClaim.Visible = True
                        Else
                            ltrlThankyouUpdate.Visible = True
                        End If
                    Else
                        ltrlThankyouUpdate.Visible = True
                    End If
                ElseIf Session.Item(CNMode) = Mode.PayClaim Or Session.Item(CNMode) = Mode.SalvageClaim Or Session.Item(CNMode) = Mode.TPRecovery Then
                    Session.Remove(CNEnablePayClaim)
                    'If claim is "Authorize Payment"
                    If Session(CNStatus) IsNot Nothing And Session(CNAuthorizeStatus) = "Authorize Payment" Then
                        ltrlAuthorizeClaimPayment.Visible = True
                    Else
                        ltrlThankyouUpdate.Visible = True
                    End If
                End If
            End If
            'Updation of Progress Bar status
            ucProgressBar.OverviewStyle = "complete"
            ucProgressBar.ReinsuranceStyle = "complete"
            ucProgressBar.PerilsStyle = "complete"
            ucProgressBar.SummaryStyle = "complete"
            ucProgressBar.CompleteStyle = "in-progress"

            ' Load the Document template
            Dim oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
            Dim oPortal As Nexus.Library.Config.Portal = oNexusConfig.Portals.Portal(Portal.GetPortalID())
            Dim WebControlPath As String
            Dim sFolder As String = "~/Claims/ClientPages/" & oPortal.Claims.ScreenLocation
            WebControlPath = sFolder & "/claimcomplete.ascx"
            If (System.IO.File.Exists(Request.MapPath(WebControlPath))) Then
                Dim tempControl As Control = LoadControl(WebControlPath)
                TransactionConfirmation.Controls.Clear()
                TransactionConfirmation.Controls.Add(tempControl)
            End If
	    
	    ClearTemporarySessionValues()
        End Sub
#End Region

        Protected Sub hypClaimNumber_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hypClaimNumber.Click
            Session(CNMode) = Mode.ViewClaim
            Session.Remove(CNClaim)
            Session.Remove(CNClaimsSearchData)
            Session.Remove(CNClaimStatus)

            Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oClaimVersions As NexusProvider.VersionsCollections = Nothing
            Dim oQuote As NexusProvider.Quote = Nothing
            Dim oBaseParty As NexusProvider.BaseParty = Nothing
            Dim sClaimNumber As String = CStr(hypClaimNumber.Text)
            Dim iHighest As Integer = 0
            Dim oOpenClaim As New NexusProvider.ClaimOpen
            Dim oClaimDetails As NexusProvider.ClaimDetails = Nothing
            Dim oCashListItem As NexusProvider.CashListItemsCollection = Nothing
            Dim oClaimRisk As NexusProvider.ClaimRisk = Nothing

            Try
                oClaimVersions = oWebservice.GetVersionsForClaim(sClaimNumber)
                If oClaimVersions IsNot Nothing Then
                    'Find Highest Version

                    For iCount As Integer = 0 To oClaimVersions.Count - 1
                        If oClaimVersions(iCount).Version > iHighest Then
                            iHighest = oClaimVersions(iCount).Version
                        End If
                    Next

                    'Updating of claim quote oQuote
                    oQuote = oWebservice.GetHeaderAndSummariesByKey(oClaimVersions(0).InsuranceFileKey)
                    If oQuote IsNot Nothing Then
                        oBaseParty = oWebservice.GetParty(oQuote.PartyKey)
                        Session.Item(CNParty) = oBaseParty
                        Session.Item(CNRisks) = oQuote.Risks
                        Session.Item(CNRenewalDate) = oQuote.RenewalDate
                        Session.Item(CNAddress) = oBaseParty.Addresses(0).Address1 & ", " & oBaseParty.Addresses(0).Address4
                        Session.Item(CNDate_Header) = oQuote.CoverStartDate.ToShortDateString & " - " & oQuote.CoverEndDate.ToShortDateString
                        Session(CNInsurer_Header) = oQuote.InsuredName
                        Session(CNProductCode) = oQuote.ProductCode
                        Session(CNClaimQuote) = oQuote
                    End If
                    Session(CNClaimVersion) = oClaimVersions
                    Session.Item(CNInsuranceFileKey) = oClaimVersions(0).InsuranceFileKey
                    Session.Item(CNPolicyNumber) = oClaimVersions(0).InsuranceRef
                    'Response.Redirect("FindClaim.aspx")
                End If
                For iCount As Integer = 0 To oClaimVersions.Count - 1
                    If oClaimVersions(iCount).Version = iHighest Then


                        'Retreival of claim details
                        Dim sBranchCode As String = oQuote.BranchCode
                        'arch issue 268
                        oClaimDetails = GetClaimDetailsCall(oClaimVersions(iCount).ClaimKey, sBranchCode)

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
                            'Retreival of the risk related values 
                            Dim bClaimBuilder As Boolean = False
                            Boolean.TryParse(Session(CNClaimBuilder), bClaimBuilder)
                            If bClaimBuilder Then
                                oClaimRisk = GetClaimRiskCall(.BaseClaimKey, .ClaimKey, sBranchCode)
                                Session(CNDataSet) = oClaimRisk.XMLDataSet
                            End If


                        End With
                    End If
                Next

                Session(CNClaim) = oOpenClaim
                Response.Redirect("~/Claims/Overview.aspx")
            Finally
                oClaimDetails = Nothing
                oWebservice = Nothing
                oClaimRisk = Nothing
            End Try
        End Sub
    End Class

End Namespace
