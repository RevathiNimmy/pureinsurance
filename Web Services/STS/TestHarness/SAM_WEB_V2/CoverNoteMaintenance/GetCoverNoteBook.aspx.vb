Imports Microsoft.Web.Services3.Security.Tokens
Imports SAMForInsuranceV2
Partial Class CoverNote_CoverNoteBook
    Inherits System.Web.UI.Page
    Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")
    'set up the proxy object
    Dim oSAM As New SAMForInsuranceV2

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        If Not Page.IsPostBack Then
            'Session("CoverNoteBookKey") = 3
            oSAM.SetClientCredential(UserToken)
            Dim oRequest As New GetCoverNoteBookRequestType
            Dim oResponse As GetCoverNoteBookResponseType
            oSAM.SetPolicy("SamClientPolicy")
            If Not Page.IsPostBack Then
                BuildLists(oSAM, ddlBranch, STSListType.PMLookup, "Source", "")

                BuildLists(oSAM, ddlCoverNoteStatus, STSListType.PMLookup, "cover_note_book_status", "")

                'BuildListbox(oSAM, lstAllProducts, STSListType.PMLookup, "Product", "")

                oRequest.BranchCode = "HeadOff"
                oRequest.CoverNoteBookKey = Session("CoverNoteBookKey")

                oResponse = oSAM.GetCoverNoteBook(oRequest)
                Session("Timestamp") = oResponse.CoverNoteBookTimestamp
                Session("AgentKey") = oResponse.AgentKey
                If Not (oResponse.Errors) Is Nothing Then
                    'errors returned, so throw an exception
                    Throw New SamResponseException(oResponse.Errors)
                Else
                    Try


                        With oResponse
                            txtBookNumber.Text = .BookNumber
                            txtStartNumber.Text = .StartNumber
                            txtendnumber.Text = .EndNumber
                            If .EffectiveDateSpecified Then
                                txteffectivedate.Text = .EffectiveDate
                            End If

                            txtAgent.Text = .AgentName
                            If Not .CoverNoteBranchCode = "" Then
                                ddlBranch.Items.FindByValue(.CoverNoteBranchCode.Trim()).Selected = True
                            Else
                                ddlBranch.Items.Insert(0, New ListItem("", ""))
                            End If


                            ddlCoverNoteStatus.Items.FindByValue(.CoverNoteBookStatusCode.Trim()).Selected = True
                            lblcreateddate.Text = .DateCreated

                            gvCoverSheet.DataSource = .CoverNoteSheets
                            gvCoverSheet.DataBind()


                            For i As Integer = 0 To .CoverNoteBookProducts.Length - 1
                                If .CoverNoteBookProducts(i).Chosen = 0 Then
                                    Dim liall As New ListItem
                                    liall.Text = .CoverNoteBookProducts(i).Description
                                    liall.Value = .CoverNoteBookProducts(i).ProductCode
                                    lstAllProducts.Items.Add(liall)
                                Else
                                    Dim lichoosen As New ListItem
                                    lichoosen.Text = .CoverNoteBookProducts(i).Description
                                    lichoosen.Value = .CoverNoteBookProducts(i).ProductCode
                                    lstSelectedProducts.Items.Add(lichoosen)
                                End If
                            Next


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
                End If

            End If



        End If


    End Sub

    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        'Dim oRequest As New AddCoverNoteBookRequestType
        'Dim oResponse As AddCoverNoteBookResponseType
        'Dim nLookupError As String = "102"
        'Dim nBusinessError As String = "224"

        '' Add the Party/Quote/Risk and update the risk
        'Try
        '    With oRequest
        '        .BranchCode = "HEADOFF"
        '        .BookNumber = txtBookNumber.Text
        '        If (Not txtStartNumber.Text.Equals(String.Empty)) Then
        '            .StartNumber = Convert.ToInt32(txtStartNumber.Text)
        '        End If
        '        If (Not txtendnumber.Text.Equals(String.Empty)) Then
        '            .EndNumber = Convert.ToInt32(txtendnumber.Text)
        '        End If
        '        If (Not txteffectivedate.Text.Equals(String.Empty)) Then
        '            .EffectiveDate = CDate(txteffectivedate.Text)
        '        End If

        '        .CoverNoteStatusCode = ddlCoverNoteStatus.SelectedItem.Value
        '        If (Not txtAgent.Text.Equals(String.Empty)) Then
        '            .AgentKey = Convert.ToInt32(hfAgentKey.Value)
        '            .AgentKeySpecified = True
        '        End If


        '        ReDim .CoverNoteProducts(0 To lstSelectedProducts.Items.Count - 1)
        '        For i As Integer = 0 To lstSelectedProducts.Items.Count - 1
        '            oRequest.CoverNoteProducts(i) = New BaseCoverNoteBookTypeRow()
        '            oRequest.CoverNoteProducts(i).ProductCode = lstSelectedProducts.Items(i).Value

        '        Next
        '        .CoverNoteBranchCode = ddlBranch.SelectedItem.Value

        '        'ReDim oRequest.CoverNoteProducts(0 To 1)
        '        'oRequest.CoverNoteProducts(0) = New BaseCoverNoteBookTypeRow

        '        'oRequest.CoverNoteProducts(0).ProductCode = "MOBILE"
        '        'oRequest.CoverNoteProducts(1) = New BaseCoverNoteBookTypeRow
        '        'oRequest.CoverNoteProducts(1).ProductCode = "RETAILER"

        '    End With


        '    oResponse = oSAM.AddCoverNoteBook(oRequest)
        '    With oResponse
        '        If Not (.Errors) Is Nothing Then
        '            'errors returned, so throw an exception
        '            Throw New SamResponseException(.Errors)
        '        Else

        '        End If

        '    End With
        'Catch os As SamResponseException
        '    'should do some error handling here. Just output error for now
        '    Response.Write("An error occured calling SAM:<br>" & os.Message)

        'Catch oe As Exception
        '    'should do some error handling here. Just output error for now
        '    Response.Write("An error occured:<br>" & oe.Message)

        'Finally
        '    'clean up any objects here
        'End Try

    End Sub
    Private Sub BuildLists(ByVal oSAM As SAMForInsuranceV2, ByRef objControl As DropDownList, ByVal ESTSLookup As STSListType, ByVal ListCode As String, ByVal BindValue As String)
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
                    objControl.DataSource = oResponse.List
                    If (BindValue = "") Then
                        objControl.Items.Insert(0, New ListItem("", ""))
                    Else
                        objControl.SelectedValue = BindValue
                    End If
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


    Protected Sub BtnRemoveTask_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnRemoveTask.Click

        lstAllProducts.Items.Add(lstSelectedProducts.SelectedItem)
        lstSelectedProducts.Items.RemoveAt(lstSelectedProducts.SelectedIndex)
        If lstAllProducts.Items.Count > 0 Then
            lstAllProducts.SelectedItem.Selected = False
        End If

    End Sub

    Protected Sub btnSingleAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSingleAdd.Click
        lstSelectedProducts.Items.Add(lstAllProducts.SelectedItem)
        lstAllProducts.Items.RemoveAt(lstAllProducts.SelectedIndex)
        If lstSelectedProducts.Items.Count > 0 Then
            lstSelectedProducts.SelectedItem.Selected = False
        End If
       
    End Sub

  

    Protected Sub btnFindAgent_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFindAgent.Click

    End Sub

    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        UserToken = GetUserToken("sirius", "sirius")
        oSAM.SetClientCredential(UserToken)
        oSAM.SetPolicy("SamClientPolicy")
        Dim oRequest As New DeleteCoverNoteSheetRequestType
        Dim oResponse As New DeleteCoverNoteSheetResponseType
        oRequest.BranchCode = "HeadOff"
        oRequest.CoverNoteBookKey = Session("CoverNoteBookKey")
        oRequest.CoverNoteBookTimestamp = Session("Timestamp")
        oRequest.CoverNoteSheetKey = Convert.ToInt32(gvCoverSheet.Rows(gvCoverSheet.SelectedRow.RowIndex).Cells(8).Text)
        oResponse = oSAM.DeleteCoverNoteSheet(oRequest)
        Session("Timestamp") = oResponse.CoverNoteBookTimestamp
        Response.Redirect("GetCoverNoteBook.aspx")

    End Sub

    Protected Sub gvCoverSheet_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvCoverSheet.SelectedIndexChanged
        
    End Sub


    Protected Sub btnaddcovernotesheet_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnaddcovernotesheet.Click
        Response.Redirect("covernotesheet.aspx")
        'UserToken = GetUserToken("sirius", "sirius")
        'oSAM.SetClientCredential(UserToken)
        'oSAM.SetPolicy("SamClientPolicy")
        'Dim oRequest As New AddCoverNoteSheetRequestType
        'Dim oResponse As New AddCoverNoteSheetResponseType
        'oRequest.BranchCode = "HeadOff"
        'oRequest.CoverNoteBookKey = Session("CoverNoteBookKey")
        'oRequest.CoverNoteBookTimestamp = Session("Timestamp") 'Session("BookTimestamp")
        'oRequest.CoverNoteStatusCode = "NOTISS"
        'oRequest.Comments = "iiiiwh"
        'oRequest.CoverNoteSheetNumber = 0 'Convert.ToInt32(gvCoverSheet.Rows(gvCoverSheet.SelectedRow.RowIndex).Cells(1).Text)
        'oResponse = oSAM.AddCoverNoteSheet(oRequest)
        ''Session("BookTimestamp") = oResponse.CoverNoteBookTimestamp
        'Response.Redirect("GetCoverNoteBook.aspx")
    End Sub

    Protected Sub btnEdit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEdit.Click
        Session("CoverNoteSheetKey") = Convert.ToInt32(gvCoverSheet.Rows(gvCoverSheet.SelectedRow.RowIndex).Cells(1).Text)

        Response.Redirect("CoverNoteSheetEdit.aspx")
    End Sub

   
    Private Sub getcovernotebook()

        Dim oRequest As New GetCoverNoteBookRequestType
        Dim oResponse As GetCoverNoteBookResponseType

        oRequest.BranchCode = "HeadOff"
        oRequest.CoverNoteBookKey = Session("CoverNoteBookKey")

        oResponse = oSAM.GetCoverNoteBook(oRequest)

        If Not (oResponse.Errors) Is Nothing Then
            'errors returned, so throw an exception
            Throw New SamResponseException(oResponse.Errors)
        Else



            Try
                With oResponse
                    txtBookNumber.Text = .BookNumber
                    txtStartNumber.Text = .StartNumber
                    txtendnumber.Text = .EndNumber
                    If .EffectiveDateSpecified Then
                        txteffectivedate.Text = .EffectiveDate
                    End If

                    txtAgent.Text = .AgentName
                    hfAgentKey.Value = .AgentKey

                    If Not .CoverNoteBranchCode = "" Then
                        ddlBranch.Items.FindByValue(.CoverNoteBranchCode.Trim()).Selected = True
                    Else
                        ddlBranch.Items.Insert(0, New ListItem("", ""))
                    End If
                   
                    ' ddlBranch.Items.FindByValue(.CoverNoteBranchCode.Trim()).Selected = True
                    ddlCoverNoteStatus.Items.FindByValue(.CoverNoteBookStatusCode.Trim()).Selected = True
                    lblcreateddate.Text = .DateCreated

                    gvCoverSheet.DataSource = .CoverNoteSheets
                    gvCoverSheet.DataBind()

                    lstSelectedProducts.Items.Clear()
                    lstAllProducts.Items.Clear()
                    For i As Integer = 0 To .CoverNoteBookProducts.Length - 1
                        If .CoverNoteBookProducts(i).Chosen = 0 Then
                            Dim liall As New ListItem
                            liall.Text = .CoverNoteBookProducts(i).Description
                            liall.Value = .CoverNoteBookProducts(i).ProductCode
                            lstAllProducts.Items.Add(liall)
                        Else

                            Dim lichoosen As New ListItem
                            lichoosen.Text = .CoverNoteBookProducts(i).Description
                            lichoosen.Value = .CoverNoteBookProducts(i).ProductCode
                            lstSelectedProducts.Items.Add(lichoosen)
                        End If
                    Next
                    Session("TimeStamp") = .CoverNoteBookTimestamp
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
        End If
    End Sub

    Protected Sub BtnOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnOk.Click
        oSAM.SetClientCredential(UserToken)
        Dim oRequest As New UpdateCoverNoteBookRequestType
        Dim oResponse As UpdateCoverNoteBookResponseType
        oSAM.SetPolicy("SamClientPolicy")
        oRequest.BranchCode = "HeadOff"
        If (Not txtAgent.Text.Equals(String.Empty)) Then

            If Session("AgentKey") = Nothing AndAlso hfAgentKey.Value <> "" Then
                oRequest.AgentKey = Convert.ToInt32(hfAgentKey.Value)
                oRequest.AgentKeySpecified = True
            ElseIf Session("AgentKey") = Nothing AndAlso hfAgentKey.Value = "" Then
                oRequest.AgentKey = 0
                oRequest.AgentKeySpecified = False
            ElseIf Session("AgentKey") IsNot Nothing AndAlso hfAgentKey.Value <> "" Then
                oRequest.AgentKey = Convert.ToInt32(hfAgentKey.Value)
                oRequest.AgentKeySpecified = True
            Else
                oRequest.AgentKey = Session("AgentKey")
                oRequest.AgentKeySpecified = True
            End If
            


        End If
        oRequest.CoverNoteBookKey = Session("CoverNoteBookKey")
        oRequest.CoverNoteBookTimestamp = Session("TimeStamp")
        If Not ddlBranch.SelectedItem.Value = "" Then
            oRequest.CoverNoteBranchCode = ddlBranch.SelectedValue.ToString()
        Else
            oRequest.CoverNoteBranchCode = ""
        End If



        oRequest.CoverNoteStatusCode = ddlCoverNoteStatus.SelectedValue.ToString()
        oRequest.EffectiveDate = Convert.ToDateTime(txteffectivedate.Text)


        'Dim o As BaseCoverNoteBookType



        For i As Integer = 0 To lstSelectedProducts.Items.Count - 1
            ReDim Preserve oRequest.CoverNoteProducts(lstSelectedProducts.Items.Count - 1)
            oRequest.CoverNoteProducts(i) = New BaseCoverNoteBookTypeRow
            oRequest.CoverNoteProducts(i).ProductCode = lstSelectedProducts.Items(i).Value.ToString()
        Next
        Session("TimeStamp") = oRequest.CoverNoteBookTimestamp



        oResponse = oSAM.UpdateCoverNoteBook(oRequest)

        If Not (oResponse.Errors) Is Nothing Then
            'errors returned, so throw an exception
            Throw New SamResponseException(oResponse.Errors)
        Else

            getcovernotebook()

            Try



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

    Protected Sub BtnAddAllProducts_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnAddAllProducts.Click
        lstSelectedProducts.Items.Clear()
        BuildListbox(oSAM, lstAllProducts, STSListType.PMLookup, "Product", "")
    End Sub

    Protected Sub RemoveAllProducts_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RemoveAllProducts.Click
        lstAllProducts.Items.Clear()
        BuildListbox(oSAM, lstSelectedProducts, STSListType.PMLookup, "Product", "")
    End Sub
End Class
