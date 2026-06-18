Imports SAMForInsuranceV2
Imports Microsoft.Web.Services3.Security.Tokens
Imports System.Data

'Created by : Jai Prakash Gupta
'Dated : 15/03/2010
Partial Class CashDepositAccount
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If (Not IsPostBack) Then
            Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")
            Dim oSAM As New SAMForInsuranceV2
            oSAM.SetClientCredential(UserToken)
            oSAM.SetPolicy("SamClientPolicy")

            BtnEdit.Enabled = False

            'check if a cash deposit record added/edited 
            Dim oFindCashDepositRequestType As New FindCashDepositRequestType
            Dim oFindCashDepositResponseType As New FindCashDepositResponseType

            Try

                txtPartyCode.Text = Trim(Session("PartyKey"))
                txtPartyName.Text = Session("CashDepositPartyName")
                oFindCashDepositRequestType.PartyCode = txtPartyCode.Text
                oFindCashDepositRequestType.BranchCode = "HEADOFF"

                oFindCashDepositResponseType = oSAM.FindCashDeposit(oFindCashDepositRequestType)
                If (oFindCashDepositResponseType.Errors) Is Nothing Then
                    gvCashDeposit.DataSource = oFindCashDepositResponseType.CashDeposit
                    gvCashDeposit.DataBind()
                Else
                    Throw New SamResponseException(oFindCashDepositResponseType.Errors)
                End If
            Catch os As SamResponseException
                Response.Write("An error occured calling SAM:<br>" & os.Message)

            Catch oe As Exception
                Response.Write("An error occured:<br>" & oe.Message)
            End Try
        End If
    End Sub

    Protected Sub BtnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnAdd.Click
        Response.Redirect("CashDepositAccountSetup.aspx")
        Session("CashDepositKey") = 0
    End Sub

    Protected Sub gvCashDeposit_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvCashDeposit.SelectedIndexChanged
        If gvCashDeposit.SelectedRow IsNot Nothing Then
            BtnEdit.Enabled = True
        Else
            BtnEdit.Enabled = False
        End If
        Session("CashDepositKey") = DirectCast(Session("CashDepositDetails"), BaseCashDepositItemType())(gvCashDeposit.SelectedIndex).CashDepositKey
    End Sub

    Protected Sub BtnClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnClose.Click
        Response.Redirect("FindCashDepositAccount.aspx")
    End Sub

    Protected Sub BtnEdit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnEdit.Click
        If gvCashDeposit.SelectedRow IsNot Nothing Then
            Response.Redirect("CashDepositAccountSetup.aspx")
        Else
            Me.ClientScript.RegisterClientScriptBlock(GetType(String), "OnEdited", "alert('Please select a record!');", True)
        End If
    End Sub
End Class
