Imports System.Data
Imports CMS.Library
Imports System.Web.Configuration.WebConfigurationManager
Imports Nexus.Library
Imports Nexus.Utils
Imports System.Exception
Imports Nexus.Constants.Constant
Imports Nexus.Constants.Session

Namespace Nexus

    Partial Class secure_payment_PaymentSelect : Inherits Frontend.clsCMSPage

        ' declaring local variables for getting UserAuthority
        Dim bUserInvoice As Boolean = False
        Dim bUserPayNow As Boolean = False
        Dim bUserBankGuarantee As Boolean = False
        Dim bUserCashDeposit As Boolean = False
        Dim bUserDirectDebit As Boolean = False

        ' declaring local variables for getting Product Payment options
        Dim bProductInvoice As Boolean = False
        Dim bProductPayNow As Boolean = False
        Dim bProductBankGuarantee As Boolean = False
        Dim bProductCashDeposit As Boolean = False
        Dim bProductDirectDebit As Boolean = False

        ' declaring local variables for getting Agent Payment options
        Dim bAgentInvoice As Boolean = False
        Dim bAgentPayNow As Boolean = False
        Dim bAgentBankGuarantee As Boolean = False
        Dim bAgentCashDeposit As Boolean = False
        Dim bAgentDirectDebit As Boolean = False

        Dim sProductCode As String = Nothing

        Dim oQuote As NexusProvider.Quote

        Protected Shadows Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            If Not Page.IsPostBack Then
                hfAgentType.Value = ""
                hfPrePayment.Value = ""
                hfTransType.Value = ""
                Dim oPortalConfig As Config.Portal = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID())
                Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                oQuote = Session(CNQuote)
                If oQuote.BusinessTypeCode <> "DIRECT" Then
                    Dim breturncode As String
                    breturncode = oWebService.GetOptionSetting(NexusProvider.OptionType.ProductOption, 87).OptionValue
                    If breturncode <> "1" Then
                        hfPrePayment.Value = "0"
                        Dim oTempParty As NexusProvider.PartyCollection
                        Dim oTempSearchCriteria As New NexusProvider.PartySearchCriteria
                        oTempSearchCriteria.AgentType = Nothing
                        oTempSearchCriteria.ShortName = CType(Session(CNQuote), NexusProvider.Quote).AgentCode
                        oTempSearchCriteria.PartyType = CType(Session(CNAgentDetails), NexusProvider.UserDetails).PartyType
                        oTempSearchCriteria.PartyTypes.Add(NexusProvider.PartyTypeType.AG)

                        oTempParty = oWebService.FindParty(oTempSearchCriteria)
                        If oTempParty IsNot Nothing Then
                            If oTempParty.Count > 0 Then
                                If oTempParty(0).AgentType.Trim().ToString().ToUpper() = "INTERMEDIARY" Then
                                    hfAgentType.Value = "INTERMEDIARY"
                                Else
                                    hfAgentType.Value = oTempParty(0).AgentType.Trim().ToString().ToUpper()
                                End If
                            End If
                        End If
                    Else
                        hfPrePayment.Value = "1"
                    End If

                End If
                SetPaymentAccessPermissions()
                If Session(CNMTAType) Is Nothing And Session(CNRenewal) Is Nothing Then
                    hfTransType.Value = "NB"
                End If
                Dim bStatementsAgreed As Boolean
                'If oPortalConfig Is Nothing Then
                '    'ShowStatements = true, is the default
                '    If Session(CNStatementsAgreed) Is Nothing Then
                '        'Statements page has not been visited
                '        bStatementsAgreed = False
                '    Else
                '        'use session value from statements page
                '        bStatementsAgreed = CType(Session(CNStatementsAgreed), Boolean)
                '    End If

                'Else
                '    If oPortalConfig.ShowStatements And Request.QueryString("quotecollection") <> "true" Then
                '        'use session value from statements page
                '        bStatementsAgreed = CType(Session(CNStatementsAgreed), Boolean)
                '    Else
                '        'Statements aren't required, or else we're in quote collection so set 
                '        'statements agreed to true to prevent redirecting to the statements page
                '        bStatementsAgreed = True
                '    End If
                'End If
                Boolean.TryParse(Session(CNStatementsAgreed), bStatementsAgreed)
                If bStatementsAgreed Then
                    SetPageProgress(6)
                    Dim oPaymentOptions As Config.PaymentTypes = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID()).PaymentTypes
                    Dim sRadioButtonLabel As String
                    Dim dPremiumGross As Decimal
                    Dim dPremiumNet As Decimal
                    Dim dPremiumIPT As Decimal
                    Dim dInstallmentAmount As Decimal
                    'Dim oQuote As NexusProvider.Quote = Session.Item(CNQuote)
                    Dim i As Int32 = 0
                    Dim bAvailable As Boolean = False 'for RequiredPaymentMethodForMTA option

                    Dim dt As New DataTable
                    Dim dr As DataRow
                    dt.Columns.Add(New DataColumn("Column1"))
                    dt.Columns.Add(New DataColumn("Column2"))
                    dt.Columns.Add(New DataColumn("Column3"))

                    For Each oPaymentType As Config.PaymentType In oPaymentOptions

                        'To Check the Availability of RequiredPaymentMethodForMTA option with Payment Method
                        If Not oPaymentType.RequiredPaymentMethodForMTA Is Nothing Then
                            If String.IsNullOrEmpty(oPaymentType.RequiredPaymentMethodForMTA.Trim()) = False Then
                                bAvailable = True
                                Exit For
                            End If
                        End If

                    Next

                    For Each oPaymentType As Config.PaymentType In oPaymentOptions
                        If Session(CNLoginType) = LoginType.Customer And oPaymentType.Type.ToString() <> PaymentTypes.PayNow.ToString() _
                        And oPaymentType.Type.ToString() <> PaymentTypes.BankGuarantee.ToString() And _
                        oPaymentType.Type.ToString() <> PaymentTypes.Invoice.ToString() And _
                        oPaymentType.Type.ToString() <> PaymentTypes.DirectDebit.ToString() Then
                            If Not Session(CNMTAType) Is Nothing Then
                                'if user is doing MTA
                                sRadioButtonLabel = oPaymentType.MTADisplayName
                            Else
                                'if user is doing NB
                                sRadioButtonLabel = oPaymentType.DisplayName
                            End If
                            'Calculate Premium
                            If oQuote IsNot Nothing And Request.QueryString("quotecollection") <> "true" Then
                                If oQuote.Risks.Count > 0 Then
                                    dPremiumGross = CheckAndCalculateRoundOff()
                                    dPremiumNet = oQuote.NetTotal
                                    dPremiumIPT = oQuote.TaxTotal + oQuote.FeeTotal
                                End If
                            ElseIf Request.QueryString("quotecollection") = "true" Then
                                'quote collection, so set the total to pay from session
                                dPremiumGross = CType(Session(CNTotalForQuoteCollection), Decimal)
                            End If

                            If oPaymentType.FeePercent > 0 Then
                                dPremiumGross += dPremiumGross * (oPaymentType.FeePercent / 100)
                            End If

                            sRadioButtonLabel = sRadioButtonLabel.Replace("[!FeePercent!]", oPaymentType.FeePercent)
                            sRadioButtonLabel = sRadioButtonLabel.Replace("[!NetPremium!]", New Money(dPremiumNet, Session(CNCurrenyCode)).Formatted)
                            sRadioButtonLabel = sRadioButtonLabel.Replace("[!PremiumIPT!]", New Money(dPremiumIPT, Session(CNCurrenyCode)).Formatted)
                            sRadioButtonLabel = sRadioButtonLabel.Replace("[!Premium!]", New Money(dPremiumGross, Session(CNCurrenyCode)).Formatted)
                            'adding row to data table
                            dr = dt.NewRow
                            If Session.Item("Quote_Mode") = QuoteMode.MTAQuote Then 'MTA
                                If Not oPaymentType.RequiredPaymentMethodForMTA Is Nothing Then
                                    If String.IsNullOrEmpty(oPaymentType.RequiredPaymentMethodForMTA.Trim()) = False Then
                                        If oQuote.PaymentMethodCode.Trim().ToUpper() = oPaymentType.RequiredPaymentMethodForMTA.Trim().ToUpper() And oQuote.PaymentMethodCode.Trim().ToUpper() = oPaymentType.Name.Trim().ToUpper() Then
                                            'if RequiredPaymentMethodForMTA is set and matches with original policy payment method then  
                                            'show the matched payment option
                                            dr(0) = sRadioButtonLabel
                                            dr(1) = Trim(oPaymentType.Name & "#" & Convert.ToString(dPremiumGross))
                                            dr(2) = "PayOption" & dt.Rows.Count
                                            dt.Rows.Add(dr)
                                        End If
                                    End If
                                ElseIf bAvailable = False Then
                                    'if RequiredPaymentMethodForMTA is not set then show all payment option
                                    dr(0) = sRadioButtonLabel
                                    dr(1) = Trim(oPaymentType.Name & "#" & Convert.ToString(dPremiumGross))
                                    dr(2) = "PayOption" & dt.Rows.Count
                                    dt.Rows.Add(dr)
                                End If
                            Else 'New Bussiness, need to show all the Payment Options from conig
                                dr(0) = sRadioButtonLabel
                                dr(1) = Trim(oPaymentType.Name & "#" & Convert.ToString(dPremiumGross))
                                dr(2) = "PayOption" & dt.Rows.Count
                                dt.Rows.Add(dr)
                            End If
                            i = i + 1

                        ElseIf Session(CNLoginType) <> LoginType.Customer Then

                            If GetPaymentAccess(oPaymentType) Then

                                'if SkipPaymentSelect is set to true then redirect to the first payment option (there should only be one option configured anyway)
                                If CType(GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID()).SkipPaymentSelect = True Then
                                    Session.Add(CNSelectedPaymentIndex, 0)
                                    Response.Redirect(oPaymentType.Url, False)
                                    'else will redirect to transaction confirmation page directly
                                End If

                                If Not Session(CNMTAType) Is Nothing And oPaymentType.Type.ToString.Trim.ToUpper = PaymentTypes.DirectDebit.ToString Then
                                    sRadioButtonLabel = GetLocalResourceObject("lbl_displaymsg")
                                Else
                                    If Not Session(CNMTAType) Is Nothing Then
                                        'if user is doing MTA
                                        sRadioButtonLabel = oPaymentType.MTADisplayName
                                    Else
                                        'if user is doing NB
                                        sRadioButtonLabel = oPaymentType.DisplayName
                                    End If
                                End If

                                'Calculate Premium
                                If oQuote IsNot Nothing And Request.QueryString("quotecollection") <> "true" Then
                                    If oQuote.Risks.Count > 0 Then
                                        dPremiumGross = CheckAndCalculateRoundOff()
                                        dPremiumNet = oQuote.NetTotal
                                        dPremiumIPT = oQuote.TaxTotal + oQuote.FeeTotal
                                    End If
                                ElseIf Request.QueryString("quotecollection") = "true" Then
                                    'quote collection, so set the total to pay from session
                                    dPremiumGross = CType(Session(CNTotalForQuoteCollection), Decimal)
                                End If

                                If oPaymentType.FeePercent > 0 Then
                                    dPremiumGross += dPremiumGross * (oPaymentType.FeePercent / 100)
                                End If

                                sRadioButtonLabel = sRadioButtonLabel.Replace("[!FeePercent!]", oPaymentType.FeePercent)
                                sRadioButtonLabel = sRadioButtonLabel.Replace("[!NoOfInstallment!]", oPaymentType.NoOfInstalments)

                                If oPaymentType.NoOfInstalments > 0 Then
                                    dInstallmentAmount = dPremiumGross / oPaymentType.NoOfInstalments
                                    sRadioButtonLabel = sRadioButtonLabel.Replace("[!InstallmentAmount!]", New Money(dInstallmentAmount, Session(CNCurrenyCode)).Formatted)
                                End If

                                sRadioButtonLabel = sRadioButtonLabel.Replace("[!NetPremium!]", New Money(dPremiumNet, Session(CNCurrenyCode)).Formatted)
                                sRadioButtonLabel = sRadioButtonLabel.Replace("[!PremiumIPT!]", New Money(dPremiumIPT, Session(CNCurrenyCode)).Formatted)
                                sRadioButtonLabel = sRadioButtonLabel.Replace("[!Premium!]", New Money(dPremiumGross, Session(CNCurrenyCode)).Formatted)

                                'adding row to data table
                                dr = dt.NewRow
                                If Session.Item("Quote_Mode") = QuoteMode.MTAQuote Then 'MTA
                                    If Not oPaymentType.RequiredPaymentMethodForMTA Is Nothing Then
                                        If String.IsNullOrEmpty(oPaymentType.RequiredPaymentMethodForMTA.Trim()) = False Then
                                            If oQuote.PaymentMethodCode.Trim().ToUpper() = oPaymentType.RequiredPaymentMethodForMTA.Trim().ToUpper() And oQuote.PaymentMethodCode.Trim().ToUpper() = oPaymentType.Name.Trim().ToUpper() Then
                                                'if RequiredPaymentMethodForMTA is set and matches with original policy payment method then  
                                                'show the matched payment option
                                                dr(0) = sRadioButtonLabel
                                                dr(1) = Trim(oPaymentType.Name & "#" & Convert.ToString(dPremiumGross))
                                                dr(2) = "PayOption" & dt.Rows.Count
                                                dt.Rows.Add(dr)
                                            End If
                                        End If
                                    ElseIf bAvailable = False Then
                                        'if RequiredPaymentMethodForMTA is not set then show all payment option
                                        dr(0) = sRadioButtonLabel
                                        dr(1) = Trim(oPaymentType.Name & "#" & Convert.ToString(dPremiumGross))
                                        dr(2) = "PayOption" & dt.Rows.Count
                                        dt.Rows.Add(dr)
                                    End If
                                ElseIf Session(CNMTAType) = MTAType.CANCELLATION Then
                                    If oPaymentType.Name = "PayNow" Then
                                        dr(0) = sRadioButtonLabel
                                        dr(1) = Trim(oPaymentType.Name & "-" & Convert.ToString(dPremiumGross))
                                        dr(2) = "PayOption" & dt.Rows.Count
                                        dt.Rows.Add(dr)
                                        dr = dt.NewRow
                                        dr(0) = "<ul><li> Invoice </li></ul>"
                                        dr(1) = Trim("Invoice" & "-" & Convert.ToString(dPremiumGross))
                                        dr(2) = "PayOption" & dt.Rows.Count
                                        dt.Rows.Add(dr)
                                    End If
                                Else 'New Bussiness, need to show all the Payment Options from conig
                                    dr(0) = sRadioButtonLabel
                                    dr(1) = Trim(oPaymentType.Name & "#" & Convert.ToString(dPremiumGross))
                                    dr(2) = "PayOption" & dt.Rows.Count
                                    dt.Rows.Add(dr)
                                End If
                                i = i + 1
                            End If
                        End If
                    Next

                    rdoPaymentOptionsCount.Text = dt.Rows.Count
                    GridPaymentOptions.DataSource = dt
                    GridPaymentOptions.DataBind()

                    'oQuote = Nothing
                Else
                    'statements haven't been agreed so go to statements page before allowing payment.
                    Response.Redirect("~/secure/Statements.aspx", False)
                End If
            End If
        End Sub

        Protected Sub btnBuy_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBuy.Click
            If Page.IsValid Then
                If hdselectaccount.Value = "Client" Or hdselectaccount.Value = "Agent" Then
                    Session(CNSelectedAccount) = hdselectaccount.Value
                Else
                    Session(CNSelectedAccount) = ""
                End If
                Dim SelectedPaymentOption As String = Request("rdoPaymentOptions")
                Dim SelectedPaymentIndex As String = Mid(Trim(SelectedPaymentOption), 1, SelectedPaymentOption.IndexOf("-"))
                Dim SelectedPaymentValue As Decimal = Mid(Trim(SelectedPaymentOption), (SelectedPaymentOption.IndexOf("-") + 2))
                Dim charges As Double
                Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                'SET THE VALUE FOR PAID TO "FALSE". WILL GET "TRUE" AFTER SUCCESSFUL MONEY TRANSACTION
                Session(CNPaid) = False

                'CHECK FOR SELECTED VALUE AND REDIRECT THE PAGE ACCORDINGLY.
                Dim oPaymentOptions As Config.PaymentTypes = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID()).PaymentTypes

                '6389 - MTA Refund Process on Instalments
                Dim oPaymentType As Config.PaymentType = New Config.PaymentType
                If SelectedPaymentIndex.Trim.ToUpper.Contains("DIRECT DEBIT") AndAlso SelectedPaymentOption.Trim.ToUpper.Contains("--") And Session(CNMTAType) IsNot Nothing Then
                    SelectedPaymentIndex = Mid(Trim(SelectedPaymentOption), 1, SelectedPaymentOption.IndexOf("-"))
                    oPaymentType = oPaymentOptions.PaymentType(SelectedPaymentIndex)
                    SelectedPaymentValue = Mid(Trim(SelectedPaymentOption), (SelectedPaymentOption.LastIndexOf("-") + 1))
                Else
                    oPaymentType = oPaymentOptions.PaymentType(SelectedPaymentIndex)

                End If

                If oPaymentType IsNot Nothing Then
                    'strip those unwanted characters which we have added in page_load event
                    'PN41169
                    If oPaymentType.FeePercent > 0 Then
                        For iRiskCount As Integer = 0 To oQuote.Risks.Count - 1
                            charges += CType(Session(CNQuote), NexusProvider.Quote).Risks(iRiskCount).PremiumDueGross * (oPaymentType.FeePercent / 100)
                        Next
                    End If
                    Session.Add(CNChargetoPay, charges)

                    'Check if Agent is Broker then Agent Commission should be deducted from Total AMount
                    'If Session(CNLoginType) = LoginType.Agent Then
                    '    Dim bFound As Boolean = False

                    '    If Session(CNAgentType) IsNot Nothing And Session(CNAgentComm) IsNot Nothing Then
                    '        If Session(CNAgentType).ToString.Trim.ToUpper = "BROKER" Then
                    '            Dim dPremium As Decimal = SelectedPaymentValue
                    '            Dim dAgentComm As Decimal = Session(CNAgentComm)
                    '            dPremium = dPremium - dAgentComm
                    '            Session.Add(CNAmountToPay, dPremium)
                    '            bFound = True
                    '        End If
                    '    ElseIf Session(CNQuote) IsNot Nothing Then
                    '        'Find The AgentType through SAM Call
                    '        Dim oTempParty As NexusProvider.PartyCollection
                    '        Dim oTempSearchCriteria As New NexusProvider.PartySearchCriteria

                    '        oTempSearchCriteria.AgentType = Nothing
                    '        oTempSearchCriteria.ShortName = CType(Session(CNQuote), NexusProvider.Quote).AgentCode
                    '        oTempSearchCriteria.PartyType = CType(Session(CNAgentDetails), NexusProvider.UserDetails).PartyType
                    '        oTempSearchCriteria.PartyTypes.Add(NexusProvider.PartyTypeType.AG)

                    '        oTempParty = oWebService.FindParty(oTempSearchCriteria)

                    '        If oTempParty IsNot Nothing Then
                    '            If oTempParty.Count > 0 Then
                    '                Session(CNAgentType) = oTempParty(0).AgentType
                    '                'Check if Agent is Broker then Agent Commission should be deducted from Total AMount
                    '                If Session(CNAgentType).ToString.Trim.ToUpper = "BROKER" Then
                    '                    Dim dPremium As Decimal = SelectedPaymentValue
                    '                    Dim dAgentComm As Decimal = Session(CNAgentComm)
                    '                    dPremium = dPremium - dAgentComm
                    '                    Session.Add(CNAmountToPay, dPremium)
                    '                    bFound = True
                    '                End If
                    '            End If
                    '        End If
                    '    End If
                    '    'if bFound is False it means that Agnet is Not Broker so that Full AMount will move further
                    '    If bFound = False Then
                    '        Session.Add(CNAmountToPay, SelectedPaymentValue)
                    '    End If
                    'End If
                    'End
                    UpdatePremiumWithAgentCommision()
                    Dim i As Integer
                    For i = 0 To oPaymentOptions.Count - 1 Step 1
                        If oPaymentOptions.PaymentType(i).Type = SelectedPaymentIndex Then
                            Session("SelectedItem") = i
                        End If
                    Next
                    Session.Add(CNSelectedPaymentIndex, SelectedPaymentIndex)

                    Dim sIsPrepaymentOptionEnabled As String
                    'Dim oQuote As NexusProvider.Quote
                    oQuote = Session(CNQuote)
                    sIsPrepaymentOptionEnabled = oWebService.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance, NexusProvider.ProductRiskOptions.IsPrepaymentOptionEnabled, NexusProvider.RiskTypeOptions.None, oQuote.ProductCode, Nothing)
                    'code for Marked Collection and redirection

                    'code for Marked Collection and redirection
                    If (sIsPrepaymentOptionEnabled = "1" And Session(CNTotalForQuoteCollection) IsNot Nothing) And oPaymentType.Name.Trim.ToUpper <> "PAYNOW" And oPaymentType.Name.ToUpper <> "BANKGUARANTEE" _
                    And oPaymentType.Name.Trim.ToUpper <> "DIRECT DEBIT" And oPaymentType.Name.Trim.ToUpper <> "CREDIT CARD" Then
                        Response.Redirect("PrePayment.aspx?quotecollection=true", False)
                    ElseIf Session(CNTotalForQuoteCollection) IsNot Nothing Then
                        Session.Remove(CNCashListItem) 'Loads Cash List screen when PayNow option selection
                        Response.Redirect(oPaymentType.Url & "?quotecollection=true", False)
                    ElseIf sIsPrepaymentOptionEnabled = "1" And oPaymentType.Name.Trim.ToUpper <> "PAYNOW" And oPaymentType.Name.ToUpper <> "BANKGUARANTEE" And oPaymentType.Name.ToUpper <> "CASHDEPOSIT" _
                    And oPaymentType.Name.Trim.ToUpper <> "DIRECT DEBIT" And oPaymentType.Name.Trim.ToUpper <> "CREDIT CARD" Then
                        Response.Redirect("PrePayment.aspx", False)
                    ElseIf Session(CNMTAType) = MTAType.CANCELLATION Then
                        If oPaymentType.Name.Trim.ToUpper = "PAYNOW" Then
                            Session.Remove(CNCashListItem) 'Loads Cash List screen when PayNow option selection
                            Session.Remove(CNQuoteMode)
                            Response.Redirect(oPaymentType.Url, False)
                        Else
                            Response.Redirect("~/secure/TransactionConfirmation.aspx", False)
                        End If
                    Else
                        Session.Remove(CNCashListItem) 'Loads Cash List screen when PayNow option selection
                        Response.Redirect(oPaymentType.Url, False)
                    End If
                Else
                    Response.Redirect("~/secure/TransactionConfirmation.aspx", False)
                End If
            End If
        End Sub

        Protected Sub CustVldPaymentOptionRequired_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles CustVldPaymentOptionRequired.ServerValidate
            If Not Request("rdoPaymentOptions") Is Nothing Then
                args.IsValid = True
                If Request("__EVENTARGUMENT") = "GetAccount" Then
                    btnBuy_Click(Nothing, Nothing)
                End If
            Else
                args.IsValid = False
            End If
        End Sub

        ''' <summary>
        ''' Setting the payment options for Agent, Product and User 
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub SetPaymentAccessPermissions()

            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oUserAuthority As New NexusProvider.UserAuthority
            Dim oAgentSetting As NexusProvider.AgentSettings
            Dim iAgentKey As Integer

            Dim oPaymentOptions As Config.PaymentTypes = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID()).PaymentTypes

            If oPaymentOptions.PaymentType(PaymentTypes.Invoice) IsNot Nothing Then
                ' Obtaining and setting authority value for User AgentCollection/Invoice
                oUserAuthority.UserAuthorityOption = NexusProvider.UserAuthority.UserAuthorityOptionType.CanMakeLiveInvoice
                oUserAuthority.UserCode = Session(CNLoginName) ' TO DO need to be made dynamic
                oWebService.GetUserAuthorityValue(oUserAuthority)
                If String.IsNullOrEmpty(oUserAuthority.UserAuthorityValue) = False AndAlso oUserAuthority.UserAuthorityValue.Trim = "1" Then
                    bUserInvoice = True
                End If
            End If

            If oPaymentOptions.PaymentType(PaymentTypes.PayNow) IsNot Nothing Then
                ' Obtaining and setting authority value for User PayNow/Direct Debit   
                oUserAuthority.UserAuthorityOption = Nothing
                oUserAuthority.UserAuthorityOption = NexusProvider.UserAuthority.UserAuthorityOptionType.CanMakeLivePayNow
                oUserAuthority.UserCode = Session(CNLoginName) ' TO DO need to be made dynamic
                oWebService.GetUserAuthorityValue(oUserAuthority)
                If String.IsNullOrEmpty(oUserAuthority.UserAuthorityValue) = False AndAlso oUserAuthority.UserAuthorityValue.Trim = "1" Then
                    bUserPayNow = True
                End If
            End If

            If oPaymentOptions.PaymentType(PaymentTypes.BankGuarantee) IsNot Nothing Then
                ' Obtaining and setting authority value for User BankGuarantee
                oUserAuthority.UserAuthorityOption = Nothing
                oUserAuthority.UserAuthorityOption = NexusProvider.UserAuthority.UserAuthorityOptionType.CanMakeLiveBankGuarantee
                oUserAuthority.UserCode = Session(CNLoginName) ' TO DO need to be made dynamic
                oWebService.GetUserAuthorityValue(oUserAuthority)
                If String.IsNullOrEmpty(oUserAuthority.UserAuthorityValue) = False AndAlso oUserAuthority.UserAuthorityValue.Trim = "1" Then
                    bUserBankGuarantee = True
                End If
            End If

            If oPaymentOptions.PaymentType(PaymentTypes.CashDeposit) IsNot Nothing Then
                'Check Agent level permission for Payment Type-Cash Deposit 
                oUserAuthority.UserAuthorityOption = Nothing
                oUserAuthority.UserAuthorityOption = NexusProvider.UserAuthority.UserAuthorityOptionType.CanMakeLiveCashDeposit
                oUserAuthority.UserCode = Session(CNLoginName) ' TO DO need to be made dynamic
                oWebService.GetUserAuthorityValue(oUserAuthority)
                If String.IsNullOrEmpty(oUserAuthority.UserAuthorityValue) = False AndAlso oUserAuthority.UserAuthorityValue.Trim = "1" Then
                    bUserCashDeposit = True
                End If
            End If

            If oPaymentOptions.PaymentType(PaymentTypes.DirectDebit) IsNot Nothing Then
                'Check Agent level permission for Payment Type-Direct Debit 
                oUserAuthority.UserAuthorityOption = Nothing
                oUserAuthority.UserAuthorityOption = NexusProvider.UserAuthority.UserAuthorityOptionType.CanMakeLiveInstalments
                oUserAuthority.UserCode = Session(CNLoginName) ' TO DO need to be made dynamic
                oWebService.GetUserAuthorityValue(oUserAuthority)
                If String.IsNullOrEmpty(oUserAuthority.UserAuthorityValue) = False AndAlso oUserAuthority.UserAuthorityValue.Trim = "1" Then
                    bUserDirectDebit = True
                End If
            End If
            'Only do the following if we're not in quote collection. 
            'We could be dealing with multiple products during quote 
            'collection so cannot set product related options
            If Request.QueryString("quotecollection") <> "true" Then
                sProductCode = CType(Session(CNQuote), NexusProvider.Quote).ProductCode
                Dim oOptionTypeSetting As New NexusProvider.OptionTypeSetting
                Dim ProductRiskOptionValue As String

                If bUserInvoice Then
                    ' Obtaining and setting authority value for Product AgentCollection/Invoice
                    ProductRiskOptionValue = oWebService.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance, NexusProvider.ProductRiskOptions.CanMakeLiveInvoice, NexusProvider.RiskTypeOptions.None, sProductCode, "")
                    If String.IsNullOrEmpty(ProductRiskOptionValue) = False AndAlso ProductRiskOptionValue.Trim = "1" Then
                        bProductInvoice = True
                    End If
                End If

                If bUserPayNow Then
                    ' Obtaining and setting authority value for Product PayNow
                    ProductRiskOptionValue = Nothing
                    ProductRiskOptionValue = oWebService.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance, NexusProvider.ProductRiskOptions.CanMakeLivePaynow, NexusProvider.RiskTypeOptions.None, sProductCode, "")
                    If String.IsNullOrEmpty(ProductRiskOptionValue) = False AndAlso ProductRiskOptionValue.Trim = "1" Then
                        bProductPayNow = True
                    End If
                End If

                If bUserBankGuarantee Then
                    ' Obtaining and setting authority value for Product BankGuarantee
                    ProductRiskOptionValue = Nothing
                    ProductRiskOptionValue = oWebService.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance, NexusProvider.ProductRiskOptions.CanMakeBankGuarantee, NexusProvider.RiskTypeOptions.None, sProductCode, "")
                    If String.IsNullOrEmpty(ProductRiskOptionValue) = False AndAlso ProductRiskOptionValue.Trim = "1" Then
                        bProductBankGuarantee = True
                    End If
                End If

                If bUserCashDeposit Then
                    'Check Product Risk Option permission for Payment Type-Cash Deposit
                    ProductRiskOptionValue = Nothing
                    ProductRiskOptionValue = oWebService.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance, NexusProvider.ProductRiskOptions.CanMakeLiveCashDeposit, NexusProvider.RiskTypeOptions.None, sProductCode, "")
                    If String.IsNullOrEmpty(ProductRiskOptionValue) = False AndAlso ProductRiskOptionValue.Trim = "1" Then
                        bProductCashDeposit = True
                    End If
                End If

                If bUserDirectDebit Then
                    'Check Product Risk Option permission for Payment Type-Direct Debit
                    ProductRiskOptionValue = Nothing
                    ProductRiskOptionValue = oWebService.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance, NexusProvider.ProductRiskOptions.CanMakeLiveInstalments, NexusProvider.RiskTypeOptions.None, sProductCode, "")
                    If String.IsNullOrEmpty(ProductRiskOptionValue) = False AndAlso ProductRiskOptionValue.Trim = "1" Then
                        bProductDirectDebit = True
                    End If
                End If
            End If

            If oQuote IsNot Nothing Then
                If oQuote.Agent IsNot Nothing Then
                    Integer.TryParse(oQuote.Agent, iAgentKey)
                End If
            End If

            If iAgentKey > 0 Then
                'Call agent setting for current agent
                GetAgentSettingsCall(oAgentSetting, iAgentKey)
                If oAgentSetting IsNot Nothing Then
                    ' Setting and setting authority value for Agent AgentCollection/Invoice
                    bAgentInvoice = True
                    ' Setting and setting authority value for Agent PayNow
                    bAgentPayNow = True
                    ' Setting and setting authority value for Agent BankGuarantee
                    bAgentBankGuarantee = True
                    ' Setting and setting authority value for Agent CashDeposit
                    bAgentCashDeposit = True
                    ' Setting and setting authority value for Agent Direct Debit
                    bAgentDirectDebit = True
                End If
            End If


        End Sub

        Private Function GetPaymentAccess(ByVal oPaymentType As Config.PaymentType) As Boolean
            Dim bReturnvalue As Boolean = False

            If oPaymentType.Enabled = True Then
                Dim sPaymentType As String = oPaymentType.Type.ToString
                If oQuote IsNot Nothing And Request.QueryString("quotecollection") <> "true" Then
                    If (oQuote.BusinessTypeCode = "DIRECT") Then
                        'If (sPaymentType = PaymentTypes.AgentCollection.ToString()) andalso (oPaymentType.enabled=true And bUserAgentCollection And bProductAgentCollection Then
                        If (sPaymentType = PaymentTypes.Invoice.ToString()) And bUserInvoice And bProductInvoice Then
                            bReturnvalue = True
                        ElseIf (sPaymentType = PaymentTypes.PayNow.ToString()) And bUserPayNow And bProductPayNow Then
                            bReturnvalue = True
                        ElseIf (sPaymentType = PaymentTypes.BankGuarantee.ToString()) And bUserBankGuarantee And bProductBankGuarantee Then
                            bReturnvalue = True
                        ElseIf (sPaymentType = PaymentTypes.CreditCard.ToString()) Then
                            bReturnvalue = True
                        ElseIf (sPaymentType = PaymentTypes.CashDeposit.ToString()) And bUserCashDeposit And bProductCashDeposit Then
                            bReturnvalue = True
                        ElseIf (sPaymentType = PaymentTypes.DirectDebit.ToString()) And bUserDirectDebit And bProductDirectDebit Then
                            bReturnvalue = True
                        End If
                    Else
                        If (sPaymentType = PaymentTypes.Invoice.ToString()) And bUserInvoice And bProductInvoice And bAgentInvoice Then
                            bReturnvalue = True
                        ElseIf (sPaymentType = PaymentTypes.PayNow.ToString()) And bUserPayNow And bProductPayNow And bAgentPayNow Then
                            bReturnvalue = True
                        ElseIf (sPaymentType = PaymentTypes.BankGuarantee.ToString()) And bUserBankGuarantee And bProductBankGuarantee And bAgentBankGuarantee Then
                            bReturnvalue = True
                        ElseIf (sPaymentType = PaymentTypes.CreditCard.ToString()) Then
                            bReturnvalue = True
                        ElseIf (sPaymentType = PaymentTypes.CashDeposit.ToString()) And bUserCashDeposit And bProductCashDeposit And bAgentCashDeposit Then
                            bReturnvalue = True
                        ElseIf (sPaymentType = PaymentTypes.DirectDebit.ToString()) And bUserDirectDebit And bProductDirectDebit And bAgentDirectDebit Then
                            bReturnvalue = True
                        End If
                    End If
                Else
                    If Request.QueryString("quotecollection") = True Then
                        'we're in quote collection but this option is not enabled for quote collection 
                        'so show option according to whether or not it is enabled for quote collection
                        If oPaymentType.UseForQuoteCollection Then
                            bReturnvalue = True
                        Else
                            bReturnvalue = False
                        End If
                    End If
                End If
            End If
            Return bReturnvalue
        End Function
    End Class
End Namespace
