Imports System.Configuration.ConfigurationManager
Imports NexusProvider.SAMForInsurance
Imports Nexus
Imports Nexus.Utils
Imports Nexus.Constants
Imports Nexus.Constants.Session
Imports CMS.Library.Portal
Imports Nexus.Library

Namespace Nexus
    Partial Class Claims_DuplicateClaimCheck
        Inherits CMS.Library.Frontend.clsCMSPage
        ''' <summary>
        ''' Page_Load Method
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub Page_Load1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            'population of the grid with duplicate claim
            If Not Page.IsPostBack Then
                Dim oDuplicateClaims As New NexusProvider.ClaimCollection
                oDuplicateClaims = CType(Session.Item(CNClaimsSearchData), NexusProvider.ClaimCollection)
                grdvDuplicateClaims.DataSource = oDuplicateClaims
                grdvDuplicateClaims.DataBind()
                'Checking of User Authority for Overriding the Duplicate Claim
                Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                Dim oUserAuthority As New NexusProvider.UserAuthority
                oUserAuthority.UserCode = Session.Item(CNLoginName)
                oUserAuthority.UserAuthorityOption = NexusProvider.UserAuthority.UserAuthorityOptionType.CanDuplicateClaimOverride
                oWebservice.GetUserAuthorityValue(oUserAuthority)
                If oUserAuthority.UserAuthorityValue = "1" Then
                    btnNext.Visible = True
                    pnlDuplicateClaimOverride.Visible = False
                    btnOK.Visible = False
                    rfvUserName.Enabled = False
                    rfvPassword.Enabled = False
                Else
                    btnNext.Visible = False
                    pnlDuplicateClaimOverride.Visible = True
                    btnOK.Visible = True
                    rfvUserName.Enabled = True
                    rfvPassword.Enabled = True
                 End If
            End If

        End Sub
        ''' <summary>
        ''' On Next Button. If logged user has overriding authority
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub btnNext_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNext.Click
            'Overriding the Duplicate Claim
            OverrideDuplicateClaim()
        End Sub
        ''' <summary>
        ''' This function will be called when user will proceed with duplicate claim override
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub OverrideDuplicateClaim()
            'Overriding the Duplicate Claim
            Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oClaimResponse As NexusProvider.ClaimResponse = Nothing
            Dim oUserDetails As NexusProvider.UserDetails = CType(Session(CNAgentDetails), NexusProvider.UserDetails)
            Dim oClaimOpen As New NexusProvider.ClaimOpen
            Dim oClaimRisk As NexusProvider.ClaimRisk = Nothing
            Dim oQuote As NexusProvider.Quote = Session(CNClaimQuote)
            Dim sBranchCode As String = oQuote.BranchCode
            Dim oOpenClaim As NexusProvider.ClaimOpen = CType(Session(CNClaim), NexusProvider.ClaimOpen)

            If Request.QueryString("infoonly") = "1" Then
                If Session(CNBaseCaseKey) IsNot Nothing Then
                    Integer.TryParse(Convert.ToString(Session(CNBaseCaseKey)), oOpenClaim.BaseCaseKey)
                End If
                Try
                    oClaimResponse = OpenClaimCall(oOpenClaim, sBranchCode)
                    If oClaimResponse Is Nothing Then
                        Exit Sub
                    End If

                    With oClaimResponse
                        oClaimOpen.ClaimKey = .ClaimKey
                        oClaimOpen.ClaimNumber = .ClaimNumber
                        oClaimOpen.BaseClaimKey = .BaseClaimKey
                        Session.Item(CNClaimTimeStamp) = .TimeStamp
                        oClaimOpen.ClaimVersion = .Version
                        oClaimOpen.ClaimStatus = .ResultingStatus
                        Session.Item(CNClaimNumber) = .ClaimNumber
                        oClaimOpen.Client = oOpenClaim.Client
                    End With
                Catch ex As NexusProvider.NexusException
                    If ex.Errors(0).Code = "1000001" Then
                        Dim strAlertMessage As String = "alert('" & GetLocalResourceObject("error_InvalidUserDetails") & "')"
                        ScriptManager.RegisterStartupScript(Me.Page, GetType(String), "InValidUserDetails", strAlertMessage, True)
                        Exit Sub
                    End If
                End Try
                Session(CNClaim) = oClaimOpen
                'Check the Claim Builder Hidden product option
                Dim oOptionType As New NexusProvider.OptionTypeSetting
                oOptionType = oWebservice.GetOptionSetting(NexusProvider.OptionType.ProductOption, 12)
                If (oOptionType IsNot Nothing AndAlso String.IsNullOrEmpty(oOptionType.OptionValue) = False) _
