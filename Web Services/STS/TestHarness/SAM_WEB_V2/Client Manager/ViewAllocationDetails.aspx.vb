Imports Microsoft.Web.Services3.Security.Tokens
Imports SAMForInsuranceV2
Imports System.Data
Partial Class MTA_ViewAllocationDetails
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Session("BranchCode") = "HeadOff"

        'Praveen
        Dim oGetAllocationDetailsRequestType As New GetAllocationDetailsRequestType
        Dim oGetAllocationDetailsResponseType As New GetAllocationDetailsResponseType
        Dim oSAM As New SAMForInsuranceV2

        Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")
        oSAM.SetClientCredential(UserToken)
        oSAM.SetPolicy("SamClientPolicy")
        With oGetAllocationDetailsRequestType
            .BranchCode = Session("BranchCode")
            .TransDetailKey = Session("TransactionDetailId")
        End With

        oGetAllocationDetailsResponseType = oSAM.GetAllocationDetails(oGetAllocationDetailsRequestType)
        If oGetAllocationDetailsResponseType.Row IsNot Nothing Then
            Dim dsFilter As New DataSet
            With oGetAllocationDetailsResponseType.Row
                If (oGetAllocationDetailsResponseType) IsNot Nothing And (oGetAllocationDetailsResponseType.Row.Length > 0) Then
                    dsFilter.Tables.Add()
                    dsFilter.Tables(0).Columns.Add("DocRef", GetType(System.String))
                    dsFilter.Tables(0).Columns.Add("TransDate", GetType(System.DateTime))
                    dsFilter.Tables(0).Columns.Add("AllocatedDate", GetType(System.DateTime))
                    dsFilter.Tables(0).Columns.Add("AllocatedAmount", GetType(System.Double))
                    dsFilter.Tables(0).Columns.Add("OriginalAmount", GetType(System.Double))
                    dsFilter.Tables(0).Columns.Add("WriteOffAmount", GetType(System.Double))
                    dsFilter.Tables(0).Columns.Add("DocType", GetType(System.String))
                    dsFilter.Tables(0).Columns.Add("InsuranceRef", GetType(System.String))
                    dsFilter.Tables(0).Columns.Add("Account", GetType(System.String))
                    dsFilter.Tables(0).Columns.Add("User", GetType(System.String))
                    Dim iCountOne As Integer
                    For iCountOne = 0 To oGetAllocationDetailsResponseType.Row.Length - 1
                        dsFilter.Tables(0).Rows.Add()
                        dsFilter.Tables(0).Rows(iCountOne)("DocRef") = oGetAllocationDetailsResponseType.Row(iCountOne).DocRef
                        dsFilter.Tables(0).Rows(iCountOne)("TransDate") = oGetAllocationDetailsResponseType.Row(iCountOne).TransDate
                        dsFilter.Tables(0).Rows(iCountOne)("AllocatedDate") = oGetAllocationDetailsResponseType.Row(iCountOne).AllocatedDate
                        dsFilter.Tables(0).Rows(iCountOne)("AllocatedAmount") = oGetAllocationDetailsResponseType.Row(iCountOne).AllocatedAmount
                        dsFilter.Tables(0).Rows(iCountOne)("OriginalAmount") = oGetAllocationDetailsResponseType.Row(iCountOne).OriginalAmount
                        dsFilter.Tables(0).Rows(iCountOne)("WriteOffAmount") = oGetAllocationDetailsResponseType.Row(iCountOne).WriteOffAmount
                        dsFilter.Tables(0).Rows(iCountOne)("DocType") = oGetAllocationDetailsResponseType.Row(iCountOne).DocType
                        dsFilter.Tables(0).Rows(iCountOne)("InsuranceRef") = oGetAllocationDetailsResponseType.Row(iCountOne).InsuranceRef
                        dsFilter.Tables(0).Rows(iCountOne)("Account") = oGetAllocationDetailsResponseType.Row(iCountOne).Account
                        dsFilter.Tables(0).Rows(iCountOne)("User") = oGetAllocationDetailsResponseType.Row(iCountOne).User
                    Next
                End If
            End With


            Dim dvCredit As New DataView(dsFilter.Tables(0))
            dvCredit.RowFilter = "AllocatedAmount<0 AND OriginalAmount<0"
            grdCredit.DataSource = dvCredit
            grdCredit.DataBind()
            lblOutput.Visible = True

            Dim dvDebit As New DataView(dsFilter.Tables(0))
            dvDebit.RowFilter = "AllocatedAmount>=0 AND OriginalAmount >= 0"
            grdDebit.DataSource = dvDebit
            grdDebit.DataBind()
            lblOutput1.Visible = True
        Else
            Label1.Visible = True
        End If
    End Sub

    Protected Sub Menu1_MenuItemClick(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.MenuEventArgs) Handles Menu1.MenuItemClick
        
    End Sub

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        Response.Redirect("FindAccountDetails.aspx")
    End Sub
End Class
