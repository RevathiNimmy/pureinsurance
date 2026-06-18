Imports Microsoft.Web.Services3.Security.Tokens
Imports SAMForInsuranceV2
Partial Class Temp_covennotes_CoverNoteSheetEdit
    Inherits System.Web.UI.Page
    Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")

    'set up the proxy object
    Dim oSAM As New SAMForInsuranceV2

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        UserToken = GetUserToken("sirius", "sirius")
        oSAM.SetClientCredential(UserToken)
        oSAM.SetPolicy("SamClientPolicy")
        Try
            If Not IsPostBack Then
                BuildLists(oSAM, ddlSheetStatus, STSListType.PMLookup, "cover_note_book_status", "")
                ddlSheetStatus.Items.RemoveAt(1)


                Dim oRequest As New GetCoverNoteSheetRequestType
                Dim oResponse As New GetCoverNoteSheetResponseType
                oRequest.BranchCode = "HeadOff"
                oRequest.CoverNoteBookKey = Session("CoverNoteBookKey")
                oRequest.CoverNoteSheetNumber = Session("CoverNoteSheetKey")
                oResponse = oSAM.GetCoverNoteSheet(oRequest)
                'oResponse = oSAM.UpdateCoverNoteSheet(oRequest)
                With oResponse
                    If Not (.Errors) Is Nothing Then
                        'errors returned, so throw an exception
                        Throw New SamResponseException(.Errors)
                    Else
                        Session("CoverNoteBookTimestamp") = oResponse.CoverNoteBookTimestamp
                        Session("Insurancecnt") = oResponse.InsuranceFileCnt
                        'Session("Insurancecntspecified") = oResponse.InsuranceFileCntSpecified
                        txtSheetNumber.Text = oResponse.CoverNoteSheetNumber
                        If (oResponse.InsuranceRef <> "") Then
                            txtPolicyNumber.Text = oResponse.InsuranceRef


                        Else
                            txtPolicyNumber.Text = ""
                            txtPolicyNumber.Enabled = False
                            txtnewnumber.Enabled = False
                            txtoldnumber.Enabled = False


                        End If



                        txtAssignedDate.Text = Date.Now()
                        txtAssignedDate.Enabled = False
                        If Not String.IsNullOrEmpty(oResponse.Code) Then
                            ddlSheetStatus.Items.FindByValue(oResponse.Code).Selected = True
                        End If
                        txtComments.Text = oResponse.Comments
                        'Response.Write("<script>window.open('Covernote maintenance.aspx');</script>")
                        'Session("TimeStamp") = oResponse.CoverNoteBookTimestamp
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

    End Sub

 

    Protected Sub btnEdit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEdit.Click
        UserToken = GetUserToken("sirius", "sirius")
        oSAM.SetClientCredential(UserToken)
        oSAM.SetPolicy("SamClientPolicy")
        'Dim oRequest1 As New GetCoverNoteSheetRequestType
        'Dim oResponse1 As New GetCoverNoteSheetResponseType
        'oRequest1.BranchCode = "HeadOff"
        'oRequest1.CoverNoteBookKey = Session("CoverNoteBookKey")
        'oRequest1.CoverNoteSheetNumber = Session("CoverNoteSheetKey")
        'oResponse1 = oSAM.GetCoverNoteSheet(oRequest1)
        Try
            Dim oRequest As New UpdateCoverNoteSheetRequestType
            Dim oResponse As New UpdateCoverNoteSheetResponseType
            oRequest.BranchCode = "HeadOff"
            oRequest.CoverNoteBookKey = Session("CoverNoteBookKey")
            oRequest.Comments = txtComments.Text
            oRequest.CoverNoteBookTimestamp = Session("TimeStamp")
            oRequest.CoverNoteStatusCode = ddlSheetStatus.SelectedValue.ToString()
            If (txtPolicyNumber.Text <> "") Then
                oRequest.InsuranceFileCnt = Session("Insurancecnt")
                oRequest.InsuranceFileCntSpecified = True
                oRequest.NewCoverNoteSheetNumber = Convert.ToInt32(IIf(txtnewnumber.Text <> "", txtnewnumber.Text, Nothing))
                oRequest.OldCoverNoteSheetNumber = Convert.ToInt32(IIf(txtoldnumber.Text <> "", txtoldnumber.Text, Nothing))

            Else
                oRequest.NewCoverNoteSheetNumber = Session("CoverNoteSheetKey")
                oRequest.OldCoverNoteSheetNumber = Session("CoverNoteSheetKey")
                oRequest.InsuranceFileCnt = Nothing
                oRequest.InsuranceFileCntSpecified = False
            End If
            oRequest.AssignedDate = Date.Now()
            oRequest.CoverNoteBookTimestamp = Session("CoverNoteBookTimestamp")
            oResponse = oSAM.UpdateCoverNoteSheet(oRequest)
            With oResponse
                If Not (.Errors) Is Nothing Then
                    'errors returned, so throw an exception
                    Throw New SamResponseException(.Errors)
                Else

                    Session("TimeStamp") = oResponse.CoverNoteBookTimestamp
                    Response.Redirect("GetCoverNoteBook.aspx")
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

    Protected Sub Btncancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Btncancel.Click
        Response.Redirect("GetCoverNoteBook.aspx")
    End Sub
End Class
