Imports SAMForInsuranceV2
Imports Microsoft.Web.Services3.Security.Tokens
Imports System.Data

'Created by : Jai Prakash Gupta
'Dated : 15/03/2010
Partial Class FindCashDepositAccount 
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.ClientScript.RegisterStartupScript(GetType(String), "OnClientKeyTextChanged", "OnClientKeyTextChanged();", True)
        Me.ClientScript.RegisterStartupScript(GetType(String), "OnAgentKeyTextChanged", "OnAgentKeyTextChanged();", True)
        If (Not IsPostBack) Then
            Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")
            Dim oSAM As New SAMForInsuranceV2
            oSAM.SetClientCredential(UserToken)
            oSAM.SetPolicy("SamClientPolicy")

            BuildLists(oSAM, ddlBankName, STSListType.PMLookup, "CashListItem_Bank", "")
            BtnEdit.Enabled = False

            'check if a cash deposit record added/edited 
            Try
                Dim oFindCashDepositRequestType As New FindCashDepositRequestType
                Dim oFindCashDepositResponseType As FindCashDepositResponseType

                If Session("FindCashDepositRequest") IsNot Nothing Then
                    oFindCashDepositRequestType = Session("FindCashDepositRequest")

                    With oFindCashDepositRequestType
                        If Session("CashDepositPartyType") = "Client" Then
                            txtClient.Text = .PartyCode
                            txtAgent.Text = ""
                        Else
                            txtAgent.Text = .PartyCode
                            txtClient.Text = ""
                        End If
                        ddlBankName.SelectedValue = .BankCode
                        txtCashDepositNumber.Text = .CashDepositRef

                    End With
                    oFindCashDepositResponseType = oSAM.FindCashDeposit(oFindCashDepositRequestType)
                    If (oFindCashDepositResponseType.Errors) Is Nothing Then
                        gvFindCashDeposit.DataSource = oFindCashDepositResponseType.CashDeposit
                        gvFindCashDeposit.DataBind()
                        Session("CashDepositDetails") = oFindCashDepositResponseType.CashDeposit
                    Else
                        Throw New SamResponseException(oFindCashDepositResponseType.Errors)
                    End If
                ElseIf Session("PartyKey") IsNot Nothing Then
                    oFindCashDepositRequestType.BranchCode = "HEADOFF"
                    oFindCashDepositRequestType.PartyCode = Session("PartyKey")
                    oFindCashDepositResponseType = oSAM.FindCashDeposit(oFindCashDepositRequestType)
                    If (oFindCashDepositResponseType.Errors) Is Nothing Then
                        gvFindCashDeposit.DataSource = oFindCashDepositResponseType.CashDeposit
                        gvFindCashDeposit.DataBind()
                        Session("CashDepositDetails") = oFindCashDepositResponseType.CashDeposit
                    Else
                        Throw New SamResponseException(oFindCashDepositResponseType.Errors)
                    End If
                End If
                Catch os As SamResponseException
                Response.Write("An error occured calling SAM:<br>" & os.Message)

            Catch oe As Exception
                Response.Write("An error occured:<br>" & oe.Message)
            End Try
        End If

    End Sub

    Protected Sub btnFind_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFind.Click
        Dim strPartycode As String
        Dim oSAM As New SAMForInsuranceV2
        Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")
        oSAM.SetClientCredential(UserToken)
        oSAM.SetPolicy("SamClientPolicy")
        Dim oFindCashDepositRequestType As New FindCashDepositRequestType
        Dim oFindCashDepositResponseType As New FindCashDepositResponseType
        Try

            If txtClient.Text.Trim <> "" Then
                strPartycode = txtClient.Text.Trim
                Session("CashDepositPartyType") = "Client"
            Else
                strPartycode = txtAgent.Text.Trim
                Session("CashDepositPartyType") = "Agent"
            End If

            With oFindCashDepositRequestType
                .BranchCode = "HeadOff"
                If (ddlBankName.SelectedIndex > 0) Then
                    .BankCode = ddlBankName.SelectedValue
                End If
                .PartyCode = strPartycode
                .CashDepositRef = txtCashDepositNumber.Text.Trim

            End With

            oFindCashDepositResponseType = oSAM.FindCashDeposit(oFindCashDepositRequestType)
            If (oFindCashDepositResponseType.Errors) Is Nothing Then
                gvFindCashDeposit.DataSource = oFindCashDepositResponseType.CashDeposit
                gvFindCashDeposit.DataBind()
                'jp
                Session("FindCashDepositRequest") = oFindCashDepositRequestType
                Session("CashDepositPartyName") = oFindCashDepositResponseType.CashDeposit(0).PartyName
                Session("CashDepositDetails") = oFindCashDepositResponseType.CashDeposit
            Else
                Throw New SamResponseException(oFindCashDepositResponseType.Errors)
            End If
        Catch os As SamResponseException
            Response.Write("An error occured calling SAM:<br>" & os.Message)

        Catch oe As Exception
            Response.Write("An error occured:<br>" & oe.Message)
        End Try
    End Sub

    Private Sub BuildLists(ByVal oSAM As SAMForInsuranceV2, ByRef objControl As DropDownList, ByVal ESTSLookup As STSListType, ByVal ListCode As String, ByVal BindValue As String)
        Dim oRequest As New GetListRequestType
        Dim oResponse As New GetListResponseType
        oRequest.BranchCode = "HeadOff"
        oRequest.ListType = STSListType.PMLookup
        oRequest.ListCode = ListCode

        Try
            oResponse = oSAM.GetList(oRequest)
            With oResponse
                If Not (.Errors) Is Nothing Then
                    'errors returned, so throw an exception
                    Throw New SamResponseException(.Errors)
                Else
                    objControl.DataSource = oResponse.List
                    objControl.DataTextField = "Description"
                    objControl.DataValueField = "code"
                    objControl.DataBind()
                    objControl.Items.Insert(0, "")
                    If (BindValue <> "") Then
                        objControl.SelectedValue = BindValue
                    End If
                End If
            End With

        Catch os As SamResponseException
            Response.Write("An error occured calling SAM:<br>" & os.Message)

        Catch oe As Exception
            Response.Write("An error occured:<br>" & oe.Message)
        End Try
    End Sub




    Protected Sub BtnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnAdd.Click
        Session("CashDepositKey") = 0
        If txtClient.Text.Trim <> "" Then
            Session("PartyKey") = txtClient.Text.Trim
            Session("CashDepositPartyType") = "Client"
        ElseIf txtAgent.Text.Trim <> "" Then
            Session("PartyKey") = txtClient.Text.Trim
            Session("CashDepositPartyType") = "Agent"
        End If
        
        Response.Redirect("CashDepositAccountSetup.aspx")
    End Sub

    Protected Sub gvFindCashDeposit_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvFindCashDeposit.SelectedIndexChanged
        If gvFindCashDeposit.SelectedRow IsNot Nothing Then
            BtnEdit.Enabled = True
        Else
            BtnEdit.Enabled = False
        End If
        Session("CashDepositRef") = DirectCast(Session("CashDepositDetails"), BaseCashDepositItemType())(gvFindCashDeposit.SelectedIndex).CashDepositRef
    End Sub

    Protected Sub BtnClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnClose.Click
        Response.Write("<script>self.close();</script>")
    End Sub

    Protected Sub BtnEdit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnEdit.Click
        If txtClient.Text.Trim <> "" Then
            Session("PartyKey") = txtClient.Text.Trim
            Session("CashDepositPartyType") = "Client"
        ElseIf txtAgent.Text.Trim <> "" Then
            Session("PartyKey") = txtClient.Text.Trim
            Session("CashDepositPartyType") = "Agent"
        End If
        Response.Redirect("CashDepositAccountSetup.aspx")
    End Sub

    Protected Sub BtnNewsearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnNewsearch.Click
        gvFindCashDeposit.DataSource = Nothing
        gvFindCashDeposit.DataBind()
        txtAgent.Text = ""
        txtCashDepositNumber.Text = ""
        txtClient.Text = ""
        ddlBankName.SelectedIndex = 0
    End Sub

End Class
