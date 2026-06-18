Imports Nexus.Library
Imports CMS.Library
Imports System.Data
Imports System.Web.Configuration.WebConfigurationManager
Imports Nexus.Utils
Imports Nexus.Constants.Constant
Imports Nexus.Constants.Session
Imports NexusProvider
Imports System.Linq
Namespace Nexus

    Partial Class Controls_CreditCardDetails : Inherits System.Web.UI.UserControl

        Dim InstalmentQuotesCacheID As Guid
        Dim PartyBankdetailsCacheID As Guid
        Dim SelectedAccountTypeCacheId As Guid
        Dim SelectedInstalmentQuoteCacheId As Guid
        Dim oFinancePlanDetails As New NexusProvider.FinancePlanDetails

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            If Not IsPostBack Then
                Dim oFinancePlanDetails As New NexusProvider.FinancePlanDetails
                Dim oCCHistoryDetails As New NexusProvider.FinancePlanBankHistoryCollection
                If Session("FinancePlanDetails") IsNot Nothing Then
                    oFinancePlanDetails = CType(Session("FinancePlanDetails"), NexusProvider.PremiumFinancePlan).PremiumFinanceDetails
                    oCCHistoryDetails = CType(Session("FinancePlanDetails"), NexusProvider.PremiumFinancePlan).PFBankHistory

                    If oCCHistoryDetails IsNot Nothing AndAlso oCCHistoryDetails.Count > 0 Then
                        FillGrid(oCCHistoryDetails)
                    End If

                    FillFormFields(oFinancePlanDetails)
                    PopulateTokenNumber()
                    EnableRequiredFields(False)
                End If
            End If
        End Sub
        ''' <summary>
        ''' This method is used to fill the History of CC
        ''' </summary>
        ''' <param name="oCCHistoryDetails"></param>
        ''' <remarks></remarks>
        Protected Sub FillGrid(ByVal oCCHistoryDetails As FinancePlanBankHistoryCollection)
            Dim oCreditCardHistory = From v In oCCHistoryDetails Where Trim(v.MediatypeValidationCode) = "CC"
            grdCCHistory.DataSource = oCCHistoryDetails
            grdCCHistory.DataBind()
        End Sub

        
        ''' <summary>
        '''     This is used to Fill the credit card details
        ''' </summary>
        ''' <param name="oFinancePlanDetails"></param>
        ''' <remarks></remarks>
        Protected Sub FillFormFields(ByVal oFinancePlanDetails As FinancePlanDetails)
            If oFinancePlanDetails IsNot Nothing Then
                txtTokenNumber.Text = oFinancePlanDetails.DepositCCTrackingNumber
                chkCCCancelled.Checked = oFinancePlanDetails.IsCCCancelled
                If oFinancePlanDetails.IsCCCancelled Then
                    chkCCCancelled.Enabled = False
                Else
                    chkCCCancelled.Enabled = True
                End If
            End If
        End Sub

        ''' <summary>
        '''     This function is called from the parent page whenever release button is called to release the CC details.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub ReleasePlanCall()
            chkCCCancelled.Checked = False
            txtTokenNumber.Text = ""
            txtTokenNumber.Enabled = True
            ddlExistingTokens.Visible = True
            EnableRequiredFields(True)
        End Sub

        
        ''' <summary>
        '''     This Method is used to prepare the Save request as per supplied value and Get called from its Parent page.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub CreditCardDetails_Save()
            Dim oProcessPFPlan As New PremiumFinancePlan
            Dim oPayment As Payment = Nothing
            If Page.IsValid Then
                If Session(CNFinancePlanDetails) IsNot Nothing Then
                    oProcessPFPlan = CType(Session(CNFinancePlanDetails), PremiumFinancePlan)
                End If
                oProcessPFPlan.PFCreditCardDetails = New CreditCard
                oProcessPFPlan.PFCreditCardDetails = _
                    CType(Session(CNFinancePlanDetails), PremiumFinancePlan).PFCreditCardDetails

                If ddlExistingTokens.SelectedIndex > 0 Then
                    oProcessPFPlan.PFCreditCardDetails.PartyBankKey = CInt(ddlExistingTokens.SelectedValue)
                    oProcessPFPlan.PFCreditCardDetails.AuthCode = ddlExistingTokens.SelectedItem.ToString().Trim()
                    oProcessPFPlan.PremiumFinanceDetails.PartyBankKey = CInt(ddlExistingTokens.SelectedValue)
                Else
                    oProcessPFPlan.PFCreditCardDetails.PartyBankKey = Nothing
                    oProcessPFPlan.PFCreditCardDetails.AuthCode = txtTokenNumber.Text
                    ''***********Set PartyBankKey = 0 only when ddlExistingTokens.SelectedIndex <= 0 and at the time of release CC----
                    If ddlExistingTokens.Visible = True AndAlso ddlExistingTokens.SelectedIndex = 0 AndAlso txtTokenNumber.Text.Trim().Length > 0 Then
                        oProcessPFPlan.PremiumFinanceDetails.PartyBankKey = 0
                    End If
                End If
                If ViewState("PaymentHubEnabled") IsNot Nothing AndAlso ViewState("PaymentHubEnabled") = "1" AndAlso Session(CNPayment) IsNot Nothing Then
                    oPayment = Session(CNPayment)
                    If oPayment.CreditCard IsNot Nothing Then
                        oProcessPFPlan.PFCreditCardDetails.PartyBankKey = oPayment.CreditCard.PartyBankKey
                        oProcessPFPlan.PFCreditCardDetails.AuthCode = oPayment.CreditCard.AuthCode
                        oProcessPFPlan.PFCreditCardDetails.Number = oPayment.CreditCard.Number
                        oProcessPFPlan.PFCreditCardDetails.ExpiryDate = oPayment.CreditCard.ExpiryDate
                        oProcessPFPlan.PFCreditCardDetails.NameOnCreditCard = oPayment.CreditCard.NameOnCreditCard
                        oProcessPFPlan.PFCreditCardDetails.TrackingNumber = oPayment.CreditCard.TrackingNumber
                        oProcessPFPlan.PFCreditCardDetails.VIAPaymentHub = True
                        oProcessPFPlan.PremiumFinanceDetails.PartyBankKey = oPayment.CreditCard.PartyBankKey
                    End If
                End If
                'Sending multiple objects because of redundant requests of same type
                With oProcessPFPlan.PremiumFinanceDetails
                    If chkCCCancelled.Checked Then
                        .StatusInd = NexusProvider.FinancePlanStatus.Item140
                        .Authcode = Trim(txtTokenNumber.Text)
                        .IsCCCancelled = chkCCCancelled.Checked
                    End If
                    Session(CNFinancePlanDetails) = oProcessPFPlan
                End With
            End If
        End Sub

        Protected Sub cvTokenNumber_ServerValidate(ByVal source As Object, ByVal args As ServerValidateEventArgs) _
            Handles cvTokenNumber.ServerValidate
            Dim oFinancePlanDetails As New FinancePlanDetails
            If Session("FinancePlanDetails") IsNot Nothing Then
                oFinancePlanDetails = CType(Session("FinancePlanDetails"), PremiumFinancePlan).PremiumFinanceDetails
            End If
            If oFinancePlanDetails.IsCCCancelled Then
                If ddlExistingTokens.SelectedIndex > 0 Then
                    args.IsValid = True
                Else
                    If String.IsNullOrEmpty(txtTokenNumber.Text) Then
                        args.IsValid = False
                    Else
                        args.IsValid = True
                    End If
                End If
            End If
        End Sub
        ''' <summary>
        '''     This Routine is used to populate the Token dropdown.
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub PopulateTokenNumber()
            Dim oParty As BaseParty = Session(CNParty)
            Dim oPartyBankDetails As BankCollection
            Dim oPartyBankDetail As Bank
            Dim oWebService As ProviderBase = New ProviderManager().Provider
            Dim sTokenNumber As String = String.Empty
            Dim sPartyBankKey As String = String.Empty

            ddlExistingTokens.Items.Clear()

            'the first index should be none item.
            ddlExistingTokens.Items.Add(GetLocalResourceObject("lblNoneToken"))
            
            If oParty IsNot Nothing Then
                'Base on the session value is personal / corporate client is loaded
                Select Case True
                    Case TypeOf Session(CNParty) Is PersonalParty
                        oParty = CType(Session(CNParty), PersonalParty)
                    Case TypeOf Session(CNParty) Is CorporateParty
                        oParty = CType(Session(CNParty), CorporateParty)
                End Select
                'Populate Party bank Details
                oPartyBankDetails = oWebService.GetPartyBankDetails(oParty.Key)

                If oPartyBankDetails IsNot Nothing Then
                    For Each oPartyBankDetail In oPartyBankDetails
                        If oPartyBankDetail.CreditCard IsNot Nothing Then
                            sTokenNumber = oPartyBankDetail.CreditCard.ManualAuthCode
                            sPartyBankKey = oPartyBankDetail.PartyBankKey.ToString()
                            If Not String.IsNullOrEmpty(sTokenNumber) Then
                                Dim lstTokenNumber As New ListItem(sTokenNumber, sPartyBankKey)
                                ddlExistingTokens.Items.Add(lstTokenNumber)
                            End If
                        End If
                    Next

                End If
            End If
        End Sub

        
        ''' <summary>
        '''     This method is responsible to enable and disable the Token textbox or related display control over the page.
        ''' </summary>
        ''' <param name="bEnableFlag"></param>
        ''' <remarks></remarks>
        Private Sub EnableRequiredFields(ByVal bEnableFlag As Boolean)
            ddlExistingTokens.Enabled = bEnableFlag
            ddlExistingTokens.Visible = bEnableFlag
            lblExistingTokens.Visible = bEnableFlag
            txtTokenNumber.ReadOnly = Not bEnableFlag
        End Sub

    End Class
End Namespace