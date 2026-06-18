Imports Microsoft.Web.Services3.Security.Tokens
Imports SAMForInsuranceV2
Partial Class Maintain_Claim_Default
    Inherits System.Web.UI.Page

    Dim UserToken As UsernameToken
    Dim oSAM As New SAMForInsuranceV2
    Dim hidtext As New HiddenField()
    Dim StartDate As Date
    Dim bProdRiskOptionClaimAutoNum As Boolean = False

    Protected Sub gvPaymenyHistory_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvPaymenyHistory.SelectedIndexChanged

    End Sub

    Protected Sub Menu1_MenuItemClick(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.MenuEventArgs) Handles Menu1.MenuItemClick
        mvClaimDetails.ActiveViewIndex = Int32.Parse(e.Item.Value)
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim oProductRiskOptionValueRequestType As New ProductRiskOptionValueRequestType     'Ankit Jain
        Dim oProductRiskOptionValueResponseType As New ProductRiskOptionValueResponseType   'Ankit Jain


        UserToken = GetUserToken("sirius", "sirius")
        'set up the proxy object
        oSAM.SetClientCredential(UserToken)
        oSAM.SetPolicy("SamClientPolicy")

        oProductRiskOptionValueRequestType.BranchCode = "HeadOff"                                                           'Ankit Jain
        oProductRiskOptionValueRequestType.ProductCode = DirectCast(Session("OpenClaimSelectedPolicy"), BaseFindInsuranceFileResponseTypeRow).ProductCode.Trim()  'Ankit Jain
        oProductRiskOptionValueRequestType.ActionType = ProductConfigActionType.ProductRiskMaintenance                           'Ankit Jain
        oProductRiskOptionValueRequestType.ProducRiskOption = ProductRiskOptions.FullClaimAutoNumberingID                      'Ankit Jain
        oProductRiskOptionValueRequestType.ProducRiskOptionSpecified = True                                                     'Ankit Jain
        oProductRiskOptionValueResponseType = oSAM.GetProductRiskOptionValue(oProductRiskOptionValueRequestType)                'Ankit Jain

        'checking whether claim number is Auto in product option -JP 17/02/10
        With oProductRiskOptionValueResponseType
            If Not (.Errors) Is Nothing Then
                'errors returned, so throw an exception
                Throw New SamResponseException(.Errors)
            Else
                If oProductRiskOptionValueResponseType.ProductRiskOptionValue.Trim = "0" Then
                    txtClaimNumber.Enabled = True
                    bProdRiskOptionClaimAutoNum = False
                Else
                    txtClaimNumber.Enabled = False
                    bProdRiskOptionClaimAutoNum = True
                End If
            End If
        End With

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

            '' Binding Risk Type  Drop down 
            'txtClaimStatusDate.Text = Now
            Dim oOpenClaimSelectedPolicy As New BaseFindInsuranceFileResponseTypeRow
            oOpenClaimSelectedPolicy = DirectCast(Session("OpenClaimSelectedPolicy"), BaseFindInsuranceFileResponseTypeRow)

            With oOpenClaimSelectedPolicy
                txtClientName.Text = .ClientName
                txtAddress.Text = .ClientAddressLine1
                txtAddressPostCode.Text = .ClientPostCode
                txtStreetName.Text = .ClientAddressLine1
            End With

            ''Binding PrimaryCause Drop down - 
            Dim oGetValidPrimaryCausesRequest As New GetValidPrimaryCausesRequestType
            Dim oGetValidPrimaryCausesResponse As New GetValidPrimaryCausesResponseType
            With oGetValidPrimaryCausesRequest
                .BranchCode = "HeadOff"
                .InsuranceFileKey = oOpenClaimSelectedPolicy.InsuranceFileKey
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

            Dim oGetHeaderAndSummariesByKeyRequest As New GetHeaderAndSummariesByKeyRequestType
            Dim oGetHeaderAndSummariesByKeyResponse As New GetHeaderAndSummariesByKeyResponseType

            txtLossDate.Text = Convert.ToDateTime(Session("ClaimDate")).Date
            txtReportedDate.Text = Today.Date
            txtLossToDate.Text = Convert.ToDateTime(Session("ClaimDate")).Date
            txtClaimStatus.Enabled = False
            txtClaimStatusDate.Text = Today.Date
            txtClaimStatusDate.Enabled = False
            'Praveen
            txtLastModifiedDate.Enabled = False
            ddlCountry.SelectedIndex = 1
            ddlAddressType.SelectedIndex = 1
            'Praveen

            With oGetHeaderAndSummariesByKeyRequest
                .BranchCode = "HeadOff"
                .InsuranceFileKey = oOpenClaimSelectedPolicy.InsuranceFileKey
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
                        ddlCurrency.SelectedIndex = ddlCurrency.Items.IndexOf(ddlCurrency.Items.FindByValue(.CurrencyCode))
                        'Praveen
                        ddlCurrency.Enabled = False
                        'Praveen
                        'For iCnt As Integer = 0 To ddlCurrency.Items.Count - 1
                        '    If (ddlCurrency.Items(iCnt).Value = .CurrencyCode) Then
                        '        ddlCurrency.SelectedIndex = iCnt
                        '        ddlCurrency.Enabled = False
                        '        Exit For
                        '    End If
                        'Next
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
        End If

    End Sub

    Protected Sub btnOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOk.Click

        If hidtext.Value = "TRUE" Then
            Exit Sub
        End If

        Dim oOpenClaimRequestType As New OpenClaimRequestType

        Dim oOpenClaimSelectedPolicy As New BaseFindInsuranceFileResponseTypeRow
        oOpenClaimSelectedPolicy = DirectCast(Session("OpenClaimSelectedPolicy"), BaseFindInsuranceFileResponseTypeRow)

        ''''Getting Risk Perils

        Dim oGetClaimRiskLinksRequest As New GetClaimRiskLinksRequestType
        Dim oGetClaimRiskLinksResponse As New GetClaimRiskLinksResponseType
        Dim oFindClaimRequest As New FindClaimRequestType
        Dim oFindClaimResponse As New FindClaimResponseType

        Try
            'validate claim no. if claim number is not Auto in product option -JP 17/02/10
            If Not bProdRiskOptionClaimAutoNum Then
                If txtClaimNumber.Text.Trim = "" Then
                    Throw New Exception("Please Enter Claim Number!")
                Else
                    oFindClaimRequest.BranchCode = "HeadOff"
                    oFindClaimRequest.ClaimNumber = txtClaimNumber.Text.Trim
                    oFindClaimRequest.IncludeClosedClaim = True
                    oFindClaimResponse = oSAM.FindClaim(oFindClaimRequest)
                    If oFindClaimResponse.Claims.Length > 0 Then
                        Throw New Exception("The Claim Number you entered already exists! Please Enter different Claim Number.")
                    End If
                End If
            End If

            With oGetClaimRiskLinksRequest
                .BranchCode = "HeadOff"
                .InsuranceFileKey = oOpenClaimSelectedPolicy.InsuranceFileKey
                .RiskKey = ddlRiskType.SelectedValue
            End With
            StartDate = Date.Now
            oGetClaimRiskLinksResponse = oSAM.GetClaimRiskLinks(oGetClaimRiskLinksRequest)
            WriteToLog(Session, "2_Claim Details.aspx", "SAMForInsuranceV2", "GetClaimRiskLinks", StartDate, Date.Now)

            With oGetClaimRiskLinksResponse
                If Not (.Errors) Is Nothing Then
                    'errors returned, so throw an exception
                    Throw New SamResponseException(.Errors)
                Else
                    Session("GetClaimRisksLinksResponse") = oGetClaimRiskLinksResponse

                    '' Creating a peril type Object.
                    Dim oPerilType() As Peril
                    ReDim Preserve oPerilType(oGetClaimRiskLinksResponse.PerilType.Length - 1)

                    For Count As Integer = 0 To oGetClaimRiskLinksResponse.PerilType.Length - 1

                        oPerilType(Count) = New Peril
                        oPerilType(Count).PerilTypeCode = oGetClaimRiskLinksResponse.PerilType(Count).Code
                        oPerilType(Count).Description = oGetClaimRiskLinksResponse.PerilType(Count).Description
                        oPerilType(Count).SumInsured = oGetClaimRiskLinksResponse.PerilType(Count).SumInsured

                        If oGetClaimRiskLinksResponse.PerilType(Count).ReserveType IsNot Nothing Then
                            Dim oReserve() As ReserveDetails

                            ReDim Preserve oReserve(oGetClaimRiskLinksResponse.PerilType(Count).ReserveType.Length - 1)

                            For ReserveCount As Integer = 0 To oGetClaimRiskLinksResponse.PerilType(Count).ReserveType.Length - 1
                                oReserve(ReserveCount) = New ReserveDetails
                                oReserve(ReserveCount).TypeCode = oGetClaimRiskLinksResponse.PerilType(Count).ReserveType(ReserveCount).Code
                                oReserve(ReserveCount).Description = oGetClaimRiskLinksResponse.PerilType(Count).ReserveType(ReserveCount).Description
                            Next

                            oPerilType(Count).Reserves = oReserve
                        End If
                    Next

                    Session("oOpenClaimPerils") = oPerilType

                    With oOpenClaimRequestType
                        .Claim = New BaseClaimOpenType
                        .BranchCode = "HeadOff"
                        If Not bProdRiskOptionClaimAutoNum Then
                            .Claim.ClaimNumber = txtClaimNumber.Text.Trim
                        End If
                        .Claim.InsuranceFileKey = oOpenClaimSelectedPolicy.InsuranceFileKey
                        .Claim.HandlerCode = ddlClaimHandler.SelectedValue
                        .Claim.ProgressStatusCode = ddlProgressStatus.SelectedValue
                        .Claim.Description = txtDescription.Text
                        .Claim.PrimaryCauseCode = ddlPrimaryCause.SelectedValue
                        .Claim.SecondaryCauseCode = ddlSecondary.SelectedValue
                        .Claim.CatastropheCode = ddlCatastrophe.SelectedValue
                        .Claim.TownCode = ddlTown.SelectedValue
                        .Claim.Location = txtLocation.Text
                        .Claim.LossFromDate = txtLossDate.Text
                        .Claim.InfoOnly = chkInformation.Checked

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
                            '.Claim.Client.Address.AddressTypeCode = ddlAddressType.SelectedValue
                        End If

                        Dim oContact As New BaseContactType
                        oContact.ContactDetail = New BaseContactDetailType
                        oContact.ContactDetail.ItemElementName = ItemChoiceType.EmailAddress
                        oContact.ContactDetail.Item = txtEmailNumber.Text

                        ReDim Preserve .Claim.Client.Contact(0)
                        .Claim.Client.Contact(0) = oContact

                        If txtLossToDate.Text <> "" Then
                            .Claim.LossToDate = txtLossToDate.Text
                            .Claim.LossToDateSpecified = True
                        Else
                            .Claim.LossToDateSpecified = False
                        End If
                        .Claim.ReportedDate = txtReportedDate.Text
                        .Claim.CurrencyCode = ddlCurrency.SelectedValue
                        .Claim.RiskKey = Convert.ToInt32(ddlRiskType.SelectedValue)
                        Session("RiskKey") = Convert.ToInt32(ddlRiskType.SelectedValue)

                    End With

                    Session("OpenClaimRequestType") = oOpenClaimRequestType

                    Response.Redirect("3_CheckUnPaidPremium.aspx")
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

    Protected Sub btnAddress_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddress.Click
        pnlAddress.Visible = True
        btnOk.Enabled = False
        Menu1.Enabled = False
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
                    objControl.Items.Insert(0, "")
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
