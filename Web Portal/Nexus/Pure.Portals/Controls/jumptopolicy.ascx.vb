Imports CMS.Library.Frontend
Imports System.Web.Configuration.WebConfigurationManager
Imports CMS.Library
Imports System.Web.Configuration
Imports Nexus.Library
Imports Nexus.Utils
Imports Nexus.Constants
Imports Nexus.Constants.Session
Namespace Nexus

    Partial Class Controls_jumptopolicy
        Inherits System.Web.UI.UserControl

        Protected Sub btnGo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGo.Click
            If txtPolicyNo.Text.Trim.Length <> 0 Then
                Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                Dim oSearchCriteria As New NexusProvider.PartySearchCriteria
                Dim oPartyCollection As NexusProvider.PartyCollection
                Dim oParty As NexusProvider.BaseParty
                Dim oPartySummary As NexusProvider.PartySummary
                Dim oQuote As NexusProvider.Quote
                If txtPolicyNo.Text.Trim.Contains("%") Then
                    lbl_ErrMsg.Visible = True
                    lbl_ErrMsg.Text = GetLocalResourceObject("Err_InvalidPolicyNumber")
                    Exit Sub
                End If

                lbl_ErrMsg.Visible = False
                oSearchCriteria.PolicyRef = txtPolicyNo.Text.Trim
                oSearchCriteria.PartyType = NexusProvider.PartyTypeType.GC
                oSearchCriteria.PartyTypes.Add(NexusProvider.PartyTypeType.PC)
                oSearchCriteria.PartyTypes.Add(NexusProvider.PartyTypeType.CC)
                oSearchCriteria.Status = "ALL"
                oPartyCollection = oWebService.FindParty(oSearchCriteria)
                If oPartyCollection IsNot Nothing Then
                    If oPartyCollection.Count > 0 Then
                        oParty = oWebService.GetParty(oPartyCollection(0).Key)

                        Session(CNParty) = oParty

                        Select Case True
                            Case TypeOf oParty Is NexusProvider.CorporateParty
                                With CType(oParty, NexusProvider.CorporateParty)
                                    oPartySummary = oWebService.GetPartyPolicies(.ClientSharedData.ShortName.Trim)
                                End With
                            Case TypeOf oParty Is NexusProvider.PersonalParty
                                With CType(oParty, NexusProvider.PersonalParty)
                                    oPartySummary = oWebService.GetPartyPolicies(.ClientSharedData.ShortName.Trim)
                                End With
                        End Select

                        Dim oPolicyColl As New NexusProvider.PolicyCollection

                        For iCount As Integer = 0 To oPartySummary.Policies.Count - 1
                            If oPartySummary.Policies(iCount).InsuranceFileTypeCode IsNot Nothing Then
                                If oPartySummary.Policies(iCount).Reference.Trim.ToUpper = txtPolicyNo.Text.Trim.ToUpper Then
                                    oPolicyColl.Add(oPartySummary.Policies(iCount))
                                End If
                            End If
                        Next
                        If oPolicyColl.Count > 1 Then
                            'Error Msg will come
                        Else
                            oQuote = oWebService.GetHeaderAndSummariesByKey(oPolicyColl(0).InsuranceFileKey)

                            For i As Integer = 0 To oQuote.Risks.Count - 1
                                oWebService.GetRisk(oQuote.Risks(i).Key, i, oQuote, oQuote.BranchCode)
                            Next
                            Session(CNCurrenyCode) = oQuote.CurrencyCode

                            oWebService.GetHeaderAndRisksByKey(oQuote)

                            Session(CNQuote) = oQuote

                            'Use the GetDataSetDefinition to interogate the dataset to get the datamodelcode into session
                            GetDataSetDefinition()

                            If oPolicyColl(0).InsuranceFileTypeCode.Trim.ToUpper = "POLICY" Then
                                Session(CNRenewal) = Nothing
                                Session(CNMode) = Mode.View
                                Session.Remove(CNOI)
                                Session.Remove(CNQuoteMode)
                                Session(CNQuoteInSync) = False
                                Session(CNQuoteMode) = QuoteMode.FullQuote
                                Session(CNClientMode) = Mode.View
                                Response.Redirect("~/secure/PremiumDisplay.aspx", False)
                            Else 'For Quote
                                Session(CNRenewal) = Nothing
                                Session(CNMode) = Mode.Edit
                                Session(CNQuoteInSync) = False
                                Session.Remove(CNOI)
                                Session(CNInsuranceFileKey) = oPolicyColl(0).InsuranceFileKey
                                Session(CNQuoteInSync) = False
                                If IsDataSetQuickQuote() = False Then
                                    Session(CNQuoteMode) = QuoteMode.FullQuote
                                Else
                                    Session(CNQuoteMode) = QuoteMode.QuickQuote

                                End If
                                Response.Redirect("~/secure/PremiumDisplay.aspx", False)
                            End If

                        End If
                    Else
                        lbl_ErrMsg.Visible = True
                        lbl_ErrMsg.Text = GetLocalResourceObject("Err_InvalidPolicyNumber")
                        Exit Sub
                    End If
                Else
                    lbl_ErrMsg.Visible = True
                    lbl_ErrMsg.Text = GetLocalResourceObject("Err_InvalidPolicyNumber")
                    Exit Sub
                End If
            Else
                lbl_ErrMsg.Visible = True
                lbl_ErrMsg.Text = GetLocalResourceObject("Err_InvalidPolicyNumber")
                Exit Sub
            End If

        End Sub

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            If HttpContext.Current.User.Identity.IsAuthenticated Then
                Me.Visible = True
            Else
                Me.Visible = False
            End If
        End Sub
    End Class
End Namespace
