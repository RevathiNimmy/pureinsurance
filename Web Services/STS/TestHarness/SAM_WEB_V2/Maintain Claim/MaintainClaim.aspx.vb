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

        If Not Page.IsPostBack Then

            Dim oMaintainClaimResponseType As New MaintainClaimResponseType
            Dim oMaintainClaimRequestType As New MaintainClaimRequestType

            oMaintainClaimRequestType = DirectCast(Session("MaintainClaimRequestType"), MaintainClaimRequestType)
            oPerilType = DirectCast(Session("oMaintainClaimPerils"), Peril())

            If oMaintainClaimRequestType.Claim.InfoOnly Then
                oMaintainClaimRequestType.Claim.ClaimPeril = Nothing
            Else
                If oPerilType IsNot Nothing Then
                    ReDim Preserve oMaintainClaimRequestType.Claim.ClaimPeril(oPerilType.Length - 1)
                    For PerilCount As Integer = 0 To oPerilType.Length - 1
                        Dim oBaseClaimPeril As New BaseClaimPerilMaintainType
                        oBaseClaimPeril.Description = oPerilType(PerilCount).Description
                        oBaseClaimPeril.TypeCode = oPerilType(PerilCount).PerilTypeCode
                        oBaseClaimPeril.BaseClaimPerilKey = oPerilType(PerilCount).BaseClaimPerilKey
                        If oPerilType(PerilCount).BaseClaimPerilKey = 0 Then
                            oBaseClaimPeril.BaseClaimPerilKeySpecified = False
                        Else
                            oBaseClaimPeril.BaseClaimPerilKeySpecified = True
                        End If

                        If oPerilType(PerilCount).Reserves IsNot Nothing Then
                            ReDim Preserve oBaseClaimPeril.Reserve(oPerilType(PerilCount).Reserves.Length - 1)
                            For ReserveCount As Integer = 0 To oPerilType(PerilCount).Reserves.Length - 1
                                oBaseClaimPeril.Reserve(ReserveCount) = New BaseClaimPerilReserveType
                                oBaseClaimPeril.Reserve(ReserveCount).RevisionAmount = oPerilType(PerilCount).Reserves(ReserveCount).ThisRevision
                                oBaseClaimPeril.Reserve(ReserveCount).TypeCode = oPerilType(PerilCount).Reserves(ReserveCount).TypeCode
                            Next
                        End If

                        If oPerilType(PerilCount).Recoveries IsNot Nothing Then
                            For RecoveryCount As Integer = 0 To oPerilType(PerilCount).Recoveries.Length - 1
                                If Not oPerilType(PerilCount).Recoveries(RecoveryCount).ThisRevision = 0 Then
                                    If oBaseClaimPeril.Recovery IsNot Nothing Then
                                        ReDim Preserve oBaseClaimPeril.Recovery(oBaseClaimPeril.Recovery.Length)
                                    Else
                                        ReDim Preserve oBaseClaimPeril.Recovery(0)
                                    End If
                                    oBaseClaimPeril.Recovery(oBaseClaimPeril.Recovery.Length - 1) = New BaseClaimPerilRecoveryType
                                    oBaseClaimPeril.Recovery(oBaseClaimPeril.Recovery.Length - 1).RevisionAmount = oPerilType(PerilCount).Recoveries(RecoveryCount).ThisRevision
                                    oBaseClaimPeril.Recovery(oBaseClaimPeril.Recovery.Length - 1).TypeCode = oPerilType(PerilCount).Recoveries(RecoveryCount).TypeCode
                                    'JP oBaseClaimPeril.Recovery(oBaseClaimPeril.Recovery.Length - 1).RecoveryPartyTypeCode = oPerilType(PerilCount).Recoveries(RecoveryCount).PartyTypeCode
                                    'JP oBaseClaimPeril.Recovery(oBaseClaimPeril.Recovery.Length - 1).RecoveryPartyCode = oPerilType(PerilCount).Recoveries(RecoveryCount).PartyCode
                                End If
                            Next
                        End If
                        oMaintainClaimRequestType.Claim.ClaimPeril(PerilCount) = New BaseClaimPerilMaintainType
                        oMaintainClaimRequestType.Claim.ClaimPeril(PerilCount) = oBaseClaimPeril

                        'oMaintainClaimRequestType.Claim.Client()
                    Next
                End If
                oMaintainClaimRequestType.TimeStamp = Session("TimeStamp")
                Session("MaintainClaimRequestType") = oMaintainClaimRequestType
            End If

            Try
                'Commented by PraveenGora
                'oMaintainClaimRequestType.Claim.Client.Address.CountryCode = "INDIA"
                 StartDate = Date.Now
                oMaintainClaimResponseType = oSAM.MaintainClaim(oMaintainClaimRequestType)
                WriteToLog(Session, "MaintainClaim.aspx", "SAMForInsuranceV2", "MaintainClaim", StartDate, Date.Now)
                With oMaintainClaimResponseType
                    If Not (.Errors) Is Nothing Then
                        'errors returned, so throw an exception
                        Throw New SamResponseException(.Errors)
                        'JP 18/02/10
                    ElseIf Not .Warnings Is Nothing Then
                        Response.Write("An warning come \incalling SAM:<br>" & .Warnings(0).Description)
                    Else
                        lblClaimNumber.Text = .ClaimNumber.ToString()
                        lblClaimKey.Text = .ClaimKey.ToString()
                        Session("ClaimKey") = .ClaimKey
                        Response.Redirect("RiskReinsuranceArrangements.aspx?id=" + .ClaimNumber + "&name=Claim")
                    End If
                End With
            Catch os As SamResponseException
                'should do some error handling here. Just output error for now
                Response.Write("An error occured \incalling SAM:<br>" & os.Message)
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

    Protected Sub btnReInsuranceBreakDown_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReInsuranceBreakDown.Click
        Response.Write("<script>window.showModalDialog('ClaimReinsuranceBreakdown.aspx');</script>")
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
            Dim StartDate As Date
            oGenerateClaimDocumentResponse = oSAM.GenerateClaimsDocuments(oGenerateClaimDocumentRequest)
            WriteToLog(Session, "MaintainClaim.aspx", "SAMForInsuranceV2", "GenerateClaimsDocuments", StartDate, Date.Now)
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

    Protected Sub chkGenerateClaimDocument_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkGenerateClaimDocument.CheckedChanged
        If chkGenerateClaimDocument.Checked Then
            btnGenerateDocument.Enabled = True
        Else
            btnGenerateDocument.Enabled = False
        End If
    End Sub
