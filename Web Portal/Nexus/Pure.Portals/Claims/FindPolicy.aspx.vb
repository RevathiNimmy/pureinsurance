Imports System.Configuration.ConfigurationManager
Imports Nexus
Imports NexusProvider.SAMForInsurance
Imports Nexus.Constants
Imports Nexus.Constants.Session

Namespace Nexus

    Partial Class Framework_FindPolicy
        Inherits CMS.Library.Frontend.clsCMSPage
        ''' <summary>
        ''' Page_Load
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Shadows Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            rngLossDate.MaximumValue = Now.Date
        End Sub
        ''' <summary>
        ''' On Find Click
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub btnFind_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFind.Click

            If Page.IsValid Then
                'Cleaning of session values
                ClearHeader()
                ClearSearch()
                ClearClaims()
                ClearQuote()

                Dim oUserDetails As NexusProvider.UserDetails

                oUserDetails = CType(Session(CNAgentDetails), NexusProvider.UserDetails)
                Dim sBranchCode As String = oUserDetails.ListOfBranches(0).Code

                Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                Dim oInsuranceFileForClaim As New NexusProvider.InsuranceFileDetails
                'Setting of search criteria
                oInsuranceFileForClaim.LossDate = FormatDateTime(CONTROL__LOSS_DATE.Text, DateFormat.ShortDate)
                oInsuranceFileForClaim.SearchDate = FormatDateTime(CONTROL__LOSS_DATE.Text, DateFormat.ShortDate)
                oInsuranceFileForClaim.InsuranceRef = CONTROL__POLICY_NUMBER.Text
                Dim oQuote As NexusProvider.Quote
                Dim oInsuranceDetails As NexusProvider.InsuranceFileDetailsCollection

                Try
                    'calling of sam method based on search criteria entered
                    oInsuranceDetails = oWebService.FindInsuranceFileForClaims(oInsuranceFileForClaim, sBranchCode)
                    If Not IsNothing(oInsuranceDetails) And oInsuranceDetails.Count > 0 Then
                        If oInsuranceDetails(0).InsuranceFileKey <> 0 Then
                            'Retreving the quote information and storing the values in session
                            oQuote = oWebService.GetHeaderAndSummariesByKey(oInsuranceDetails(0).InsuranceFileKey, sBranchCode)

                            If CDate(CONTROL__LOSS_DATE.Text) >= oQuote.CoverStartDate Then

                                Session.Item(CNInsuranceFileKey) = oInsuranceDetails(0).InsuranceFileKey
                                Session.Item(CNLossDate) = FormatDateTime(CONTROL__LOSS_DATE.Text, DateFormat.ShortDate)

                                Session.Item(CNRisks) = oQuote.Risks
                                Session.Item(CNPartyKey) = oQuote.PartyKey

                                Session.Item(CNDate_Header) = oQuote.CoverStartDate.ToShortDateString & " - " & oQuote.CoverEndDate.ToShortDateString
                                Session.Item(CNPolicyNumber) = oQuote.InsuranceFileRef
                                Session.Item(CNCurrency) = AppSettings("CurrencyISOCode")
                                Session.Item(CNClaimNumber) = "TBA" 'add to resource!
                                Session.Item(CNStatus) = AppSettings("NewClaimStatus")

                                Session.Item(CNMode) = Mode.Add

                                Response.Redirect("Overview.aspx", False)
                            Else
                                IsValidDate.IsValid = False
                                'End If

                            End If

                        Else
                            'Policy not found or not within the date supplied
                            IsPolicyExist.IsValid = False
                        End If
                    Else
                        'Policy not found or not within the date supplied
                        IsPolicyExist.IsValid = False
                    End If
                Catch ex As NexusProvider.NexusException
                    oInsuranceDetails = Nothing
                    oQuote = Nothing
                    IsPolicyExist.IsValid = False
                Finally
                    oInsuranceDetails = Nothing
                    oQuote = Nothing
                End Try
            End If
        End Sub
    End Class
End Namespace
