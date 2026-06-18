Imports Microsoft.Web.Services3.Security.Tokens
Imports SAMForInsuranceV2
Imports System.Data
Partial Class CoverNote_CoverNoteBook
    Inherits System.Web.UI.Page
    Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")

    'set up the proxy object
    Dim oSAM As New SAMForInsuranceV2

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        oSAM.SetClientCredential(UserToken)
        Dim oRequest As New AddCoverNoteBookRequestType
        oSAM.SetPolicy("SamClientPolicy")
        If Not Page.IsPostBack Then
            BuildLists(oSAM, ddlBranch, STSListType.PMLookup, "Source", "")
            ddlBranch.Items.Insert(0, New ListItem("", ""))
            BuildLists(oSAM, ddlCoverNoteStatus, STSListType.PMLookup, "cover_note_book_status", "")
            ddlCoverNoteStatus.SelectedIndex = 2
            BuildListbox(oSAM, lstAllProducts, STSListType.PMLookup, "Product", "")
            lblcreateddate.Text = Date.Now()
            oRequest.AgentKey = Session("SelectedAgentKey")
            Dim ds As New DataSet
            Dim dt As New DataTable
            dt.Columns.Add("1", GetType(System.Int16))
            dt.Rows.Add(0)
            ds.Tables.Add(dt)
            gvCoverSheet.DataSource = ds
            gvCoverSheet.DataBind()
        End If

    End Sub

    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        

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
        'lstSelectedProducts.Items.Remove(lstSelectedProducts.SelectedItem)
        lstAllProducts.Items.Add(lstSelectedProducts.SelectedItem)
        lstSelectedProducts.Items.RemoveAt(lstSelectedProducts.SelectedIndex)
        If lstAllProducts.Items.Count > 0 Then
            lstAllProducts.SelectedItem.Selected = False
        End If
    End Sub

    Protected Sub btnSingleAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSingleAdd.Click
        'Dim blnflag As Boolean = False
        'For i As Integer = 0 To lstSelectedProducts.Items.Count - 1
        '    If lstSelectedProducts.Items(i).Text = lstAllProducts.SelectedItem.Text Then
        '        blnflag = True
        '        Exit For
        '    End If
        'Next
        'If Not blnflag Then lstSelectedProducts.Items.Add(lstAllProducts.SelectedItem)


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

    Protected Sub RemoveAllProducts_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RemoveAllProducts.Click
        lstSelectedProducts.Items.Clear()
        BuildListbox(oSAM, lstAllProducts, STSListType.PMLookup, "Product", "")
    End Sub

    

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        Response.Redirect("")
    End Sub

    Protected Sub Btnapply_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Btnapply.Click
        '  Response.Write("<script>window.open('Covernote maintenance.aspx');</script>")
        Dim oRequest As New AddCoverNoteBookRequestType
        Dim oResponse As AddCoverNoteBookResponseType
        Dim nLookupError As String = "102"
        Dim nBusinessError As String = "224"

        ' Add the Party/Quote/Risk and update the risk
        Try

            With oRequest
                .BranchCode = "HEADOFF"
                .BookNumber = txtBookNumber.Text
                If (Not txtStartNumber.Text.Equals(String.Empty)) Then
                    .StartNumber = Convert.ToInt32(txtStartNumber.Text)
                End If
                If (Not txtendnumber.Text.Equals(String.Empty)) Then
                    .EndNumber = Convert.ToInt32(txtendnumber.Text)
                End If
                If (Not txteffectivedate.Text.Equals(String.Empty)) Then
                    .EffectiveDate = CDate(txteffectivedate.Text)
                End If

                .CoverNoteStatusCode = ddlCoverNoteStatus.SelectedItem.Value
                If (Not txtAgent.Text.Equals(String.Empty)) Then
                    .AgentKey = Convert.ToInt32(hfAgentKey.Value)
                    .AgentKeySpecified = True
                    Session("AgentKey") = Convert.ToInt32(hfAgentKey.Value)
                End If


                ReDim .CoverNoteProducts(0 To lstSelectedProducts.Items.Count - 1)
                For i As Integer = 0 To lstSelectedProducts.Items.Count - 1
                    oRequest.CoverNoteProducts(i) = New BaseCoverNoteBookTypeRow()
                    oRequest.CoverNoteProducts(i).ProductCode = lstSelectedProducts.Items(i).Value

                Next
                If Not ddlBranch.SelectedItem.Value = "" Then
                    .CoverNoteBranchCode = ddlBranch.SelectedItem.Value
                Else
                    .CoverNoteBranchCode = ""
                End If


                'ReDim oRequest.CoverNoteProducts(0 To 1)
                'oRequest.CoverNoteProducts(0) = New BaseCoverNoteBookTypeRow

                'oRequest.CoverNoteProducts(0).ProductCode = "MOBILE"
                'oRequest.CoverNoteProducts(1) = New BaseCoverNoteBookTypeRow
                'oRequest.CoverNoteProducts(1).ProductCode = "RETAILER"

            End With


            oResponse = oSAM.AddCoverNoteBook(oRequest)
            With oResponse
                If Not (.Errors) Is Nothing Then
                    'errors returned, so throw an exception
                    Throw New SamResponseException(.Errors)
                Else
                    Dim oFindCoverNoteBooksRequestType As New FindCoverNoteBooksRequestType
                    Dim oFindCoverNoteBooksResponseType As New FindCoverNoteBooksResponseType


                    With oFindCoverNoteBooksRequestType
                        .BranchCode = "HeadOff"
                        .BookNumber = txtBookNumber.Text
                        oFindCoverNoteBooksResponseType = oSAM.FindCoverNoteBooks(oFindCoverNoteBooksRequestType)
                        Session("CoverNoteBookKey") = oFindCoverNoteBooksResponseType.FindCoverNoteBooks(0).CoverNoteBookKey

                        '  Response.Redirect("Covernote maintenance.aspx")
                    End With

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

        Response.Redirect("Covernote maintenance.aspx")

    End Sub

    Protected Sub btncancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btncancel.Click
        Response.Redirect("FindCoverNoteBook.aspx")
    End Sub
End Class