End Class
'Imports Microsoft.Web.Services3.Security.Tokens
'Imports SAMForInsuranceV2
'Partial Class OpenClaim_OpenClaim
'    Inherits System.Web.UI.Page
'    Dim UserToken As UsernameToken
'    Dim oSAM As New SAMForInsuranceV2
'    Dim oPerilType() As Peril

'    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
'        UserToken = GetUserToken("sirius", "sirius")
'        'set up the proxy object
'        oSAM.SetClientCredential(UserToken)
'        oSAM.SetPolicy("SamClientPolicy")

'        If Not Page.IsPostBack Then

'            Dim oMaintainClaimResponseType As New MaintainClaimResponseType
'            Dim oMaintainClaimRequestType As New MaintainClaimRequestType

'            oMaintainClaimRequestType = DirectCast(Session("MaintainClaimRequestType"), MaintainClaimRequestType)
'            oPerilType = DirectCast(Session("oMaintainClaimPerils"), Peril())

'            If oMaintainClaimRequestType.Claim.InfoOnly Then
'                oMaintainClaimRequestType.Claim.ClaimPeril = Nothing
'            Else
'                If oPerilType IsNot Nothing Then
'                    ReDim Preserve oMaintainClaimRequestType.Claim.ClaimPeril(oPerilType.Length - 1)
'                    For PerilCount As Integer = 0 To oPerilType.Length - 1
'                        Dim oBaseClaimPeril As New BaseClaimPerilMaintainType
'                        oBaseClaimPeril.Description = oPerilType(PerilCount).Description
'                        oBaseClaimPeril.TypeCode = oPerilType(PerilCount).PerilTypeCode
'                        oBaseClaimPeril.BaseClaimPerilKey = oPerilType(PerilCount).BaseClaimPerilKey
'                        If oPerilType(PerilCount).BaseClaimPerilKey = 0 Then
'                            oBaseClaimPeril.BaseClaimPerilKeySpecified = False
'                        Else
'                            oBaseClaimPeril.BaseClaimPerilKeySpecified = True
'                        End If

'                        If oPerilType(PerilCount).Reserves IsNot Nothing Then
'                            ReDim Preserve oBaseClaimPeril.Reserve(oPerilType(PerilCount).Reserves.Length - 1)
'                            For ReserveCount As Integer = 0 To oPerilType(PerilCount).Reserves.Length - 1
'                                oBaseClaimPeril.Reserve(ReserveCount) = New BaseClaimPerilReserveType
'                                oBaseClaimPeril.Reserve(ReserveCount).RevisionAmount = oPerilType(PerilCount).Reserves(ReserveCount).ThisRevision
'                                oBaseClaimPeril.Reserve(ReserveCount).TypeCode = oPerilType(PerilCount).Reserves(ReserveCount).TypeCode
'                            Next
'                        End If

