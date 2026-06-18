Imports CMS.library
Imports System.Web.Configuration.WebConfigurationManager
Imports Nexus.Library
Imports Nexus.Utils
Imports Nexus.Constants.Constant
Imports Nexus.Constants.Session

Namespace Nexus

    Partial Class secure_payment_DirectDebit : Inherits BasePayment ' Frontend.clsCMSPage

        Protected Shadows Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            Page.SetFocus(txtTitle)
            If Session(CNPaid) = True Then
                SetPaymentTakenAndRedirect()
            End If
            If Not IsPostBack Then

                'SetPageProgress(6)

                'GET INSTALLMENT CODE IMPLEMENTAION AS IT WILL BE CALLED NOW ON PAGE LOAD TO DISPLAY THE 
                'PREMIUM AND THE INSTALLMENT NUMBER

                Dim oQuoteList As NexusProvider.InstallmentQuoteCollection
                Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                Dim oQuote As NexusProvider.Quote
                Dim paymentOptions As New Config.PaymentTypes
                Dim oPayment As New NexusProvider.Payment(NexusProvider.PaymentTypes.None)
                'Getting Installment Quotes
                Try
                    oQuote = Session(CNQuote)
                    'HERE WE PASS THE INCEPTION DATE AS CURRENT DATE
                    oQuote.InceptionDate = Date.Today
                    oQuoteList = oWebService.GetInstalmentQuotes(Session(CNAmountToPay), _
                    oQuote.CoverStartDate, oQuote.CoverEndDate, oQuote.InceptionDate _
                    , DateTime.Now, Convert.ToInt16(ddlPaymentDate.Text), 1, oQuote.InsuranceFileKey, 0, 0, False, oQuote.BranchCode)
                Catch
                    lblDirectDebitText.Visible = False
                    pnlPaymentDetails.Visible = False
                    lblErrorMsg.Visible = True
                    btnPay.Visible = False
                    Exit Sub
                Finally
                    oWebService = Nothing
                End Try
                'Finding the Highest Pref ID
                Dim iHighestPref_ID As Integer
                paymentOptions = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID()).PaymentTypes
                Dim oTempQuoteList As New NexusProvider.InstallmentQuoteCollection
                Dim iCounter As Integer
                'Adding Matching Records(CompanyNo,SchemeNo,SchemeVersion) into Temp Collection
                For iCounter = 0 To oQuoteList.Count - 1
                    If oQuoteList.Item(iCounter).CompanyNo = paymentOptions.PaymentType(Session(CNSelectedPaymentIndex)).PaymentProviderValue("CompanyNo").Value _
                     And oQuoteList.Item(iCounter).SchemeNo = paymentOptions.PaymentType(Session(CNSelectedPaymentIndex)).PaymentProviderValue("SchemeNo").Value _
                     And oQuoteList.Item(iCounter).SchemeVersion = paymentOptions.PaymentType(Session(CNSelectedPaymentIndex)).PaymentProviderValue("VersionNo").Value Then
                        oTempQuoteList.Add(oQuoteList.Item(iCounter))
                    End If
                Next

                If oTempQuoteList.Count > 1 Then
                    'if More than one quotes Retreived
                    Dim iCount As Integer
                    For iCount = 0 To oTempQuoteList.Count - 1
                        If oTempQuoteList(iCount).PFRF_ID > iHighestPref_ID Then
                            iHighestPref_ID = oTempQuoteList(iCount).PFRF_ID
                        End If
                    Next

                ElseIf oTempQuoteList.Count = 1 Then
                    'if only quote Retreived
                    iHighestPref_ID = oTempQuoteList(0).PFRF_ID
                End If
                'Initialize oPayment Object
                '               Dim oPayment As New NexusProvider.Payment(NexusProvider.PaymentTypes.None, CDec(Session(CNAmountToPay)))
                For iCounter = 0 To oQuoteList.Count - 1
                    If oQuoteList.Item(iCounter).PFRF_ID = iHighestPref_ID Then
                        'DISPLAY THE CHARGES AND THE PREMIUM ALONGWITH THE NUMBER OF INSTALLMENTS
                        LblChargesHeading.Text = LblChargesHeading.Text & "(" _
                            & paymentOptions.PaymentType(Session(CNSelectedPaymentIndex)).FeePercent & "%)"

                        LblPremium.Text = New Money(oQuote.Risks(0).PremiumDueGross, Session(CNCurrenyCode)).Formatted
                        LblCharges.Text = New Money(Session(CNChargetoPay), Session(CNCurrenyCode)).Formatted
                        Dim Total As Double = oQuoteList.Item(iCounter).TotalInstalmentsAmount
                        LblTotal.Text = New Money(Total, Session(CNCurrenyCode)).Formatted

                        'TODO: SHOW INSTALLMENT ONLY IF IT IS GREATER THAN 1. MIGHT NEED TO CHANGE THE LOGIC  - MB - 21 MAY 07

                        If CInt(paymentOptions.PaymentType(Session(CNSelectedPaymentIndex)).NoOfInstalments) Then
                            Dim Installments As Double = Total / oQuoteList.Item(iCounter).InstalmentsToPay
                            LblInstallments.Text = New Money(Installments, Session(CNCurrenyCode)).Formatted
                            LblInstallmentNo.Text = oQuoteList.Item(iCounter).InstalmentsToPay
                        Else
                            LblInstallmentsHeading.Visible = False
                        End If

                        'DISPLAY SECTION END

                        'UPDATING THE SESSION CNPAYMENT HERE AFTER FINDING THE HIGHEST PREFID
                        oPayment.AmountToFinance = oQuoteList.Item(iCounter).TotalAmountInput

                        oPayment.OverrideInterestRate = 0
                        oPayment.OverrideRate = 0
                        oPayment.PaymentProtection = False
                        oPayment.QuoteDate = DateTime.Now
                        oPayment.SelectedSchemeNo = oQuoteList.Item(iCounter).SchemeNo
                        oPayment.SelectedSchemeVersion = oQuoteList.Item(iCounter).SchemeVersion
                        oPayment.StartDate = oQuoteList.Item(iCounter).FirstInstalmentDate
                        oPayment.EndDate = oQuoteList.Item(iCounter).LastInstalmentDate
                        oPayment.MonthDay = Convert.ToInt16(ddlPaymentDate.Text)
                        oPayment.WeekDay = 1
                        oPayment.Pref_ID = oQuoteList.Item(iCounter).PFRF_ID
                        Session(CNPayment) = oPayment
                    End If
                Next
            End If

        End Sub

        Protected Sub btnPay_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPay.Click
            'THIS SECTION IS FOR UPDATING THE INCEPTION DATE FROM THE TXTINCEPTIONDATE
            'SIMILARLY ALSO UPDATING THE BANK DETAILS AND THEN UPDATING THE CNPAYMENT AND CNQUOTE
            If Page.IsValid Then
                Dim oQuote As NexusProvider.Quote
                oQuote = Session(CNQuote)
                oQuote.InceptionDate = Convert.ToDateTime(txtInceptionDate.Text)
                Session(CNQuote) = oQuote
                Dim oPayment As New NexusProvider.Payment(NexusProvider.PaymentTypes.None, CDec(Session(CNAmountToPay)))
                oPayment = Session(CNPayment)
                oPayment.PreferredDate = oQuote.InceptionDate
                oPayment.BankAccountName = TxtAccName.Text
                oPayment.BankAccountNo = TxtAccNumber.Text
                oPayment.BankAddress = AddressCntrl.Address 'Bank Address
                oPayment.BankSortCode = TxtSort1.Text + TxtSort2.Text + TxtSort3.Text
                Session(CNPayment) = oPayment
                SetPaymentTakenAndRedirect() 'SS PN 41618
            End If
        End Sub

        Protected Sub custvldAccountNumber_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles custvldAccountNumber.ServerValidate
            If IsNumeric(Trim(TxtAccNumber.Text)) Then
                args.IsValid = True
            Else
                args.IsValid = False
            End If
        End Sub

        Protected Sub custvldtxtInceptionDate_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles custvldtxtInceptionDate.ServerValidate
            If IsDate(Trim(txtInceptionDate.Text)) Then
                args.IsValid = True
            Else
                args.IsValid = False
            End If
        End Sub

        Protected Sub CustvldSortCode1_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles CustvldSortCode1.ServerValidate
            If IsNumeric(Trim(TxtSort1.Text)) Then
                args.IsValid = True
            Else
                args.IsValid = False
            End If
        End Sub

        Protected Sub CustvldSortCode2_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles CustvldSortCode2.ServerValidate
            If IsNumeric(Trim(TxtSort2.Text)) Then
                args.IsValid = True
            Else
                args.IsValid = False
            End If
        End Sub

        Protected Sub CustvldSortCode3_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles CustvldSortCode3.ServerValidate
            If IsNumeric(Trim(TxtSort3.Text)) Then
                args.IsValid = True
            Else
                args.IsValid = False
            End If
        End Sub
    End Class

End Namespace