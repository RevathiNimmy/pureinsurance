Imports Microsoft.Web.Services3.Security.Tokens
Imports SAMForInsuranceV2
Imports System.Data

Partial Class MTA_ListPolicyVersions
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If (Not IsPostBack) Then

            Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")

            'set up the proxy object
            Dim oSAM As New SAMForInsuranceV2
            oSAM.SetClientCredential(UserToken)
            oSAM.SetPolicy("SamClientPolicy")

            Dim oGetAllPolicyVersionsRequest As New GetAllPolicyVersionsRequestType
            Dim oGetAllPolicyVersionsResponseType As GetAllPolicyVersionsResponseType

            With oGetAllPolicyVersionsRequest
                .BranchCode = "Headoff"
                .InsuranceFolderKey = Session("InsuranceFolderKey")
            End With

            Try
                oGetAllPolicyVersionsResponseType = oSAM.GetAllPolicyVersions(oGetAllPolicyVersionsRequest)

                With oGetAllPolicyVersionsResponseType
                    If Not (.Errors) Is Nothing Then
                        'errors returned, so throw an exception
                        Throw New SamResponseException(.Errors)
                    Else
                        txtClientCode.Text = oGetAllPolicyVersionsResponseType.Policies(0).PartyShortName
                        txtPolicy.Text = oGetAllPolicyVersionsResponseType.Policies(0).PolicyRef
                        gvResult.DataSource = oGetAllPolicyVersionsResponseType.Policies
                        gvResult.DataBind()
                    End If
                End With
            Catch os As SamResponseException
                'should do some error handling here. Just output error for now
                Response.Write("An error occured calling SAM:<br>" & os.Message)
            Catch oe As Exception
                'should do some error handling here. Just output error for now
                Response.Write("An error occured:<br>" & oe.Message)
            Finally
                'clean
            End Try
        End If
    End Sub

    Protected Sub gvResult_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvResult.RowCommand
        If (e.CommandName.Equals("Select")) Then
            Session("INSURANCEFILEKEY") = e.CommandArgument
        End If
    End Sub

    Protected Sub gvResult_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvResult.SelectedIndexChanged
        Session("COVERFROM") = gvResult.SelectedRow.Cells(4).Text
        Session("COVERTO") = gvResult.SelectedRow.Cells(5).Text
        If (gvResult.SelectedRow.Cells(2).Text = "Live") Then
            Session("PROCESS") = "MTA"
            Response.Redirect("MTACaptureDate.aspx")
        Else
            Session("PROCESS") = "NONMTA"
            Response.Redirect("PolicyHeader.aspx")
        End If

    End Sub

    Protected Sub btnRisks_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRisks.Click

        If gvResult.Rows.Count = 1 Then

            Dim hd As HiddenField = Nothing

            hd = DirectCast(gvResult.Rows(0).FindControl("hd"), HiddenField)

            Session("INSURANCEFILEKEY") = hd.Value

        End If

        Response.Redirect("GetListRisks-Risk.aspx")
    End Sub
End Class
