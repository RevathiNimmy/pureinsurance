Imports Microsoft.Web.Services3.Security.Tokens
Imports SAMForInsuranceV2
Imports System.Data
Partial Class OpenClaim_Peril
    Inherits System.Web.UI.Page
    Dim oGetClaimDetailsResponse As New GetClaimDetailsResponseType
    Dim oPayClaimRequestType As New PayClaimRequestType
    Dim oClaimPaymentItem() As BaseClaimPaymentItemType
    Dim oSAM As New SAMForInsuranceV2
    Dim totalReserve, TotalCurrentReserve, totalPaidToDate, TotalPaymentTax As Double
    Dim TotalPerilPaid, TotalPerilCurrentReserve, TotalPerilIncurred As Double
     Dim StartDate As Date


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ''''Claim Payment----------Saurabh

        oGetClaimDetailsResponse = DirectCast(Session("GetClaimDetailsResponse"), GetClaimDetailsResponseType)
        '*****************************************************************************
        'New Changes
        '*****************************************************************************
        'Start Arul 
        'gvPerils.DataSource = oGetClaimDetailsResponse.ClaimDetails.ClaimPeril
        'gvPerils.DataBind()
        If Not Page.IsPostBack Then

            Dim dsPerils As New DataSet

            dsPerils.Tables.Add("perils")
            dsPerils.Tables("perils").Columns.Add("Description")
            dsPerils.Tables("perils").Columns.Add("PerilDescription")
            dsPerils.Tables("perils").Columns.Add("SumInsured")
            dsPerils.Tables("perils").Columns.Add("Incurred")
            dsPerils.Tables("perils").Columns.Add("Paid")
            dsPerils.Tables("perils").Columns.Add("Recoveries")
            dsPerils.Tables("perils").Columns.Add("Salvage")
            dsPerils.Tables("perils").Columns.Add("currentreserve")
            dsPerils.Tables("perils").Columns.Add("PolicyCurrency")
            dsPerils.Tables("perils").Columns.Add("Loss currency")
            Dim CurrentReserve As Double
            Dim PaidToDate As Double
            Dim Recovery As Double
            Dim salvage As Double
            Dim iCount As Integer = 0
            'PraveenGora
            If oGetClaimDetailsResponse.ClaimDetails.ClaimPeril IsNot Nothing Then
                'PraveenGora
                For iCount = 0 To oGetClaimDetailsResponse.ClaimDetails.ClaimPeril.Length - 1
                    dsPerils.Tables("perils").Rows.Add()
                    dsPerils.Tables("perils").Rows(iCount).Item("Description") = oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(iCount).Description
                    dsPerils.Tables("perils").Rows(iCount).Item("PerilDescription") = oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(iCount).Description
                    dsPerils.Tables("perils").Rows(iCount).Item("SumInsured") = oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(iCount).SumInsured
                    If oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(iCount).Reserve IsNot Nothing Then
                        CurrentReserve = 0
                        PaidToDate = 0
                        For iReserveCount As Integer = 0 To oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(iCount).Reserve.Length - 1
                            CurrentReserve += oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(iCount).Reserve(iReserveCount).InitialReserve + oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(iCount).Reserve(iReserveCount).RevisedReserve - oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(iCount).Reserve(iReserveCount).PaidAmount
                            PaidToDate += oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(iCount).Reserve(iReserveCount).PaidAmount
                        Next
                        dsPerils.Tables("perils").Rows(iCount).Item("currentreserve") = CurrentReserve
                        dsPerils.Tables("perils").Rows(iCount).Item("Paid") = PaidToDate
                        dsPerils.Tables("perils").Rows(iCount).Item("Incurred") = CurrentReserve + PaidToDate
                    Else
                        CurrentReserve = 0
                        PaidToDate = 0
                        dsPerils.Tables("perils").Rows(iCount).Item("currentreserve") = 0
                        dsPerils.Tables("perils").Rows(iCount).Item("Paid") = 0
                        dsPerils.Tables("perils").Rows(iCount).Item("Incurred") = CurrentReserve + PaidToDate
                    End If


                    If oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(iCount).Recovery IsNot Nothing Then

                        For iRecoveriesCount As Integer = 0 To oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(iCount).Recovery.Length - 1
                            'if (oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(iCount).Recovery(iRecoveriesCount).IsSalvage = False) Then
                            'Recovery += oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(iCount).Recovery(iRecoveriesCount).InitialRecovery + oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(iCount).Recovery(iRecoveriesCount).RevisedRecovery - (oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(iCount).Recovery(iRecoveriesCount).ReceiptedAmount)
                            ' Else
                            salvage += oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(iCount).Recovery(iRecoveriesCount).InitialRecovery + oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(iCount).Recovery(iRecoveriesCount).RevisedRecovery - (oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(iCount).Recovery(iRecoveriesCount).ReceiptedAmount)
                            'end If
                        Next
                        dsPerils.Tables("perils").Rows(iCount).Item("Recoveries") = Recovery
                        dsPerils.Tables("perils").Rows(iCount).Item("Salvage") = salvage
                    Else
                        salvage = 0
                        Recovery = 0
                        dsPerils.Tables("perils").Rows(iCount).Item("Recoveries") = 0
                        dsPerils.Tables("perils").Rows(iCount).Item("Salvage") = 0
                    End If

                    dsPerils.Tables("perils").Rows(iCount).Item("Loss currency") = Session("Currency").ToString()
                    'dsPerils.Tables("perils").Rows(iCount).Item("PolicyCurrency")=

                Next

                dsPerils.Tables("perils").Rows.Add()
                gvPerils.DataSource = dsPerils
                gvPerils.DataBind()

                'JP 19/02/10
                gvPerils.Rows(oGetClaimDetailsResponse.ClaimDetails.ClaimPeril.Length).Cells(0).Text = ""
                gvPerils.Rows(oGetClaimDetailsResponse.ClaimDetails.ClaimPeril.Length).Cells(3).Text = "Total"
                gvPerils.Rows(oGetClaimDetailsResponse.ClaimDetails.ClaimPeril.Length).Cells(4).Text = TotalPerilIncurred
                gvPerils.Rows(oGetClaimDetailsResponse.ClaimDetails.ClaimPeril.Length).Cells(5).Text = TotalPerilPaid
                gvPerils.Rows(oGetClaimDetailsResponse.ClaimDetails.ClaimPeril.Length).Cells(8).Text = TotalPerilCurrentReserve
                gvPerils.GridLines = GridLines.Both
                gvReserves.GridLines = GridLines.Both
                gvPaymentDetails.GridLines = GridLines.Both
                'lblRiskType.Text = oGetClaimDetailsResponse.ClaimDetails.ClaimDetails.RiskKey.ToString()
                'lblLossCurrency.Text = oGetClaimDetailsResponse.ClaimDetails.ClaimDetails.CurrencyCode.ToString()
                lblLossDate.Text = oGetClaimDetailsResponse.ClaimDetails.ClaimDetails.LossFromDate.ToString()
                'End ARul
                'PraveenGora
            End If
        End If

        'PraveenGora
        If Not Page.IsPostBack Then
            Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")

            'set up the proxy object

            oSAM.SetClientCredential(UserToken)
            oSAM.SetPolicy("SamClientPolicy")

            Dim oRequest As New GetListRequestType
            Dim oResponse As New GetListResponseType

            '*****************************************************************************
            'New Changes
            '*****************************************************************************
            'Start Arul 
            BuildLists(oSAM, ddlProgressStatus, STSListType.PMLookup, "Progress_status", "")
            BuildLists(oSAM, ddlPrimaryCause, STSListType.PMLookup, "primary_cause", "")
            BuildLists(oSAM, ddlSecondary, STSListType.PMLookup, "Secondary_cause", "")

            ddlProgressStatus.SelectedValue = oGetClaimDetailsResponse.ClaimDetails.ClaimDetails.ProgressStatusCode.Trim()
            ddlSecondary.SelectedValue = oGetClaimDetailsResponse.ClaimDetails.ClaimDetails.SecondaryCauseCode
            ddlPrimaryCause.SelectedValue = oGetClaimDetailsResponse.ClaimDetails.ClaimDetails.PrimaryCauseCode.Trim()
            txtDescription.Text = oGetClaimDetailsResponse.ClaimDetails.ClaimDetails.Description
            ddlProgressStatus.Items.Insert(0, "")
            ddlPrimaryCause.Items.Insert(0, "")
            ddlSecondary.Items.Insert(0, "")
            'txtStatus.Text = oGetClaimDetailsResponse.ClaimDetails.ClaimDetails.ClaimStatus
            'End Arul

            lblRisk.Text = Session("RiskTypeDesc").ToString()
            lblRiskType.Text = Session("RiskTypeDesc").ToString()
            lblLossCurrency.Text = Session("Currency").ToString()
            lblCurrency.Text = Session("Currency").ToString()

            oRequest.BranchCode = "HeadOff"
            oRequest.ListType = STSListType.PMLookup
            oRequest.ListCode = "tax_group"
            Try
                 StartDate = Date.Now
                oResponse = oSAM.GetList(oRequest)
                WriteToLog(Session, "4_Perils.aspx", "SAMForInsuranceV2", "GetList", StartDate, Date.Now)
                With oResponse
                    If Not (.Errors) Is Nothing Then
                        'errors returned, so throw an exception
                        Throw New SamResponseException(.Errors)
                    Else
                        ddlTaxGroup.DataSource = oResponse.List
                        ddlTaxGroup.DataTextField = "Description"
                        ddlTaxGroup.DataValueField = "Code"

                        ddlTaxGroup.DataBind()
                        ddlTaxGroup.Items.Insert(0, New ListItem("None", Nothing))
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

            oRequest.BranchCode = "HeadOff"
            oRequest.ListType = STSListType.PMLookup
            oRequest.ListCode = "MediaType"
            Try

                StartDate = Date.Now
                oResponse = oSAM.GetList(oRequest)
                WriteToLog(Session, "4_Perils.aspx", "SAMForInsuranceV2", "GetList", StartDate, Date.Now)
  
                With oResponse
                    If Not (.Errors) Is Nothing Then
                        'errors returned, so throw an exception
                        Throw New SamResponseException(.Errors)
                    Else
                        ddlMediaType.DataSource = oResponse.List
                        ddlMediaType.DataTextField = "Description"
                        ddlMediaType.DataValueField = "Code"
                        ddlMediaType.DataBind()
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
            oRequest.BranchCode = "HeadOff"
            oRequest.ListType = STSListType.PMLookup
            oRequest.ListCode = "Currency"
            Try
                 StartDate = Date.Now
                oResponse = oSAM.GetList(oRequest)
                WriteToLog(Session, "4_Perils.aspx", "SAMForInsuranceV2", "GetList", StartDate, Date.Now)
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
        End If




    End Sub

    Private Sub BuildLists(ByVal oSAM As SAMForInsuranceV2, ByRef objControl As DropDownList, ByVal ESTSLookup As STSListType, ByVal ListCode As String, ByVal BindValue As String)
        Dim oRequest As New GetListRequestType
        Dim oResponse As New GetListResponseType


        oRequest.BranchCode = "HeadOff"
        oRequest.ListType = STSListType.PMLookup
        oRequest.ListCode = ListCode


        Try
             StartDate = Date.Now
            oResponse = oSAM.GetList(oRequest)
            WriteToLog(Session, "4_Perils.aspx", "SAMForInsuranceV2", "GetList", StartDate, Date.Now)

            With oResponse
                If Not (.Errors) Is Nothing Then
                    'errors returned, so throw an exception
                    Throw New SamResponseException(.Errors)
                Else

                    objControl.DataSource = oResponse.List
                    objControl.DataTextField = "Description"
                    objControl.DataValueField = "Code"
                    objControl.DataBind()
                    If (BindValue <> "") Then
                        objControl.SelectedValue = BindValue
                    End If


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

    Protected Sub gvPerils_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvPerils.RowDataBound
        If (e.Row.RowType = DataControlRowType.DataRow) Then
            If (e.Row.Cells(3).Text.Contains("&nbsp;") <> True) Then

                TotalPerilIncurred += Convert.ToDouble(e.Row.Cells(4).Text)

                TotalPerilPaid += Convert.ToDouble(e.Row.Cells(5).Text)

                TotalPerilCurrentReserve += Convert.ToDouble(e.Row.Cells(8).Text)


            End If
        End If
    End Sub
    Protected Sub gvPerils_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvPerils.SelectedIndexChanged
        Dim oGetClaimPaymentTaxesRequest As New GetClaimPaymentTaxesRequestType
        Dim oGetClaimPaymentTaxesResponse As New GetClaimPaymentTaxesResponseType

        With oGetClaimPaymentTaxesRequest
            .BranchCode = "HeadOff"
            .ClaimPayment = DirectCast(Session("PayClaimRequest"), PayClaimRequestType).ClaimPayment
            .ClaimPayment.BaseClaimPerilKey = oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(gvPerils.SelectedIndex).BaseClaimPerilKey
            .TimeStamp = Session("TimeStamp")

        End With
        Try
            ' oGetClaimPaymentTaxesResponse = oSAM.GetClaimPaymentTaxes(oGetClaimPaymentTaxesRequest)
            With oGetClaimPaymentTaxesResponse
                If Not (.Errors) Is Nothing Then
                    'errors returned, so throw an exception
                    Throw New SamResponseException(.Errors)
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


        gvReserves.EditIndex = -1
        gvReserves.DataSource = oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(gvPerils.SelectedIndex).Reserve
        gvReserves.DataBind()
        Dim Payments() As ClaimPayment
        Dim ReserveLength As New Integer
        If Not oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(gvPerils.SelectedIndex).Reserve Is Nothing Then
            ReserveLength = oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(gvPerils.SelectedIndex).Reserve.Length
            ReDim Preserve Payments(ReserveLength)

            For ReserveCount As Integer = 0 To ReserveLength - 1
                Payments(ReserveCount) = New ClaimPayment
                Payments(ReserveCount).ReserveDescription = oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(gvPerils.SelectedIndex).Reserve(ReserveCount).TypeCode
                Payments(ReserveCount).CurrentReserve = oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(gvPerils.SelectedIndex).Reserve(ReserveCount).InitialReserve + oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(gvPerils.SelectedIndex).Reserve(ReserveCount).RevisedReserve - oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(gvPerils.SelectedIndex).Reserve(ReserveCount).PaidAmount
                Payments(ReserveCount).BaseReserveKey = oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(gvPerils.SelectedIndex).Reserve(ReserveCount).BaseReserveKey
                'Payments(ReserveCount).PaidToDate = oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(gvPerils.SelectedIndex).Reserve(ReserveCount).PaidAmount
                Payments(ReserveCount).ThisPaymentInclTax = oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(gvPerils.SelectedIndex).Reserve(ReserveCount).PaidAmount
                'Payments(ReserveCount).CurrentReserve = oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(gvPerils.SelectedIndex).Reserve(ReserveCount).InitialReserve + Payments(ReserveCount).ThisPaymentInclTax

            Next
            Payments(ReserveLength) = New ClaimPayment
            For ReserveCount As Integer = 0 To ReserveLength - 1
                '  Payments(ReserveLength).TotalReserve = Payments(ReserveLength).TotalReserve + (oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(gvPerils.SelectedIndex).Reserve(ReserveCount).InitialReserve + oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(gvPerils.SelectedIndex).Reserve(ReserveCount).RevisedReserve) - oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(gvPerils.SelectedIndex).Reserve(ReserveCount).PaidAmount
            Next

            Payments(ReserveLength).ReserveDescription = "Total"
            If Not oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(gvPerils.SelectedIndex).ClaimPayments Is Nothing Then
                For PaymentCount As Integer = 0 To oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(gvPerils.SelectedIndex).ClaimPayments.Length - 1
                    ' Payments(ReserveLength).PaidToDate = Payments(ReserveLength).PaidToDate + oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(gvPerils.SelectedIndex).ClaimPayments(PaymentCount).PaymentAmount
                    'Payments(ReserveLength).PaidToDateTax = Payments(ReserveLength).PaidToDateTax + oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(gvPerils.SelectedIndex).ClaimPayments(PaymentCount).TaxAmount
                Next


                For PaymentCount As Integer = 0 To oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(gvPerils.SelectedIndex).ClaimPayments.Length - 2
                    For PaymentItemCount As Integer = 0 To oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(gvPerils.SelectedIndex).ClaimPayments(PaymentCount).ClaimPaymentItems.Length - 1
                        For ReserveCount As Integer = 0 To oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(gvPerils.SelectedIndex).Reserve.Length - 1
                            If oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(gvPerils.SelectedIndex).ClaimPayments(PaymentCount).ClaimPaymentItems(PaymentItemCount).BaseReserveKey = oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(gvPerils.SelectedIndex).Reserve(ReserveCount).BaseReserveKey Then
                                ' Payments(ReserveCount).PaidToDateTax = Payments(ReserveCount).PaidToDateTax + oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(gvPerils.SelectedIndex).ClaimPayments(PaymentCount).ClaimPaymentItems(PaymentItemCount).TaxAmount
                                'Payments(ReserveCount).PaidToDate = Payments(ReserveCount).PaidToDate + oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(gvPerils.SelectedIndex).ClaimPayments(PaymentCount).ClaimPaymentItems(PaymentItemCount).PaymentAmount
                            End If
                        Next
                    Next
                Next


            End If

            ''Payments(ReserveLength).PaidToDate = Payments(ReserveLength).PaidToDate - Payments(ReserveLength).PaidToDateTax
            Session("Payments") = Payments
            gvPaymentDetails.DataSource = Payments
            gvPaymentDetails.DataBind()

            'JP 19/02/10
            gvPaymentDetails.Rows(Payments.Length - 1).Cells(0).Text = ""
            gvPaymentDetails.Rows(Payments.Length - 1).Cells(3).Text = Session("totalReserve ")
            gvPaymentDetails.Rows(Payments.Length - 1).Cells(4).Text = TotalCurrentReserve
            gvPaymentDetails.Rows(Payments.Length - 1).Cells(8).Text = totalPaidToDate
            gvPaymentDetails.Rows(Payments.Length - 1).Cells(6).Text = TotalPaymentTax
        End If


    End Sub



    Protected Sub btnOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOk.Click

        oPayClaimRequestType = DirectCast(Session("PayClaimRequest"), PayClaimRequestType)
        oPayClaimRequestType.ClaimPayment.Payee = New BaseClaimPayeeType
        oPayClaimRequestType.ClaimPayment.Payee.MediaReference = txtMediaReference.Text
        oPayClaimRequestType.ClaimPayment.Payee.BankCode = txtBankCode.Text
        oPayClaimRequestType.ClaimPayment.Payee.BankName = txtBankName.Text
        oPayClaimRequestType.ClaimPayment.Payee.MediaTypeCode = "CA"
        oPayClaimRequestType.ClaimPayment.Payee.Name = txtPayeeName.Text
        oPayClaimRequestType.ClaimPayment.Payee.BankNumber = txtBankAccountNo.Text
        oPayClaimRequestType.ClaimPayment.Payee.TheirReference = txtTheirReference.Text
        oPayClaimRequestType.ClaimPayment.CurrencyCode = ddlCurrency.SelectedValue
        Session("PayClaimRequest") = oPayClaimRequestType
        Session("AccountShortCode") = txtShortName.Text
        'Praveen
        Session("MediaReference") = txtMediaReference.Text
        'Praveen

        'Dim oGetOptionSettingRequest As New GetOptionSettingRequestType
        'Dim oGetOptionSettingResponse As New GetOptionSettingResponseType
        'With oGetOptionSettingRequest
        '    .BranchCode = "HeadOff"
        '    .OptionNumber = 5017
        '    .OptionType = OptionType.SystemOption
        'End With

        Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")
        'set up the proxy object
        Dim oSAM As New SAMForInsuranceV2
        oSAM.SetClientCredential(UserToken)
        oSAM.SetPolicy("SamClientPolicy")
        Dim oGetProductClaimsWorkflowRequest As New GetProductClaimsWorkflowOptionsRequestType
        Dim oGetProductClaimsWorkflowResponse As New GetProductClaimsWorkflowOptionsResponseType
        With oGetProductClaimsWorkflowRequest
            If (Session("BranchCode") IsNot Nothing) Then
                .BranchCode = Session("BranchCode").ToString()
            Else
                .BranchCode = "HeadOff"
            End If
            If (Session("ProductCode") IsNot Nothing) Then
                .ProductCode = Session("ProductCode").ToString()
            Else
                .ProductCode = "PMOTOR"
            End If
            .ClaimProcessType = ClaimProcessType.ClaimPayment
        End With

        Try
             StartDate = Date.Now
            oGetProductClaimsWorkflowResponse = oSAM.GetProductClaimsWorkflowOptions(oGetProductClaimsWorkflowRequest)
            WriteToLog(Session, "4_Perils.aspx", "SAMForInsuranceV2", "GetOptionSetting", StartDate, Date.Now)

            With oGetProductClaimsWorkflowResponse
                If Not (.Errors) Is Nothing Then
                    'errors returned, so throw an exception
                    Throw New SamResponseException(.Errors)
                Else
                    If oGetProductClaimsWorkflowResponse.CashPaymentProcess Then
                        Response.Redirect("CashList.aspx")
                    Else
                        Response.Redirect("PayClaim.aspx")
                    End If
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



    Protected Sub Menu2_MenuItemClick(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.MenuEventArgs) Handles Menu2.MenuItemClick
        MultiView1.ActiveViewIndex = Int32.Parse(e.Item.Value)
    End Sub

    Protected Sub gvPaymentDetails_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvPaymentDetails.SelectedIndexChanged
        If gvPaymentDetails.SelectedIndex = -1 Or rblPaymentPartyType.SelectedIndex = -1 Then
            btnEditPayment.Enabled = False
        Else
            btnEditPayment.Enabled = True
        End If
    End Sub

    Protected Sub btnEditPayment_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEditPayment.Click
        pnlPaymentDetails.Visible = True
        Dim Payments() As ClaimPayment
        Payments = DirectCast(Session("Payments"), ClaimPayment())
        'lblRiskType.Text = oGetClaimDetailsResponse.ClaimDetails.ClaimDetails.RiskKey
        lblReserve.Text = oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(gvPerils.SelectedIndex).Reserve(gvPaymentDetails.SelectedIndex).TypeCode
        lblTotalReserve.Text = oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(gvPerils.SelectedIndex).Reserve(gvPaymentDetails.SelectedIndex).InitialReserve - oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(gvPerils.SelectedIndex).Reserve(gvPaymentDetails.SelectedIndex).RevisedReserve
        lblPaidToDate.Text = oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(gvPerils.SelectedIndex).Reserve(gvPaymentDetails.SelectedIndex).PaidAmount
        'lblLossCurrency.Text = oGetClaimDetailsResponse.ClaimDetails.ClaimDetails.CurrencyCode
    End Sub

    Protected Sub btnPDCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPDCancel.Click
        pnlPaymentDetails.Visible = False
    End Sub

    Protected Sub btnPaymentDetailOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPaymentDetailOk.Click
        pnlPaymentDetails.Visible = False
        Dim ItemIndex As New Integer
        ItemIndex = -1
        oPayClaimRequestType = DirectCast(Session("PayClaimRequest"), PayClaimRequestType)

        oClaimPaymentItem = oPayClaimRequestType.ClaimPayment.ClaimPaymentItem

        Dim Payments() As ClaimPayment
        Payments = DirectCast(Session("Payments"), ClaimPayment())

        If Not oClaimPaymentItem Is Nothing Then


            For PaymentItemCount As Integer = 0 To oClaimPaymentItem.Length - 1
                If oClaimPaymentItem(PaymentItemCount).BaseReserveKey = Payments(gvPaymentDetails.SelectedIndex).BaseReserveKey Then
                    ItemIndex = PaymentItemCount
                End If

            Next
            If ItemIndex = -1 Then
                ReDim Preserve oClaimPaymentItem(oClaimPaymentItem.Length)
                ItemIndex = oClaimPaymentItem.Length - 1
            End If

        Else
            ReDim Preserve oClaimPaymentItem(0)
            ItemIndex = oClaimPaymentItem.Length - 1
        End If
        Session("totalReserve ") = Convert.ToDecimal(Session("totalReserve ")) - Convert.ToDecimal(txtPaymentAmount.Text)
        Payments(gvPaymentDetails.SelectedIndex).ThisPaymentInclTax += Convert.ToDecimal(txtPaymentAmount.Text)
        'Praveen
        Payments(gvPaymentDetails.SelectedIndex).CurrentReserve = Payments(gvPaymentDetails.SelectedIndex).CurrentReserve - Convert.ToDecimal(txtPaymentAmount.Text)
        'Payments(gvPerils.SelectedIndex).CurrentReserve = Payments(gvPerils.SelectedIndex).CurrentReserve - Convert.ToDecimal(txtPaymentAmount.Text)
        'Payments(gvPerils.SelectedIndex).PaidToDate = Convert.ToDecimal(txtPaymentAmount.Text)
        'Praveen
        oClaimPaymentItem(ItemIndex) = New BaseClaimPaymentItemType
        oClaimPaymentItem(ItemIndex).BaseReserveKey = Payments(gvPaymentDetails.SelectedIndex).BaseReserveKey
        oClaimPaymentItem(ItemIndex).PaymentAmount += Convert.ToDecimal(txtPaymentAmount.Text)

        If ddlTaxGroup.SelectedValue = "None" Then
            oClaimPaymentItem(ItemIndex).TaxGroupCode = Nothing
        Else
            oClaimPaymentItem(ItemIndex).TaxGroupCode = ddlTaxGroup.SelectedValue
        End If
        oPayClaimRequestType.ClaimPayment.ClaimPaymentItem = oClaimPaymentItem
        oPayClaimRequestType.ClaimPayment.BaseClaimPerilKey = oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(gvPerils.SelectedIndex).BaseClaimPerilKey
        oPayClaimRequestType.ClaimPayment.PaymentPartyType = Int32.Parse(rblPaymentPartyType.SelectedValue)
        If rblPaymentPartyType.SelectedValue = "1" Then
            oPayClaimRequestType.ClaimPayment.PartyKey = Session("PartyKey")
        End If


        Session("PayClaimRequest") = oPayClaimRequestType
        Session("Payments") = Payments
        gvPaymentDetails.DataSource = Payments
        gvPaymentDetails.DataBind()
        Menu3.Items(1).Enabled = True

        'JP 19/02/2010
        gvPaymentDetails.Rows(Payments.Length - 1).Cells(0).Text = ""
        gvPaymentDetails.Rows(Payments.Length - 1).Cells(3).Text = Session("totalReserve ")
        gvPaymentDetails.Rows(Payments.Length - 1).Cells(4).Text = TotalCurrentReserve
        gvPaymentDetails.Rows(Payments.Length - 1).Cells(8).Text = totalPaidToDate
        gvPaymentDetails.Rows(Payments.Length - 1).Cells(6).Text = TotalPaymentTax
        gvPerils.Rows(gvPerils.SelectedIndex).Cells(5).Text = Convert.ToDecimal(gvPerils.Rows(gvPerils.SelectedIndex).Cells(5).Text) + Convert.ToDecimal(txtPaymentAmount.Text)
        gvPerils.Rows(gvPerils.SelectedIndex).Cells(8).Text = Convert.ToDecimal(gvPerils.Rows(gvPerils.SelectedIndex).Cells(8).Text) - Convert.ToDecimal(txtPaymentAmount.Text)
        gvPerils.Rows(oGetClaimDetailsResponse.ClaimDetails.ClaimPeril.Length).Cells(5).Text = TotalPaymentTax
        gvPerils.Rows(oGetClaimDetailsResponse.ClaimDetails.ClaimPeril.Length).Cells(8).Text = TotalCurrentReserve


    End Sub

    Protected Sub Menu3_MenuItemClick(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.MenuEventArgs) Handles Menu3.MenuItemClick
        MvPayment.ActiveViewIndex = Int32.Parse(e.Item.Value)
    End Sub

    Protected Sub TextBox3_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtBankName.TextChanged

    End Sub

    Protected Sub TextBox6_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtTheirReference.TextChanged

    End Sub

    Protected Sub RadioButtonList1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rblPaymentPartyType.SelectedIndexChanged
        txtShortName.Text = ""
        If rblPaymentPartyType.SelectedValue <> "0" Then
            txtShortName.Enabled = True
            btnFindParty.Enabled = True
        Else
            txtShortName.Enabled = False
            btnFindParty.Enabled = False
            txtShortName.Text = "CLMPAYABLE"
        End If
        If rblPaymentPartyType.SelectedValue = "3" Then
            txtShortName.Text = DirectCast(Session("SelectedClaim"), BaseFindClaimResponseTypeRow).ClientShortName
        End If
        If gvPaymentDetails.SelectedIndex = -1 Or rblPaymentPartyType.SelectedIndex = -1 Then
            btnEditPayment.Enabled = False
        Else
            btnEditPayment.Enabled = True
        End If
    End Sub



    Protected Sub btnFindParty_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFindParty.Click
        Session("OpenerPage") = "CP"
    End Sub

    Protected Sub gvPaymentDetails_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvPaymentDetails.RowDataBound
        If (e.Row.RowType = DataControlRowType.DataRow) Then
            totalReserve += Convert.ToDouble(e.Row.Cells(3).Text)
            Session("totalReserve ") = totalReserve
        End If
        If (e.Row.RowType = DataControlRowType.DataRow) Then
            totalPaidToDate += Convert.ToDouble(e.Row.Cells(8).Text)
        End If
        If (e.Row.RowType = DataControlRowType.DataRow) Then
            TotalCurrentReserve += Convert.ToDouble(e.Row.Cells(4).Text)
        End If
        If (e.Row.RowType = DataControlRowType.DataRow) Then
            TotalPaymentTax += Convert.ToDouble(e.Row.Cells(6).Text)
        End If
    End Sub
End Class

Public Class ClaimPayment
    Private reserveDescriptionField As String
    Private totalReserveField As Decimal
    Private paidToDateField As Decimal
    Private paidTodateTaxField As Decimal
    Private currentReserveField As Decimal
    Private thisPaymenInclTaxField As Decimal
    Private thisPaymnetTaxField As Decimal
    Private costToClaimField As Decimal
    Private BaseReserveKeyField As Decimal




    Public Property ReserveDescription() As String
        Get
            Return Me.reserveDescriptionField
        End Get
        Set(ByVal value As String)
            Me.reserveDescriptionField = value
        End Set
    End Property

    Public Property BaseReserveKey() As Integer
        Get
            Return Me.BaseReserveKeyField
        End Get
        Set(ByVal value As Integer)
            Me.BaseReserveKeyField = value

        End Set
    End Property
    Public Property TotalReserve() As Decimal
        Get
            Return Me.totalReserveField
        End Get
        Set(ByVal value As Decimal)
            Me.totalReserveField = value

        End Set
    End Property

    Public Property PaidToDate() As Decimal
        Get
            Return Me.paidToDateField
        End Get
        Set(ByVal value As Decimal)
            Me.paidToDateField = value

        End Set
    End Property
    Public Property PaidToDateTax() As Decimal
        Get
            Return Me.paidTodateTaxField
        End Get
        Set(ByVal value As Decimal)
            Me.paidTodateTaxField = value
        End Set
    End Property

    Public Property CurrentReserve() As Decimal
        Get
            Return Me.currentReserveField
        End Get
        Set(ByVal value As Decimal)
            Me.currentReserveField = value

        End Set
    End Property
    Public Property ThisPaymentInclTax() As Decimal
        Get
            Return Me.thisPaymenInclTaxField

        End Get
        Set(ByVal value As Decimal)
            Me.thisPaymenInclTaxField = value
        End Set
    End Property
    Public Property ThisPaymentTax() As Decimal
        Get
            Return Me.thisPaymnetTaxField

        End Get
        Set(ByVal value As Decimal)
            Me.thisPaymnetTaxField = value
        End Set
    End Property
    Public Property CostToClaim() As Decimal
        Get
            Return Me.costToClaimField
        End Get
        Set(ByVal value As Decimal)
            Me.costToClaimField = value
        End Set
    End Property

End Class
