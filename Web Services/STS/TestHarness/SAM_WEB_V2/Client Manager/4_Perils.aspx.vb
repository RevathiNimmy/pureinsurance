Imports Microsoft.Web.Services3.Security.Tokens
Imports SAMForInsuranceV2
Partial Class OpenClaim_Peril
    Inherits System.Web.UI.Page
    Dim oGetClaimDetailsResponse As New GetClaimDetailsResponseType
    Dim oPayClaimRequestType As New PayClaimRequestType
    Dim oClaimPaymentItem() As BaseClaimPaymentItemType


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ''''Payment CLaim ----------Saurabh
        oGetClaimDetailsResponse = DirectCast(Session("GetClaimDetailsResponse"), GetClaimDetailsResponseType)
        gvPerils.DataSource = oGetClaimDetailsResponse.ClaimDetails.ClaimPeril
        gvPerils.DataBind()
        lblRiskType.Text = oGetClaimDetailsResponse.ClaimDetails.ClaimDetails.RiskKey.ToString()
        lblLossCurrency.Text = oGetClaimDetailsResponse.ClaimDetails.ClaimDetails.CurrencyCode.ToString()
        lblLossDate.Text = oGetClaimDetailsResponse.ClaimDetails.ClaimDetails.LossFromDate.ToString()

    End Sub

    Protected Sub gvPerils_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvPerils.SelectedIndexChanged
        gvReserves.EditIndex = -1
        gvReserves.DataSource = oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(gvPerils.SelectedIndex).Reserve
        gvReserves.DataBind()
        ''Dim tc As DataControlField
        ''tc.HeaderText = "Current Reserve"
        ''tc.Visible = True
        'gvReserves.Columns.Add(tc)
        'gvReserves.Columns.Item(6).HeaderText = "Current Reserve"
        'gvReserves.Columns(6).InsertVisible = True

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
                Payments(ReserveCount).PaidToDate = oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(gvPerils.SelectedIndex).Reserve(ReserveCount).PaidAmount

            Next
            Payments(ReserveLength) = New ClaimPayment
            For ReserveCount As Integer = 0 To ReserveLength - 1
                Payments(ReserveLength).TotalReserve = Payments(ReserveLength).TotalReserve + (oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(gvPerils.SelectedIndex).Reserve(ReserveCount).InitialReserve + oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(gvPerils.SelectedIndex).Reserve(ReserveCount).RevisedReserve) - oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(gvPerils.SelectedIndex).Reserve(ReserveCount).PaidAmount
            Next

            Payments(ReserveLength).ReserveDescription = "Total"
            If Not oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(gvPerils.SelectedIndex).ClaimPayments Is Nothing Then
                For PaymentCount As Integer = 0 To oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(gvPerils.SelectedIndex).ClaimPayments.Length - 1
                    Payments(ReserveLength).PaidToDate = Payments(ReserveLength).PaidToDate + oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(gvPerils.SelectedIndex).ClaimPayments(PaymentCount).ClaimPaymentItems(0).PaymentAmount
                    Payments(ReserveLength).PaidToDateTax = Payments(ReserveLength).PaidToDateTax + oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(gvPerils.SelectedIndex).ClaimPayments(PaymentCount).TaxAmount
                Next


                If oGetClaimDetailsResponse.ClaimDetails IsNot Nothing Then
                    If oGetClaimDetailsResponse.ClaimDetails.ClaimPeril.Length > 0 Then
                        For PerilCount As Integer = 0 To oGetClaimDetailsResponse.ClaimDetails.ClaimPeril.Length - 1
                            If oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(PerilCount).ClaimPayments IsNot Nothing Then
                                If oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(PerilCount).ClaimPayments.Length > 0 Then
                                    For PaymentCount As Integer = 0 To oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(PerilCount).ClaimPayments.Length - 1
                                        For PaymentItemCount As Integer = 0 To oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(PerilCount).ClaimPayments(PaymentCount).ClaimPaymentItems.Length - 1
                                            For ReserveCount As Integer = 0 To oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(PerilCount).Reserve.Length - 1
                                                If oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(PerilCount).ClaimPayments(PaymentCount).ClaimPaymentItems(PaymentItemCount).BaseReserveKey = oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(PerilCount).Reserve(ReserveCount).BaseReserveKey Then
                                                    Payments(ReserveCount).PaidToDateTax = Payments(ReserveCount).PaidToDateTax + oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(PerilCount).ClaimPayments(PaymentCount).ClaimPaymentItems(PaymentItemCount).TaxAmount
                                                    Payments(ReserveCount).PaidToDate = Payments(ReserveCount).PaidToDate + oGetClaimDetailsResponse.ClaimDetails.ClaimPeril(PerilCount).ClaimPayments(PaymentCount).ClaimPaymentItems(PaymentItemCount).PaymentAmount
                                                End If
                                            Next
                                        Next
                                    Next
                                End If
                            End If
                        Next
                    End If
                End If
            End If

            Payments(ReserveLength).PaidToDate = Payments(ReserveLength).PaidToDate - Payments(ReserveLength).PaidToDateTax
            Session("Payments") = Payments
            gvPaymentDetails.DataSource = Payments
            gvPaymentDetails.DataBind()
        End If

        
    End Sub

   

    Protected Sub btnOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOk.Click
        Response.Redirect("5_Recoveries.aspx")
    End Sub

    

    Protected Sub Menu2_MenuItemClick(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.MenuEventArgs) Handles Menu2.MenuItemClick
        MultiView1.ActiveViewIndex = Int32.Parse(e.Item.Value)
    End Sub

    Protected Sub gvPaymentDetails_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvPaymentDetails.SelectedIndexChanged
        
    End Sub

    Protected Sub btnEditPayment_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEditPayment.Click
        
    End Sub

    Protected Sub btnPDCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPDCancel.Click

    End Sub

    Protected Sub btnPaymentDetailOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPaymentDetailOk.Click
        

    End Sub

    Protected Sub Menu3_MenuItemClick(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.MenuEventArgs) Handles Menu3.MenuItemClick
        MvPayment.ActiveViewIndex = Int32.Parse(e.Item.Value)
    End Sub

    Protected Sub TextBox3_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtBankName.TextChanged

    End Sub

    Protected Sub TextBox6_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtTheirReference.TextChanged

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
