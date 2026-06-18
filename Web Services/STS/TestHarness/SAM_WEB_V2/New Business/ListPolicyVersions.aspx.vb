Imports Microsoft.Web.Services3.Security.Tokens
Imports SAMForInsuranceV2
Partial Class MTA_ListPolicyVersions
    Inherits System.Web.UI.Page
    Dim StartDate As Date
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
                StartDate = Date.Now
                oGetAllPolicyVersionsResponseType = oSAM.GetAllPolicyVersions(oGetAllPolicyVersionsRequest)
                WriteToLog(Session, "ListPolicyVersions.aspx", "SAMForInsuranceV2", "GetAllPolicyVersions",StartDate, Date.Now)
                Session("GetAllPolicyVersion") = oGetAllPolicyVersionsResponseType


                With oGetAllPolicyVersionsResponseType
                    If Not (.Errors) Is Nothing Then
                        'errors returned, so throw an exception

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

        Session("SelectedPolicy") = DirectCast(Session("GetAllPolicyVersion"), GetAllPolicyVersionsResponseType).Policies(gvResult.SelectedIndex)
        Response.Redirect("PolicyHeader.aspx")

    End Sub
End Class
