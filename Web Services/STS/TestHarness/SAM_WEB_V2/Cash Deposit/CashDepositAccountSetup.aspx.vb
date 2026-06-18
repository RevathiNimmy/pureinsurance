Imports Microsoft.Web.Services3.Security.Tokens
Imports SAMForInsuranceV2
Imports System.Data
Partial Class CashDeposit_CashDepositAccountSetup
    Inherits System.Web.UI.Page
    Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")

    'set up the proxy object
    Dim oSAM As New SAMForInsuranceV2

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        oSAM.SetClientCredential(UserToken)
        Dim oRequest As New AddCashDepositRequestType
        oSAM.SetPolicy("SamClientPolicy")
        If Not Page.IsPostBack Then

            BuildListbox(oSAM, lstAllBranches, STSListType.PMLookup, "Source", "")
            BuildListbox(oSAM, lstAllProducts, STSListType.PMLookup, "Product", "")

            Try
                If Session("CashDepositKey") <= 0 Then
                    Dim oImpRequest As New GetNextCashDepositRefRequestType
                    Dim oImpResponse As New GetNextCashDepositRefResponseType

                    oImpRequest.BranchCode = "HeadOff"
                    oImpRequest.PartyCode = Session("PartyKey")

                    oImpResponse = oSAM.GetNextCashDepositRef(oImpRequest)
                    If Not (oImpResponse.Errors) Is Nothing Then
                        'errors returned, so throw an exception
                        Throw New SamResponseException(oImpResponse.Errors)
                    Else
                        txtCDNumber.Text = oImpResponse.CashDepositRef
                        Session("TimeStamp") = oImpResponse.CDTimeStamp
                    End If

                Else
                    Dim oImpRequest As New GetCashDepositRequestType
                    Dim oImpResponse As New GetCashDepositResponseType
                    Dim oProduct As BaseGetCashDepositResponseTypeProducts
                    Dim oBranch As BaseGetCashDepositResponseTypeBranches
                    Dim oListItem As ListItem

                    oImpRequest.BranchCode = "HeadOff"
                    oImpRequest.CashDepositRef = Session("CashDepositRef")
                    If Session("FindCashDepositRequest") IsNot Nothing Then
                        oImpRequest.PartyCode = Session("FindCashDepositRequest").PartyCode
                    Else
                        oImpRequest.PartyCode = Session("PartyKey")
                    End If

                    oImpResponse = oSAM.GetCashDeposit(oImpRequest)

                    With oImpResponse
                        If Not (.Errors) Is Nothing Then
                            'errors returned, so throw an exception
                            Throw New SamResponseException(.Errors)
                        Else
                            txtCDNumber.Text = .CashDepositKey
                            ChkSinglePolicyLock.Checked = .IsSinglePolicy
                            Session("TimeStamp") = .CDTimeStamp

                            For Each oProduct In oImpResponse.Products
                                lstAllProducts.Items.Remove(oProduct.Description)
                                oListItem = New ListItem(oProduct.Description, oProduct.ProductCode)
                                'oListItem.Attributes.Add("ProductKey", oProduct.ProductKey)
                                lstSelectedProducts.Items.Add(oListItem)
                            Next
                            If lstSelectedProducts.Items.Count > 0 Then
                                lstSelectedProducts.SelectedItem.Selected = False
                            End If

                            For Each oBranch In oImpResponse.Branches
                                lstAllBranches.Items.Remove(oBranch.Description)
                                oListItem = New ListItem(oBranch.Description, oBranch.BranchCode)
                                'oListItem.Attributes.Add("BranchKey", oBranch.BranchKey)
                                lstSelectedBranches.Items.Add(oListItem)
                            Next
                            If lstSelectedProducts.Items.Count > 0 Then
                                lstSelectedProducts.SelectedItem.Selected = False
                            End If
                        End If
                    End With
                End If

            Catch os As SamResponseException
                'should do some error handling here. Just output error for now
                Response.Write("An error occured calling SAM:<br>" & os.Message)

            Catch oe As Exception
                'should do some error handling here. Just output error for now
                Response.Write("An error occured:<br>" & oe.Message)

            Finally
                'clean up any objects here
            End Try
        End If
    End Sub

    Private Sub BuildListbox(ByVal oSAM As SAMForInsuranceV2, ByRef objControl As ListBox, ByVal ESTSLookup As STSListType, ByVal ListCode As String, ByVal BindValue As String)
        Dim oRequest As New GetListRequestType
        Dim oResponse As New GetListResponseType


        oRequest.BranchCode = "HeadOff"
        oRequest.ListType = ESTSLookup
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
                    objControl.DataValueField = "Code"
                    objControl.DataBind()

                End If
                If ListCode = "source" Then
                    Session("Branch") = oResponse.List
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

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Response.Redirect("FindCashDepositAccount.aspx")
    End Sub

    Protected Sub BtnApply_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnApply.Click
        ' Add/Update the CashDeposit 
        Try
            If lstSelectedBranches.Items.Count <= 0 Then
                Response.Write("Please select atleast one branch")
                Exit Sub
            ElseIf lstSelectedProducts.Items.Count <= 0 Then
                Response.Write("Please select atleast one Product")
                Exit Sub
            End If
            If Session("CashDepositKey") <= 0 Then
                ' Add the CashDeposit 
                Dim oRequest As New AddCashDepositRequestType
                Dim oResponse As AddCashDepositResponseType
                Dim i As Integer
                Dim objBranch As BaseCommonCashDepositItemTypeBranches
                Dim objProduct As BaseCommonCashDepositItemTypeProducts

                With oRequest
                    .BranchCode = "HEADOFF"
                    ReDim .CashDeposit(0)
                    .CashDeposit(0) = New BaseCommonCashDepositItemType
                    .CashDeposit(0).UserName = "sirius"
                    .CashDeposit(0).CashDepositRef = txtCDNumber.Text.Trim
                    .CashDeposit(0).CDTimeStamp = Session("TimeStamp")
                    .CashDeposit(0).IsSinglePolicy = ChkSinglePolicyLock.Checked
                    .CashDeposit(0).IsDeleted = 0
                    .CashDeposit(0).PartyCode = Session("PartyKey")

                    If Session("CashDepositPartyType") = "Agent" Then
                        .CashDeposit(0).PartyType = ClientAgentType.A
                    Else
                        .CashDeposit(0).PartyType = ClientAgentType.C
                    End If

                    ReDim .CashDeposit(0).Products(0 To lstSelectedProducts.Items.Count - 1)
                    For i = 0 To lstSelectedProducts.Items.Count - 1
                        objProduct = New BaseCommonCashDepositItemTypeProducts
                        objProduct.ProductCode = lstSelectedProducts.Items(i).Value
                        objProduct.Description = lstSelectedProducts.Items(i).Text.Trim
                        .CashDeposit(0).Products(i) = objProduct
                    Next

                    ReDim .CashDeposit(0).Branches(0 To lstSelectedBranches.Items.Count - 1)
                    For i = 0 To lstSelectedBranches.Items.Count - 1
                        objBranch = New BaseCommonCashDepositItemTypeBranches
                        objBranch.BranchCode = lstSelectedBranches.Items(i).Value
                        objBranch.Description = lstSelectedBranches.Items(i).Text.Trim
                        .CashDeposit(0).Branches(i) = objBranch
                    Next
                End With

                oResponse = oSAM.AddCashDeposit(oRequest)
                With oResponse
                    If Not (.Errors) Is Nothing Then
                        'errors returned, so throw an exception
                        Throw New SamResponseException(.Errors)
                    Else
                        If oResponse.CashDeposit.Length > 0 Then
                            Session("TimeStamp") = .CashDeposit(0).CDTimeStamp
                            Response.Redirect("CashDepositAccount.aspx")
                        Else
                            Response.Write("Cash deposit account could not be added")
                        End If
                    End If

                End With

            Else
                ' Update the CashDeposit 
                Dim oRequest As New UpdateCashDepositRequestType
                Dim oResponse As UpdateCashDepositResponseType
                Dim i As Integer
                Dim objBranch As BaseCommonCashDepositItemTypeBranches
                Dim objProduct As BaseCommonCashDepositItemTypeProducts

                With oRequest
                    .BranchCode = "HEADOFF"
                    ReDim .CashDeposit(0)
                    .CashDeposit(0) = New BaseCommonCashDepositItemType
                    .CashDeposit(0).UserName = "sirius"
                    .CashDeposit(0).CashDepositRef = txtCDNumber.Text.Trim
                    .CashDeposit(0).CDTimeStamp = Session("TimeStamp")
                    .CashDeposit(0).IsSinglePolicy = ChkSinglePolicyLock.Checked
                    .CashDeposit(0).IsDeleted = 0
                    .CashDeposit(0).PartyCode = Session("FindCashDepositRequest").PartyCode
                    If Session("CashDepositPartyType") = "Agent" Then
                        .CashDeposit(0).PartyType = ClientAgentType.A
                    Else
                        .CashDeposit(0).PartyType = ClientAgentType.C
                    End If

                    ReDim .CashDeposit(0).Products(0 To lstSelectedProducts.Items.Count - 1)
                    For i = 0 To lstSelectedProducts.Items.Count - 1
                        objProduct = New BaseCommonCashDepositItemTypeProducts
                        objProduct.ProductCode = lstSelectedProducts.Items(i).Value
                        objProduct.Description = lstSelectedProducts.Items(i).Text.Trim
                        .CashDeposit(0).Products(i) = objProduct
                    Next

                    ReDim .CashDeposit(0).Branches(0 To lstSelectedBranches.Items.Count - 1)
                    For i = 0 To lstSelectedBranches.Items.Count - 1
                        objBranch = New BaseCommonCashDepositItemTypeBranches
                        objBranch.BranchCode = lstSelectedBranches.Items(i).Value
                        objBranch.Description = lstSelectedBranches.Items(i).Text.Trim
                        .CashDeposit(0).Branches(i) = objBranch
                    Next
                End With

                oResponse = oSAM.UpdateCashDeposit(oRequest)
                With oResponse
                    If Not (.Errors) Is Nothing Then
                        'errors returned, so throw an exception
                        Throw New SamResponseException(.Errors)
                    Else
                        If oResponse.CashDeposit.Length > 0 Then
                            Session("TimeStamp") = .CashDeposit(0).CDTimeStamp
                            Response.Redirect("CashDepositAccount.aspx")
                        Else
                            Response.Write("Cash deposit account could not be added")
                        End If
                    End If

                End With
            End If
        Catch os As SamResponseException
            'should do some error handling here. Just output error for now
            Response.Write("An error occured calling SAM:<br>" & os.Message)

        Catch oe As Exception
            'should do some error handling here. Just output error for now
            Response.Write("An error occured:<br>" & oe.Message)
        End Try
    End Sub

