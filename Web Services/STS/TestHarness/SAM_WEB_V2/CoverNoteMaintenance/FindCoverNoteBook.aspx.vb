Imports SAMForInsuranceV2
Imports Microsoft.Web.Services3.Security.Tokens
Imports System.Data
Partial Class Cover_Note_Maintenance_Default
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If (Not IsPostBack) Then
            Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")
            Dim oSAM As New SAMForInsuranceV2
            oSAM.SetClientCredential(UserToken)
            oSAM.SetPolicy("SamClientPolicy")
            BuildLists(oSAM, ddlCoverNoteBranchCode, STSListType.PMLookup, "Source", "")
            BuildLists(oSAM, ddlConverNoteStatusCode, STSListType.PMLookup, "Cover_Note_Book_status", "")
            BtnEdit.Enabled = False
            txtAssignedDate.Text = Today.Date
            txtLastUpdated.Text = Today.Date


            'Dim ds As New DataSet
            'Dim dt As New DataTable
            'dt.Columns.Add("1", GetType(System.Int16))
            'dt.Rows.Add(0)
            'ds.Tables.Add(dt)
            'gvFindCoverNoteBook.DataSource = ds
            'gvFindCoverNoteBook.DataBind()

        End If
    End Sub

    Protected Sub btnFind_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFind.Click
        Dim oSAM As New SAMForInsuranceV2
        Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")
        oSAM.SetClientCredential(UserToken)
        oSAM.SetPolicy("SamClientPolicy")
        Dim oFindCoverNoteBooksRequestType As New FindCoverNoteBooksRequestType
        Dim oFindCoverNoteBooksResponseType As New FindCoverNoteBooksResponseType
        Try


            With oFindCoverNoteBooksRequestType
                .BranchCode = "HeadOff"
                .BookNumber = txtBookNumber.Text
                If (Not String.IsNullOrEmpty(txtStartNumber.Text)) Then
                    .StartNumber = Convert.ToInt32(txtStartNumber.Text)
                    .StartNumberSpecified = True
                Else
                    .StartNumberSpecified = False
                End If
                If Not (String.IsNullOrEmpty(txtEndNumber.Text)) Then
                    .EndNumber = txtEndNumber.Text
                    .EndNumberSpecified = True
                Else
                    .EndNumberSpecified = False
                End If
                If Not (String.IsNullOrEmpty(txtAgent.Text)) Then
                    .AgentKey = Convert.ToInt32(hfAgentKey.Value)
                    .AgentKeySpecified = True
                Else
                    .AgentKeySpecified = False
                End If
                If (ddlCoverNoteBranchCode.SelectedIndex > 0) Then
                    .CoverNoteBranchCode = ddlCoverNoteBranchCode.SelectedValue
                End If
                If (IsDate(txtLastUpdated.Text)) Then
                    .LastUpdated = Convert.ToDateTime(txtLastUpdated.Text)
                    .LastUpdatedSpecified = True
                Else
                    .LastUpdatedSpecified = False
                End If
                If (ddlConverNoteStatusCode.SelectedIndex > 0) Then
                    .CoverNoteStatusCode = ddlConverNoteStatusCode.SelectedValue
                End If
                .PolicyNumber = txtPolicyNumber.Text

                If (IsDate(txtAssignedDate.Text)) Then
                    .AssignedDateSpecified = True
                    .AssignedDate = txtAssignedDate.Text
                Else
                    .AssignedDateSpecified = False
                End If
            End With

            oFindCoverNoteBooksResponseType = oSAM.FindCoverNoteBooks(oFindCoverNoteBooksRequestType)
            If (oFindCoverNoteBooksResponseType.Errors) Is Nothing Then
                gvFindCoverNoteBook.DataSource = oFindCoverNoteBooksResponseType.FindCoverNoteBooks
                gvFindCoverNoteBook.DataBind()
                Session("Bookdetails") = oFindCoverNoteBooksResponseType.FindCoverNoteBooks
            Else
                Throw New SamResponseException(oFindCoverNoteBooksResponseType.Errors)
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




    Protected Sub BtnNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnNew.Click
        Response.Redirect("CoverNoteBook.aspx")
    End Sub

    Protected Sub gvFindCoverNoteBook_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvFindCoverNoteBook.SelectedIndexChanged

        BtnEdit.Enabled = True
        Session("CoverNoteBookKey") = DirectCast(Session("Bookdetails"), BaseFindCoverNoteBooksResponseTypeRow())(gvFindCoverNoteBook.SelectedIndex).CoverNoteBookKey
    End Sub

    Protected Sub BtnClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnClose.Click
        Response.Write("<script>self.close();</script>")
    End Sub

    Protected Sub BtnEdit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnEdit.Click

        Response.Redirect("GetCoverNoteBook.aspx")
    End Sub

    Protected Sub BtnNewsearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnNewsearch.Click

        'Dim ds As New DataSet
        'Dim dt As New DataTable
        'dt.Columns.Add("1", GetType(System.Int16))
        'dt.Rows.Add(0)
        'ds.Tables.Add(dt)
        gvFindCoverNoteBook.DataSource = Nothing
        gvFindCoverNoteBook.DataBind()
        txtBookNumber.Text = ""
        txtStartNumber.Text = ""
        txtEndNumber.Text = ""
        txtAgent.Text = ""
        txtAssignedDate.Text = ""
        txtLastUpdated.Text = ""
        txtPolicyNumber.Text = ""
        ddlConverNoteStatusCode.SelectedIndex = 0
        ddlCoverNoteBranchCode.SelectedIndex = 0


    End Sub
End Class
