Imports Nexus.Library
Imports CMS.Library
Imports Nexus.Constants.Constant
Imports Nexus.Constants.Session
Imports System.Data
Imports System.Linq
Imports System.Security.Authentication
Imports System.Web.Configuration.WebConfigurationManager



Namespace Nexus

    Partial Class secure_payment_OnlineCardPayment
        Inherits Frontend.clsCMSPage

        Protected Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init
            Dim oPaymentHubConfig As NexusProvider.PaymentHubConfig = GetPaymentHubConfig()
            ViewState("MarkDefaultCard") = oPaymentHubConfig.MarkDefaultCreditCard
        End Sub


        Protected Shadows Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            If Request("__EVENTARGUMENT") = "UpdateContact" Then
                Page.ClientScript.GetPostBackEventReference(Me, "")
                Dim sContactData() As String = txtContactData.Value.Split(";")

                'Need to Retreive the Data from Session
                Dim oParty As NexusProvider.BaseParty = Nothing
                If Session(CNParty) IsNot Nothing Then
                    Select Case True
                        Case TypeOf Session(CNParty) Is NexusProvider.CorporateParty
                            oParty = CType(Session(CNParty), NexusProvider.CorporateParty)
                        Case TypeOf Session(CNParty) Is NexusProvider.PersonalParty
                            oParty = CType(Session(CNParty), NexusProvider.PersonalParty)
                    End Select
                Else

                    Dim oWebservice As NexusProvider.ProviderBase
                    oWebservice = New NexusProvider.ProviderManager().Provider
                    oParty = oWebservice.GetParty(Request.QueryString("PartyKey"))
                    Session(CNParty) = oParty
                End If

                If sContactData(0).ToUpper = "ADD" Then

                    Dim oNewContact As New NexusProvider.Contact
                    oNewContact.ContactType = NexusProvider.ContactType.MEMAIL


                    With oNewContact
                        Select Case oNewContact.ContactType
                            Case NexusProvider.ContactType.Email Or NexusProvider.ContactType.MEMAIL
                                .ContactDetailType = NexusProvider.ItemChoiceTypes.EmailAddress
                            Case Else
                                .ContactDetailType = NexusProvider.ItemChoiceTypes.Number
                        End Select
                        .Description = sContactData(2)
                        .AreaCode = sContactData(3)
                        .Number = sContactData(4)
                        .Extension = sContactData(5)
                        .OtherContactTypeCode = sContactData(7)
                        .ContactTypeDescription = sContactData(7)
                    End With

                    oParty.Contacts.Add(oNewContact)

                    Dim oTempContactCollection As New NexusProvider.ContactCollection

                    For i = 0 To oParty.Contacts.Count - 1
                        If Not String.IsNullOrEmpty(oParty.Contacts(i).Number.Trim()) Then
                            oTempContactCollection.Add(oParty.Contacts(i))
                        End If

                    Next
                    oParty.Contacts = oTempContactCollection
                    
                    'SAM call
                    If Not UpdatePartyCall(oParty, oParty.BranchCode) Then
                        Exit Sub
                    End If

                    Session(CNParty) = oParty
                    FillCardDetails()

                End If


                End If
                If Not Page.IsPostBack Then
                    If Session(CNPaymentHubDetails) IsNot Nothing AndAlso CType(Session(CNPaymentHubDetails), NexusProvider.PaymentHubDetails).ResultDescription = PaymentHub.ResultDescription.IncorrectCardDetailsEntered Then
                        LblCardRegisterationFailed.Text = GetLocalResourceObject("lblerr_CardRegisteration").ToString()
                    Else
                        LblCardRegisterationFailed.Text = ""
                    End If
                    FillCardDetails()
                End If


        End Sub

        Sub FillCardDetails()
            Dim oCreditCardCollection As Object
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oPartyBankDetails As NexusProvider.BankCollection
            Dim oParty As NexusProvider.BaseParty
            Dim oPaymentHubDetail As NexusProvider.PaymentHubDetails
            Dim oAddress As NexusProvider.Address
            Dim oContact As NexusProvider.Contact
            Dim oCurencyCollection As NexusProvider.CurrencyCollection
            Dim nPartyKey As Integer
            Dim sBranchCode As String

            If Session(CNCashListItem) IsNot Nothing AndAlso Request.QueryString("PartyKey") IsNot Nothing AndAlso Request.QueryString("PartyType") Is Nothing Then
                nPartyKey = Request.QueryString("PartyKey")
                sBranchCode = Session(CNTransBranchCode)
                btnOk.Visible = False
            ElseIf Request.QueryString("PartyKey") IsNot Nothing Then
                nPartyKey = Request.QueryString("PartyKey")
                sBranchCode = Session(CNTransBranchCode)
                If Request.QueryString("PartyType") IsNot Nothing Then
                    btnProcess.Visible = False
                    grdCard.Columns.Item(3).HeaderText = GetLocalResourceObject("lblgrdHeader_DefaultCard")
                Else
                    btnOk.Visible = False
                End If
            ElseIf Session(CNQuote) IsNot Nothing Then
                nPartyKey = CType(Session(CNQuote), NexusProvider.Quote).PartyKey
                sBranchCode = CType(Session(CNQuote), NexusProvider.Quote).BranchCode
                btnOk.Visible = False
                If CDec(Session(CNAmountToPay)) < 0 Then
                    btnAddCard.Visible = False
                End If
            End If


            Try
                oPartyBankDetails = oWebService.GetPartyBankDetails(nPartyKey, sBranchCode)
                ViewState("PartyBankDetails") = oPartyBankDetails
                oCreditCardCollection = (From r In oPartyBankDetails Where r.CreditCard IsNot Nothing AndAlso r.CreditCard.TrackingNumber IsNot Nothing AndAlso r.CreditCard.TrackingNumber <> "" _
                 AndAlso (Convert.ToInt32(Regex.Split(r.CreditCard.ExpiryDate, "/")(1)) > Convert.ToInt32(DateTime.Now.Year.ToString().Substring(2, 2)) _
                 OrElse (Convert.ToInt32(Regex.Split(r.CreditCard.ExpiryDate, "/")(1)) = Convert.ToInt32(DateTime.Now.Year.ToString().Substring(2, 2)) AndAlso Convert.ToInt32(Regex.Split(r.CreditCard.ExpiryDate, "/")(0)) >= Convert.ToInt32(DateTime.Now.Month.ToString()))) _
                          Select r.CreditCard.Number, r.CreditCard.NameOnCreditCard, r.PartyBankKey, r.CreditCard.ManualAuthCode, r.CreditCard.TrackingNumber, r.CreditCard.ExpiryDate, r.CreditCard.IsDefaultCreditCard).ToList()
                If Request.QueryString("PartyType") IsNot Nothing Then
                    oPaymentHubDetail = New NexusProvider.PaymentHubDetails
                    oCurencyCollection = oWebService.GetCurrenciesByBranch(Session(CNTransBranchCode))
                    oPaymentHubDetail.TransactionCurrency = oCurencyCollection.Item(0).BaseCurrencyCode
                    oPaymentHubDetail.ReturnURL = "~/secure/Payment/OnlineCardPayment.aspx?PartyKey=" & Request.QueryString("PartyKey") & "&Code=" & Request.QueryString("Code") & "&PartyType=" & Request.QueryString("PartyType")
                    oPaymentHubDetail.RequestType = PaymentHub.RequestType.TokenRegistration
                    oPaymentHubDetail.PartyKey = Request.QueryString("PartyKey")
                    oPaymentHubDetail.PartyCode = Request.QueryString("Code")
                    Session(CNPaymentHubDetails) = oPaymentHubDetail
                End If
                oParty = oWebService.GetParty(nPartyKey)
                If oParty IsNot Nothing Then
                    'Base on the session value is personal / corporate client is loaded
                    Select Case True
                        Case TypeOf Session(CNParty) Is NexusProvider.PersonalParty
                            oParty = CType(Session(CNParty), NexusProvider.PersonalParty)

                        Case TypeOf Session(CNParty) Is NexusProvider.CorporateParty
                            oParty = CType(Session(CNParty), NexusProvider.CorporateParty)
                    End Select
                End If
                If Session(CNPaymentHubDetails) IsNot Nothing Then
                    oPaymentHubDetail = Session(CNPaymentHubDetails)
                    If oParty.Addresses.FindItemByAddressType(NexusProvider.AddressType.CorrespondenceAddress) IsNot Nothing Then
                        oAddress = New NexusProvider.Address()
                        oAddress = oParty.Addresses.FindItemByAddressType(NexusProvider.AddressType.CorrespondenceAddress)
                        oPaymentHubDetail.CustomerDetails.Address1 = oAddress.Address1
                        oPaymentHubDetail.CustomerDetails.Address2 = oAddress.Address2
                        oPaymentHubDetail.CustomerDetails.Town = oAddress.Address3
                        oPaymentHubDetail.CustomerDetails.County = oAddress.Address4
                        oPaymentHubDetail.CustomerDetails.Country = GetDescriptionForCode(NexusProvider.ListType.PMLookup, oAddress.CountryCode, "Country")
                        oPaymentHubDetail.CustomerDetails.Postcode = oAddress.PostCode
                    End If
                    If oParty.Contacts.FindItemByContactType(NexusProvider.ContactType.MEMAIL) IsNot Nothing Then
                        oContact = oParty.Contacts.FindItemByContactType(NexusProvider.ContactType.MEMAIL)
                        oPaymentHubDetail.CustomerDetails.Email = oContact.Number

                    End If
                    If AppSettings("PaymentHub.MandatoryFields") = "Email" AndAlso oPaymentHubDetail.CustomerDetails.Email Is Nothing Then
                        Dim sURL As String
                        If HttpContext.Current.Session.IsCookieless Then
                            sURL = AppSettings("webroot") & "(S(" & Session.SessionID.ToString() + "))" & "/Modal/Contact.aspx?PostbackTo=" & "PnlContact.ClientID.ToString.Trim()" & "&Source=PaymentHub&modal=true&KeepThis=true&TB_iframe=true&height=500&width=750"
                        Else
                            sURL = AppSettings("webroot") & "Modal/Contact.aspx?PostbackTo=" & "PnlContact.ClientID.ToString.Trim()" & "&Source=PaymentHub&modal=true&KeepThis=true&TB_iframe=true&height=500&width=750"
                        End If
                        Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "tb_show",
                   "<script language=""JavaScript"" type=""text/javascript"">$(document).ready(function(){tb_show( null,'" & sURL & "' , null);});</script>")
                        Exit Sub
                    End If

                    Select Case True
                        Case TypeOf oParty Is NexusProvider.PersonalParty
                            oParty = CType(oParty, NexusProvider.PersonalParty)
                            oPaymentHubDetail.CustomerDetails.Firstname = CType(oParty, NexusProvider.PersonalParty).Forename
                            oPaymentHubDetail.CustomerDetails.Lastname = CType(oParty, NexusProvider.PersonalParty).Lastname
                            oPaymentHubDetail.TransactionCurrency = CType(oParty, NexusProvider.PersonalParty).Currency

                        Case TypeOf oParty Is NexusProvider.CorporateParty
                            oParty = CType(oParty, NexusProvider.CorporateParty)
                            oPaymentHubDetail.CustomerDetails.Firstname = CType(oParty, NexusProvider.CorporateParty).CompanyName
                            oPaymentHubDetail.TransactionCurrency = CType(oParty, NexusProvider.CorporateParty).Currency

                    End Select
                End If
                Session(CNPaymentHubDetails) = oPaymentHubDetail
                If oCreditCardCollection.Count = 0 Then
                    AddCard()
                Else
                    grdCard.DataSource = oCreditCardCollection
                    grdCard.DataBind()

                End If
                If ViewState("MarkDefaultCard") <> "1" AndAlso Request.QueryString("PartyType") IsNot Nothing Then
                    grdCard.Columns(3).Visible = False
                End If
            Catch ex As Exception

            Finally
                oCreditCardCollection = Nothing
                oWebService = Nothing
                oPartyBankDetails = Nothing
                oParty = Nothing
                oPaymentHubDetail = Nothing
                oAddress = Nothing
                oContact = Nothing
                oCurencyCollection = Nothing
            End Try
        End Sub


        Protected Sub btnAddCard_Click(sender As Object, e As EventArgs) Handles btnAddCard.Click
            AddCard()
        End Sub

        Protected Sub btnProcess_Click(sender As Object, e As EventArgs) Handles btnProcess.Click
            Dim oQuote As NexusProvider.Quote = Session(CNQuote)
            Dim grd_Card As System.Web.UI.WebControls.GridView
            grd_Card = grdCard
            Dim oCreditCard As New NexusProvider.CreditCard
            For CardIndex As Integer = 0 To grd_Card.Rows.Count - 1
                If CType(grd_Card.Rows(CardIndex).FindControl("rdDefaultCard"), System.Web.UI.WebControls.RadioButton).Checked Then

                    oCreditCard.Number = grd_Card.Rows(CardIndex).Cells(0).Text
                    oCreditCard.AuthCode = CType(grd_Card.Rows(CardIndex).FindControl("hdnAuthCode"), System.Web.UI.WebControls.HiddenField).Value
                    oCreditCard.ExpiryDate = grd_Card.Rows(CardIndex).Cells(1).Text

                    oCreditCard.PartyBankKey = grd_Card.DataKeys(CardIndex).Value
                    oCreditCard.NameOnCreditCard = grd_Card.Rows(CardIndex).Cells(2).Text
                    oCreditCard.TrackingNumber = CType(grd_Card.Rows(CardIndex).FindControl("hdnTokenNo"), System.Web.UI.WebControls.HiddenField).Value
                    oCreditCard.VIAPaymentHub = True
                End If
            Next
            Session(CNCardDetails) = oCreditCard

            If oQuote IsNot Nothing AndAlso oQuote.PaymentMethod.ToUpper.Trim() = "PREMIUMFINANCE" Then
                If oQuote.InstDepositAmount > 0 AndAlso oQuote.DepositTransactasInstalment = False Then
                    Response.Redirect("~/secure/payment/CashList.aspx?Mode=INSDEPOSIT", False)

                Else
                    Session(CNPaid) = True
                    Response.Redirect("~/secure/TransactionConfirmation.aspx", False)
                End If
            ElseIf oQuote IsNot Nothing AndAlso oQuote.PaymentMethod.ToUpper.Trim() = "PAYNOW" Then
                Dim oPaymentHubDetail As NexusProvider.PaymentHubDetails = Session(CNPaymentHubDetails)
                oPaymentHubDetail.CardNumber = oCreditCard.Number
                oPaymentHubDetail.AuthCode = oCreditCard.AuthCode
                oPaymentHubDetail.CardExpiry = oCreditCard.ExpiryDate

                oPaymentHubDetail.PartyBankKey = oCreditCard.PartyBankKey
                'oPaymentHubDetail.NameOnCreditCard = grd_Card.Rows(CardIndex).Cells(3).Text
                oPaymentHubDetail.TokenID = oCreditCard.TrackingNumber
                oPaymentHubDetail.IntegrationToken = oCreditCard.AuthCode

                If oPaymentHubDetail.RequestType = PaymentHub.RequestType.Payment Then
                    PaymentHubProcessPurchase(oPaymentHubDetail)
                ElseIf oPaymentHubDetail.RequestType = PaymentHub.RequestType.Refund Then
                    PaymentHubProcessRefund(oPaymentHubDetail)
                End If

                If oPaymentHubDetail.ResultDescription = "0" Then
                    Session(CNPaid) = True
                Else
                    oPaymentHubDetail.ResultDescription = PaymentHub.ResultDescription.Declined
                    Session(CNPaid) = False
                End If
                Response.Redirect(oPaymentHubDetail.ReturnURL, False)
            ElseIf Session(CNCashListItem) IsNot Nothing Then
                Dim oPaymentHubDetail As NexusProvider.PaymentHubDetails = Session(CNPaymentHubDetails)
                Dim CashListItemIndex As Integer = oPaymentHubDetail.CashListItemIndex

                If CashListItemIndex >= 0 Then
                    Dim oReceiptCashListItemType As NexusProvider.ReceiptCashListItemType = CType(Session(CNCashListItem), NexusProvider.ReceiptCashListItemType)


                    With oReceiptCashListItemType.ReceiptItems.Item(CashListItemIndex)

                        .PaymentHubDetails.CardNumber = oCreditCard.Number
                        .PaymentHubDetails.AuthCode = oCreditCard.AuthCode
                        .PaymentHubDetails.CardExpiry = oCreditCard.ExpiryDate

                        .PaymentHubDetails.PartyBankKey = oCreditCard.PartyBankKey
                        'oCreditCard.NameOnCreditCard = grd_Card.Rows(CardIndex).Cells(3).Text
                        .PaymentHubDetails.TokenID = oCreditCard.TrackingNumber
                        .PaymentHubDetails.IntegrationToken = oCreditCard.AuthCode

                    End With

                    Session(CNCashListItem) = oReceiptCashListItemType
                    Response.Redirect("~/secure/payment/CashListItems.aspx?TypeTrans=" & Request.QueryString("TypeTrans"))




                End If

                'Dim oPaymentHubDetail As NexusProvider.PaymentHubDetails = Session(CNPaymentHubDetails)

            End If



        End Sub

        Protected Sub btnOk_Click(sender As Object, e As EventArgs) Handles btnOk.Click
            Session(CNPaymentHubDetails) = Nothing
            Session(CNCardDetails) = Nothing
            If ViewState("MarkDefaultCard") = "1" Then
                Dim grd_Card As System.Web.UI.WebControls.GridView
                grd_Card = grdCard
                Dim oCreditCard As New NexusProvider.CreditCard
                For CardIndex As Integer = 0 To grd_Card.Rows.Count - 1
                    If CType(grd_Card.Rows(CardIndex).FindControl("rdDefaultCard"), System.Web.UI.WebControls.RadioButton).Checked Then
                        Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                        Dim oBankCollection As New NexusProvider.BankCollection
                        If ViewState("PartyBankDetails") IsNot Nothing Then
                            For Each oBank In ViewState("PartyBankDetails")
                                If oBank.PartyBankKey = grd_Card.DataKeys(CardIndex).Value Then
                                    oBank.CreditCard.IsDefaultCreditCard = True
                                    oBankCollection.Add(oBank)
                                    oBankCollection(0).TaskMode = NexusProvider.Bank.Mode.Edit
                                    oWebService.ManageBankDetails(Request.QueryString("PartyKey"), oBankCollection)
                                    If Request.QueryString("PartyType") IsNot Nothing AndAlso Request.QueryString("PartyType") = "PC" Then
                                        Response.Redirect("~/secure/agent/PersonalClientDetails.aspx?PartyKey=" & Request.QueryString("PartyKey") & "&Code=" & Request.QueryString("Code") & "")
                                    ElseIf Request.QueryString("PartyType") IsNot Nothing AndAlso Request.QueryString("PartyType") = "CC" Then
                                        Response.Redirect("~/secure/agent/CorporateClientDetails.aspx?PartyKey=" & Request.QueryString("PartyKey") & "&Code=" & Request.QueryString("Code") & "")
                                    End If
                                    Exit For
                                End If
                            Next
                        End If
                    End If
                Next
            Else
                If Request.QueryString("PartyType") IsNot Nothing AndAlso Request.QueryString("PartyType") = "PC" Then
                    Response.Redirect("~/secure/agent/PersonalClientDetails.aspx?PartyKey=" & Request.QueryString("PartyKey") & "&Code=" & Request.QueryString("Code") & "")
                ElseIf Request.QueryString("PartyType") IsNot Nothing AndAlso Request.QueryString("PartyType") = "CC" Then
                    Response.Redirect("~/secure/agent/CorporateClientDetails.aspx?PartyKey=" & Request.QueryString("PartyKey") & "&Code=" & Request.QueryString("Code") & "")
                End If
            End If
        End Sub

        Private Sub AddCard()
            If Session(CNPaymentHubDetails) IsNot Nothing Then
                Dim oPaymentHubDetail As NexusProvider.PaymentHubDetails = Session(CNPaymentHubDetails)
                Dim oQuote As NexusProvider.Quote = Session(CNQuote)

                If AppSettings("PaymentHub.MandatoryFields") = "Email" AndAlso oPaymentHubDetail.CustomerDetails.Email Is Nothing Then
                    Dim sURL As String
                    If HttpContext.Current.Session.IsCookieless Then
                        sURL = AppSettings("webroot") & "(S(" & Session.SessionID.ToString() + "))" & "/Modal/Contact.aspx?PostbackTo=" & "PnlContact.ClientID.ToString.Trim()" & "&Source=PaymentHub&modal=true&KeepThis=true&TB_iframe=true&height=500&width=750"
                    Else
                        sURL = AppSettings("webroot") & "Modal/Contact.aspx?PostbackTo=" & "PnlContact.ClientID.ToString.Trim()" & "&Source=PaymentHub&modal=true&KeepThis=true&TB_iframe=true&height=500&width=750"
                    End If
                    Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "tb_show", _
               "<script language=""JavaScript"" type=""text/javascript"">$(document).ready(function(){tb_show( null,'" & sURL & "' , null);});</script>")
                    Exit Sub

                End If

                If oPaymentHubDetail.TransactionCurrency = Nothing OrElse oPaymentHubDetail.TransactionCurrency = "" Then
                    If Request.QueryString("PartyType") IsNot Nothing Then
                        oPaymentHubDetail.TransactionCurrency = Session(CNCurrenyCode)
                    ElseIf oQuote IsNot Nothing Then
                        oPaymentHubDetail.TransactionCurrency = oQuote.CurrencyCode
                    Else
                        oPaymentHubDetail.TransactionCurrency = Session(CNCurrenyCode)
                    End If
                End If
                If Request("RequestType") = "TokenRegistration" Then
                    oPaymentHubDetail.RequestType = PaymentHub.RequestType.TokenRegistration
                End If
                Session(CNPaymentHubDetails) = oPaymentHubDetail
                Dim sReturnUrl As String = GetPaymentHubPageURL()
                If Not String.IsNullOrEmpty(sReturnUrl) Then
                    Response.Redirect(sReturnUrl, False)
                End If

            End If
        End Sub
        Protected Sub grdCard_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdCard.RowDataBound
            If e.Row.RowType = DataControlRowType.DataRow AndAlso ViewState("MarkDefaultCard") <> "1" Then
                CType(e.Row.FindControl("rdDefaultCard"), System.Web.UI.WebControls.RadioButton).Checked = False

            End If
        End Sub

       
       End Class
End Namespace