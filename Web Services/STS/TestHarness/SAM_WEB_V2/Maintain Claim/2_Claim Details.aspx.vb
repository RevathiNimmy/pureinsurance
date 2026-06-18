Imports Microsoft.Web.Services3.Security.Tokens
Imports SAMForInsuranceV2
Partial Class Maintain_Claim_Default
    Inherits System.Web.UI.Page
    Dim UserToken As UsernameToken
    Dim oSAM As New SAMForInsuranceV2
     Dim StartDate As Date
    Protected Sub gvPaymenyHistory_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvPaymenyHistory.SelectedIndexChanged

    End Sub

    Protected Sub Menu1_MenuItemClick(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.MenuEventArgs) Handles Menu1.MenuItemClick
        mvClaimDetails.ActiveViewIndex = Int32.Parse(e.Item.Value)
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        UserToken = GetUserToken("sirius", "sirius")
        'set up the proxy object
        oSAM.SetClientCredential(UserToken)
        oSAM.SetPolicy("SamClientPolicy")

        '' Binding the LookUp DropDowns
        If Not Page.IsPostBack Then
            BuildLists(oSAM, ddlClaimHandler, STSListType.PMLookup, "Handler")
            BuildLists(oSAM, ddlProgressStatus, STSListType.PMLookup, "Progress_status")

            BuildLists(oSAM, ddlSecondary, STSListType.PMLookup, "secondary_cause")
            BuildLists(oSAM, ddlCatastrophe, STSListType.PMLookup, "catastrophe_code")
            BuildLists(oSAM, ddlTown, STSListType.PMLookup, "Town")
            BuildLists(oSAM, ddlCurrency, STSListType.PMLookup, "Currency")
            BuildLists(oSAM, ddlAddressType, STSListType.PMLookup, "Address_Usage_type")
            BuildLists(oSAM, ddlCountry, STSListType.PMLookup, "Country")
            ddlSecondary.Items.Insert(0, "")

            ''Binding PrimaryCause Drop down - 
            Dim oGetValidPrimaryCausesRequest As New GetValidPrimaryCausesRequestType
            Dim oGetValidPrimaryCausesResponse As New GetValidPrimaryCausesResponseType
            With oGetValidPrimaryCausesRequest
                .BranchCode = "HeadOff"
                .InsuranceFileKey = (DirectCast(Session("SelectedClaim"), BaseFindClaimResponseTypeRow)).InsuranceFileKey
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

            '' Binding Risk Type  Drop down 
            'Praveen - Issue No.SAMB10014
            'txtClaimStatusDate.Text = Now
            'txtClaimStatusDate.Enabled = False
            'txtClaimStatus.Enabled = False
            'Praveen - Issue No.SAMB10014
            Dim oGetHeaderAndSummariesByKeyRequest As New GetHeaderAndSummariesByKeyRequestType
            Dim oGetHeaderAndSummariesByKeyResponse As New GetHeaderAndSummariesByKeyResponseType

            With oGetHeaderAndSummariesByKeyRequest
                .BranchCode = "HeadOff"
                .InsuranceFileKey = (DirectCast(Session("SelectedClaim"), BaseFindClaimResponseTypeRow)).InsuranceFileKey
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

                txtLastModifiedDate.Text = Today.Date

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
                'adding claim key value to the session  JP 18/02/10
                Session.Add("ClaimKey", .ClaimKey)
            End With

            Try

                 StartDate = Date.Now  
                oGetClaimDetailsResponse = oSAM.GetClaimDetails(oGetClaimDetailsRequest)
                WriteToLog(Session, "2_Claim Details.aspx", "SAMForInsuranceV2", "GetClaimDetails", StartDate, Date.Now)

                Session("TimeStamp") = oGetClaimDetailsResponse.TimeStamp

                With oGetClaimDetailsResponse
                    If Not (.Errors) Is Nothing Then
                        'errors returned, so throw an exception
                        Throw New SamResponseException(.Errors)
                    Else
                        Session("GetClaimDetailsResponse") = oGetClaimDetailsResponse

                        Dim oMaintainClaimSelectedClaim As New BaseFindClaimResponseTypeRow
                        oMaintainClaimSelectedClaim = DirectCast(Session("SelectedClaim"), BaseFindClaimResponseTypeRow)

                        txtClaimNumber.Text = oMaintainClaimSelectedClaim.ClaimNumber
                        txtClaimNumber.Enabled = False
                        ddlClaimHandler.SelectedIndex = ddlClaimHandler.Items.IndexOf(ddlClaimHandler.Items.FindByValue(.ClaimDetails.ClaimDetails.HandlerCode.Trim()))
                        ddlProgressStatus.SelectedIndex = ddlProgressStatus.Items.IndexOf(ddlProgressStatus.Items.FindByValue(.ClaimDetails.ClaimDetails.ProgressStatusCode.Trim()))
                        txtDescription.Text = .ClaimDetails.ClaimDetails.Description
                        ddlPrimaryCause.SelectedIndex = ddlPrimaryCause.Items.IndexOf(ddlPrimaryCause.Items.FindByValue(.ClaimDetails.ClaimDetails.PrimaryCauseCode.Trim()))
                        'ddlSecondary.SelectedIndex = ddlSecondary.Items.IndexOf(ddlSecondary.Items.FindByValue(.ClaimDetails.ClaimDetails.SecondaryCauseCode.Trim()))
                        If Not .ClaimDetails.ClaimDetails.CatastropheCode = Nothing Then
                            ddlCatastrophe.SelectedIndex = ddlCatastrophe.Items.IndexOf(ddlCatastrophe.Items.FindByValue(.ClaimDetails.ClaimDetails.CatastropheCode.Trim()))
                        End If

                        If Not .ClaimDetails.ClaimDetails.TownCode = Nothing Then
                            ddlTown.SelectedIndex = ddlTown.Items.IndexOf(ddlTown.Items.FindByValue(.ClaimDetails.ClaimDetails.TownCode.Trim()))
                        End If

                        txtLocation.Text = .ClaimDetails.ClaimDetails.Location
                        'txtLossDate.Text = .ClaimDetails.ClaimDetails.LossFromDate.Date JP 19/02/10
                        txtLossDate.Text = .ClaimDetails.ClaimDetails.LossFromDate
                        If .ClaimDetails.ClaimDetails.LossToDateSpecified Then
                            'txtLossToDate.Text = .ClaimDetails.ClaimDetails.LossToDate.Date JP 19/02/10
                            txtLossToDate.Text = .ClaimDetails.ClaimDetails.LossToDate
                        End If

                        'txtReportedDate.Text = .ClaimDetails.ClaimDetails.ReportedDate.Date JP 19/02/10
                        txtReportedDate.Text = .ClaimDetails.ClaimDetails.ReportedDate
                        'Currently putting the claim status date us now
                        txtClaimStatusDate.Text = Today.Date
                        txtClaimStatus.Enabled = False
                        txtClaimStatusDate.Enabled = False
                        ddlRiskType.SelectedIndex = ddlRiskType.Items.IndexOf(ddlRiskType.Items.FindByValue(.ClaimDetails.ClaimDetails.RiskKey.ToString()))
                        ddlCurrency.SelectedIndex = ddlCurrency.Items.IndexOf(ddlCurrency.Items.FindByValue(.ClaimDetails.ClaimDetails.CurrencyCode.ToString()))
                        ddlCurrency.Enabled = False
                        chkInformationOnly.Checked = oGetClaimDetailsResponse.ClaimDetails.ClaimDetails.InfoOnly

                        txtStreetName.Text = oGetClaimDetailsResponse.ClaimDetails.ClaimDetails.Client.Address.AddressLine1
                        txtLocality.Text = oGetClaimDetailsResponse.ClaimDetails.ClaimDetails.Client.Address.AddressLine2
                        txtpostTown.Text = oGetClaimDetailsResponse.ClaimDetails.ClaimDetails.Client.Address.AddressLine3
                        ddlCountry.SelectedItem.Value = oGetClaimDetailsResponse.ClaimDetails.ClaimDetails.Client.Address.CountryCode
                        txtAddressPostCode.Text = oGetClaimDetailsResponse.ClaimDetails.ClaimDetails.Client.Address.PostCode
                        ddlAddressType.SelectedItem.Value = oGetClaimDetailsResponse.ClaimDetails.ClaimDetails.Client.Address.AddressTypeCode
                        txtAddress.Text = txtStreetName.Text + ", " + txtLocality.Text + ", " + txtpostTown.Text + " ," + ddlCountry.SelectedItem.Text + " ," + txtAddressPostCode.Text

                        If oGetClaimDetailsResponse.ClaimDetails.ClaimDetails.Client.Contact IsNot Nothing Then
                            txtEmailNumber.Text = oGetClaimDetailsResponse.ClaimDetails.ClaimDetails.Client.Contact(0).ContactDetail.Item
                        End If

                        txtVATRegistrationNumber.Text = oGetClaimDetailsResponse.ClaimDetails.ClaimDetails.Client.TaxRegistrationNumber
                        chkVatRegistered.Checked = oGetClaimDetailsResponse.ClaimDetails.ClaimDetails.Client.TaxRegistered
                        txtClientClaimNumber.Text = oGetClaimDetailsResponse.ClaimDetails.ClaimDetails.Client.PartyClaimNumber

                        Dim oPaymentDetails As BaseGetClaimPaymentDetailsType()

                        Dim TotalPayment As Integer
                        TotalPayment = 0

                        For PerilCount As Integer = 0 To .ClaimDetails.ClaimPeril.Length - 1
                            If .ClaimDetails.ClaimPeril(PerilCount).ClaimPayments IsNot Nothing Then
                                TotalPayment = TotalPayment + .ClaimDetails.ClaimPeril(PerilCount).ClaimPayments.Length
                            End If

                        Next

                        ReDim Preserve oPaymentDetails(TotalPayment - 1)
                        TotalPayment = 0
                        For Count As Integer = 0 To .ClaimDetails.ClaimPeril.Length - 1
                            If .ClaimDetails.ClaimPeril(Count).ClaimPayments IsNot Nothing Then
                                For PaymentCount As Integer = 0 To .ClaimDetails.ClaimPeril(Count).ClaimPayments.Length - 1
                                    oPaymentDetails(TotalPayment) = New BaseGetClaimPaymentDetailsType
                                    oPaymentDetails(TotalPayment) = .ClaimDetails.ClaimPeril(Count).ClaimPayments(PaymentCount)
                                    TotalPayment = TotalPayment + 1
                                Next
                            End If
                        Next
                        gvPaymenyHistory.DataSource = oPaymentDetails
                        gvPaymenyHistory.DataBind()
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

    Protected Sub btnOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOk.Click

        Dim oMantainClaimRequestType As New MaintainClaimRequestType

        Dim oMaintainClaimSelectedClaim As New BaseFindClaimResponseTypeRow
        oMaintainClaimSelectedClaim = DirectCast(Session("SelectedClaim"), BaseFindClaimResponseTypeRow)

        Dim oGetClaimDetailsResponse As New GetClaimDetailsResponseType
        oGetClaimDetailsResponse = DirectCast(Session("GetClaimDetailsResponse"), GetClaimDetailsResponseType)

        'Getting Risk Perils
        Dim oGetClaimRiskLinksRequest As New GetClaimRiskLinksRequestType
        Dim oGetClaimRiskLinksResponse As New GetClaimRiskLinksResponseType

        With oGetClaimRiskLinksRequest
            .BranchCode = "HeadOff"
            .InsuranceFileKey = oMaintainClaimSelectedClaim.InsuranceFileKey
            .RiskKey = ddlRiskType.SelectedValue
        End With
        Try
              StartDate = Date.Now
            oGetClaimRiskLinksResponse = oSAM.GetClaimRiskLinks(oGetClaimRiskLinksRequest)
             WriteToLog(Session, "2_Claim Details.aspx", "SAMForInsuranceV2", "GetClaimRiskLinks", StartDate, Date.Now)

            With oGetClaimRiskLinksResponse
                If Not (.Errors) Is Nothing Then
                    'errors returned, so throw an exception
                    Throw New SamResponseException(.Errors)
                Else
                    Session("GetClaimRisksLinksResponse") = oGetClaimRiskLinksResponse
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

        If oGetClaimDetailsResponse.ClaimDetails.ClaimDetails.InfoOnly Then

            '' Creating a peril type Object.
            Dim oPerilType() As Peril
            ReDim Preserve oPerilType(oGetClaimRiskLinksResponse.PerilType.Length - 1)

            For Count As Integer = 0 To oGetClaimRiskLinksResponse.PerilType.Length - 1
                oPerilType(Count) = New Peril
                oPerilType(Count).PerilTypeCode = oGetClaimRiskLinksResponse.PerilType(Count).Code
                oPerilType(Count).Description = oGetClaimRiskLinksResponse.PerilType(Count).Description
                oPerilType(Count).SumInsured = oGetClaimRiskLinksResponse.PerilType(Count).SumInsured

                Dim oReserve() As ReserveDetails
                ReDim Preserve oReserve(oGetClaimRiskLinksResponse.PerilType(Count).ReserveType.Length - 1)

                For ReserveCount As Integer = 0 To oGetClaimRiskLinksResponse.PerilType(Count).ReserveType.Length - 1
                    oReserve(ReserveCount) = New ReserveDetails
                    oReserve(ReserveCount).TypeCode = oGetClaimRiskLinksResponse.PerilType(Count).ReserveType(ReserveCount).Code
                    oReserve(ReserveCount).Description = oGetClaimRiskLinksResponse.PerilType(Count).ReserveType(ReserveCount).Description
                Next
                oPerilType(Count).Reserves = oReserve

                'Dim oRecovery() As RecoveryDetails
                'ReDim Preserve oRecovery(oGetClaimRiskLinksResponse.PerilType(Count).RecoveryType.Length - 1)

                'For RecoveryCount As Integer = 0 To oGetClaimRiskLinksResponse.PerilType(Count).RecoveryType.Length - 1
                '    oRecovery(RecoveryCount) = New RecoveryDetails
                '    oRecovery(RecoveryCount).TypeCode = oGetClaimRiskLinksResponse.PerilType(Count).RecoveryType(RecoveryCount).Code
                '    oRecovery(RecoveryCount).Description = oGetClaimRiskLinksResponse.PerilType(Count).RecoveryType(RecoveryCount).Description
                '    oRecovery(RecoveryCount).IsSalvage = oGetClaimRiskLinksResponse.PerilType(Count).RecoveryType(RecoveryCount).IsSalvage
                '    oPerilType(Count).Recoveries = oRecovery
                'Next
            Next
            Session("oMaintainClaimPerils") = oPerilType
        Else
            Dim oPerilType() As Peril
            ReDim Preserve oPerilType(oGetClaimDetailsResponse.ClaimDetails.ClaimPeril.Length - 1)

            For Count As Integer = 0 To oGetClaimDetailsResponse.ClaimDetails.ClaimPeril.Length - 1
                For PerilLinkCount As Integer = 0 To oGetClaimRiskLinksResponse.PerilType.Length - 1
                    If oGetClaimRiskLinksResponse.PerilType(PerilLinkCount).Code = oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(Count).TypeCode Then
                        oPerilType(Count) = New Peril
                        oPerilType(Count).PerilTypeCode = oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(Count).TypeCode
                        oPerilType(Count).Description = oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(Count).Description
                        oPerilType(Count).SumInsured = oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(Count).SumInsured
                        oPerilType(Count).BaseClaimPerilKey = oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(Count).BaseClaimPerilKey

                        If oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(Count).Reserve IsNot Nothing Then
                            Dim oReserve() As ReserveDetails
                            ReDim Preserve oReserve(oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(Count).Reserve.Length - 1)
                            For iCounter As Integer = 0 To oGetClaimRiskLinksResponse.PerilType(PerilLinkCount).ReserveType.Length - 1
                                For ReserveCount As Integer = 0 To oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(Count).Reserve.Length - 1
                                    If (oGetClaimRiskLinksResponse.PerilType(PerilLinkCount).ReserveType(iCounter).Code = oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(Count).Reserve(ReserveCount).TypeCode) Then
                                        oReserve(ReserveCount) = New ReserveDetails
                                        oReserve(ReserveCount).TypeCode = oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(Count).Reserve(ReserveCount).TypeCode
                                        oReserve(ReserveCount).BaseResereveKey = oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(Count).Reserve(ReserveCount).BaseReserveKey
                                        oReserve(ReserveCount).Description = oGetClaimRiskLinksResponse.PerilType(PerilLinkCount).ReserveType(iCounter).Description
                                        'Praveen
                                        oReserve(ReserveCount).RevisionAmount = oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(Count).Reserve(ReserveCount).RevisedReserve
                                        ' oReserve(ReserveCount).RevisionAmount = oReserve(ReserveCount).ThisRevision
                                        'Praveen
                                        oReserve(ReserveCount).InitialReserve = oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(Count).Reserve(ReserveCount).InitialReserve
                                        oReserve(ReserveCount).SumIncurred = oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(Count).Reserve(ReserveCount).SumInsured
                                        'Praveen
                                        oReserve(ReserveCount).Incurred = oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(Count).Reserve(ReserveCount).InitialReserve + oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(Count).Reserve(ReserveCount).RevisedReserve
                                        oReserve(ReserveCount).CurrentReserve = oReserve(ReserveCount).Incurred - oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(Count).Reserve(ReserveCount).PaidAmount

                                        ' oReserve(ReserveCount).Incurred = oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(Count).Reserve(ReserveCount).InitialReserve
                                        ' oReserve(ReserveCount).CurrentReserve = oReserve(ReserveCount).InitialReserve - oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(Count).Reserve(ReserveCount).PaidAmount
                                        'Praveen
                                    End If
                                Next
                            Next
                            oPerilType(Count).Reserves = oReserve
                        End If

                        If oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(Count).Recovery IsNot Nothing Then
                            Dim oRecovery() As RecoveryDetails
                            ReDim Preserve oRecovery(oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(Count).Recovery.Length - 1)

                            For RecoveryCount As Integer = 0 To oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(Count).Recovery.Length - 1

                                For LinkRecoveryCount As Integer = 0 To oGetClaimRiskLinksResponse.PerilType(PerilLinkCount).RecoveryType.Length - 1
                                    If oGetClaimRiskLinksResponse.PerilType(PerilLinkCount).RecoveryType(LinkRecoveryCount).Code.Trim = oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(Count).Recovery(RecoveryCount).TypeCode.Trim Then
                                        oRecovery(RecoveryCount) = New RecoveryDetails
                                        oRecovery(RecoveryCount).TypeCode = oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(Count).Recovery(RecoveryCount).TypeCode
                                        oRecovery(RecoveryCount).Description = oGetClaimRiskLinksResponse.PerilType(PerilLinkCount).RecoveryType(LinkRecoveryCount).Description
                                        oRecovery(RecoveryCount).IsSalvage = oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(Count).Recovery(RecoveryCount).IsSalvage
                                        oRecovery(RecoveryCount).InitialReserve = oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(Count).Recovery(RecoveryCount).InitialRecovery
                                        oRecovery(RecoveryCount).TotalReserve = oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(Count).Recovery(RecoveryCount).InitialRecovery + oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(Count).Recovery(RecoveryCount).RevisedRecovery
                                        oRecovery(RecoveryCount).RevisionAmount = oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(Count).Recovery(RecoveryCount).RevisedRecovery
                                        'JP If oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(Count).Recovery(RecoveryCount).RecoveryPartyCode IsNot Nothing Then
                                        'JP oRecovery(RecoveryCount).PartyCode = oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(Count).Recovery(RecoveryCount).RecoveryPartyCode
                                        'JP Else
                                        oRecovery(RecoveryCount).PartyCode = ""
                                        'JP End If
                                        'JP If oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(Count).Recovery(RecoveryCount).RecoveryPartyTypeCode IsNot Nothing Then
                                        'JP oRecovery(RecoveryCount).PartyTypeCode = oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(Count).Recovery(RecoveryCount).RecoveryPartyTypeCode
                                        'JP Else
                                        oRecovery(RecoveryCount).PartyTypeCode = ""
                                        'JP End If
                                        'JP oRecovery(RecoveryCount).PartyTypeKey = oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(Count).Recovery(RecoveryCount).RecoveryPartyTypeKey
                                        'JP oRecovery(RecoveryCount).PartyKey = oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(Count).Recovery(RecoveryCount).RecoveryPartyKey

                                    End If
                                Next
                            Next
                            oPerilType(Count).Recoveries = oRecovery
                        End If

                    End If
                Next
            Next
            Session("oMaintainClaimPerils") = oPerilType
        End If

        With oMantainClaimRequestType
            .Claim = New BaseClaimMaintainType
            .BranchCode = "HeadOff"
            .Claim.BaseClaimKey = DirectCast(Session("GetClaimDetailsResponse"), GetClaimDetailsResponseType).ClaimDetails.ClaimDetails.BaseClaimKey
            .Claim.HandlerCode = ddlClaimHandler.SelectedValue
            .Claim.ProgressStatusCode = ddlProgressStatus.SelectedValue
            .Claim.Description = txtDescription.Text
            .Claim.PrimaryCauseCode = ddlPrimaryCause.SelectedValue
            .Claim.SecondaryCauseCode = ddlSecondary.SelectedValue
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
            .Claim.InfoOnly = chkInformationOnly.Checked

            .Claim.Client = New BaseClaimPartyClientType
            .Claim.Client.TaxRegistrationNumber = txtVATRegistrationNumber.Text
            .Claim.Client.TaxRegistered = chkVatRegistered.Checked
            .Claim.Client.PartyClaimNumber = txtClientClaimNumber.Text

            If Not (String.IsNullOrEmpty(txtAddress.Text)) Then
                .Claim.Client.Address = New BaseAddressType
                .Claim.Client.Address.AddressLine1 = txtStreetName.Text
                .Claim.Client.Address.AddressLine2 = txtLocality.Text
                .Claim.Client.Address.AddressLine3 = txtpostTown.Text
                .Claim.Client.Address.AddressLine4 = ddlCountry.SelectedItem.Text
                .Claim.Client.Address.CountryCode = ddlCountry.SelectedValue
                .Claim.Client.Address.PostCode = txtAddressPostCode.Text
                .Claim.Client.Address.AddressTypeCode = ddlAddressType.SelectedValue
            End If

            Dim oContact As New BaseContactType
            oContact.ContactDetail = New BaseContactDetailType
            oContact.ContactDetail.ItemElementName = ItemChoiceType.EmailAddress
            oContact.ContactDetail.Item = txtEmailNumber.Text

            ReDim Preserve .Claim.Client.Contact(0)
            .Claim.Client.Contact(0) = oContact

            'oGetClaimDetailsResponse = DirectCast(Session("GetClaimDetailsResponse"), GetClaimDetailsResponseType)
        End With
        Session("RiskKey") = Convert.ToInt32(ddlRiskType.SelectedValue)
        Session("MaintainClaimRequestType") = oMantainClaimRequestType
        Session("ClientShortCode") = (DirectCast(Session("SelectedClaim"), BaseFindClaimResponseTypeRow)).ClientShortName
        Session("InsuranceFileKey") = (DirectCast(Session("SelectedClaim"), BaseFindClaimResponseTypeRow)).InsuranceFileKey
        If Not oMantainClaimRequestType.Claim.InfoOnly Then
            Response.Redirect("3_Perils.aspx")
        Else
            Response.Redirect("MaintainClaim.aspx")
        End If
    End Sub

    Private Sub BuildLists(ByVal oSAM As SAMForInsuranceV2, ByRef objControl As DropDownList, ByVal ESTSLookup As STSListType, ByVal ListCode As String)
        Dim oRequest As New GetListRequestType
        Dim oResponse As New GetListResponseType

        oRequest.BranchCode = "HeadOff"
        oRequest.ListType = STSListType.PMLookup
        oRequest.ListCode = ListCode

        Try
            
            StartDate = Date.Now
            oResponse = oSAM.GetList(oRequest)
            WriteToLog(Session, "2_Claim Details.aspx", "SAMForInsuranceV2", "GetList", StartDate, Date.Now)

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

    Protected Sub btnAddressCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddressCancel.Click
        pnlAddress.Visible = False
        txtAddressPostCode.Text = ""
        txtCountry.Text = ""
        txtLocality.Text = ""
        txtpostTown.Text = ""
        txtStreetName.Text = ""
        btnOk.Enabled = True
        Menu1.Enabled = True
    End Sub

    Protected Sub btnAddressOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddressOk.Click
        pnlAddress.Visible = False
        btnOk.Enabled = True
        Menu1.Enabled = True
        txtAddress.Text = txtStreetName.Text + ", " + txtLocality.Text + ", " + txtpostTown.Text + " ," + ddlCountry.SelectedItem.Text + " ," + txtAddressPostCode.Text
    End Sub

    Protected Sub btnAddress_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddress.Click
        pnlAddress.Visible = True
        btnOk.Enabled = False
        Menu1.Enabled = False
    End Sub

    Protected Sub txtClientName_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtClientName.TextChanged

    End Sub
End Class
