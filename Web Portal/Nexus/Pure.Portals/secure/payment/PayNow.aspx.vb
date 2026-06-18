Imports Nexus.Library
Imports CMS.Library
Imports Nexus.Constants.Constant
Imports Nexus.Constants.Session

Namespace Nexus

    Partial Class secure_payment_PayNow
        Inherits BasePayment

        Protected Shadows Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            If Session(CNPaid) = True Then
                SetPaymentTakenAndRedirect()
            End If
            If Not IsPostBack Then
                If Session(CNCashListItem) Is Nothing Then
                    DisplayControls()
                ElseIf Session(CNQuoteCollectionFiles) IsNot Nothing Then
                    DisplayControls("CashListItem")
                Else
                    DisplayControls(Session(CNCashListItem))
                End If
            End If
        End Sub

        Public Sub DisableControls(ByVal enableOption As Boolean)
            PayNow_CashList.Visible = enableOption
            PayNow_CashListItem.Visible = enableOption
        End Sub

        Public Sub DisplayControls(Optional ByVal controls As String = Nothing)

            DisableControls(False)

            Select Case controls
                Case "CashList"
                    PayNow_CashList.Visible = True
                Case "CashListItem"
                    PayNow_CashListItem.Visible = True
                Case Else
                    PayNow_CashList.Visible = True
            End Select
        End Sub
    End Class
End Namespace