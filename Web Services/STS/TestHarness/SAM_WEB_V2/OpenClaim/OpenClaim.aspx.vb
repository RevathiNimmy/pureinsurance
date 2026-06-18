Imports Microsoft.Web.Services3.Security.Tokens
Imports SAMForInsuranceV2
Partial Class OpenClaim_OpenClaim
    Inherits System.Web.UI.Page
    Dim UserToken As UsernameToken
    Dim oSAM As New SAMForInsuranceV2
    Dim oPerilType() As Peril
    Dim StartDate As Date
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        UserToken = GetUserToken("sirius", "sirius")

        'set up the proxy object
        oSAM.SetClientCredential(UserToken)
        oSAM.SetPolicy("SamClientPolicy")
        'btnCoInsuranceBreakDown.Attributes.Add("onclick", "javascript:fnOpenModal()")

        If Not Page.IsPostBack Then

            Dim oOpenClaimResponseType As New OpenClaimResponseType
            Dim oOpenClaimRequestType As New OpenClaimRequestType

            oOpenClaimRequestType = DirectCast(Session("OpenClaimRequestType"), OpenClaimRequestType)
            oPerilType = DirectCast(Session("oOpenClaimPerils"), Peril())

            If oOpenClaimRequestType.Claim.InfoOnly Then
                oOpenClaimRequestType.Claim.ClaimPeril = Nothing
            Else

                If oPerilType IsNot Nothing Then
                    ReDim Preserve oOpenClaimRequestType.Claim.ClaimPeril(oPerilType.Length - 1)
                    For PerilCount As Integer = 0 To oPerilType.Length - 1
                        Dim oBaseClaimPeril As New BaseClaimPerilType
                        oBaseClaimPeril.Description = oPerilType(PerilCount).Description
                        oBaseClaimPeril.TypeCode = oPerilType(PerilCount).PerilTypeCode


                        If oPerilType(PerilCount).Reserves IsNot Nothing Then
                            ReDim Preserve oBaseClaimPeril.Reserve(oPerilType(PerilCount).Reserves.Length - 1)
                            For ReserveCount As Integer = 0 To oPerilType(PerilCount).Reserves.Length - 1
                                oBaseClaimPeril.Reserve(ReserveCount) = New BaseClaimPerilReserveType
                                oBaseClaimPeril.Reserve(ReserveCount).RevisionAmount = oPerilType(PerilCount).Reserves(ReserveCount).InitialReserve
                                oBaseClaimPeril.Reserve(ReserveCount).TypeCode = oPerilType(PerilCount).Reserves(ReserveCount).TypeCode
                            Next

                        End If
                        If oPerilType(PerilCount).Recoveries IsNot Nothing Then
                            For RecoveryCount As Integer = 0 To oPerilType(PerilCount).Recoveries.Length - 1
                                If Not oPerilType(PerilCount).Recoveries(RecoveryCount).TotalReserve = 0 Then
                                    If oBaseClaimPeril.Recovery IsNot Nothing Then
                                        ReDim Preserve oBaseClaimPeril.Recovery(oBaseClaimPeril.Recovery.Length)
                                    Else
                                        ReDim Preserve oBaseClaimPeril.Recovery(0)
                                    End If

                                    oBaseClaimPeril.Recovery(oBaseClaimPeril.Recovery.Length - 1) = New BaseClaimPerilRecoveryType
                                    oBaseClaimPeril.Recovery(oBaseClaimPeril.Recovery.Length - 1).RevisionAmount = oPerilType(PerilCount).Recoveries(RecoveryCount).TotalReserve
                                    oBaseClaimPeril.Recovery(oBaseClaimPeril.Recovery.Length - 1).TypeCode = oPerilType(PerilCount).Recoveries(RecoveryCount).TypeCode
                                    If oPerilType(PerilCount).Recoveries(RecoveryCount).PartyCode <> "" Then
                                        'JP oBaseClaimPeril.Recovery(oBaseClaimPeril.Recovery.Length - 1).RecoveryPartyCode = oPerilType(PerilCount).Recoveries(RecoveryCount).PartyCode
                                        'JP oBaseClaimPeril.Recovery(oBaseClaimPeril.Recovery.Length - 1).RecoveryPartyTypeCode = oPerilType(PerilCount).Recoveries(RecoveryCount).PartyTypeCode
                                    End If
                                End If
                            Next

                        End If
                        oOpenClaimRequestType.Claim.ClaimPeril(PerilCount) = New BaseClaimPerilType
                        oOpenClaimRequestType.Claim.ClaimPeril(PerilCount) = oBaseClaimPeril
                    Next
                End If

                End If


            Try
                'Commented by PraveenGora
                'oOpenClaimRequestType.Claim.Client.Address.CountryCode = "INDIA"

                 StartDate = Date.Now
                oOpenClaimResponseType = oSAM.OpenClaim(oOpenClaimRequestType)
                WriteToLog(Session, "OpenClaim.aspx", "SAMForInsuranceV2", "OpenClaim", StartDate, Date.Now)

                With oOpenClaimResponseType
                    If Not (.Errors) Is Nothing Then
                        'errors returned, so throw an exception
                        Throw New SamResponseException(.Errors)
                    Else
                        lblClaimNumber.Text = .ClaimNumber.ToString()
                        ''                        lblClaimKey.Text = .ClaimKey.ToString()
                        Session("ClaimKey") = .ClaimKey

                        ' Response.Redirect("RiskReinsuranceArrangements.aspx?id=" + .ClaimNumber + "&name=Claim")

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

    Protected Sub btnCoInsuranceBreakDown_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCoInsuranceBreakDown.Click
        Response.Redirect("6_CoinsuranceRecoveries.aspx")

    End Sub

    Protected Sub chkGenerateClaimDocument_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkGenerateClaimDocument.CheckedChanged
        If chkGenerateClaimDocument.Checked Then
            btnGenerateDocument.Enabled = True
        Else
            btnGenerateDocument.Enabled = False
        End If
    End Sub

    Protected Sub btnGenerateDocument_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGenerateDocument.Click

        Dim oGenerateClaimDocumentRequest As New GenerateClaimsDocumentsRequestType
        Dim oGenerateClaimDocumentResponse As New GenerateClaimsDocumentsResponseType

        With oGenerateClaimDocumentRequest
            .BranchCode = "HeadOff"
            .ClaimKey = Session("ClaimKey")
            .Mode = 1
            .OutputAsHTML = True
            .ParameterXML = "1"
            .TransactionType = "C_CP"
        End With


        Try
             StartDate = Date.Now
            oGenerateClaimDocumentResponse = oSAM.GenerateClaimsDocuments(oGenerateClaimDocumentRequest)
            WriteToLog(Session, "OpenClaim.aspx", "SAMForInsuranceV2", "GenerateClaimsDocuments", StartDate, Date.Now)


            With oGenerateClaimDocumentResponse
                If Not (.Errors) Is Nothing Then
                    'errors returned, so throw an exception
                    Throw New SamResponseException(.Errors)
                Else

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



    End Sub

    Protected Sub btnRrinsuranceBreakdown_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRrinsuranceBreakdown.Click
        Response.Write("<script>window.showModalDialog('ClaimReinsuranceBreakdown.aspx');</script>")
    End Sub
End Class