#Region "Handlers for Product and Branch Buttons"

    Protected Sub BtnRemoveTaskProd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnRemoveTaskProd.Click
        lstAllProducts.Items.Add(lstSelectedProducts.SelectedItem)
        lstSelectedProducts.Items.RemoveAt(lstSelectedProducts.SelectedIndex)
        If lstAllProducts.Items.Count > 0 Then
            lstAllProducts.SelectedItem.Selected = False
        End If
    End Sub

    Protected Sub btnSingleAddProd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSingleAddProd.Click
        lstSelectedProducts.Items.Add(lstAllProducts.SelectedItem)
        lstAllProducts.Items.RemoveAt(lstAllProducts.SelectedIndex)
        If lstSelectedProducts.Items.Count > 0 Then
            lstSelectedProducts.SelectedItem.Selected = False
        End If
    End Sub

    Protected Sub BtnAddAllProducts_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnAddAllProducts.Click
        lstAllProducts.Items.Clear()
        BuildListbox(oSAM, lstSelectedProducts, STSListType.PMLookup, "Product", "")
    End Sub

    Protected Sub BtnRemoveAllProducts_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnRemoveAllProducts.Click
        lstSelectedProducts.Items.Clear()
        BuildListbox(oSAM, lstAllProducts, STSListType.PMLookup, "Product", "")
    End Sub

    Protected Sub btnSingleAddBranch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSingleAddBranch.Click
        lstSelectedBranches.Items.Add(lstAllBranches.SelectedItem)
        lstAllBranches.Items.RemoveAt(lstAllBranches.SelectedIndex)
        If lstSelectedBranches.Items.Count > 0 Then
            lstSelectedBranches.SelectedItem.Selected = False
        End If
    End Sub

    Protected Sub BtnRemoveTaskBranch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnRemoveTaskBranch.Click
        lstAllBranches.Items.Add(lstSelectedBranches.SelectedItem)
        lstSelectedBranches.Items.RemoveAt(lstSelectedBranches.SelectedIndex)
        If lstAllBranches.Items.Count > 0 Then
            lstAllBranches.SelectedItem.Selected = False
        End If
    End Sub

    Protected Sub BtnAddAllBranches_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnAddAllBranches.Click
        lstAllBranches.Items.Clear()
        BuildListbox(oSAM, lstSelectedBranches, STSListType.PMLookup, "Source", "")
    End Sub

    Protected Sub BtnRemoveAllBranches_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnRemoveAllBranches.Click
        lstSelectedBranches.Items.Clear()
        BuildListbox(oSAM, lstAllBranches, STSListType.PMLookup, "Source", "")
    End Sub
#End Region
End Class