'                        If oPerilType(PerilCount).Recoveries IsNot Nothing Then
'                            For RecoveryCount As Integer = 0 To oPerilType(PerilCount).Recoveries.Length - 1
'                                If Not oPerilType(PerilCount).Recoveries(RecoveryCount).ThisRevision = 0 Then
'                                    If oBaseClaimPeril.Recovery IsNot Nothing Then
'                                        ReDim Preserve oBaseClaimPeril.Recovery(oBaseClaimPeril.Recovery.Length)
'                                    Else
'                                        ReDim Preserve oBaseClaimPeril.Recovery(0)
'                                    End If
'                                    oBaseClaimPeril.Recovery(oBaseClaimPeril.Recovery.Length - 1) = New BaseClaimPerilRecoveryType
'                                    oBaseClaimPeril.Recovery(oBaseClaimPeril.Recovery.Length - 1).RevisionAmount = oPerilType(PerilCount).Recoveries(RecoveryCount).ThisRevision
'                                    oBaseClaimPeril.Recovery(oBaseClaimPeril.Recovery.Length - 1).TypeCode = oPerilType(PerilCount).Recoveries(RecoveryCount).TypeCode
'                                End If
'                            Next
'                        End If
'                        oMaintainClaimRequestType.Claim.ClaimPeril(PerilCount) = New BaseClaimPerilMaintainType
'                        oMaintainClaimRequestType.Claim.ClaimPeril(PerilCount) = oBaseClaimPeril
'                    Next
'                End If
'                oMaintainClaimRequestType.TimeStamp = Session("TimeStamp")
'            End If

'            Try
'                oMaintainClaimResponseType = oSAM.MaintainClaim(oMaintainClaimRequestType)

'                With oMaintainClaimResponseType
'                    If Not (.Errors) Is Nothing Then
'                        'errors returned, so throw an exception
'                        Throw New SamResponseException(.Errors)
'                    Else
'                        lblClaimNumber.Text = .ClaimNumber.ToString()
'                        lblClaimKey.Text = .ClaimKey.ToString()
'                        Session("ClaimKey") = .ClaimKey
'                    End If
'                End With
'            Catch os As SamResponseException
'                'should do some error handling here. Just output error for now
'                Response.Write("An error occured calling SAM:<br>" & os.Message)
'            Catch oe As Exception
'                'should do some error handling here. Just output error for now
'                Response.Write("An error occured:<br>" & oe.Message)
'            Finally
'                'clean
'            End Try
'        End If
'    End Sub

'    Protected Sub btnCoInsuranceBreakDown_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCoInsuranceBreakDown.Click
'        Response.Redirect("6_CoinsuranceRecoveries.aspx")
'    End Sub

'    Protected Sub btnReInsuranceBreakDown_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReInsuranceBreakDown.Click
'        Response.Write("<script>window.showModalDialog('ClaimReinsuranceBreakdown.aspx');</script>")
'    End Sub

'    Protected Sub btnGenerateDocument_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGenerateDocument.Click
'        Dim oGenerateClaimDocumentRequest As New GenerateClaimsDocumentsRequestType
'        Dim oGenerateClaimDocumentResponse As New GenerateClaimsDocumentsResponseType

'        With oGenerateClaimDocumentRequest
'            .BranchCode = "HeadOff"
'            .ClaimKey = Session("ClaimKey")
'            .Mode = 1
'            .OutputAsHTML = True
'            .ParameterXML = "1"
'            .TransactionType = "C_CP"
'        End With

'        Try
'            oGenerateClaimDocumentResponse = oSAM.GenerateClaimsDocuments(oGenerateClaimDocumentRequest)

'            With oGenerateClaimDocumentResponse
'                If Not (.Errors) Is Nothing Then
'                    'errors returned, so throw an exception
'                    Throw New SamResponseException(.Errors)
'                Else

'                End If

'            End With
'        Catch os As SamResponseException
'            'should do some error handling here. Just output error for now
'            Response.Write("An error occured calling SAM:<br>" & os.Message)
'        Catch oe As Exception
'            'should do some error handling here. Just output error for now
'            Response.Write("An error occured:<br>" & oe.Message)
'        Finally
'            'clean
'        End Try
'    End Sub

'    Protected Sub chkGenerateClaimDocument_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkGenerateClaimDocument.CheckedChanged
'        If chkGenerateClaimDocument.Checked Then
'            btnGenerateDocument.Enabled = True
'        Else
'            btnGenerateDocument.Enabled = False
'        End If
'    End Sub
'End Class
