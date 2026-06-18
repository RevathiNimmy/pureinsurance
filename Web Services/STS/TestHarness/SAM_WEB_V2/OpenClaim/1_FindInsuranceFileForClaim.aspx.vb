Imports Microsoft.Web.Services3.Security.Tokens
Imports SAMForInsuranceV2
Imports System.Collections
Partial Class OpenClaim_1_FindInsuranceFile
    Inherits System.Web.UI.Page
    Dim oFindInsurenceFileForClaimResponse As New FindInsuranceFileForClaimsResponseType
Dim StartDate As Date
    Protected Sub Button4_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click

    End Sub

    Protected Sub mnuFindInsuranceFile_MenuItemClick(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.MenuEventArgs) Handles mnuFindInsuranceFile.MenuItemClick
        mvFindInsuranceFileForClaim.ActiveViewIndex = Int32.Parse(e.Item.Value)
    End Sub

    Protected Sub btnFind_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFind.Click
        Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")

        'set up the proxy object
        Dim oSAM As New SAMForInsuranceV2
        oSAM.SetClientCredential(UserToken)
        oSAM.SetPolicy("SamClientPolicy")

        Dim oFindInsurenceFileForClaimRequest As New FindInsuranceFileForClaimsRequestType



        With oFindInsurenceFileForClaimRequest
            .BranchCode = "HeadOff"
            .InsuranceRef = txtPolicyNumber.Text.Trim()
            If txtCoverNoteSheetNumber.Text <> "" Then
                .CoverNoteSheetNumber = txtCoverNoteSheetNumber.Text
                .CoverNoteSheetNumberSpecified = True
            Else
                .CoverNoteSheetNumberSpecified = False
            End If
            .RiskIndex = txtRiskIndex.Text.Trim
            If txtClaimDate.Text <> "" Then
                .LossDate = txtClaimDate.Text
            End If

            .ClientShortName = txtShortName.Text.Trim
            .PostCode = txtPostCode.Text

            If txtInForceFrom.Text <> "" Then
                .InForceFrom = txtInForceFrom.Text
                .InForceFromSpecified = True
            Else
                .InForceFrom = Nothing
                .InForceFromSpecified = False
            End If

            If txtInForceTo.Text <> "" Then
                .InForceTo = txtInForceTo.Text
                .InForceToSpecified = True
            Else
                .InForceTo = Nothing
                .InForceToSpecified = False

            End If


        End With
        Dim oAddress As New BaseAddressType
        Dim oGetAddress As New GetAddressRequestType



        Try
             StartDate = Date.Now
            oFindInsurenceFileForClaimResponse = oSAM.FindInsuranceFileForClaims(oFindInsurenceFileForClaimRequest)
            WriteToLog(Session, "1_FindInsuranceFileForClaim.aspx", "SAMForInsuranceV2", "FindInsuranceFileForClaims", StartDate, Date.Now)

            With oFindInsurenceFileForClaimResponse
                If Not (.Errors) Is Nothing Then
                    'errors returned, so throw an exception
                    Throw New SamResponseException(.Errors)
                Else
                    gvResult.DataSource = oFindInsurenceFileForClaimResponse.InsuranceFileDetails
                    gvResult.DataBind()
                End If
                Session("FindInsurenceFileForClaimResponse") = oFindInsurenceFileForClaimResponse
            End With

            Session("ClaimDate") = txtClaimDate.Text

        Catch os As SamResponseException
            'should do some error handling here. Just output error for now
            Response.Write("An error occured calling SAM:<br>" & os.Message)
        Catch oe As Exception
            'should do some error handling here. Just output error for now
            Response.Write("An error occured:<br>" & oe.Message)
        Finally
            'clean
        End Try

    End Sub

    Protected Sub btnOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOk.Click
        If gvResult.SelectedIndex <> -1 Then
            Response.Redirect("2_Claim details.aspx")
        End If

    End Sub

    Protected Sub gvResult_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvResult.SelectedIndexChanged
        Dim oFindInsuranceFileForClaimResponse As New FindInsuranceFileForClaimsResponseType
        oFindInsuranceFileForClaimResponse = DirectCast(Session("FindInsurenceFileForClaimResponse"), FindInsuranceFileForClaimsResponseType)
        Session("OpenClaimSelectedPolicy") = oFindInsuranceFileForClaimResponse.InsuranceFileDetails(gvResult.SelectedIndex)
        Session("InsuranceFileKey") = oFindInsuranceFileForClaimResponse.InsuranceFileDetails(gvResult.SelectedIndex).InsuranceFileKey
        Session("ClientShortCode") = oFindInsuranceFileForClaimResponse.InsuranceFileDetails(gvResult.SelectedIndex).ClientShortName
        Response.Redirect("2_Claim details.aspx")
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        Me.btnFind.Focus()
    End Sub

    Protected Sub txtClaimDate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtClaimDate.TextChanged
    End Sub
End Class
