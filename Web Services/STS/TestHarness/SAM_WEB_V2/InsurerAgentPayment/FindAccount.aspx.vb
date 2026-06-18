Imports Microsoft.Web.Services3.Security.Tokens
Imports SAMForInsuranceV2
Imports System.Data.SqlClient
Imports System.Collections.Generic
Imports System.Data
Partial Class findaccount
    Inherits System.Web.UI.Page

    
        
    Protected Sub btnFindAcconut_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFindAcconut.Click
        Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")
        Dim oSAM As New SAMForInsuranceV2
        oSAM.SetClientCredential(UserToken)
        oSAM.SetPolicy("SamClientPolicy")
       

        Dim oFindAccountsRequestType As New FindAccountsRequestType
        Dim oFindAccountsResponseType As New FindAccountsResponseType
        With oFindAccountsRequestType
            .BranchCode = "HeadOff"
            .AccountName = txtShortAccountName.Text
            '.AccountTypeCode = txtName.Text
            '.InsuranceRef = txtInsuranceRef.Text
            '.LedgerCode = ""

            '.PurchaseInvoiceNo = txtPurChaseInvoiceNo.Text
            '.PurchaseOrderNo = txtPurChaseOrderNo.Text
            'If .OperatorKeySpecified = True Then
            '    .OperatorKey = ddlOperator.SelectedValue
            'End If



        End With
        oFindAccountsResponseType = oSAM.FindAccounts(oFindAccountsRequestType)
        gvFindAccount.DataSource = oFindAccountsResponseType.Accounts
        gvFindAccount.DataBind()
        Session("FindAccount") = oFindAccountsResponseType
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")
        Dim oSAM As New SAMForInsuranceV2
        oSAM.SetClientCredential(UserToken)
        oSAM.SetPolicy("SamClientPolicy")

        If Not Page.IsPostBack Then
            BuildLists(oSAM, ddlOperator, STSListType.PMLookup, "pmuser", "")
        End If

    End Sub

    Private Sub BuildLists(ByVal oSAM As SAMForInsuranceV2, ByRef objControl As DropDownList, ByVal ESTSLookup As STSListType, ByVal ListCode As String, ByVal BindValue As String)
        Dim oRequest As New GetListRequestType
        Dim oResponse As New GetListResponseType


        oRequest.BranchCode = "HeadOff"
        oRequest.ListType = STSListType.PMLookup
        oRequest.ListCode = ListCode
        oRequest.ExcludeDeletedRecords = True




        Try
            Dim StartDate As Date
            StartDate = Date.Now
            oResponse = oSAM.GetList(oRequest)
            WriteToLog(Session, "InsurerAgentpayment.aspx", "SAMForInsuranceV2", "GetList", StartDate, Date.Now)
            With oResponse
                If Not (.Errors) Is Nothing Then
                    'errors returned, so throw an exception

                Else

                    objControl.DataSource = oResponse.List
                    objControl.DataTextField = "Description"
                    objControl.DataValueField = "Key"
                    objControl.DataBind()
                    If (BindValue = "") Then
                        objControl.Items.Insert(0, New ListItem("", ""))
                    Else
                        objControl.SelectedValue = BindValue
                    End If
                End If
            End With

        Catch os As SamResponseException
            'should do some error handling here. Just output error for now
            Response.Write("An error occured calling SAM:<br>" & os.Message)

        Catch oe As Exception
            'should do some error handling here. Just output error for now
            Response.Write("An error occured:<br>" & oe.Message)

        Finally
            'clean up any objects here
        End Try

    End Sub

    Protected Sub gvFindAccount_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvFindAccount.SelectedIndexChanged
        Dim oFindAccountsResponseType As New FindAccountsResponseType
        oFindAccountsResponseType = DirectCast(Session("FindAccount"), FindAccountsResponseType)
        txtShortName.Text = oFindAccountsResponseType.Accounts(gvFindAccount.SelectedIndex).ShortCode
        Session("AccountCode") = txtShortName.Text
        hfAccountKey.Value = oFindAccountsResponseType.Accounts(gvFindAccount.SelectedIndex).AccountKey
    End Sub
End Class
