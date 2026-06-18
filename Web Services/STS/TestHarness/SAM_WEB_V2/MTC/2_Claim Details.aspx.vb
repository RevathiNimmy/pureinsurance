Imports Microsoft.Web.Services3.Security.Tokens
Imports SAMForInsuranceV2
Partial Class Maintain_Claim_Default
    Inherits System.Web.UI.Page

    Protected Sub gvPaymenyHistory_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvPaymenyHistory.SelectedIndexChanged

    End Sub

    Protected Sub Menu1_MenuItemClick(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.MenuEventArgs) Handles Menu1.MenuItemClick
        mvClaimDetails.ActiveViewIndex = Int32.Parse(e.Item.Value)
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load





        Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")

        'set up the proxy object
        Dim oSAM As New SAMForInsuranceV2
        oSAM.SetClientCredential(UserToken)
        oSAM.SetPolicy("SamClientPolicy")


        Dim oRequest As New GetListRequestType
        Dim oResponse As New GetListResponseType


        'Saurabh -- Start DataBind to Dropdowns
        '''Binding Claim Handler Drop down - 
        If Not Page.IsPostBack Then


            oRequest.BranchCode = "HeadOff"
            oRequest.ListType = STSListType.PMLookup
            oRequest.ListCode = "Handler"
            Try
                oResponse = oSAM.GetList(oRequest)

                With oResponse
                    If Not (.Errors) Is Nothing Then
                        'errors returned, so throw an exception
                        Response.Write(GetMessageFromSamError(.Errors))
                    Else
                        ddlClaimHandler.DataSource = oResponse.List
                        ddlClaimHandler.DataTextField = "Description"
                        ddlClaimHandler.DataValueField = "Code"
                        ddlClaimHandler.DataBind()
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



            '''Binding Progres Status Drop down - 
            oRequest.BranchCode = "HeadOff"
            oRequest.ListType = STSListType.PMLookup
            oRequest.ListCode = "Progress_status"
            Try
                oResponse = oSAM.GetList(oRequest)

                With oResponse
                    If Not (.Errors) Is Nothing Then
                        'errors returned, so throw an exception
                        Response.Write(GetMessageFromSamError(.Errors))
                    Else
                        ddlProgressStatus.DataSource = oResponse.List
                        ddlProgressStatus.DataTextField = "Description"
                        ddlProgressStatus.DataValueField = "Code"
                        ddlProgressStatus.DataBind()
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

            '''Binding PrimaryCause Drop down - 
            oRequest.BranchCode = "HeadOff"
            oRequest.ListType = STSListType.PMLookup
            oRequest.ListCode = "primary_cause"
            Try
                oResponse = oSAM.GetList(oRequest)

                With oResponse
                    If Not (.Errors) Is Nothing Then
                        'errors returned, so throw an exception
                        Response.Write(GetMessageFromSamError(.Errors))
                    Else
                        ddlPrimaryCause.DataSource = oResponse.List
                        ddlPrimaryCause.DataTextField = "Description"
                        ddlPrimaryCause.DataValueField = "Code"
                        ddlPrimaryCause.DataBind()
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

            '''Binding SecondaryCause Drop down - 
            oRequest.BranchCode = "HeadOff"
            oRequest.ListType = STSListType.PMLookup
            oRequest.ListCode = "Secondary_cause"
            Try
                oResponse = oSAM.GetList(oRequest)

                With oResponse
                    If Not (.Errors) Is Nothing Then
                        'errors returned, so throw an exception
                        Response.Write(GetMessageFromSamError(.Errors))
                    Else
                        ddlSecondary.DataSource = oResponse.List
                        ddlSecondary.DataTextField = "Description"
                        ddlSecondary.DataValueField = "Code"
                        ddlSecondary.DataBind()
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


            '''Binding Catastrophe code  Drop down - 
            oRequest.BranchCode = "HeadOff"
            oRequest.ListType = STSListType.PMLookup
            oRequest.ListCode = "Catastrophe_code"
            Try
                oResponse = oSAM.GetList(oRequest)

                With oResponse
                    If Not (.Errors) Is Nothing Then
                        'errors returned, so throw an exception
                        Response.Write(GetMessageFromSamError(.Errors))
                    Else
                        ddlCatastrophe.DataSource = oResponse.List
                        ddlCatastrophe.DataTextField = "Description"
                        ddlCatastrophe.DataValueField = "Code"
                        ddlCatastrophe.DataBind()
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

            '''Binding Town  Drop down - 
            oRequest.BranchCode = "HeadOff"
            oRequest.ListType = STSListType.PMLookup
            oRequest.ListCode = "Town"
            Try
                oResponse = oSAM.GetList(oRequest)

                With oResponse
                    If Not (.Errors) Is Nothing Then
                        'errors returned, so throw an exception
                        Response.Write(GetMessageFromSamError(.Errors))
                    Else
                        ddlTown.DataSource = oResponse.List
                        ddlTown.DataTextField = "Description"
                        ddlTown.DataValueField = "Code"
                        ddlTown.DataBind()
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





            '''Binding Currency  Drop down - 
            oRequest.BranchCode = "HeadOff"
            oRequest.ListType = STSListType.PMLookup
            oRequest.ListCode = "Currency"
            Try
                oResponse = oSAM.GetList(oRequest)

                With oResponse
                    If Not (.Errors) Is Nothing Then
                        'errors returned, so throw an exception
                        Response.Write(GetMessageFromSamError(.Errors))
                    Else
                        ddlCurrency.DataSource = oResponse.List
                        ddlCurrency.DataTextField = "Description"
                        ddlCurrency.DataValueField = "Code"
                        ddlCurrency.DataBind()
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




            Dim oGetClaimDetailsRequest As New GetClaimDetailsRequestType
            Dim oGetClaimDetailsResponse As New GetClaimDetailsResponseType

            With oGetClaimDetailsRequest
                .BranchCode = "HeadOff"
                .ClaimKey = (DirectCast(Session("SelectedClaim"), BaseFindClaimResponseTypeRow)).ClaimKey
            End With

            Try
                oGetClaimDetailsResponse = oSAM.GetClaimDetails(oGetClaimDetailsRequest)
                Session("TimeStamp") = oGetClaimDetailsResponse.TimeStamp

                ''''Binding Risk Type Combo Box

                Dim oGetHeaderAndSummariesByKeyRequest As New GetHeaderAndSummariesByKeyRequestType
                Dim oGetHeaderAndSummariesByKeyResponse As New GetHeaderAndSummariesByKeyResponseType



                With oGetHeaderAndSummariesByKeyRequest
                    .BranchCode = "HeadOff"
                    .InsuranceFileKey = (DirectCast(Session("SelectedClaim"), BaseFindClaimResponseTypeRow)).InsuranceFileKey
                End With
                Try
                    oGetHeaderAndSummariesByKeyResponse = oSAM.GetHeaderAndSummariesByKey(oGetHeaderAndSummariesByKeyRequest)
                    With oGetHeaderAndSummariesByKeyResponse
                        If Not (.Errors) Is Nothing Then
                            'errors returned, so throw an exception
                            Response.Write(GetMessageFromSamError(.Errors))
                        Else
                            ddlRiskType.DataSource = .Risks
                            ddlRiskType.DataTextField = "Description"
                            ddlRiskType.DataValueField = "RiskKey"
                            ddlRiskType.DataBind()
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

                Session("GetClaimDetailsResponse") = oGetClaimDetailsResponse
                With oGetClaimDetailsResponse
                    If Not (.Errors) Is Nothing Then
                        'errors returned, so throw an exception
                        Response.Write(GetMessageFromSamError(.Errors))
                    Else
                        Session("GetClaimDetailsResponseType") = oGetClaimDetailsResponse
                        txtClaimNumber.Text = .ClaimDetails.ClaimDetails.ClaimNumber

                        ddlClaimHandler.SelectedIndex = ddlClaimHandler.Items.IndexOf(ddlClaimHandler.Items.FindByValue(.ClaimDetails.ClaimDetails.HandlerCode.Trim()))
                        ddlProgressStatus.SelectedIndex = ddlProgressStatus.Items.IndexOf(ddlProgressStatus.Items.FindByValue(.ClaimDetails.ClaimDetails.ProgressStatusCode.Trim()))
                        txtDescription.Text = .ClaimDetails.ClaimDetails.Description
                        ddlPrimaryCause.SelectedIndex = ddlPrimaryCause.Items.IndexOf(ddlPrimaryCause.Items.FindByValue(.ClaimDetails.ClaimDetails.PrimaryCauseCode.Trim()))
                        ' ddlSecondary.SelectedIndex = ddlSecondary.Items.IndexOf(ddlSecondary.Items.FindByValue(.ClaimDetails.ClaimDetails.SecondaryCauseCode.Trim()))
                        If Not .ClaimDetails.ClaimDetails.CatastropheCode = Nothing Then
                            ddlCatastrophe.SelectedIndex = ddlCatastrophe.Items.IndexOf(ddlCatastrophe.Items.FindByValue(.ClaimDetails.ClaimDetails.CatastropheCode.Trim()))
                        End If

                        If Not .ClaimDetails.ClaimDetails.TownCode = Nothing Then
                            ddlTown.SelectedIndex = ddlTown.Items.IndexOf(ddlTown.Items.FindByValue(.ClaimDetails.ClaimDetails.TownCode.Trim()))
                        End If

                        txtLocation.Text = .ClaimDetails.ClaimDetails.Location
                        txtLossDate.Text = .ClaimDetails.ClaimDetails.LossFromDate.ToString()
                        If .ClaimDetails.ClaimDetails.LossToDateSpecified Then
                            txtLossToDate.Text = .ClaimDetails.ClaimDetails.LossToDate.Date.ToString()
                        End If

                        txtReportedDate.Text = .ClaimDetails.ClaimDetails.ReportedDate.Date.ToString()
                        ddlRiskType.SelectedIndex = ddlRiskType.Items.IndexOf(ddlRiskType.Items.FindByValue(.ClaimDetails.ClaimDetails.RiskKey.ToString()))
                        ddlCurrency.SelectedIndex = ddlCurrency.Items.IndexOf(ddlCurrency.Items.FindByValue(.ClaimDetails.ClaimDetails.CurrencyCode.ToString()))



                        Dim oPaymentDetails As BaseGetClaimPaymentDetailsType()

                        Dim TotalPayment As Integer
                        TotalPayment = 0
                        For PerilCount As Integer = 0 To .ClaimDetails.ClaimPeril.Length - 1
                            TotalPayment = TotalPayment + .ClaimDetails.ClaimPeril(PerilCount).ClaimPayments.Length
                        Next

                        ReDim Preserve oPaymentDetails(TotalPayment - 1)
                        TotalPayment = 0
                        For Count As Integer = 0 To .ClaimDetails.ClaimPeril.Length - 1

                            For PaymentCount As Integer = 0 To .ClaimDetails.ClaimPeril(Count).ClaimPayments.Length - 1
                                oPaymentDetails(TotalPayment) = New BaseGetClaimPaymentDetailsType
                                oPaymentDetails(TotalPayment) = .ClaimDetails.ClaimPeril(Count).ClaimPayments(PaymentCount)
                                TotalPayment = TotalPayment + 1
                            Next
                        Next

                        gvPaymenyHistory.DataSource = oPaymentDetails
                        gvPaymenyHistory.DataBind()

                    End If


                End With
            Catch ex As Exception

            End Try


        End If




    End Sub

    Protected Sub btnOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOk.Click

        Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")

        'set up the proxy object
        Dim oSAM As New SAMForInsuranceV2
        oSAM.SetClientCredential(UserToken)
        oSAM.SetPolicy("SamClientPolicy")

        Dim oMantainClaimRequestType As New MaintainClaimRequestType

        Dim oMaintainClaimSelectedClaim As New BaseFindClaimResponseTypeRow
        oMaintainClaimSelectedClaim = DirectCast(Session("SelectedClaim"), BaseFindClaimResponseTypeRow)

        ''''Getting Risk Perils

        Dim oGetClaimRiskLinksRequest As New GetClaimRiskLinksRequestType
        Dim oGetClaimRiskLinksResponse As New GetClaimRiskLinksResponseType

        With oGetClaimRiskLinksRequest
            .BranchCode = "HeadOff"
            .InsuranceFileKey = oMaintainClaimSelectedClaim.InsuranceFileKey
            .RiskKey = ddlRiskType.SelectedValue
        End With


        Try
            oGetClaimRiskLinksResponse = oSAM.GetClaimRiskLinks(oGetClaimRiskLinksRequest)
            With oGetClaimRiskLinksResponse
                If Not (.Errors) Is Nothing Then
                    'errors returned, so throw an exception
                    Response.Write(GetMessageFromSamError(.Errors))
                Else

                    '           ''''Mapping Risk Perils to OpenClientRequestType


                    Dim oPerilType As BaseClaimPerilMaintainType()
                    ReDim Preserve oPerilType(oGetClaimRiskLinksResponse.PerilType.Length - 1)

                    For Count As Integer = 0 To oGetClaimRiskLinksResponse.PerilType.Length - 1


                        With oMantainClaimRequestType
                            oPerilType(Count) = New BaseClaimPerilMaintainType
                            oPerilType(Count).TypeCode = oGetClaimRiskLinksResponse.PerilType(Count).Code

                            Dim oReserve As BaseClaimPerilReserveType()
                            ReDim Preserve oReserve(oGetClaimRiskLinksResponse.PerilType(Count).ReserveType.Length - 1)

                            For ReserveCount As Integer = 0 To oGetClaimRiskLinksResponse.PerilType(Count).ReserveType.Length - 1
                                oReserve(ReserveCount) = New BaseClaimPerilReserveType
                                oReserve(ReserveCount).TypeCode = oGetClaimRiskLinksResponse.PerilType(Count).ReserveType(ReserveCount).Code
                            Next
                            oPerilType(Count).Reserve = oReserve

                            Dim oRecovery As BaseClaimPerilRecoveryType()
                            ReDim Preserve oRecovery(oGetClaimRiskLinksResponse.PerilType(Count).RecoveryType.Length - 1)

                            For RecoveryCount As Integer = 0 To oGetClaimRiskLinksResponse.PerilType(Count).RecoveryType.Length - 1
                                oRecovery(RecoveryCount) = New BaseClaimPerilRecoveryType
                                oRecovery(RecoveryCount).TypeCode = oGetClaimRiskLinksResponse.PerilType(Count).RecoveryType(RecoveryCount).Code
                                oPerilType(Count).Recovery = oRecovery

                            Next
                            oPerilType(Count).Description = oGetClaimRiskLinksResponse.PerilType(Count).Description
                            Dim oMaintainClaim As New BaseClaimMaintainType
                            oMaintainClaim.ClaimPeril = oPerilType
                            .Claim = oMaintainClaim
                        End With

                    Next

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


        With oMantainClaimRequestType
            .BranchCode = "HeadOff"

            .Claim.BaseClaimKey = DirectCast(Session("GetClaimDetailsResponse"), GetClaimDetailsResponseType).ClaimDetails.ClaimDetails.BaseClaimKey

            '.Claim.InsuranceFileKey = oOpenClaimSelectedClaim.InsuranceFileKey
            .Claim.HandlerCode = ddlClaimHandler.SelectedValue
            .Claim.ProgressStatusCode = ddlProgressStatus.SelectedValue
            .Claim.Description = txtDescription.Text
            .Claim.PrimaryCauseCode = ddlPrimaryCause.SelectedValue
            ''.Claim.SecondaryCauseCode = ddlSecondary.SelectedValue
            .Claim.CatastropheCode = ddlCatastrophe.SelectedValue
            .Claim.TownCode = ddlTown.SelectedValue
            .Claim.Location = txtLocation.Text
            .Claim.LossFromDate = txtLossDate.Text

            If txtLossToDate.Text <> "" Then
                .Claim.LossToDate = txtLossToDate.Text
                .Claim.LossToDateSpecified = True
            Else
                .Claim.LossToDateSpecified = False
            End If
            .Claim.ReportedDate = txtReportedDate.Text
            '.Claim.CurrencyCode = ddlCurrency.SelectedValue
            '.Claim.RiskKey = ddlRiskType.SelectedValue
            '.Claim.Client.Address.AddressLine1 = txtAddress.Text


        End With

        Session("MaintainClaimRequestType") = oMantainClaimRequestType
        Response.Redirect("3_Perils.aspx")


    End Sub
End Class
