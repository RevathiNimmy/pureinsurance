Imports CMS.library
Imports Nexus.Utils
Imports System.Web.Configuration.WebConfigurationManager
Imports Nexus.Library
Imports Nexus.Constants.Constant
Imports Nexus.Constants.Session

Namespace Nexus

    Partial Class secure_payment_CardPayment : Inherits BasePayment ' Frontend.clsCMSPage

        Protected Shadows Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            'Page.SetFocus(txtTitle)
            'If Session(CNPaid) = True Then
            '    SetPaymentTakenAndRedirect()
            'End If
            'If Not IsPostBack Then
            '    Dim oQuote As NexusProvider.Quote = Session(CNQuote)
            '    SetPageProgress(6)

            '    Dim paymentOptions As New Config.PaymentTypes
            '    Dim dTatalPremium As Decimal
            '    If oQuote.Risks.Count > 0 Then
            '        dTatalPremium = oQuote.GrossTotal
            '    End If

            '    paymentOptions = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID()).PaymentTypes
            '    LblChargesHeading.Text = LblChargesHeading.Text & "(" _
            '        & paymentOptions.PaymentType(CInt(Session("SelectedItem"))).FeePercent & "%)"

            '    LblPremium.Text = New Money(dTatalPremium, Session(CNCurrenyCode)).Formatted ' Format(CType(Session(CNAmountToPay), Decimal), "£#,###0.00")
            '    LblDCTotal.Text = New Money(dTatalPremium, Session(CNCurrenyCode)).Formatted
            '    Dim charges As Decimal = (dTatalPremium / 100) * 2
            '    LblCharges.Text = New Money(CType(charges, Decimal), Session(CNCurrenyCode)).Formatted
            '    'Format(CType(charges, Decimal), "£#,###0.00")
            '    Dim Total As Decimal = charges + dTatalPremium
            '    LblTotal.Text = New Money(CType(Total, Decimal), Session(CNCurrenyCode)).Formatted

            '    Select Case UCase(paymentOptions.PaymentType(CInt(Session("SelectedItem"))).Type)
            '        Case "CREDITCARD"
            '            phldrDC.Visible = False
            '            phldrCC.Visible = True
            '            pnlDC.Visible = False
            '            pnlCC.Visible = True
            '        Case "DEBITCARD"
            '            phldrDC.Visible = True
            '            phldrCC.Visible = False
            '            pnlDC.Visible = True
            '            pnlCC.Visible = False
            '    End Select
            'End If
        End Sub

        Protected Sub btnPay_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPay.Click
            ' Response.Redirect("~/secure/TransactionConfirmation.aspx")
            Dim oQuote As NexusProvider.Quote = Session(CNQuote)
            Dim paymentTypes As New Config.PaymentTypes

            paymentTypes = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID).PaymentTypes

            Dim PaymentCollectionUrl As String = paymentTypes.PaymentType(Session(CNSelectedPaymentIndex)).PaymentCollectionUrl

            'set appropriate session values here to indicate payment taken and then redirect to end page
            If PaymentCollectionUrl <> "" Then
                If oQuote.InstDepositAmount > 0 AndAlso oQuote.DepositTransactasInstalment = False Then
                    Dim opayment As NexusProvider.Payment
                    opayment = CType(Session(CNPayment), NexusProvider.Payment)
                    Dim oCreditCard As New NexusProvider.CreditCardType
                    oCreditCard.Number = TxtCCNumber.Text
                    oCreditCard.ExpiryDate = ddlMonth.SelectedItem.Text.ToString().Trim() + "\" + ddlYear.SelectedItem.Text.ToString().Trim()
                    oCreditCard.AuthCode = 12345 'Temp AuthCode number need to be replaced with payment gateway return AuthCode
                    opayment.CreditCard = oCreditCard
                    Session(CNPayment) = opayment
                End If
            End If
            SetPaymentTakenAndRedirect() 'SS PN 41618
        End Sub

    End Class

End Namespace