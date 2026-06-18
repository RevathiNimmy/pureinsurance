Imports Microsoft.Web.Services3.Security.Tokens
Imports SAMForInsuranceV2
Partial Class Maintain_Claim_Default
    Inherits System.Web.UI.Page
     Dim StartDate As Date
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
                 StartDate = Date.Now
                oResponse = oSAM.GetList(oRequest)
                WriteToLog(Session, "2_Claim Details.aspx", "SAMForInsuranceV2", "GetList", StartDate, Date.Now)
                With oResponse
                    If Not (.Errors) Is Nothing Then
                        'errors returned, so throw an exception
                        Throw New SamResponseException(.Errors)
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
                StartDate = Date.Now
                oResponse = oSAM.GetList(oRequest)
                WriteToLog(Session, "2_Claim Details.aspx", "SAMForInsuranceV2", "GetList", StartDate, Date.Now)
                With oResponse
                    If Not (.Errors) Is Nothing Then
                        'errors returned, so throw an exception
                        Throw New SamResponseException(.Errors)
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

            '''Binding SecondaryCause Drop down - 
            oRequest.BranchCode = "HeadOff"
            oRequest.ListType = STSListType.PMLookup
            oRequest.ListCode = "Secondary_cause"
            Try
                StartDate = Date.Now
                oResponse = oSAM.GetList(oRequest)
                WriteToLog(Session, "2_Claim Details.aspx", "SAMForInsuranceV2", "GetList", StartDate, Date.Now)
                With oResponse
                    If Not (.Errors) Is Nothing Then
                        'errors returned, so throw an exception
                        Throw New SamResponseException(.Errors)
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
                 StartDate = Date.Now
                oResponse = oSAM.GetList(oRequest)
                WriteToLog(Session, "2_Claim Details.aspx", "SAMForInsuranceV2", "GetList", StartDate, Date.Now)
                With oResponse
                    If Not (.Errors) Is Nothing Then
                        'errors returned, so throw an exception
                        Throw New SamResponseException(.Errors)
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
                StartDate = Date.Now
                oResponse = oSAM.GetList(oRequest)
                WriteToLog(Session, "2_Claim Details.aspx", "SAMForInsuranceV2", "GetList", StartDate, Date.Now)
                With oResponse
                    If Not (.Errors) Is Nothing Then
                        'errors returned, so throw an exception
                        Throw New SamResponseException(.Errors)
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
                StartDate = Date.Now
                oResponse = oSAM.GetList(oRequest)
                WriteToLog(Session, "2_Claim Details.aspx", "SAMForInsuranceV2", "GetList", StartDate, Date.Now)
                With oResponse
                    If Not (.Errors) Is Nothing Then
                        'errors returned, so throw an exception
                        Throw New SamResponseException(.Errors)
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

            ''Binding PrimaryCause Drop down - 
            Dim oGetValidPrimaryCausesRequest As New GetValidPrimaryCausesRequestType
            Dim oGetValidPrimaryCausesResponse As New GetValidPrimaryCausesResponseType
            With oGetValidPrimaryCausesRequest
                .BranchCode = "HeadOff"
                .InsuranceFileKey = (DirectCast(Session("SelectedClaim"), BaseGetVersionsForClaimResponseTypeRow)).InsuranceFileKey
            End With

            Try
                StartDate = Date.Now
                oGetValidPrimaryCausesResponse = oSAM.GetValidPrimaryCauses(oGetValidPrimaryCausesRequest)
                WriteToLog(Session, "2_Claim Details.aspx", "SAMForInsuranceV2", "GetValidPrimaryCauses", StartDate, Date.Now)
                With oGetValidPrimaryCausesResponse
                    If Not (.Errors) Is Nothing Then
                        'errors returned, so throw an exception
                        Throw New SamResponseException(.Errors)
                    Else
                        ddlPrimaryCause.DataSource = .PrimaryCauses
                        ddlPrimaryCause.DataTextField = "Description"
                        ddlPrimaryCause.DataValueField = "Code"
                        ddlPrimaryCause.Items.Insert(0, "")
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

            Dim oGetClaimDetailsRequest As New GetClaimDetailsRequestType
            Dim oGetClaimDetailsResponse As New GetClaimDetailsResponseType

            With oGetClaimDetailsRequest
                .BranchCode = "HeadOff"
                .ClaimKey = (DirectCast(Session("SelectedClaim"), BaseGetVersionsForClaimResponseTypeRow)).ClaimKey
            End With

            Try
                  StartDate = Date.Now
                oGetClaimDetailsResponse = oSAM.GetClaimDetails(oGetClaimDetailsRequest)
                WriteToLog(Session, "2_Claim Details.aspx", "SAMForInsuranceV2", "GetClaimDetails", StartDate, Date.Now)
                Session("TimeStamp") = oGetClaimDetailsResponse.TimeStamp

                ''''Binding Risk Type Combo Box

                Dim oGetHeaderAndSummariesByKeyRequest As New GetHeaderAndSummariesByKeyRequestType
                Dim oGetHeaderAndSummariesByKeyResponse As New GetHeaderAndSummariesByKeyResponseType



                With oGetHeaderAndSummariesByKeyRequest
                    .BranchCode = "HeadOff"
                    .InsuranceFileKey = (DirectCast(Session("SelectedClaim"), BaseGetVersionsForClaimResponseTypeRow)).InsuranceFileKey
                End With
                Try
                       StartDate = Date.Now
                    oGetHeaderAndSummariesByKeyResponse = oSAM.GetHeaderAndSummariesByKey(oGetHeaderAndSummariesByKeyRequest)
                    WriteToLog(Session, "2_Claim Details.aspx", "SAMForInsuranceV2", "GetHeaderAndSummariesByKey", StartDate, Date.Now)
                    With oGetHeaderAndSummariesByKeyResponse
                        If Not (.Errors) Is Nothing Then
                            'errors returned, so throw an exception
                            Throw New SamResponseException(.Errors)
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
                        Throw New SamResponseException(.Errors)
                    Else

                        Dim oMaintainClaimSelectedClaim As New BaseGetVersionsForClaimResponseTypeRow
                        oMaintainClaimSelectedClaim = DirectCast(Session("SelectedClaim"), BaseGetVersionsForClaimResponseTypeRow)

                        txtClaimNumber.Text = oMaintainClaimSelectedClaim.claim_number


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
                        chkInformationOnly.Checked = oGetClaimDetailsResponse.ClaimDetails.ClaimDetails.InfoOnly

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

        If chkInformationOnly.Checked Then
            Response.Redirect("2_1ClaimVersions.aspx")
        Else
            Response.Redirect("4_Perils.aspx")
        End If

        
    End Sub
End Class

