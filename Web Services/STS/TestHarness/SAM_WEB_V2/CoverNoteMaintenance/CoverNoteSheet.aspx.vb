Imports Microsoft.Web.Services3.Security.Tokens
Imports SAMForInsuranceV2
Partial Class CoverNote_CoverNoteSheet
    Inherits System.Web.UI.Page
    Dim UserToken As UsernameToken
    Dim oSAM As New SAMForInsuranceV2

    Protected Sub btnOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOk.Click
        UserToken = GetUserToken("sirius", "sirius")
        oSAM.SetClientCredential(UserToken)
        oSAM.SetPolicy("SamClientPolicy")
        Try

            'Session("CoverNoteBookKey") = 3
            Dim oRequest As New AddCoverNoteSheetRequestType
            Dim oResponse As New AddCoverNoteSheetResponseType
            oRequest.BranchCode = "HeadOff"
            oRequest.CoverNoteBookKey = Session("CoverNoteBookKey")
            oRequest.CoverNoteSheetNumber = Convert.ToInt32(txtSheetNumber.Text)
            oRequest.CoverNoteStatusCode = ddlSheetStatus.SelectedItem.Value
            oRequest.Comments = txtComments.Text
            oRequest.CoverNoteBookTimestamp = Session("TimeStamp")
            oResponse = oSAM.AddCoverNoteSheet(oRequest)

            With oResponse
                If Not (.Errors) Is Nothing Then
                    'errors returned, so throw an exception
                    Throw New SamResponseException(.Errors)
                Else
                    Session("CoverNoteBookTimestamp") = .CoverNoteBookTimestamp
                    Response.Redirect("GetCoverNoteBook.aspx", True)

                End If

            End With


        Catch os As SamResponseException
            'should do some error handling here. Just output error for now
            Response.Write("An error occured calling SAM:<br>" & os.Message)
        Catch oe As System.Threading.ThreadAbortException


        Catch oe As Exception
            'should do some error handling here. Just output error for now
            Response.Write("An error occured:<br>" & oe.Message)

        Finally
            'clean up any objects here
        End Try

    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Response.Redirect("GetCoverNoteBook.aspx")
    End Sub

   
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        UserToken = GetUserToken("sirius", "sirius")
        oSAM.SetClientCredential(UserToken)
        oSAM.SetPolicy("SamClientPolicy")
        txtassignedate.Text = Date.Now()
        BuildLists(oSAM, ddlSheetStatus, STSListType.PMLookup, "cover_note_book_status", "")
        ddlSheetStatus.SelectedIndex = 2


        txtPolicyNumber.Text = ""
        txtPolicyNumber.Enabled = False


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
End Class
