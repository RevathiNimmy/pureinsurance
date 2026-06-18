Imports CMS.library
Imports Nexus.Library
Imports System.Web.Configuration
Imports System.Web.Configuration.WebConfigurationManager
Imports Nexus.Constants.Constant
Imports Nexus.Constants.Session
Imports Nexus.Utils

Namespace Nexus

    Partial Class Statements : Inherits Frontend.clsCMSPage
        Const ClientMode As String = "CLIENT_MODE"
        Dim objBasePayment As Nexus.BasePayment
        Protected Shadows Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            Dim oPortalConfig As Config.Portal = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID())
            If Not oPortalConfig.ShowStatements Then
                ' IF ShowStatements ="false" then no need to show this page
                CheckPremiumAndRedirect()
            End If


            'To set the Focus
            Page.SetFocus(chkConfirmation)

            If Session(CNRenewal) = True Then
                'For Confirmation of Contact Details
                If oPortalConfig.ConfirmDetailsOnRenewal Then

                    pnlConfirmContactDetails.Visible = True

                    RetrieveClient()
                End If

                'For Editing the Contact Details
                If UserCanDoTask("EditClientDetails") Then

                    lnkEditDetails.Visible = True

                    'To set the session if user is directly going back from Browoser Back Button
                    Session(CNIsSummaryVisited) = True

                    Dim oParty As NexusProvider.BaseParty = Session(CNParty)

                    Select Case True
                        Case TypeOf oParty Is NexusProvider.PersonalParty
                            ViewState.Add(ClientMode, Mode.Edit)
                            Session(CNMode) = Mode.Edit
                            'For passing the mode of opening  Personal Client Details Page.
                            Session("IsThickBox") = True
                            lnkEditDetails.OnClientClick = "tb_show(null , '../Secure/Agent/PersonalClientDetails.aspx?mode=edit" _
                                    & "&partykey=" & oParty.Key.ToString() & "&Code=" & oParty.UserName & "&RequestType=REN" & "&modal=true&KeepThis=true&FromPage=PC&TB_iframe=true&height=500&width=750' , null);return false;"

                        Case TypeOf oParty Is NexusProvider.CorporateParty
                            ViewState.Add(ClientMode, Mode.Edit)
                            Session(CNMode) = Mode.Edit
                            'For passing the mode of opening  Personal Client Details Page.
                            Session("IsThickBox") = True
                            lnkEditDetails.OnClientClick = "tb_show(null , '../Secure/Agent/CorporateClientDetails.aspx?mode=edit" _
                                    & "&partykey=" & oParty.Key.ToString() & "&Code=" & oParty.UserName & "&RequestType=REN" & "&modal=true&KeepThis=true&FromPage=PC&TB_iframe=true&height=500&width=750' , null);return false;"


                    End Select

                End If


            End If

            SetPageProgress(5)
            ClientScript.RegisterClientScriptBlock(Me.GetType(), "EnsureChecked", _
                "<script language=""JavaScript"" type=""text/javascript"">function EnsureChecked(oSrc, args){args.IsValid = document.all[""" + chkConfirmation.ClientID + """].checked;}</script>")
            Session.Remove(CNStatementsAgreed)

            Dim oQuote As NexusProvider.Quote = Session(CNQuote)
            If Session(CNNoTrans) Is Nothing Then
                If (oQuote.InstDepositAmount > 0 AndAlso oQuote.DepositTransactasInstalment = False) Then
                    btnContinue.Attributes.Add("onclick", "javascript:ConfirmDepositCollection('" + GetLocalResourceObject("msgDepositCollection") + "');")
                End If
            End If
        End Sub

        Sub RetrieveClient()

            If TypeOf Session(CNParty) Is NexusProvider.PersonalParty Then

                Dim oParty As NexusProvider.PersonalParty = Session(CNParty)
                With oParty

                    lblNameTitle.Text = GetLocalResourceObject("lbl_ClientName")


                    lblName.Text = .Title & " " & .Forename & " " & .Surname

                    Dim oContact As NexusProvider.Contact = .Contacts(NexusProvider.ContactType.Email)
                    If oContact IsNot Nothing Then
                        LblEmail.Text = oContact.Number
                    End If

                    oContact = .Contacts(NexusProvider.ContactType.HomePhone)

                    'Get All HomePhone 
                    Dim sContactHomePhone As String = ""
                    For Each oHomePhoneContact As NexusProvider.Contact In .Contacts
                        If oHomePhoneContact IsNot Nothing Then
                            If oHomePhoneContact.ContactType = NexusProvider.ContactType.HomePhone Then
                                If sContactHomePhone Is Nothing Or sContactHomePhone.Length = 0 Then
                                    sContactHomePhone = oHomePhoneContact.AreaCode & oHomePhoneContact.Number
                                Else
                                    sContactHomePhone += ", " & oHomePhoneContact.AreaCode & oHomePhoneContact.Number
                                End If
                            End If
                        End If
                    Next
                    If sContactHomePhone IsNot Nothing Then
                        LblTelephone.Text = sContactHomePhone
                    End If

                    'Get Correspondence Address
                    Dim oAddress As NexusProvider.Address = .Addresses(NexusProvider.AddressType.CorrespondenceAddress)

                    If oAddress IsNot Nothing Then

                        If oAddress.Address1.Trim = "" Then
                            liAddress1Title.Visible = False
                        Else
                            LblAddress1.Text = oAddress.Address1
                        End If
                        If oAddress.Address2.Trim = "" Then
                            liAddress2Title.Visible = False
                        Else
                            LblAddress2.Text = oAddress.Address2
                        End If
                        If oAddress.Address3.Trim = "" Then
                            liAddress3Title.Visible = False
                        Else
                            LblAddress3.Text = oAddress.Address3
                        End If
                        If oAddress.Address4.Trim = "" Then
                            liAddress4Title.Visible = False
                        Else
                            LblAddress4.Text = oAddress.Address4
                        End If
                        If oAddress.Address4.Trim = "" Then
                            liPostcodeTitle.Visible = False
                        Else
                            LblPostcode.Text = oAddress.PostCode
                        End If

                        Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                        Try
                            ' This list will most likely be cached is its used often, so there won't be a performance hit
                            Dim oItem As NexusProvider.LookupListItem = oWebService.GetList(NexusProvider.ListType.PMLookup, "COUNTRY", True, False).Item(oAddress.CountryCode)
                            If oItem IsNot Nothing Then
                                lblCountry.Text = oItem.Description
                            End If
                        Finally
                            oWebService = Nothing
                        End Try

                    End If



                End With
            ElseIf TypeOf Session(CNParty) Is NexusProvider.CorporateParty Then

                Dim oParty As NexusProvider.CorporateParty = Session(CNParty)
                With oParty

                    lblNameTitle.Text = GetLocalResourceObject("lbl_CompanyName")

                    lblName.Text = oParty.CompanyName

                    liMainContact.Visible = True
                    lblMainContact.Text = oParty.MainContact

                    ' get email
                    Dim oContact As NexusProvider.Contact = .Contacts(NexusProvider.ContactType.Email)
                    If oContact IsNot Nothing Then
                        LblEmail.Text = oContact.Number
                    End If

                    'Get Main Contact Number
                    oContact = .Contacts(NexusProvider.ContactType.HomePhone)
                    If oContact IsNot Nothing Then
                        LblTelephone.Text = oContact.AreaCode & oContact.Number
                    End If

                    'Get Correspondence Address
                    Dim oAddress As NexusProvider.Address = .Addresses(NexusProvider.AddressType.CorrespondenceAddress)

                    If oAddress IsNot Nothing Then

                        If oAddress.Address1.Trim = "" Then
                            liAddress1Title.Visible = False
                        Else
                            LblAddress1.Text = oAddress.Address1
                        End If
                        If oAddress.Address2.Trim = "" Then
                            liAddress2Title.Visible = False
                        Else
                            LblAddress2.Text = oAddress.Address2
                        End If
                        If oAddress.Address3.Trim = "" Then
                            liAddress3Title.Visible = False
                        Else
                            LblAddress3.Text = oAddress.Address3
                        End If
                        If oAddress.Address4.Trim = "" Then
                            liAddress4Title.Visible = False
                        Else
                            LblAddress4.Text = oAddress.Address4
                        End If
                        If oAddress.Address4.Trim = "" Then
                            liPostcodeTitle.Visible = False
                        Else
                            LblPostcode.Text = oAddress.PostCode
                        End If

                        Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                        Try
                            Dim oItem As NexusProvider.LookupListItem = oWebService.GetList(NexusProvider.ListType.PMLookup, "COUNTRY", True, False).Item(oAddress.CountryCode)
                            If oItem IsNot Nothing Then
                                lblCountry.Text = oItem.Description
                            End If
                        Finally
                            oWebService = Nothing
                        End Try

                    End If



                End With
            End If

        End Sub

        Protected Sub btnContinue_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnContinue.Click
            If Page.IsValid Then
                'will update the premium with agent commision if agent type is BROKER
                UpdatePremiumWithAgentCommision()
                CheckPremiumAndRedirect()
            End If
        End Sub


        Protected Sub vldConfirmation_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles vldConfirmation.ServerValidate

            args.IsValid = chkConfirmation.Checked

        End Sub

        Protected Sub vldConfirmContact_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles vldConfirmContact.ServerValidate
            args.IsValid = chkConfirmContact.Checked
        End Sub

        Protected Sub btnHiddenStatement_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnHiddenStatement.Click
            Me.RetrieveClient()
        End Sub

        Sub CheckPremiumAndRedirect()
            Dim oQuote As NexusProvider.Quote = Session(CNQuote)
            Dim dTatalPremium As Decimal
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim sIsPrepaymentOptionEnabled As String
            sIsPrepaymentOptionEnabled = oWebService.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance, NexusProvider.ProductRiskOptions.IsPrepaymentOptionEnabled, NexusProvider.RiskTypeOptions.None, oQuote.ProductCode, Nothing)


            If oQuote.Risks.Count > 0 Then
                dTatalPremium = Session(CNAmountToPay)
            End If
            If dTatalPremium <= 0.0 AndAlso Session(CNMTAType) IsNot Nothing Then
                'In case of MTA
                If dTatalPremium <= 0.0 AndAlso Session(CNMTAType) = MTAType.CANCELLATION Then
                    'if this is Refund Premium or Zero premium and PrePayment = 0 then go to directly TransactionConfirmation page
                    'If sPrePaymentOption Is Nothing Or sPrePaymentOption = "0" Then
                    'During MTA Cancellation now the client needs the payment method selection screen.
                    If sIsPrepaymentOptionEnabled Is Nothing Or sIsPrepaymentOptionEnabled = "0" Then
                        Session(CNStatementsAgreed) = True
                        Session(CNPaid) = True
                        Response.Redirect("~/secure/TransactionConfirmation.aspx", False)
                    Else
                        'this will simply redirect to the PrePayment page in case for Refund Premium.
                        'if this is Refund Premium and PrePayment = 1 then go to PrePayment page and select account
                        Response.Redirect("~/secure/payment/PrePayment.aspx", False)
                    End If
                ElseIf Not Session(CNMTAType) Is Nothing And dTatalPremium = 0.0 Then
                    'this will simply redirect to the Transaction Confirmation in case when there is Return Premium
                    'Or Premium equal to Zero in case of MTA Permanent
                    Session(CNPaid) = True
                    Response.Redirect("~/secure/TransactionConfirmation.aspx", False)
                ElseIf Not Session(CNMTAType) Is Nothing And dTatalPremium < 0.0 AndAlso oQuote.PaymentMethod.Trim.ToUpper <> "DIRECT DEBIT" AndAlso oQuote.PaymentMethod.Trim.ToUpper <> "PAYNOW" Then
                    'if this is Refund Premium and PrePayment = 0 then go to directly TransactionConfirmation page
                    If String.IsNullOrEmpty(sIsPrepaymentOptionEnabled) Or sIsPrepaymentOptionEnabled = "0" Then
                        Session(CNPaid) = True
                        Response.Redirect("~/secure/TransactionConfirmation.aspx", False)
                    ElseIf sIsPrepaymentOptionEnabled = "1" Then
                        'this will simply redirect to the PrePayment page in case for Refund Premium.
                        'if this is Refund Premium and PrePayment = 1 then go to PrePayment page and select account

                        Response.Redirect("~/secure/payment/PrePayment.aspx", False)
                    End If
                Else
                    'if premium is in positive
                    Session(CNStatementsAgreed) = True
                    RedirectToPaymentPage()
                End If
            Else
                'in case of NB/Renewal
                Session(CNStatementsAgreed) = True
                RedirectToPaymentPage()
            End If
        End Sub
        ''' <summary>
        ''' Redirect To Payment Page
        ''' </summary>
        ''' <remarks></remarks>
        Sub RedirectToPaymentPage()
            Dim oQuote As NexusProvider.Quote = Session(CNQuote)
            Dim oPaymentOptions As Config.PaymentTypes = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID()).PaymentTypes
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oPaymentType As Config.PaymentType = oPaymentOptions.PaymentType(Session(CNSelectedPaymentIndex))
            Dim oPaymentHubEnabled As NexusProvider.OptionTypeSetting
            oPaymentHubEnabled = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, NexusProvider.SystemOptions.SystemOptionPaymentHubEnabled)
            Dim bIsNoTrans As Boolean = False
            Dim sIsPrepaymentOptionEnabled As String
            sIsPrepaymentOptionEnabled = oWebService.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance, NexusProvider.ProductRiskOptions.IsPrepaymentOptionEnabled, NexusProvider.RiskTypeOptions.None, oQuote.ProductCode, Nothing)

            If (Session(CNNoTrans) IsNot Nothing AndAlso (Session(CNNoTrans).ToString() = "NB" OrElse Session(CNNoTrans).ToString() = "MTA")) Then
                bIsNoTrans = True
            End If

            'code for Marked Collection and redirection
            If ((sIsPrepaymentOptionEnabled IsNot Nothing AndAlso sIsPrepaymentOptionEnabled = "1") And Session(CNTotalForQuoteCollection) IsNot Nothing) And oPaymentType.Name.Trim.ToUpper <> "PAYNOW" And oPaymentType.Name.ToUpper <> "BANKGUARANTEE" _
            And oPaymentType.Name.Trim.ToUpper <> "DIRECT DEBIT" And oPaymentType.Name.Trim.ToUpper <> "CREDIT CARD" Then
                If (bIsNoTrans) Then
                    Response.Redirect("~/secure/TransactionConfirmation.aspx", False)
                Else
                    Response.Redirect("~/secure/payment/PrePayment.aspx?quotecollection=true", False)
                End If
            ElseIf Session(CNTotalForQuoteCollection) IsNot Nothing Then
                If (bIsNoTrans) Then
                    Response.Redirect("~/secure/TransactionConfirmation.aspx", False)
                Else
                    Session.Remove(CNCashListItem) 'Loads Cash List screen when PayNow option selection
                    Response.Redirect(oPaymentType.Url & "?quotecollection=true", False)
                End If
            ElseIf (sIsPrepaymentOptionEnabled IsNot Nothing AndAlso sIsPrepaymentOptionEnabled = "1") And oPaymentType.Name.Trim.ToUpper <> "PAYNOW" And oPaymentType.Name.ToUpper <> "BANKGUARANTEE" And oPaymentType.Name.ToUpper <> "CASHDEPOSIT" _
            And oPaymentType.Name.Trim.ToUpper <> "DIRECT DEBIT" And oPaymentType.Name.Trim.ToUpper <> "CREDIT CARD" Then
                If (bIsNoTrans) Then
                    Response.Redirect("~/secure/TransactionConfirmation.aspx", False)
                Else
                    Response.Redirect("~/secure/payment/PrePayment.aspx", False)
                End If
            ElseIf oPaymentType.Type.Trim.ToUpper = "PAYMENTHUB" Then
                Dim oPaymentHubDetail As NexusProvider.PaymentHubDetails = CType(Session(CNPaymentHubDetails), NexusProvider.PaymentHubDetails)

                'If Request("RequestType") = "TokenRegistration" Then
                oPaymentHubDetail = New NexusProvider.PaymentHubDetails
                oPaymentHubDetail.TransactionAmount = Session(CNAmountToPay)
                oPaymentHubDetail.TransactionCurrency = oQuote.CurrencyCode
                oPaymentHubDetail.RequestType = PaymentHub.RequestType.Payment
                Session(CNPaymentHubDetails) = oPaymentHubDetail
                'End If

                Dim sReturnUrl As String = GetPaymentHubPageURL()
                Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "tb_show", _
      "<script language=""JavaScript"" type=""text/javascript"">$(document).ready(function(){tb_show( null,'" & sReturnUrl & "' , null);});</script>")
                Exit Sub
                'Response.Redirect(sReturnUrl, False)
            Else
                Session.Remove(CNCashListItem) 'Loads Cash List screen when PayNow option selection
                If oPaymentType.Type.Trim.ToUpper = "PREMIUMFINANCE" Then
                    Dim PaymentCollectionUrl As String = oPaymentType.PaymentCollectionUrl
                    If (oQuote.InstDepositAmount > 0 AndAlso oQuote.DepositTransactasInstalment = False) Then ''for deposit redirect to cashlist screen
                        ''show pop up else proceed normally
                        Session(CNPaid) = True
                        If hdnDepositCollection.Value = "1" Then
                            If Session(CNInstalmentMediaType) IsNot Nothing AndAlso Session(CNInstalmentMediaType).ToString().ToUpper() = "CREDIT CARD" AndAlso oPaymentHubEnabled.OptionValue = "1" Then
                                SetPaymentHubSession()
                                Response.Redirect("~/secure/Payment/OnlineCardPayment.aspx")
                            Else
                                Response.Redirect("~/secure/payment/CashListNew.aspx?Mode=INSDEPOSIT")
                            End If

                        Else
                            'set appropriate session values here to indicate payment taken and then redirect to end page
                            If PaymentCollectionUrl <> "" Then
                                If (oQuote.InstDepositAmount > 0 AndAlso oQuote.DepositTransactasInstalment = False AndAlso oPaymentType.Name.Trim.ToUpper <> "DIRECT DEBIT") Then
                                    Response.Redirect(PaymentCollectionUrl, False)
                                ElseIf Session(CNInstalmentMediaType) IsNot Nothing AndAlso Session(CNInstalmentMediaType).ToString().ToUpper() = "CREDIT CARD" AndAlso oPaymentHubEnabled.OptionValue = "1" Then
                                    SetPaymentHubSession()
                                    Response.Redirect("~/secure/Payment/OnlineCardPayment.aspx")
                                Else

                                    Session(CNPaid) = True
                                    Response.Redirect("~/secure/TransactionConfirmation.aspx", False)
                                End If
                            ElseIf Session(CNInstalmentMediaType) IsNot Nothing AndAlso Session(CNInstalmentMediaType).ToString().ToUpper() = "CREDIT CARD" AndAlso oPaymentHubEnabled.OptionValue = "1" Then
                                SetPaymentHubSession()
                                Response.Redirect("~/secure/Payment/OnlineCardPayment.aspx")
                            Else
                                Session(CNPaid) = True
                                Response.Redirect("~/secure/TransactionConfirmation.aspx", False)
                            End If
                        End If
                    End If

                    'set appropriate session values here to indicate payment taken and then redirect to end page
                    If PaymentCollectionUrl <> "" Then
                        If (oQuote.InstDepositAmount > 0 AndAlso oQuote.DepositTransactasInstalment = False AndAlso oPaymentType.Name.Trim.ToUpper <> "DIRECT DEBIT") Then
                            Response.Redirect(PaymentCollectionUrl, False)
                        ElseIf Session(CNInstalmentMediaType) IsNot Nothing AndAlso Session(CNInstalmentMediaType).ToString().ToUpper() = "CREDIT CARD" AndAlso oPaymentHubEnabled.OptionValue = "1" Then
                            SetPaymentHubSession()
                            Response.Redirect("~/secure/Payment/OnlineCardPayment.aspx")
                        Else
                            Session(CNPaid) = True
                            Response.Redirect("~/secure/TransactionConfirmation.aspx", False)
                        End If
                    ElseIf Session(CNInstalmentMediaType) IsNot Nothing AndAlso Session(CNInstalmentMediaType).ToString().ToUpper() = "CREDIT CARD" AndAlso oPaymentHubEnabled.OptionValue = "1" Then
                        SetPaymentHubSession()
                        Response.Redirect("~/secure/Payment/OnlineCardPayment.aspx")
                    Else
                        Session(CNPaid) = True
                        Response.Redirect("~/secure/TransactionConfirmation.aspx", False)
                    End If
                Else
                    If (bIsNoTrans) Then
                        Session(CNPaid) = True
                        Response.Redirect("~/secure/TransactionConfirmation.aspx", False)
                    Else
                        If oPaymentType.Name.Trim.ToUpper = "PAYNOW" Then
                            Response.Redirect("~/secure/payment/CashListNew.aspx?Mode=PayNow")
                        Else
                            Response.Redirect(oPaymentType.Url, False)
                        End If
                    End If
            End If
            End If
        End Sub
        Private Sub SetPaymentHubSession()
            Dim oPaymentHubDetail As NexusProvider.PaymentHubDetails = New NexusProvider.PaymentHubDetails

            ' If Request.UrlReferrer.ToString.Contains("Statements") Or Request.UrlReferrer.ToString.Contains("PremiumDisplay") Then
            Dim oQuote As NexusProvider.Quote = Session(CNQuote)
            oPaymentHubDetail.TransactionAmount = 0
            oPaymentHubDetail.TransactionCurrency = oQuote.CurrencyCode
            oPaymentHubDetail.RequestType = PaymentHub.RequestType.TokenRegistration
            If oQuote.InstDepositAmount > 0 AndAlso oQuote.DepositTransactasInstalment = False Then
                oPaymentHubDetail.ReturnURL = "~/secure/payment/CashListNew.aspx?Mode=INSDEPOSIT"
            Else
                oPaymentHubDetail.ReturnURL = "~/secure/TransactionConfirmation.aspx"
            End If


            Session(CNPaymentHubDetails) = oPaymentHubDetail
        End Sub
    End Class

End Namespace

