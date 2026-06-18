Imports SAMForInsuranceV2
Imports Microsoft.Web.Services3.Security.Tokens
Partial Class MTA_MTACaptureDate
    Inherits System.Web.UI.Page

    Protected Sub btnOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOK.Click
        If (IsDate(txtMTADate.Text)) Then
            If (CDate(txtMTADate.Text) >= CDate(Session("COVERFROM")) And (CDate(txtMTADate.Text) <= CDate(Session("COVERTO")))) Then
                Session("MTADATE") = txtMTADate.Text
                Session("MTAREASON") = txtMTAReason.Text
                Session("MTAISPERMANENT") = "PERMANENT"
                Response.Redirect("PolicyHeader.aspx")
            Else
                lblError.Text = "InvalidDate"
            End If
        Else
            lblError.Text = "Enter Date"
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            txtMTADate.Text = Session("COVERTO")
        End If
    End Sub

    Private Sub BuildLists(ByVal oSAM As SAMForInsuranceV2, ByRef objControl As DropDownList, ByVal ESTSLookup As STSListType, ByVal ListCode As String)
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
                    objControl.DataValueField = "Code"
                    objControl.DataBind()
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