AndAlso oOptionType.OptionValue = "1" Then
                    oClaimRisk = AddClaimRiskCall(oClaimResponse.BaseClaimKey, Session.Item(CNClaimTimeStamp), sBranchCode)
                    If oClaimRisk Is Nothing Then
                        Exit Sub
                    End If

                End If

            Dim sRedirectUrl As String
            sRedirectUrl = RedirectShowCheckUnpaidPremium ("COMPLETE")

            If sRedirectUrl.Trim <>   ""  Andalso (CType(Session.Item(CNMode), Mode) = Mode.PayClaim OrElse  CType(Session.Item(CNMode), Mode) = Mode.NewClaim)   Then
                Response.Redirect(sRedirectUrl, False)
            Else
                Response.Redirect("~/Claims/complete.aspx", False)
            End If

                
            Else
                'calling of sam method
                'To skip the posting first time
                Dim oOriginalClaim As NexusProvider.ClaimOpen = CType(Session.Item(CNClaim), NexusProvider.ClaimOpen)
                oOriginalClaim.ReserveOnly = True
                If Session(CNBaseCaseKey) IsNot Nothing Then
                    Integer.TryParse(Convert.ToString(Session(CNBaseCaseKey)), oOriginalClaim.BaseCaseKey)
                End If
                Try
                    oClaimResponse = OpenClaimCall(oOriginalClaim, sBranchCode)
                    If oClaimResponse Is Nothing Then
                        Exit Sub
                    End If
                Catch ex As NexusProvider.NexusException
                    If ex.Errors(0).Code = "1000001" Then
                        'Duplicate Claim Override
                        Dim strAlertMessage As String = "alert('" & GetLocalResourceObject("error_UnAuthorisedUserDetails") & "')"
                        ScriptManager.RegisterStartupScript(Me.Page, GetType(String), "UnAuthorisedUserDetails", strAlertMessage, True)
                        Exit Sub
                    ElseIf ex.Errors(0).Code = "1000109" Then
                        Dim strAlertMessage As String = "alert('" & GetLocalResourceObject("error_InvalidUserDetails") & "')"
                        ScriptManager.RegisterStartupScript(Me.Page, GetType(String), "InValidUserDetails", strAlertMessage, True)
                        Exit Sub
                    End If
                End Try
                'Check the Claim Builder Hidden product option
                Dim oOptionType As New NexusProvider.OptionTypeSetting
                oOptionType = oWebservice.GetOptionSetting(NexusProvider.OptionType.ProductOption, 12)
                If (oOptionType IsNot Nothing AndAlso String.IsNullOrEmpty(oOptionType.OptionValue) = False) _
