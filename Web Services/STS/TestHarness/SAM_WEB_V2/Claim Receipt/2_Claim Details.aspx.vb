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
                        txtLossDate.Text = .ClaimDetails.ClaimDetails.LossFromDate.Date
                        If .ClaimDetails.ClaimDetails.LossToDateSpecified Then
                            txtLossToDate.Text = .ClaimDetails.ClaimDetails.LossToDate.Date
                        End If

                        txtReportedDate.Text = .ClaimDetails.ClaimDetails.ReportedDate.Date
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

        Dim oClaimReceiptRequest As New ClaimReceiptRequestType

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
                                    oRecovery(RecoveryCount).ReceiptedAmount = oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(Count).Recovery(RecoveryCount).ReceiptedAmount
                                    oRecovery(RecoveryCount).ReceiptedAmountTax = oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(Count).Recovery(RecoveryCount).ReceiptedTaxAmount
                                    oRecovery(RecoveryCount).BaseRecoveryKey = oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(Count).Recovery(RecoveryCount).BaseRecoveryKey
                                    'JP oRecovery(RecoveryCount).PartyCode = oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(Count).Recovery(RecoveryCount).RecoveryPartyCode
                                    'JP oRecovery(RecoveryCount).PartyTypeCode = oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(Count).Recovery(RecoveryCount).RecoveryPartyTypeCode
                                    'JP oRecovery(RecoveryCount).PartyKey = oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(Count).Recovery(RecoveryCount).RecoveryPartyKey
                                    'JP oRecovery(RecoveryCount).PartyTypeKey = oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(Count).Recovery(RecoveryCount).RecoveryPartyTypeKey
                                End If
                            Next
                        Next
                        oPerilType(Count).Recoveries = oRecovery
                    End If

                End If
            Next
        Next
        Session("oClaimPerils") = oPerilType


        Session("RiskKey") = Convert.ToInt32(ddlRiskType.SelectedValue)
        With oClaimReceiptRequest
            .ClaimReceipt = New BaseClaimReceiptType
            .BranchCode = "HeadOff"
            .ClaimReceipt.BaseClaimKey = oGetClaimDetailsResponse.ClaimDetails.ClaimDetails.BaseClaimKey
            .ClaimReceipt.ClaimVersionDescription = "Claim Payment"
            .ClaimReceipt.CurrencyCode = oGetClaimDetailsResponse.ClaimDetails.ClaimDetails.CurrencyCode

        End With

        Session("RiskTypeDesc") = ddlRiskType.SelectedItem.Text
        Session("Currency") = ddlCurrency.SelectedItem.Text

        Session("ClaimReceiptRequest") = oClaimReceiptRequest

        Response.Redirect("4_Perils.aspx")
        
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
