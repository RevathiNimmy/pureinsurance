Imports System.Configuration.ConfigurationManager
Imports Nexus.Constants
Imports Nexus.Constants.Session
Partial Class Controls_PolicyDetails
    Inherits System.Web.UI.UserControl

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim oQuote As NexusProvider.Quote = CType(Session(CNQuote), NexusProvider.Quote)
        Dim oParty As NexusProvider.BaseParty = CType(Session(CNParty), NexusProvider.BaseParty)

        If oQuote IsNot Nothing Then
            lt_CovertStartDate.Text = Convert.ToString(oQuote.CoverStartDate)
            lt_CovertEndDate.Text = Convert.ToString(oQuote.CoverEndDate)

            If Session(CNMode) = Mode.View Then
                litPolicyNo.Text = Convert.ToString(GetLocalResourceObject("litPolicyNo"))
                lt_PolicyNo.Text = oQuote.InsuranceFileRef

                lbl_BillingMethod.Visible = True
                txtBillingMethod.Visible = True
                txtBillingMethod.Text = oQuote.PaymentMethod

                Dim sFileTypeCode As String = If(oQuote.InsuranceFileTypeCode IsNot Nothing, Trim(oQuote.InsuranceFileTypeCode).Trim().ToUpper(), String.Empty)
                sFileTypeCode = sFileTypeCode.Replace(" ", "")
                Select Case sFileTypeCode
                    Case "MTAPERM", "MTAQPERM"
                        lt_PolicyType.Text = Convert.ToString(GetLocalResourceObject("lbl_MTA_Permanent"))
                    Case "MTATEMP", "MTAQTEMP"
                        lt_PolicyType.Text = Convert.ToString(GetLocalResourceObject("lbl_MTA_Temporary"))
                    Case "MTACAN", "MTAQCAN"
                        lt_PolicyType.Text = Convert.ToString(GetLocalResourceObject("lbl_MTA_Cancellation"))
                    Case "MTAREINS", "MTAQREINS"
                        lt_PolicyType.Text = Convert.ToString(GetLocalResourceObject("lbl_MTA_Reinstatement"))
                    Case "RENEWAL"
                        lt_PolicyType.Text = Convert.ToString(GetLocalResourceObject("lbl_Renewal"))
                    Case Else
                        lt_PolicyType.Text = Convert.ToString(GetLocalResourceObject("lbl_NewBusiness"))
                End Select
            ElseIf Session(CNMode) = Mode.ClonedTransferAmendment Then
                litPolicyNo.Text = Convert.ToString(GetLocalResourceObject("litPolicyNo"))
                lt_PolicyNo.Text = oQuote.InsuranceFileRef
                lt_PolicyType.Text = Convert.ToString(GetLocalResourceObject("lbl_ClonedTransferAmendment"))
            ElseIf Session(CNMode) = Mode.PortFolioTransferAmendment Then
                litPolicyNo.Text = Convert.ToString(GetLocalResourceObject("litPolicyNo"))
                lt_PolicyNo.Text = oQuote.InsuranceFileRef
                lt_PolicyType.Text = Convert.ToString(GetLocalResourceObject("lbl_PortFolioTransferAmendment"))
            ElseIf Session(CNRenewal) Is Nothing And Session(CNMTAType) Is Nothing And (Session(CNQuoteMode) = QuoteMode.QuickQuote OrElse Session(CNQuoteMode) = QuoteMode.FullQuote) Then
                lt_PolicyType.Text = Convert.ToString(GetLocalResourceObject("lbl_NewBusiness"))
                litPolicyNo.Text = Convert.ToString(GetLocalResourceObject("litQuoteNo"))
                lt_PolicyNo.Text = oQuote.InsuranceFileRef
            ElseIf Session(CNMTAType) = MTAType.PERMANENT Then
                lt_PolicyType.Text = Convert.ToString(GetLocalResourceObject("lbl_MTA_Permanent"))
                litPolicyNo.Text = Convert.ToString(GetLocalResourceObject("litPolicyNo"))
                lt_PolicyNo.Text = oQuote.InsuranceFileRef
            ElseIf Session(CNMTAType) = MTAType.TEMPORARY Then
                lt_PolicyType.Text = Convert.ToString(GetLocalResourceObject("lbl_MTA_Temporary"))
                litPolicyNo.Text = Convert.ToString(GetLocalResourceObject("litPolicyNo"))
                lt_PolicyNo.Text = oQuote.InsuranceFileRef
            ElseIf Session(CNMTAType) = MTAType.CANCELLATION Then
                lt_PolicyType.Text = Convert.ToString(GetLocalResourceObject("lbl_MTA_Cancellation"))
                litPolicyNo.Text = Convert.ToString(GetLocalResourceObject("litPolicyNo"))
                lt_PolicyNo.Text = oQuote.InsuranceFileRef
            ElseIf Session(CNMTAType) = MTAType.REINSTATEMENT Then
                lt_PolicyType.Text = Convert.ToString(GetLocalResourceObject("lbl_MTA_Reinstatement"))
                litPolicyNo.Text = Convert.ToString(GetLocalResourceObject("litPolicyNo"))
                lt_PolicyNo.Text = oQuote.InsuranceFileRef
            ElseIf Session(CNRenewal) IsNot Nothing Then
                lt_PolicyType.Text = Convert.ToString(GetLocalResourceObject("lbl_Renewal"))
                litPolicyNo.Text = Convert.ToString(GetLocalResourceObject("litPolicyNo"))
                lt_PolicyNo.Text = oQuote.InsuranceFileRef
            End If

            If oParty IsNot Nothing Then
                Select Case True
                    Case TypeOf oParty Is NexusProvider.CorporateParty
                        With CType(oParty, NexusProvider.CorporateParty)
                            litClientName.Text = Convert.ToString(GetLocalResourceObject("litCompanyName"))
                            lt_ClientName.Text = CType(oParty, NexusProvider.CorporateParty).CompanyName
                        End With
                    Case TypeOf oParty Is NexusProvider.PersonalParty
                        With CType(oParty, NexusProvider.PersonalParty)
                            litClientName.Text = Convert.ToString(GetLocalResourceObject("litClientName"))
                            lt_ClientName.Text = .Title & " " & .Forename & " " & .Lastname
                        End With
                End Select
            End If
        Else
            Me.Visible = False
        End If
    End Sub
End Class