AndAlso oOptionType.OptionValue = "1" Then
                    'sam call for claim risk
                    oClaimRisk = AddClaimRiskCall(oClaimResponse.BaseClaimKey, oClaimResponse.TimeStamp, sBranchCode)
                    If oClaimRisk Is Nothing Then
                        Exit Sub
                    End If

                End If
                'Update the session variable
                GetClaimDetails(oClaimResponse.ClaimKey, oClaimRisk)
                 Dim sUrl As String
                sUrl = RedirectShowCheckUnpaidPremium ("ClaimBuilder")
                If sUrl.Trim <> "" Then
                          Response.Redirect(sUrl, False)
                Else
                            sUrl   = CheckClaimBuilder()
                            Response.Redirect(sUrl, False)
                End If


               
            End If
        End Sub
        ''' <summary>
        ''' Back Button Functionality
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBack.Click
            Response.Redirect("~/claims/overview.aspx", False)
        End Sub

        ''' <summary>
        ''' This event will be called after providing duplicate claim override username and password
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub btnOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOK.Click
            Dim oOpenClaim As NexusProvider.ClaimOpen = CType(Session(CNClaim), NexusProvider.ClaimOpen)
            oOpenClaim.DuplicateClaimOverrideUserName = txtUserName.Text.Trim
            oOpenClaim.DuplicateClaimOverrideUserPassword = txtPassword.Text
            Session(CNClaim) = oOpenClaim
            OverrideDuplicateClaim()
        End Sub
        Sub GetClaimDetails(ByVal v_iClaimKey As Integer, ByVal oClaimRisk As NexusProvider.ClaimRisk)
            Dim oClaimDetails As NexusProvider.ClaimDetails = Nothing
            Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oOriginalClaim As NexusProvider.ClaimOpen = CType(Session.Item(CNClaim), NexusProvider.ClaimOpen)
            Dim oQuote As NexusProvider.Quote = Session(CNClaimQuote)
            Dim sBranchCode As String = oQuote.BranchCode
            'Retreiving the latest details
            'arch issue 268
            oClaimDetails = GetClaimDetailsCall(v_iClaimKey, sBranchCode)
            'updation of latest session values 
            Session.Item(CNClaimTimeStamp) = oClaimDetails.TimeStamp
            'If there is no need to update the claim risk details
            If oClaimRisk IsNot Nothing Then
                Session.Item(CNClaimRiskTimeStamp) = oClaimRisk.TimeStamp
                Session.Item(CNDataSet) = oClaimRisk.XMLDataSet
            End If
            Session.Item(CNBaseClaimKey) = oClaimDetails.BaseClaimKey
            Session.Item(CNClaimKey) = oClaimDetails.ClaimKey
            Session.Item(CNClaimNumber) = oClaimDetails.ClaimNumber

            With oClaimDetails
                oOriginalClaim.CatastropheCode = .CatastropheCode
                oOriginalClaim.BaseClaimKey = .BaseClaimKey
                oOriginalClaim.BaseCaseKey = .BaseCaseKey
                oOriginalClaim.Claim = .Claim
                oOriginalClaim.ClaimCoInsurer = .ClaimCoInsurer
                oOriginalClaim.ClaimDescription = .ClaimDescription
                oOriginalClaim.ClaimHandlerDescription = .ClaimHandlerDescription
                oOriginalClaim.ClaimKey = .ClaimKey
                oOriginalClaim.ClaimNumber = .ClaimNumber
                oOriginalClaim.ClaimPeril = .ClaimPeril
                oOriginalClaim.ClaimStatus = .ClaimStatus
                oOriginalClaim.ClaimStatusDate = .ClaimStatusDate
                oOriginalClaim.ClaimStatusID = .ClaimStatusID
                oOriginalClaim.ClaimVersion = .ClaimVersion
                oOriginalClaim.ClaimVersionDescription = .ClaimVersionDescription
                oOriginalClaim.ClientClaimNumber = .ClientClaimNumber
                oOriginalClaim.ClientEmail = .ClientEmail
                oOriginalClaim.ClientFaxNo = .ClientFaxNo
                oOriginalClaim.ClientMobileNo = .ClientMobileNo
                oOriginalClaim.ClientName = .ClientName
                oOriginalClaim.ClientShortName = .ClientShortName
                oOriginalClaim.ClientTelNo = .ClientTelNo
                oOriginalClaim.ClientTelNoOff = .ClientTelNoOff
                oOriginalClaim.CloseClaimOnZeroReserveRecoveryBalance = .CloseClaimOnZeroReserveRecoveryBalance
                oOriginalClaim.Comments = .Comments
                oOriginalClaim.Contact = .Contact
                oOriginalClaim.CurrencyISOCode = .CurrencyISOCode
                oOriginalClaim.Description = .Description
                oOriginalClaim.ExternalHandler = .ExternalHandler
                oOriginalClaim.HandlerCode = .HandlerCode
                oOriginalClaim.IgnoreClaimMaintain = .IgnoreClaimMaintain
                oOriginalClaim.InfoOnly = .InfoOnly
                oOriginalClaim.InsuranceFileKey = .InsuranceFileKey
                oOriginalClaim.InsuranceRef = .InsuranceRef
                oOriginalClaim.InsurerClaimNumber = .InsurerClaimNumber
                oOriginalClaim.IsAllowedClosedClaims = .IsAllowedClosedClaims
                oOriginalClaim.IsDeleted = .IsDeleted
                oOriginalClaim.LastModifiedDate = .LastModifiedDate
                oOriginalClaim.LikelyClaim = .LikelyClaim
                oOriginalClaim.Location = .Location
                oOriginalClaim.LossDate = .LossDate
                oOriginalClaim.LossDateFrom = .LossDateFrom
                oOriginalClaim.LossFromDate = .LossToDate
                oOriginalClaim.LossToDate = .LossToDate
                oOriginalClaim.LossToDateSpecified = .LossToDateSpecified
                oOriginalClaim.Payments = .Payments
                oOriginalClaim.PolicyNumber = .PolicyNumber
                oOriginalClaim.PolicyType = .PolicyType
                oOriginalClaim.PrimaryCause = .PrimaryCause
                oOriginalClaim.PrimaryCauseCode = .PrimaryCauseCode
                oOriginalClaim.PrimaryCauseDescription = .PrimaryCauseDescription
                oOriginalClaim.ProductDescription = .ProductDescription
                oOriginalClaim.ProgressStatusCode = .ProgressStatusCode
                oOriginalClaim.ProgressStatusDescription = .ProgressStatusDescription
                oOriginalClaim.ReportedDate = .ReportedDate
                oOriginalClaim.RiskKey = .RiskKey
                oOriginalClaim.SecondaryCause = .SecondaryCause
                oOriginalClaim.SecondaryCauseCode = .SecondaryCauseCode
                oOriginalClaim.SecondaryCauseDescription = .SecondaryCauseDescription
                oOriginalClaim.TotalCurrentShareValue = .TotalCurrentShareValue
                oOriginalClaim.TotalShare = .TotalShare
                oOriginalClaim.Town = .Town
                oOriginalClaim.TownCode = .TownCode
                oOriginalClaim.UnderwritingYearCode = .UnderwritingYearCode
                oOriginalClaim.UserDefFldACode = .UserDefFldACode
                oOriginalClaim.UserDefFldBCode = .UserDefFldBCode
                oOriginalClaim.UserDefFldCCode = .UserDefFldCCode
                oOriginalClaim.UserDefFldDCode = .UserDefFldECode
                oOriginalClaim.UserDefFldECode = .UserDefFldECode
                oOriginalClaim.IsPolicyOutstanding = . IsPolicyOutstanding

            End With
            Session.Item(CNClaim) = oOriginalClaim
        End Sub
    End Class
End Namespace
